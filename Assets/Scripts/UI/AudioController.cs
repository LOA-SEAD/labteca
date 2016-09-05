using UnityEngine;
using System.Collections;

public class AudioController : MonoBehaviour{
	public AudioSource audioPlaying, newAudio;
	public float maxVolume, time,elapsedTime;
	private float internVolume;
	private bool transition;

	void Start(){
		transition = false;
		UpdateVolume (maxVolume);
	}
	void FixedUpdate() {
		if (transition) {
			elapsedTime+=Time.deltaTime*1000;
			newAudio.volume=(elapsedTime/time)*internVolume;
			if(newAudio.volume>internVolume*.7f)
				audioPlaying.volume=internVolume-((elapsedTime-time*.7f)/(.3f*time))*internVolume;
			if(elapsedTime>time){
				AudioSource aux = new AudioSource();
				aux=audioPlaying;
				audioPlaying=newAudio;
				newAudio=aux;
				aux=null;
				transition=false;
			}
		}
	}

	public void crossFade(){
		elapsedTime = 0;
		transition = true;
	}

	public void UpdateVolume(float volume){
		PlayerPrefs.SetFloat ("volume", volume);
		internVolume = PlayerPrefs.GetFloat("volume") * 3 / 10;

		AudioSource[] audios = GameObject.FindObjectsOfType<AudioSource> ();
		foreach (AudioSource audio in audios) {
			audio.volume = volume;
		}
	}
}


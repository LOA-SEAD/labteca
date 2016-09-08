using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class AudioController : MonoBehaviour{
	public AudioSource audioPlaying, newAudio;
	public float time,elapsedTime;
	public Slider musicSlider,effectsSlider;
	private float internVolume;
	private bool transition;

	void Start(){
		transition = false;

		if (PlayerPrefs.HasKey ("soundVolume")) {
			UpdateSoundVolume (PlayerPrefs.GetFloat ("soundVolume"));
			musicSlider.value = PlayerPrefs.GetFloat ("soundVolume");
		}
		else
			UpdateSoundVolume (1f);

		if (PlayerPrefs.HasKey ("effectsVolume")) {
			UpdateEffectsVolume (PlayerPrefs.GetFloat ("effectsVolume"));
			effectsSlider.value = PlayerPrefs.GetFloat ("effectsVolume");
		}	
		else
			UpdateEffectsVolume (1f);

		newAudio.enabled = false;
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

				newAudio.enabled = false;
			}
		}
	}

	public void crossFade(){
		elapsedTime = 0;
		transition = true;
		newAudio.enabled = true;
	}

	public void UpdateSoundVolume(float volume){
		PlayerPrefs.SetFloat ("soundVolume", volume);
		internVolume = volume*3/10;

		GameObject[] audios = GameObject.FindGameObjectsWithTag("BackgroundAudio");
		foreach (GameObject audio in audios) {
			audio.GetComponent<AudioSource>().volume = internVolume;
		}
	}

	public void UpdateEffectsVolume(float volume){
		PlayerPrefs.SetFloat ("effectsVolume", volume);
		
		AudioSource[] audios = GameObject.FindObjectsOfType<AudioSource> ();
		foreach (AudioSource audio in audios) {
			if(!audio.gameObject.CompareTag("BackgroundAudio"))
				audio.volume = volume;
		}
	}

	public void UpdateEffectsVolume(){
		float volume = effectsSlider.value;
		UpdateEffectsVolume (volume);
	}

	public void UpdateSoundVolume(){
		float volume = musicSlider.value;
		UpdateSoundVolume (volume);
	}
}


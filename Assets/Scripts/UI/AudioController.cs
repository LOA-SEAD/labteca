using UnityEngine;
using System.Collections;

public class AudioController : MonoBehaviour{
	public AudioSource audioPlaying, newAudio;
	public float maxVolume, time,elapsedTime;
	private bool transition;

	void Start(){
		transition = false;
	}
	void FixedUpdate() {
		if (transition) {
			elapsedTime+=Time.deltaTime*1000;
			newAudio.volume=(elapsedTime/time)*maxVolume;
			if(newAudio.volume>maxVolume*.7f)
				audioPlaying.volume=maxVolume-((elapsedTime-time*.7f)/(.3f*time))*maxVolume;
			Debug.Log("Sound New= "+newAudio.volume.ToString()+" Sound Playing= "+audioPlaying.volume.ToString());
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
}


using UnityEngine;
using System.Collections;

public class InGameMenu : MonoBehaviour {
	public CanvasGroup pause, options;
	private bool cursor;

	public void Pause(){
		cursor = Screen.showCursor;

		Screen.showCursor = true;
		Screen.lockCursor = false;

		Time.timeScale = 0;
		this.GetComponent<Canvas> ().enabled = true;
		pause.alpha = 1f;
		pause.interactable = true;
		pause.blocksRaycasts = true;

		GameObject[] audios = GameObject.FindGameObjectsWithTag("BackgroundAudio");
		foreach (GameObject audio in audios) {
			audio.GetComponent<AudioSource>().pitch = 0.7f;
		}
	}

	public void UnPause(){
		Screen.showCursor = cursor;
		Screen.lockCursor = !cursor;

		Time.timeScale = 1f;
		this.GetComponent<Canvas> ().enabled = false;

		GameObject[] audios = GameObject.FindGameObjectsWithTag("BackgroundAudio");
		foreach (GameObject audio in audios) {
			audio.GetComponent<AudioSource>().pitch = 1f;
		}
	}

	public void GoToOptions(){
		pause.alpha = 0f;
		pause.interactable = false;
		pause.blocksRaycasts = false;

		options.alpha = 1f;
		options.interactable = true;
		options.blocksRaycasts = true;
	}

	public void GoToInGameMenu(){
		options.alpha = 0f;
		options.interactable = false;
		options.blocksRaycasts = false;
		
		pause.alpha = 1f;
		pause.interactable = true;
		pause.blocksRaycasts = true;
	}

	public void GoToMainMenu(){
		Time.timeScale = 1f;
		Application.LoadLevel ("Menu");
	}
}

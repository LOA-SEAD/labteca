using UnityEngine;
using System.Collections;

public class InGameMenu : MonoBehaviour {
	public void Pause(){
		Time.timeScale = 0f;
	}

	public void UnPause(){
		//Time.timeScale
	}
}

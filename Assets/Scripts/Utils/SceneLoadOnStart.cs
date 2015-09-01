using UnityEngine;
using System.Collections;

//! Load the scene on start.
/*! */

public class SceneLoadOnStart : MonoBehaviour {

	public string sceneToGo;
	private bool alreadyCall = false;
	// Use this for initialization
	/*void Start () 
	{
	
	}*/
	
	// Update is called once per frame
	//! Set player positions, setup the scale (balance) and load the scene.
	// PlayerPrefs: Stores and accesses player preferences between game sessions.
	void Update () 
	{
		if (!alreadyCall) 
		{
			alreadyCall = true;
			PlayerPrefs.SetFloat ("setupBalance", 0f);
			PlayerPrefs.SetFloat ("PlayerPosX", 0f);
			PlayerPrefs.SetFloat ("PlayerPosY", 1f);
			PlayerPrefs.SetFloat ("PlayerPosZ", 4f);
			GetComponent<ChangeSceneMessage>().LoadScene();
		}
	}
}

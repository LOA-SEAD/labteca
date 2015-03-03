using UnityEngine;
using System.Collections;

public class SceneLoadOnStart : MonoBehaviour {

	public string sceneToGo;

	private bool alreadyCall = false;
	// Use this for initialization
	void Start () 
	{
	
	}
	
	// Update is called once per frame
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

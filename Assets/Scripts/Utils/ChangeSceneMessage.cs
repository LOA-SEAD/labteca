using UnityEngine;
using System.Collections;

public class ChangeSceneMessage : MonoBehaviour 
{

	public string sceneToGo;

	public void LoadScene()
	{
		Application.LoadLevel (sceneToGo);
	}

	public void LoadScene(string scene)
	{
		Application.LoadLevel (scene);
	}
}

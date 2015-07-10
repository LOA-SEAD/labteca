using UnityEngine;
using System.Collections;

//! Loads and changes the scene. 
/*! */

public class ChangeSceneMessage : MonoBehaviour 
{

	public string sceneToGo;
	private bool canChangeScene;
	private bool startDelay;
	private float currentTimeToChangeScene;

	//! Changes the scene.
	public void Update(){

		if(canChangeScene){
			Application.LoadLevel (sceneToGo);
		}
		else{
			if(startDelay){
				currentTimeToChangeScene += Time.deltaTime;
				if(currentTimeToChangeScene > 0.01f){
					canChangeScene = true;
					currentTimeToChangeScene = 0;
				}
			}
		}
	}

	//TODO: metodos parecidos. os dois sao necessarios?
	public void LoadScene()
	{
		startDelay = true;
		InventoryController inventory = FindObjectOfType(typeof(InventoryController)) as InventoryController;

	}

	public void LoadScene(string scene)
	{
		sceneToGo = scene;
		startDelay = true;

		InventoryController inventory = FindObjectOfType(typeof(InventoryController)) as InventoryController;

	}
}

using UnityEngine;
using System.Collections;

public class ChangeSceneMessage : MonoBehaviour 
{

	public string sceneToGo;
	private bool canChangeScene;
	private bool startDelay;
	private float currentTimeToChangeScene;


	public void Update(){

		if(canChangeScene){
			Application.LoadLevel (sceneToGo);
		}
		else{
			if(startDelay){
				currentTimeToChangeScene += Time.deltaTime;
				if(currentTimeToChangeScene > 0.5f){
					canChangeScene = true;
					currentTimeToChangeScene = 0;
				}
			}
		}
	}

	public void LoadScene()
	{
		startDelay = true;
		InventoryController inventory = FindObjectOfType(typeof(InventoryController)) as InventoryController;
		inventory.ShowFade();
	}

	public void LoadScene(string scene)
	{
		sceneToGo = scene;
		startDelay = true;

		InventoryController inventory = FindObjectOfType(typeof(InventoryController)) as InventoryController;
		inventory.ShowFade();
	}
}

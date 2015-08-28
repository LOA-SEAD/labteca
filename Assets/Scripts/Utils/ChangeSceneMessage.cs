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
	
	//! Loads the level.
	/*! Loads the level assigned to sceneToGo. */
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
	
	//! Loads the first scene.
	/*! Returns the first active loaded object of InventoryController. */
	public void LoadScene()
	{
		startDelay = true;
		InventoryController inventory = FindObjectOfType(typeof(InventoryController)) as InventoryController;
		
	}
	
	//! Loads the next scene.
	/*! Loads the scene assigned and returns the first active loaded object of InventoryController. */
	public void LoadScene(string scene)
	{
		sceneToGo = scene;
		startDelay = true;
		
		InventoryController inventory = FindObjectOfType(typeof(InventoryController)) as InventoryController;
		
	}
}

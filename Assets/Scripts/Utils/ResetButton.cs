using UnityEngine;
using System.Collections;

//!Load the level
/*! Loads the level by its name. Before you can load a level you have to add it to the list of levels used in the game.*/

public class ResetButton : MonoBehaviour {

	// Use this for initialization
	/*void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}*/

	/* Use File->Build Settings... in Unity and add the levels you need to the level list there.
	 * MonoBehaviour.OnLevelWasLoaded is called on all active game objects after the level has been loaded.*/
	public void MsgMouseDown()
	{
		Application.LoadLevel (Application.loadedLevel);
	}
}

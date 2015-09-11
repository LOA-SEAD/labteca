using UnityEngine;
using System.Collections;

//! In Game State is when the Player is walking around the Lab.
/*! This is the 'Default' state, for when the game starts and the player can move freely. */
public class InGameState : GameStateBase {

	public GameObject player; /*< GameObject of Player. */
	public Canvas inventoryCanvas;

	void Update(){
		base.Update();	// base is the GameStateBase
		//////////////
		if (Input.GetKeyDown (KeyCode.I)) {
			//GameObject.Find("canvas_inventory 1").SetActive(!GameObject.Find("canvas_inventory 1").activeSelf);
			player.GetComponent<MouseLook>().enabled = !player.GetComponent<MouseLook>().enabled;
			player.GetComponent<CharacterMotor>().enabled = !player.GetComponent<CharacterMotor>().enabled;
			player.GetComponent<FPSInputController>().enabled = !player.GetComponent<FPSInputController>().enabled;
			inventoryCanvas.enabled = !inventoryCanvas.enabled;
		}
		/*if (Input.GetKey (KeyCode.I)) {
			Debug.LogError("Exiting InventoryState");
			gameController.ChangeState (4);
		}*/
	}

	protected override void UpdateState ()
	{
		
	}

    //! Actions for when this State starts
    /*! When it starts, the Player is setActive true. */
	public override void OnStartRun ()
	{
		player.SetActive(true);
	}

    //! Actions for when this State stops
    /*! When it stops, the Player is setActive false. */
	public override void OnStopRun ()
	{
		player.SetActive(false);
	}
}

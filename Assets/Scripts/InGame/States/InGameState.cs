using UnityEngine;
using System.Collections;

//! In Game State is when the Player is walking around the Lab.
/*! This is the 'Default' state, for when the game starts and the player can move freely. */
public class InGameState : GameStateBase {

	public GameObject player; /*< GameObject of Player. */
	public Canvas inventoryCanvas;

	void Update(){
		base.Update();	// base is the GameStateBase
		if (Input.GetKeyDown (KeyCode.I)) {  //To access the inventory from the default state
			GameObject.Find("Main Camera").GetComponent<MouseLook>().enabled = !GameObject.Find("Main Camera").GetComponent<MouseLook>().enabled;
			GameObject.Find("Main Camera").GetComponent<Animator>().enabled = !GameObject.Find("Main Camera").GetComponent<Animator>().enabled;
			player.GetComponent<MouseLook>().enabled = !player.GetComponent<MouseLook>().enabled;
			player.GetComponent<CharacterMotor>().enabled = !player.GetComponent<CharacterMotor>().enabled;
			player.GetComponent<FPSInputController>().enabled = !player.GetComponent<FPSInputController>().enabled;
			GameObject.Find("Elaine 1").GetComponent<Animator>().enabled  = !GameObject.Find("Elaine 1").GetComponent<Animator>().enabled;
			inventoryCanvas.enabled = !inventoryCanvas.enabled;
		}
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

using UnityEngine;
using System.Collections;

//! In Game State is when the Player is walking around the Lab.
/*! This is the 'Default' state, for when the game starts and the player can move freely. */
public class InGameState : GameStateBase {

	public GameObject player; /*< GameObject of Player. */
	public Canvas HUD;
	public HUDController HUDControl;

	void Update(){
		base.Update();	// base is the GameStateBase
	}

	protected override void UpdateState ()
	{

	}

    //! Actions for when this State starts
    /*! When it starts, the Player is setActive true. */
	public override void OnStartRun ()
	{
		HUD.enabled = true;
		player.SetActive(true);

		if (HUDControl.journalUp)
			HUDControl.callJournal ();

		if (HUDControl.inventoryUp) {
			HUDControl.inventoryLocked=false;
			HUDControl.callInventory ();
		}
	}

    //! Actions for when this State stops
    /*! When it stops, the Player is setActive false. */
	public override void OnStopRun ()
	{
		if (HUDControl.journalUp)
			HUDControl.callJournal ();

		if (!HUDControl.inventoryUp) {
			HUDControl.callInventory ();
			HUDControl.inventoryLocked=true;
		}
		HUD.enabled = false;
		player.SetActive(false);
	}
}

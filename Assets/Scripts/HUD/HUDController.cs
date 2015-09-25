using UnityEngine;
using System.Collections;

//! Control of the player interaction interface.
/*!
 * Contains three methods that enable or desable the components of gameObject
 * and change the HUDCamera.
 */

public class HUDController : MonoBehaviour {
	public KeyCode journalKey;
	public bool journalUp=false,inventoryUp=false;
	public GameObject player,journal; /*< GameObject of Player. */
	public Canvas inventoryCanvas;
	public bool inventoryNotLocked=false;

	void Update(){
		if(Input.GetKeyDown(journalKey)){
			callJournal();
		}
		if (Input.GetKeyDown (KeyCode.I)&&!journalUp) {
			inventoryNotLocked=!inventoryNotLocked;
			callInventory();
		}
	}
	//!Set the local state of the gameObject.
	/*! Making a gameObject inactive will desable every component. */
    public void disableHUD()
    {
        this.gameObject.SetActive(false);
    }
	//!Set the local state of the gameObject.
    public void enableHUD()
    {
        this.gameObject.SetActive(true);
    }
	//! Change the camera when the player interacts with an object.
	/*! Used as the Camera that events will be sent through for a world space (canvas: element that can used for screen rendering). */
    public void changeHUDCamera(Camera newCamera)
    {
        GetComponent<Canvas>().worldCamera = newCamera;
    }
	public void callJournal(){
		journalUp = !journalUp;
		journal.SetActive (journalUp);
		if (!inventoryNotLocked)
			callInventory ();
	}

	public void callInventory(){
		changePlayerState ();		
		inventoryCanvas.enabled = !inventoryCanvas.enabled;
		inventoryUp = !inventoryUp;
	}

	public void changePlayerState(){
		if (player.activeSelf) {
			player.GetComponent<MouseLook> ().enabled = !player.GetComponent<MouseLook> ().enabled;
			player.GetComponent<CharacterMotor> ().enabled = !player.GetComponent<CharacterMotor> ().enabled;
			player.GetComponent<FPSInputController> ().enabled = !player.GetComponent<FPSInputController> ().enabled;
			GameObject.Find ("Main Camera").GetComponent<MouseLook> ().enabled = !GameObject.Find ("Main Camera").GetComponent<MouseLook> ().enabled;
		}
	}
}

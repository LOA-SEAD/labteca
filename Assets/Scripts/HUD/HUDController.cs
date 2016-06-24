using UnityEngine;
using System.Collections;

//! Control of the player interaction interface.
/*!
 * Contains three methods that enable or desable the components of gameObject
 * and change the HUDCamera.
 */

public class HUDController : MonoBehaviour {
	public InventoryControl invControl;
	public KeyCode journalKey,inventoryKey,mapKey;
	public bool journalUp=false,inventoryUp=false;
	public GameObject player,map; /*< GameObject of Player. */
	public Canvas inventoryCanvas;
	public bool inventoryLocked=false,lockKey;

	void Start(){
		map.SetActive (false);
		lockKey = false;
	}

	void Update(){
		if(Input.GetKeyDown(journalKey)&&!lockKey){
			callJournal();
		}
		if (Input.GetKeyDown (inventoryKey)&&!lockKey) {
			callInventory();
		}
		if(Input.GetKeyDown(mapKey)&&!lockKey){
			callMap();
		}
	}

	public void LockKeys(bool b){
		lockKey = b;
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
		journalUp = (!journalUp);
		invControl.setTabletState (journalUp);

		if (player.GetComponent<MouseLook> ().enabled && journalUp)
			changePlayerState ();
		if (!player.GetComponent<MouseLook> ().enabled && !inventoryUp && !journalUp)
			changePlayerState ();

		if (map.activeSelf)
			map.SetActive (false);	
	}

	public void callInventory(){
		if (!inventoryLocked) {
			inventoryUp = (!inventoryUp);
			invControl.setInventoryState (inventoryUp);
			if (player.GetComponent<MouseLook> ().enabled && inventoryUp)
				changePlayerState ();
			if (!player.GetComponent<MouseLook> ().enabled && !inventoryUp && !inventoryLocked && !journalUp)
				changePlayerState ();
		}

		if (map.activeSelf)
			map.SetActive (false);				
	}

	public void callMap(){
		if (inventoryUp)
			callInventory ();
		if (journalUp)
			callJournal ();
		map.SetActive (!map.activeSelf);
	}


	public void changePlayerState(){
		if (player.activeSelf) {
			GameObject.Find("Elaine 1").GetComponent<Animator>().enabled = !GameObject.Find("Elaine 1").GetComponent<Animator>().enabled;
			GameObject.Find("Main Camera").GetComponent<Animator> ().enabled = !GameObject.Find("Main Camera").GetComponent<Animator> ().enabled;
			player.GetComponent<MouseLook> ().enabled = !player.GetComponent<MouseLook> ().enabled;
			player.GetComponent<CharacterMotor> ().enabled = !player.GetComponent<CharacterMotor> ().enabled;
			player.GetComponent<FPSInputController> ().enabled = !player.GetComponent<FPSInputController> ().enabled;
			GameObject.Find ("Main Camera").GetComponent<MouseLook> ().enabled = !GameObject.Find ("Main Camera").GetComponent<MouseLook> ().enabled;
		}
	}
}

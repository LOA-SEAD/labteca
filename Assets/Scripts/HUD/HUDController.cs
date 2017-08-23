using UnityEngine;
using System.Collections;

//! Control of the player interaction interface.
/*!
 * Contains three methods that enable or desable the components of gameObject
 * and change the HUDCamera.
 */
using System.Collections.Generic;
using UnityEngine.UI;


public class HUDController : MonoBehaviour {
	public InGameMenu menu;
	public InventoryControl invControl;
	public KeyCode journalKey,inventoryKey,mapKey;
	public bool tabletUp=false,inventoryUp=false;
	public GameObject player,map; /*< GameObject of Player. */
	public Canvas inventoryCanvas;
	public bool inventoryLocked=false,mapLocked = false,lockKey;
	public List<Text> keysText;

	public RectTransform hover; 

	void Start(){
		map.SetActive (false);
		lockKey = false;
		Cursor.visible = false;
		Screen.lockCursor = true;
		RefreshKeys ();
	}

	void Update(){
		if (Input.GetKeyDown (KeyCode.P)) {
			if(!menu.IsPaused) {
				menu.Pause ();
				LockKeys(true);
			}
			else {
				menu.UnPause ();
				LockKeys(false);
			}
		}

		if(Input.GetKeyDown(journalKey)&&!lockKey){
			CallTabletTrigger();
		}
		if ((Input.GetKeyDown (inventoryKey))&&!lockKey) {
			CallInventoryTrigger();
		}
		if((Input.GetKeyDown(mapKey))&&!lockKey){
			CallMapTrigger();
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

	public void CallTabletTrigger(){
		if(!lockKey)	
			CallTablet(!tabletUp);
	}
	public void CallTablet(bool b){
		tabletUp = b;
		invControl.setTabletState (b);

		if (player.GetComponent<MouseLook> ().enabled && tabletUp)
			changePlayerState ();
		if (!player.GetComponent<MouseLook> ().enabled && !inventoryUp && !tabletUp)
			changePlayerState ();

		if (map.activeSelf)
			map.SetActive (false);

		if (tabletUp) {
			Cursor.visible = true;
			Screen.lockCursor = false;
		}else if (!inventoryUp && !map.activeSelf) {
			Cursor.visible = false;
			Screen.lockCursor = true;
		}
	}

	public void CallInventoryTrigger(){
		if (!lockKey)
			CallInventory(!inventoryUp);
	}
	/// <summary>
	/// Calls the inventory.
	/// </summary>
	/// <param name="b">If set to <c>true</c> b.</param>
	public void CallInventory(bool b){
		if (!inventoryLocked) {
			inventoryUp = b;
			invControl.setInventoryState (b);
			if (player.GetComponent<MouseLook> ().enabled && inventoryUp)
				changePlayerState ();
			if (!player.GetComponent<MouseLook> ().enabled && !inventoryUp && !inventoryLocked && !tabletUp)
				changePlayerState ();
		}

		if (map.activeSelf)
			map.SetActive (false);

		if (inventoryUp) {
			Cursor.visible = true;
			Screen.lockCursor = false;
		} else if (!tabletUp && !map.activeSelf) {
			Cursor.visible = false;
			Screen.lockCursor = true;
		}
	}
	public void CallMapTrigger(){
		if(!lockKey)
			CallMap(!map.activeSelf);
	}
	/// <summary>
	/// Calls the map.
	/// </summary>
	/// <param name="b">If set to <c>true</c> b.</param>
	public void CallMap(bool b){
		if (!mapLocked) {
			if (inventoryUp)
				CallInventory (false);
			if (tabletUp)
				CallTablet (false);
			map.SetActive (b);

			if (player.GetComponent<MouseLook> ().enabled == map.activeSelf)
				changePlayerState ();
		}
		if (map.activeSelf) {
			Cursor.visible = true;
			Screen.lockCursor = false;
		}else if (!tabletUp && !inventoryUp) {
			Cursor.visible = false;
			Screen.lockCursor = true;
		}
	}

	public void RefreshKeys(){
		keysText [0].text = inventoryKey.ToString ();
		keysText [1].text = journalKey.ToString ();
		keysText [2].text = mapKey.ToString ();
	}
	/// <summary>
	/// Alternates the state of the player.
	/// </summary>
	public void changePlayerState(){
		if (player.activeSelf) {
			GameObject.Find("GameController").GetComponent<AudioController>().crossFade();
			GameObject.Find("Elaine 1").GetComponent<Animator>().enabled = !GameObject.Find("Elaine 1").GetComponent<Animator>().enabled;
			GameObject.Find("Main Camera").GetComponent<Animator> ().enabled = !GameObject.Find("Main Camera").GetComponent<Animator> ().enabled;
			player.GetComponent<MouseLook> ().enabled = !player.GetComponent<MouseLook> ().enabled;
			player.GetComponent<CharacterMotor> ().enabled = !player.GetComponent<CharacterMotor> ().enabled;
			player.GetComponent<FPSInputController> ().enabled = !player.GetComponent<FPSInputController> ().enabled;
			GameObject.Find ("Main Camera").GetComponent<MouseLook> ().enabled = !GameObject.Find ("Main Camera").GetComponent<MouseLook> ().enabled;
		}
	}
}

﻿using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class LIAState : GameStateBase {
	
	// camera and interactive area
	public Camera cameraState;                  /*!< Camera for this state. */
	public GameObject interactBox;              /*!< BoxCollider that allows the Player to enter this state. */
	public Light LIALight;
	public Image glassware,solid,liquid;

	public string currentIndex;

	private bool up;
	private InventoryManager inventory;
	private Sprite originalSprite;
	public void Start () {
		cameraState.gameObject.SetActive(false);
		inventory = GameObject.Find ("InventoryManager").GetComponent<InventoryManager> ();
		originalSprite = glassware.sprite;
	}
	
	protected override void UpdateState ()
	{
		
	}

	public bool RecieveProduct(ItemInventoryBase item,GameObject obj){
		if (currentIndex.Length == 0) {
			if (obj != null) {
				currentIndex = item.index;
				glassware.sprite = item.icon.sprite;
				if(item.solid.isActiveAndEnabled){
					solid.sprite = item.solid.sprite;
					solid.color = new Color32(255,255,255,255);
				}
				if(item.liquid.isActiveAndEnabled){
					liquid.sprite = item.liquid.sprite;
					liquid.color = new Color32(255,255,255,255);
				}
				return true;
			} else {
				Debug.Log ("n recebi");
				return false;
			}
		} else {
			gameController.sendAlert("Ja tem um produto na LIA");
		}
		return false;
	}

	public void RetrieveProduct(){
		if (currentIndex.Length>0) {
			inventory.AddProductToInventory(GameObject.Find(currentIndex));
			glassware.sprite = originalSprite;
			solid.color = new Color32(255,255,255,0);
			liquid.color = new Color32(255,255,255,0);
			currentIndex = "";
		} else {
			gameController.sendAlert("Nao ha nenhum produto");
		}
	}
	
	void Update(){
		base.Update();
		
		if(!canRun)
			return;

		if (up&&LIALight.intensity<0.6f) {
			LIALight.intensity += Time.deltaTime / 2;
		} else {
			up=false;
			if(LIALight.intensity>0.1f)
				LIALight.intensity -= Time.deltaTime / 4;
			else
				up=true;
		}

		//Pressing Esc will exit the state
		if(Input.GetKeyDown(KeyCode.Escape)){
			interactBox.GetComponent<BoxCollider>().enabled = true;
			gameController.ChangeState(0);
			FadeScript.instance.ShowFade();
		}
	}
	
	//! Actions for when the State starts.
	/*! Set the Camera inside the state to be Active, overlaying the Main Camera used at InGameState,
     * close all dialogs that might be enabled. */
	public override void OnStartRun ()
	{
		GameObject.Find ("InventoryManager").GetComponent<InventoryManager> ().changeList (0);
		cameraState.gameObject.SetActive(true);
		cameraState.depth = 2;
		HudText.SetText("");
	}
	
	//! Actions for when the State ends.
	/*! Disable the Camera inside the state, deactivate. */
	public override void OnStopRun ()
	{
		LIALight.intensity = 0f;
		cameraState.depth = -1;
		cameraState.gameObject.SetActive(false);
	}
}

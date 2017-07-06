using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

//! It's what defines if a state may be used to make reactions	
/*! Work as a bridge for the player-equipments interactions.
 	It also manages how this interaction is going to be. */

public class WorkBench : GameStateBase{

	public Transform positionGlassEquipament;   /*!< Position of Glassware on the equipment. */
	public Transform[] positionGlass;

	//GameStateBase variables:
	//public Camera cameraState;                 /*!< Camera for this state. */
	public EquipmentControllerBase equipmentController;	/*!< Equipment controller for this state */
	//end of GameStateBase variables

	public bool cannotEndState;					//The player is holding a filled tool, so can't quit the state
	public bool writingLabel;					//No inputs are valid if the writing interaction is happening

	public AudioSource soundBeaker;				/*!< Audio for the workbench. */

	public StateUIManager stateUIManager;                /*!< The UI Manager Game Object. */
	
	//Tools on the workbench
	public Pipette pipette;
	public Spatula spatula;
	public WashBottle washBottle;
	
	public void Start () {
		writingLabel = false;
		cameraState.enabled = false;
		cameraState.GetComponent<AudioListener> ().enabled = false;
	}
	
	protected override void UpdateState (){}
	
	void Update(){

		base.Update();
		
		if(!canRun)
			return;
		
		//Pressing Esc will exit the state
		if(Input.GetKeyDown(KeyCode.Escape)){
			if(!writingLabel) {
				if(cannotEndState)
					gameController.sendAlert("Não é possível sair com reagente na mão\nColoque de volta no seu pote");
				else{
					ExitState();
					}
			}
		}
	}
	
	public Transform returnPosition(){
		for (int z=0; z<positionGlass.Length; z++) {
			if(positionGlass[z].childCount==0)
				return positionGlass[z];
		}
		return null;
	}

	//Methods for equipment

	//! Put the Glassware on the equipment.
	/*! Verifiy if glassware can be put on the equipment. */
	public bool PutGlassInEquip(GameObject lastItemSelected){
		if(positionGlassEquipament.childCount > 0){
			gameController.sendAlert("O equipamento já tem um recipiente!");
			return false;
		}

		if(lastItemSelected.transform.parent == positionGlassEquipament){
			gameController.sendAlert("O equipamento já está na bancada");
			return false;
		}

		GameObject tempGlass = lastItemSelected.gameObject;
		tempGlass.transform.SetParent(positionGlassEquipament,false);
		tempGlass.transform.localPosition = Vector3.zero;
		equipmentController.AddObjectInEquipament(tempGlass);

		return true;
	}
	//! Put into workbench position an item from the inventory
	public bool PutItemFromInventory(ItemInventoryBase item) {
		bool worked = false;
		if(!item.physicalObject){
			GameObject tempItem = Instantiate(item.itemBeingHeld.gameObject) as GameObject;
			if(!TryPutIntoPosition(tempItem)) {
				Destroy(tempItem);
				worked = false;
			}
			else {
				if(tempItem.GetComponent<ReagentPot>()!=null){
					if(item.reagent.Length!=0){
						Compound reagent = CompoundFactory.GetInstance ().GetCupboardCompound (item.reagent);
						
						if(tempItem.GetComponent<ReagentPot>().isSolid)
							tempItem.GetComponent<ReagentPot>().reagent.setValues(reagent as Compound);
						else
							tempItem.GetComponent<ReagentPot>().reagent.setValues(reagent as Compound);
					}
				}
				worked = true;
			}
		} else {
			GameObject tempItem = GameObject.Find(item.index);
			if(TryPutIntoPosition(tempItem)) {
				tempItem.SetActive(true);
				worked = true;
			}
			else {
				worked = false;
			}
		}

		if (worked) {
			soundBeaker.Play ();
			return true;
		} else {
			gameController.sendAlert ("A Bancada está cheia!");
			return false;
		}
	}
	// Put into free workbench position the item currently on the equipment
	public bool PutItemFromEquip(GameObject tempItem){
		if (TryPutIntoPosition (tempItem)) {
			equipmentController.RemoveObjectInEquipament(tempItem);
			return true;
		}
		gameController.sendAlert("A Bancada está cheia!");
		return false;
	}

	//! Verify if there is any Glassware on the equipment.
	public bool HaveGlassInEquipment(){
		if(positionGlassEquipament.childCount > 0){
			return true;
		}
		return false;
	}
	//! Try to put an item into a free workbench position
	//  Returns true if succeded
	public bool TryPutIntoPosition(GameObject tempItem) {
		foreach (Transform position in positionGlass) {
			if(position.childCount == 0) {
				tempItem.transform.SetParent (position, false);
				tempItem.transform.localPosition = Vector3.zero;
				
				return true;
			}
		}
		return false;
	}

	//! Actions for when the State starts.
	/*! Set the Camera inside the state to be Active, overlaying the Main Camera used at InGameState,
     * close all dialogs that might be enabled. */
	public override void OnStartRun ()
	{
		cameraState.enabled = true;
		cameraState.GetComponent<AudioListener> ().enabled = true;
		cameraState.depth = 2;
		HudText.SetText("");
		returnPosition ();
	}
	
	//! Actions for when the State ends.
	/*! Disable the Camera inside the state, deactivate. */
	public override void OnStopRun ()
	{
		gameController.closeAlert ();
		cameraState.depth = -1;
		cameraState.enabled = false;
		cameraState.GetComponent<AudioListener> ().enabled = false;


		if (spatula != null) 
			spatula.OnStopRun ();

		if (pipette != null)
			pipette.OnStopRun ();

		if (washBottle != null)
			washBottle.OnStopRun ();
		
			stateUIManager.CloseAll ();

		
	}

	public override void ExitState(){
		GetComponentInParent<WorkBench>().OnStopRun();
		interactBox.GetComponent<BoxCollider>().enabled = true;
		gameController.ChangeState(0);
	}

	public override EquipmentControllerBase GetEquipmentController () {
		return equipmentController;
	}
}
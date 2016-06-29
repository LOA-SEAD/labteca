using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

//! It's what defines if a state may be used to make reactions	
/*! Work as a bridge for the player-equipments interactions.
 	It also manages how this interaction is going to be. */

public class WorkBench : MonoBehaviour {

	public Transform positionGlassEquipament;   /*!< Position of Glassware on the Precision Scale. */
	public Transform[] positionGlass;
	public int i = 0;

	public bool CannotEndState;					//The player is holding a filled tool, so can't quit the state

	public AudioSource soundBeaker;				/*!< Audio for the workbench. */

	public UI_Manager uiManager;                /*!< The UI Manager Game Object. */

	public GameController gameController;
	public GameStateBase currentState;
	
	private ButtonObject[] tools;
	private bool canClickTools;

	public GameObject lastItemSelected;
	
	//Tools on the workbench
	public Pipette pipette;
	public Spatula spatula;
	public WashBottle washBottle;
	public GlassStick glassStick;

	//Error box of different reagents
	public GameObject differentReagentErrorBox; //


	// TODO: revisar esse hardcode maroto aqui, talvez separar em outros scripts para cada funcionalidade da balanca?
	//spatula
	public bool selectSpatula;
	public float porcentErrorSpatula = 5;
	public Text textDisplayAmountSpatula;
	public float amountSelectedSpatula;
	public int typeSpatulaSelected = 1;
	public CursorMouse spatulaCursor;
	public CursorMouse spatulaReagentCursor;
	//Estas duas variaves sao usadas para o texto dependendo se ele esta usando a balança ou nao
	public Text textExactValue;		//When the spatula is used for a glassware in the precision scale, the amount should exact
	public Text textVariableValue;	//Otherwise the amount is variable
	public Button confirmAddButton;
	public Button confirmRemoveButton;
	private bool usePrecision;
	public Compound heldInSpatula;  //Reagent being held by the spatula
	//
	private string lastReagentName;

	//pipeta
	public bool selectPipeta;
	public float amountSelectedPipeta;
	public CursorMouse pipetaCursor;
	//public ButtonObject pipetaCursor;
	public CursorMouse pipetaReagentCursor;
	//public ButtonObject pipetaReagentCursor;
	public Slider pipetaValue;
	public Text pipetaValueText;
	public Compound heldInPipette; //Reagent being held by the pipette
	//
	
	//water
	public bool selectWater;
	public CursorMouse waterCursor;
	//public ButtonObject waterCursos;
	public float amountSelectedWater;
	public Slider waterValue;
	public Text waterValueText;
	public float porcentErrorWater;
	//

	//! Change the mouse cursor.
	/*! Set a cursor when mouse hover an object and set it back to default when not. */
	// TODO: essa funcao teoricamente tambem existe dentro do script ButtonObject, verificar codigo redundante.
	[System.Serializable]
	public class CursorMouse{
		public Texture2D cursorTexture;
		public Vector2 hotSpot = Vector2.zero;
		
		public void CursorEnter()
		{
			CursorManager.SetNewCursor(cursorTexture, hotSpot);
		}
		
		public void CursorExit()
		{
			CursorManager.SetCursorToDefault();
		}
	}
	
	public void Start () {
		RefreshInteractiveItens ();
		DeactivateInteractObjects ();
	}

	// TODO: se for alterado o modo de interacao com objetos na cena para Raycast, isso provavelmente vai ter de ser alterado.
	private void RefreshInteractiveItens(){
		tools = GetComponentsInChildren<ButtonObject> ();
		
		foreach(ButtonObject t in tools){
			t.GetComponent<Transform>().parent.GetComponent<Canvas>().worldCamera = currentState.camera;
		}
		
	}

	void Update(){

		if(gameController.alertDialog.IsShowing() || !canClickTools){
			DeactivateInteractObjects();
		}
		else{
			ActiveInteractObjects();
			
		}
	}

	// TODO: funcao que estava dentro do script UI_ObjectManager e foi jogada aqui. Poderia usar o script já feito e não fazer redundancia no codigo.
	// Desactive tah errado >.< refatorar isso se for continuar usando aqui.
	private void DeactivateInteractObjects(){
		foreach(ButtonObject t in tools){
			if(t!=null)
				t.GetComponent<Transform>().parent.GetComponent<Canvas>().enabled = false;
		}
	}
	
	// TODO: funcao que estava dentro do script UI_ObjectManager e foi jogada aqui. Poderia usar o script já feito e não fazer redundancia no codigo.
	private void ActiveInteractObjects(){
		foreach(ButtonObject t in tools){
			if(t!=null)
				t.GetComponent<Transform>().parent.GetComponent<Canvas>().enabled = true;
		}
	}

	//! Actions for when the State starts.
	/*! Set the Camera inside the state to be Active, overlaying the Main Camera used at InGameState,
     * close all dialogs that might be enabled. */
	public void OnStartRun ()
	{
/*		CloseSpatulaDialog(false);
		CloseOptionDialogReagent();
		CloseOptionDialogGlass();
		CloseOptionDialogWater ();
		CloseOptionDialogPipeta ();
		CloseOptionDialogGlassTable ();*/
		canClickTools = true;
		returnPosition ();
	}

	//! Actions for when the State ends.
	/*! Disable the Camera inside the state, deactivate. */
	public void OnStopRun ()
	{
		/*if(selectSpatula || selectPipeta || selectWater){
			UnselectAll();
		}
		amountSelectedSpatula = 0;*/
		DeactivateInteractObjects ();

		spatula.OnStopRun ();
		pipette.OnStopRun ();
		washBottle.OnStopRun ();

		gameObject.GetComponent<StateUIManager> ().CloseAll ();

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
			gameController.sendAlert("O equipamento ja tem um recipiente!");
			return false;
		}

		if(lastItemSelected.transform.parent == positionGlassEquipament){
			gameController.sendAlert("O equipamento ja esta na bancada");
			return false;
		}

		GameObject tempGlass = lastItemSelected.gameObject;
		i--;
		tempGlass.transform.SetParent(positionGlassEquipament,false);
		tempGlass.transform.localPosition = Vector3.zero;
		GetComponent<EquipmentControllerBase>().AddObjectInEquipament(tempGlass);
		tempGlass.GetComponent<Glassware>().SetStateInUse(currentState);

		RefreshInteractiveItens ();
		return true;
	}

	public bool PutItemFromInventory(ItemInventoryBase item) {
		foreach(Transform position in positionGlass) {
			if(position.childCount == 0){
				if(!item.physicalObject){
					GameObject tempItem = Instantiate(item.itemBeingHeld.gameObject) as GameObject;
					tempItem.transform.SetParent (position, false);
					tempItem.transform.localPosition = Vector3.zero;
					if(tempItem.GetComponent<ReagentPot>()!=null){
						if(item.reagent.Length!=0){
							Compound reagent = CompoundFactory.GetInstance ().GetCompound (item.reagent);

							if(tempItem.GetComponent<ReagentPot>().isSolid)
								tempItem.GetComponent<ReagentPot>().reagent.setValues(reagent as Compound);
							else
								tempItem.GetComponent<ReagentPot>().reagent.setValues(reagent as Compound);
						}
					}
				}else{
					GameObject tempItem = GameObject.Find(item.index);
					tempItem.SetActive(true);
					tempItem.transform.SetParent (position, false);
					tempItem.transform.localPosition = Vector3.zero;
				}
				//Debug.Log(item.compounds[0].Name);
				return true;
			}
		}
		Debug.Log ("erro");
		gameController.sendAlert("A Bancada esta cheia!");
		return false;
	}

	public bool PutItemFromEquip(GameObject tempItem){
		foreach(Transform position in positionGlass) {
			if(position.childCount == 0){
				tempItem.transform.SetParent (position, false);
				tempItem.transform.localPosition = Vector3.zero;
				GetComponent<EquipmentControllerBase>().RemoveObjectInEquipament(tempItem);
				return true;
			}
		}
		Debug.Log ("erro");
		gameController.sendAlert("A Bancada esta cheia!");
		return false;
	}

	//! Verify if there is any Glassware on the equipment.
	public bool HaveGlassInEquipment(){
		if(positionGlassEquipament.childCount > 0){
			return true;
		}
		return false;
	}

}
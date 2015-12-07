using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

//! Puts the reagents in slots and mix the reagents. (on WorkBench)
/*! */
public class WorkBench : MonoBehaviour {

	public Transform positionGlassEquipament;   /*!< Position of Glassware on the Precision Scale. */
	public Transform[] positionGlass;
	public int i = 0;

	public AudioSource soundBeaker;				/*!< Audio for the workbench. */

	public UI_Manager uiManager;                /*!< The UI Manager Game Object. */
	public GameObject optionDialogGlass;        /*!< Dialog. */
	public GameObject optionDialogGlassTable;   /*!< Dialog. */
	public GameObject optionDialogReagent;      /*!< Dialog. */
	public GameObject optionDialogSpatula;      /*!< Dialog. */
	public GameObject optionDialogWater;        /*!< Dialog. */
	public GameObject optionDialogPipeta;       /*!< Dialog. */
	public GameObject optionDialogEquipment;    /*!< Dialog. */ //Should only be linke when there's an equipment in the state

	public GameController gameController;
	public GameStateBase currentState;
	
	private ButtonObject[] tools;
	private bool canClickTools;

	public GameObject lastItemSelected;

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
	public ReagentsBaseClass heldInSpatula;  //Reagent being held by the spatula
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
	public ReagentsLiquidClass heldInPipette; //Reagent being held by the pipette
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
			CursorManager.SetDefaultCursor();
		}
	}
	
	public void Start () {
		RefreshInteractiveItens ();
		DesactiveInteractObjects ();
	}

	// TODO: se for alterado o modo de interacao com objetos na cena para Raycast, isso provavelmente vai ter de ser alterado.
	private void RefreshInteractiveItens(){
		tools = GetComponentsInChildren<ButtonObject> ();
		
		foreach(ButtonObject t in tools){
			t.GetComponent<Transform>().parent.GetComponent<Canvas>().worldCamera = currentState.camera;
		}
		
	}

	void Update(){

		if(uiManager.alertDialog.IsShowing() || !canClickTools){
			DesactiveInteractObjects();
		}
		else{
			ActiveInteractObjects();
			
		}
	}

	// TODO: funcao que estava dentro do script UI_ObjectManager e foi jogada aqui. Poderia usar o script já feito e não fazer redundancia no codigo.
	// Desactive tah errado >.< refatorar isso se for continuar usando aqui.
	private void DesactiveInteractObjects(){
		foreach(ButtonObject t in tools){
			t.GetComponent<Transform>().parent.GetComponent<Canvas>().enabled = false;
		}
	}
	
	// TODO: funcao que estava dentro do script UI_ObjectManager e foi jogada aqui. Poderia usar o script já feito e não fazer redundancia no codigo.
	private void ActiveInteractObjects(){
		foreach(ButtonObject t in tools){
			t.GetComponent<Transform>().parent.GetComponent<Canvas>().enabled = true;
		}
	}

	//! Actions for when the State starts.
	/*! Set the Camera inside the state to be Active, overlaying the Main Camera used at InGameState,
     * close all dialogs that might be enabled. */
	public void OnStartRun ()
	{
		CloseSpatulaDialog(false);
		CloseOptionDialogReagent();
		CloseOptionDialogGlass();
		CloseOptionDialogWater ();
		CloseOptionDialogPipeta ();
		CloseOptionDialogGlassTable ();
		returnPosition ();
	}

	//! Actions for when the State ends.
	/*! Disable the Camera inside the state, deactivate. */
	public void OnStopRun ()
	{
		if(selectSpatula || selectPipeta || selectWater){
			UnselectAll();
		}
		amountSelectedSpatula = 0;
		DesactiveInteractObjects ();	
	}

	//TODO: metodo temporario na ausencia do inventario
	public void CallPutBequer(){
		if(gameController.totalBeakers > 0){
			OpenOptionDialogGlass();
		}
		else{
			uiManager.alertDialog.ShowAlert("Sem Bequer no inventario!");
		}
	}

	//TODO: todos esses OpenDialogX e CloseDialogX podem ser uma funcao cada, ou ateh mesmo uma funcao que use parametros.
	/* Sugestao: Se cada objeto tiver seu proprio dialog, chamar essas funcoes pelo Raycast e passar o dialog como parametro pra ser exibido,
     * se achar melhor usar uma funcao soh com dois parametros: (GameObject dialog, bool show), tambem pode funcionar.
     * 
     * Alguns metodos possuem verificacoes que sao feitas na hora de abrir o dialog ou durante sua exibicao, essas coisas podem ser feitas em scripts
     * dentro dos proprios objetos, modularizando o codigo, sendo que o BalanceState fica apenas como gerenciador dos valores ou algo assim. 
     * Os dialogs nao precisariam nem serem implementados aqui, basta usar o UI_Manager -> especializar os scripts e nao fazer um codigo macarronico.
     */
	
	// ------------------------------------- comeca aqui ---------------------------------------------------------------------
	public void OpenOptionDialogGlass(){
		optionDialogGlass.SetActive(true);
		canClickTools = false;
	}
	public void OpenOptionDialogGlassTable(){
		optionDialogGlassTable.SetActive(true);
		canClickTools = false;
	}
	
	public void CloseOptionDialogGlass(){
		optionDialogGlass.SetActive(false);
		canClickTools = true;
	} 
	
	public void CloseOptionDialogGlassTable(){
		optionDialogGlassTable.SetActive(false);
		canClickTools = true;
	} 
	
	public void OpenOptionDialogReagent(){
		optionDialogReagent.SetActive(true);
		canClickTools = false;
	}
	
	public void CloseOptionDialogReagent(){
		optionDialogReagent.SetActive(false);
		canClickTools = true;
	}
	
	public void OpenOptionDialogWater(float maxVolumeGlassware){
		optionDialogWater.SetActive(true);
		optionDialogWater.GetComponentInChildren<Slider> ().maxValue = maxVolumeGlassware;
		canClickTools = false;
		DeselectWater ();
		waterValue.value = 0;
	}
	
	public void CloseOptionDialogWater(){
		optionDialogWater.SetActive(false);
		canClickTools = true;
	} 
	
	public void OpenOptionDialogPipeta(float currentVolumeUsed){
		optionDialogPipeta.SetActive(true);
		optionDialogPipeta.GetComponentInChildren<Slider> ().maxValue = currentVolumeUsed;
		canClickTools = false;
		DeselectPipeta ();
		pipetaValue.value = 0;
	}
	
	public void CloseOptionDialogPipeta(){
		optionDialogPipeta.SetActive(false);
		canClickTools = true;
	}  
	
	public void OpenSpatulaDialog(bool useToRemove){
		optionDialogSpatula.SetActive(true);
		spatulaCursor.CursorExit();
		canClickTools = false;
		amountSelectedSpatula = 0;
		textDisplayAmountSpatula.text = "0";
		if(usePrecision){
//			textExactValue.gameObject.SetActive(true);
			textVariableValue.gameObject.SetActive(false);
		}
		else{
//			textExactValue.gameObject.SetActive(false);
			textVariableValue.gameObject.SetActive(true);
		}
		
		if(useToRemove){
			confirmRemoveButton.gameObject.SetActive(true);
			confirmAddButton.gameObject.SetActive(false);
		}
		else{
			confirmAddButton.gameObject.SetActive(true);
			confirmRemoveButton.gameObject.SetActive(false);
		}
	}
	
	public void CloseSpatulaDialog(bool reset){
		optionDialogSpatula.SetActive(false);
		if(reset)
			amountSelectedSpatula = 0;
		canClickTools = true;
	}
	// ------------------------------------- termina aqui ---------------------------------------------------------------------

	//! Put the Glassware on the table.
	/*! Verifiy each position available and let the Player choose an available, if any, position to put the glassware. */
	// TODO: Revisar este codigo maroto aqui, tem coisas muito identicas que poderiam ser funcoes menores, ou talvez feito de maneira melhor?
	public void PutGlassOnTable(bool realocate){
		if (returnPosition()==null) {
			uiManager.alertDialog.ShowAlert ("A Bancada esta cheia!");
			CloseOptionDialogGlassTable ();
		} else {
			if (!realocate) {
				if (returnPosition().childCount == 0) {
					Debug.Log(returnPosition().name);
					GameObject tempGlass = Instantiate (gameController.selectedGlassWare.gameObject, returnPosition().position, gameController.selectedGlassWare.transform.rotation) as GameObject;
					tempGlass.transform.SetParent (returnPosition(), false);
					tempGlass.transform.localPosition = Vector3.zero;
					tempGlass.GetComponent<Glassware> ().SetStateInUse (currentState);
					i++;
				}
				soundBeaker.Play ();
				CloseOptionDialogGlass ();
			}
			else { i = 0;
					if (lastItemSelected.transform.parent == positionGlassEquipament) {
						//lastGlassWareSelected.transform.SetParent (null);
					gameObject.GetComponent<EquipmentControllerBase>().RemoveObjectInEquipament(lastItemSelected.gameObject);
						
					} else {
						lastItemSelected.transform.SetParent (null);
					} i = 0;
					if (returnPosition().childCount == 0) {
						GameObject tempGlass = lastItemSelected.gameObject;
						tempGlass.transform.SetParent (returnPosition(), true);
						tempGlass.transform.localPosition = Vector3.zero;
						tempGlass.GetComponent<Glassware> ().SetStateInUse (currentState);
						i++;
					}

				CloseOptionDialogGlassTable ();
			}
		}
	}




	//! Remove the Glassware.
	public void RemoveGlass(bool inInventory){
		
		if(inInventory){
			//TODO: metodo temporario pela a ausencia do inventario
			gameController.totalBeakers--;
		}
		CloseOptionDialogGlass();
	}
	
	//! Verify if there is any Glassware on the table.
	public bool HaveGlassInTable(){
		if(i == positionGlass.Length){
			return true;
		}
		return false;
	}

	// TODO: Codigo para controle da espatula, talvez modularizar e colocar o script no GameObject da espatula
	//Spatula//////////////////////////////////////////////////////////////
	//! Set the approximate that the spatule will be taking. 
	public void SetAmountReagentSpatula(bool Increase){
		
		if (amountSelectedSpatula < 0)
			amountSelectedSpatula = 0;
		
		textDisplayAmountSpatula.text = amountSelectedSpatula.ToString();
		
		if(typeSpatulaSelected == 1){
			if(Increase)
				amountSelectedSpatula += Random.Range(1,3);
			else
				amountSelectedSpatula -= Random.Range(1,3);
		}
		else if(typeSpatulaSelected == 2){
			if(Increase)
				amountSelectedSpatula += Random.Range(4,8);
			else
				amountSelectedSpatula -= Random.Range(4,8);
		}
		else if(typeSpatulaSelected == 3){
			if(Increase)
				amountSelectedSpatula += Random.Range(9,12);
			else
				amountSelectedSpatula -= Random.Range(9,12);
		}
		else if(typeSpatulaSelected == 4){
			if(Increase)
				amountSelectedSpatula += Random.Range(13,15);
			else
				amountSelectedSpatula -= Random.Range(13,15);
		}
		
		if (amountSelectedSpatula < 0)
			amountSelectedSpatula = 0;
		
		textDisplayAmountSpatula.text = amountSelectedSpatula.ToString();
		
	}
	//! Select which spatula will be used.
	public void SelectTypeSpatula(int typeNumber){
		typeSpatulaSelected = typeNumber;
		
	}
	
	//! Define the ammount that spatula took.
	/*! This is done so the spatula isn't precise, it will only be precise if you're using the scale itself, 
     * not taking the solid reagent directly from the inventory. */
	public void DefineAmountSpatule(bool useToRemove){
		if (usePrecision) {
			CloseSpatulaDialog (false);
		
		} else {
			float currentError = amountSelectedSpatula * porcentErrorSpatula / 100;
			if ((int)(Time.time) % 2 == 0)
				amountSelectedSpatula -= currentError;
			else
				amountSelectedSpatula += currentError;
		
			CloseSpatulaDialog (false);
			if (!useToRemove) {
				spatulaReagentCursor.CursorEnter ();
			}
		}
		if (!lastItemSelected.GetComponent<Glassware> () == null) {  //Interactions Spatula-Glassware
			if (useToRemove)
				lastItemSelected.GetComponent<Glassware>().RemoveSolid (amountSelectedSpatula);	
		}
	}
	//Spatula//////////////////////////////////////////////////////////////

	// TODO: Codigo para controle da Pisseta, se for decidido modularizar, deve ser refatorado isso.
	//Water//////////////////////////////////////////////////////////////
	//! Define the approximate amount of water that will be used.
	/*! The Wash Bottle cant give any precise quantity of water so an approximate value is calculated and added. */
	public void DefineAmountWater(){
		
		float currentError = amountSelectedWater*porcentErrorWater/100;
		if((int)(Time.time) % 2 == 0)
			amountSelectedWater -= currentError;
		else
			amountSelectedWater += currentError;
		
		if (!(lastItemSelected.GetComponent<Glassware>() == null))
			lastItemSelected.GetComponent<Glassware>().AddLiquid (amountSelectedWater);
		amountSelectedWater = 0;
		CloseOptionDialogWater ();
	}
	
	//! Set the approximate amount of water.
	public void SetAmountWater(){
		amountSelectedWater = waterValue.value;
		waterValueText.text = amountSelectedWater.ToString ();
	}
	//Water//////////////////////////////////////////////////////////////
	
	//TODO: Pipeta, caso seja realizada a modularizacao dos componentes, aqui comeca o codigo da pisseta.
	//Pipeta//////////////////////////////////////////////////////////////
	//! Define the precise amount of water.
	public void DefineAmountPipeta(){
		CloseOptionDialogPipeta ();

		if (amountSelectedPipeta > 0) {
			pipetaReagentCursor.CursorEnter ();
			selectPipeta = true;
			if(!(lastItemSelected.GetComponent<ReagentsLiquidClass>() == null))
				heldInPipette = lastItemSelected.GetComponent<ReagentsLiquidClass>();
			if (!(lastItemSelected.GetComponent<Glassware> () == null)) //Only removes from the last selected object if it's a glassware
				lastItemSelected.GetComponent<Glassware>().RemoveLiquid (amountSelectedPipeta);
		}
	}
	//! Set current amount of water inside pipette.
	public void SetAmountPipeta(){
		amountSelectedPipeta = pipetaValue.value;
		pipetaValueText.text = amountSelectedPipeta.ToString ();
	}
	//Pipeta//////////////////////////////////////////////////////////////
	
	// TODO: mais uma vez codigo 'especializado' que poderia usar funcao generica, ou modularizado dentro dos respectivos objetos.
	public void SelectSpatula(){
		UnselectAll ();
		selectSpatula = true;
		spatulaCursor.CursorEnter();
	}
	
	public void SelectPipeta(){
		UnselectAll ();
		selectPipeta = true;
		pipetaCursor.CursorEnter();
	}
	
	public void SelectWater(){
		UnselectAll ();
		selectWater = true;
		waterCursor.CursorEnter();
	}
	
	public void DeselectPipeta(){
		selectPipeta = false;
		pipetaCursor.CursorExit();
		pipetaReagentCursor.CursorExit ();
		amountSelectedPipeta = 0;
	}
	
	public void DeselectWater(){
		selectWater = false;
		waterCursor.CursorExit();
		amountSelectedPipeta = 0;
	}
	
	public void DeselectSpatula(){
		selectSpatula = false;
		spatulaCursor.CursorExit();
		spatulaReagentCursor.CursorExit ();
		amountSelectedSpatula = 0;
	}
	
	private void UnselectAll(){
		DeselectPipeta ();
		DeselectWater ();
		DeselectSpatula ();
	}

	//! Click Glass.
	public void ClickGlass(GameObject glassClick){

		Glassware glass = glassClick.GetComponent<Glassware> ();
		lastItemSelected = glassClick;
		
		if (selectWater&&glassClick.GetComponent<Glassware>().volume-glassClick.GetComponent<Glassware>().currentVolumeUsed>0) {
			
			OpenOptionDialogWater(glassClick.GetComponent<Glassware>().volume-glassClick.GetComponent<Glassware>().currentVolumeUsed);
			
		}
		else if(selectPipeta){
			
			if(amountSelectedPipeta > 0){
				if(!(heldInPipette == null)) {
					glass.AddLiquid(heldInPipette.molarMass, amountSelectedPipeta);
					DeselectPipeta();
					heldInPipette = null;
				} else {
					glass.AddLiquid(amountSelectedPipeta);
					DeselectPipeta();
				}
				
			}
			else if(glass.liquid.activeSelf == true){
				OpenOptionDialogPipeta(glassClick.GetComponent<Glassware>().currentVolumeUsed);
			}
			
		}
		else if(selectSpatula){
			
			if((float)(glass.GetComponent<Rigidbody>().mass) == (float)(glass.mass) && amountSelectedSpatula == 0){
				uiManager.alertDialog.ShowAlert("Esse recipiente nao tem reagente solido");				
			}
			else{ 
				
				if((float)(glass.GetComponent<Rigidbody>().mass) != (float)(glass.mass) && 
				   GetComponent<ScaleController>().GetGlassInEquipament() == glass && 
				   glass.liquid.activeSelf == true &&
				   amountSelectedSpatula > 0){
					
					uiManager.alertDialog.ShowAlert("Voce nao pode fazer essa mistura no equipamento");	
				}
				else {
					
					if((float)(glass.GetComponent<Rigidbody>().mass) != (float)(glass.mass) && amountSelectedSpatula == 0){
						if(glass.liquid.activeSelf == false) {
							
							if(glass.transform.parent == positionGlassEquipament){
								usePrecision = true;
							}
							else{
								usePrecision = false;
							}
							
							OpenSpatulaDialog(true);
						}
					}
					else {
						if(amountSelectedSpatula > 0){
							glass.AddSolid(amountSelectedSpatula, lastReagentName);
						}
						
						
					}
				}
			}
			
			DeselectSpatula();
			
		}
		else{
			OpenOptionDialogGlassTable();
		}
	}

	//! Clicked in a Solid Reagent
	public void ClickSolidReagent(GameObject solidClick){

		lastItemSelected = solidClick;
		ReagentsBaseClass solid = solidClick.GetComponent<ReagentsBaseClass> ();

		if(selectSpatula){
			usePrecision = false;
			OpenSpatulaDialog(false);
			DeselectSpatula();
		} else{
			OpenOptionDialogReagent();
		}
	}

	//! Clicked in a Liquid Reagent
	public void ClickLiquidReagent (GameObject liquidClick) {

		lastItemSelected = liquidClick;
		ReagentsLiquidClass liquid = liquidClick.GetComponent<ReagentsLiquidClass> ();
	
		if (selectPipeta) {
			
			if (amountSelectedPipeta > 0) {

				DeselectPipeta ();
				
			}
			OpenOptionDialogPipeta (300);
			
		} else
			OpenOptionDialogReagent ();
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
	public void PutGlassInEquip(bool realocate){
		if(positionGlassEquipament.childCount > 0){
			uiManager.alertDialog.ShowAlert("O equipamento ja tem um recipiente!");
		}
		else{
			if(!realocate){
				//TODO: Temporariamente esta pegando do gamecontroller, mas tem que pegar do inventario esses dados
				GameObject tempGlass = Instantiate(gameController.selectedGlassWare.gameObject, positionGlassEquipament.position, gameController.selectedGlassWare.transform.rotation) as GameObject;
				tempGlass.transform.SetParent(positionGlassEquipament,false);
				tempGlass.transform.localPosition = Vector3.zero;
				gameController.totalBeakers--;
				GetComponent<ScaleController>().AddObjectInEquipament(tempGlass);
				tempGlass.GetComponent<Glassware>().SetStateInUse(currentState);
			}
			else{
				
				if(lastItemSelected.transform.parent == positionGlassEquipament){
					uiManager.alertDialog.ShowAlert("O equipamento ja Esta na bancada");
				}
				else{
					
					GameObject tempGlass = lastItemSelected.gameObject;
					i--;
					tempGlass.transform.SetParent(positionGlassEquipament,false);
					tempGlass.transform.localPosition = Vector3.zero;
					GetComponent<ScaleController>().AddObjectInEquipament(tempGlass);
					tempGlass.GetComponent<Glassware>().SetStateInUse(currentState);
				}
				
			}
			
		}
		
		CloseOptionDialogGlass();
		CloseOptionDialogGlassTable ();
		RefreshInteractiveItens ();
	}

	//! Put an item on the table from the inventory
	/*! Verifiy if there's an available position to put the item */
	/*public void PutItemFromInventory(GameObject item) {
		foreach(Transform position in positionGlass) {	//The first position available
			if(position.childCount == 0){
				item.transform.SetParent (position, true);
				item.transform.localPosition = Vector3.zero;
				return;
			}
		}
		uiManager.alertDialog.ShowAlert ("A Bancada esta cheia!");
	}*/
	public void PutItemFromInventory(ItemToInventory item, ItemToInventory data) {
		foreach(Transform position in positionGlass) {	//The first position available
			if(position.childCount == 0){
				GameObject tempItem = Instantiate(item.gameObject/*, position.position/*, gameController.selectedGlassWare.transform.rotation*/) as GameObject;
				tempItem.transform.SetParent (position, /*true*/false);
				tempItem.transform.localPosition = Vector3.zero;
				if(tempItem.GetComponent<ReagentsBaseClass>()!=null){
					if(tempItem.GetComponent<ReagentsBaseClass>().isSolid)
						tempItem.GetComponent<ReagentsBaseClass>().receiveValues(data as ReagentsBaseClass);
					else
						tempItem.GetComponent<ReagentsBaseClass>().receiveValues(data as ReagentsLiquidClass);
				}
				return;
			}
		}
		uiManager.alertDialog.ShowAlert ("A Bancada esta cheia!");
	}

	//! Verify if there is any Glassware on the equipment.
	public bool HaveGlassInEquipment(){
		if(positionGlassEquipament.childCount > 0){
			return true;
		}
		return false;
	}

	public void OpenOptionDialogEquipment(){
		optionDialogEquipment.SetActive(true);
		canClickTools = false;
	}
	
	public void CloseOptionDialogEquipment(){
		optionDialogEquipment.SetActive(false);
		canClickTools = true;
	}


}



	//Used for reactions. Integrate this into the class

	/*public GameObject slot0;
	public GameObject slot1;

	private string slot0Type;
	private string slot1Type;

	public GameObject slot0Position;
	public GameObject slot1Position;
	public GameObject finalSlotPosition;

	public GameObject buttonMix;

	public GameObject resultPrefab;
	public Vector3 resultPrefabScale;


	public bool resultMake = false;
	
	public Vector3 inventoryScale;
	
	public Vector3 inventoryColliderSize;
	public Vector3 inventoryColliderPosition;


	private InventoryController inventory;*/
	// Use this for initialization
	//! Deactivates buttonMix and returns object of type inventory.
	/*! */
	/*void Start () 
	{
		buttonMix.SetActive (false);
		inventory = FindObjectOfType (typeof(InventoryController)) as InventoryController;
	}
	
	// Update is called once per frame
	//! Activates/Deactivates buttonMix
	void Update () 
	{
		if(slot0 != null && slot1 != null)
		{
			buttonMix.SetActive(true);
		}
		else
		{
			buttonMix.SetActive(false);
		}
	}

	//! Puts the reagents in slots.
	//*! */
	/*public void useSlot(ReagentsLiquid liquid, ReagentsSolid solid)
	{
		if(slot0 == null)
		{
			if(liquid != null)
			{
				slot0 = liquid.gameObject;
				slot0.transform.position = slot0Position.transform.position;
				slot0Type = "liquid";
			}
			if(solid != null)
			{
				slot0 = solid.gameObject;
				slot0.transform.position = slot0Position.transform.position;
				slot0Type = "solid";
			}

			slot0.transform.localScale = resultPrefabScale;
		}
		else if(slot1 == null)
		{
			if(liquid != null)
			{
				slot1 = liquid.gameObject;
				slot1.transform.position = slot0Position.transform.position;
				slot1Type = "liquid";
			}
			if(solid != null)
			{
				slot1 = solid.gameObject;
				slot1.transform.position = slot0Position.transform.position;
				slot1Type = "solid";
			}

			slot1.transform.localScale = resultPrefabScale;
		}
		else
		{
			Debug.LogWarning("all slots used");
		}
	}
	//!Mix the reagents.
	/*! */
	/*public void Mix()
	{
		//Result of mix
		if((slot0Type == "liquid" && slot1Type == "solid" )||(slot1Type == "liquid" && slot0Type == "solid" ))
		{
			ReagentsLiquid result;
			GameObject resultGo = Instantiate(resultPrefab) as GameObject;
			result = resultGo.GetComponent<ReagentsLiquid>();
			//result.transform.position = finalSlotPosition.transform.position;
			result.transform.localScale = resultPrefabScale;

			//If liquid, sets the volume 
			if(slot0Type == "liquid")
			{
				result.volume = slot0.GetComponent<ReagentsLiquid>().volume;
			}
			else
			{
				result.volume = slot1.GetComponent<ReagentsLiquid>().volume;
			}



			float mass;
			//If solid, sets the mass 
			if(slot0Type == "solid")
			{
				result.GetInfo(slot0.GetComponent<ReagentsSolid>().liquidName);
				mass = slot0.GetComponent<ReagentsSolid>().totalMass;
				Debug.Log(slot0.GetComponent<ReagentsSolid>().liquidName);
			}
			else
			{
				result.GetInfo(slot1.GetComponent<ReagentsSolid>().liquidName);
				mass = slot1.GetComponent<ReagentsSolid>().totalMass;
			}

			result.concentration = (mass/result.info.molarMass);//*result.volume;

			resultMake = true;


			Destroy(slot0);
			Destroy(slot1);

			result.transform.parent = inventory.transform;
			result.transform.localScale = inventoryScale;
			BoxCollider col = result.collider as BoxCollider;
			col.size = inventoryColliderSize;
			col.center = inventoryColliderPosition;

			inventory.AddReagentLiquid(result);
		}
	}
}*/

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public  class BalanceState : GameStateBase {

    public Camera cameraState;
	public GameObject interactBox;

	public Transform positionGlass1;
	public Transform positionGlass2;
	public Transform positionGlass3;
	public Transform positionGlassEquipament;

    public UI_Manager uiManager;
	public GameObject optionDialogGlass;
	public GameObject optionDialogGlassTable;
	public GameObject optionDialogReagent;
	public GameObject optionDialogSpatula;
	public GameObject optionDialogWater;
	public GameObject optionDialogPipeta;
	public GameObject optionDialogBalance;

	public Hint hints;

	private ButtonObject[] tools;
	private bool canClickTools;

	//spatula
	public bool selectSpatula;
	public float porcentErrorSpatula = 5;
	public Text textDisplayAmountSpatula;
	public float amountSelectedSpatula;
	public int typeSpatulaSelected = 1;
	public CursorMouse spatulaCursor;
	public CursorMouse spatulaReagentCursor;
	//Estas duas variaves sao usadas para o texto dependendo se ele esta usando a balança ou nao
	public Text textExactValue;
	public Text textVariableValue;
	public Button confirmAddButton;
	public Button confirmRemoveButton;
	private bool usePrecision;
	private Glassware lastGlassWareSelected;
	//
	private string lastReagentName;

	//pipeta
	public bool selectPipeta;
	public float amountSelectedPipeta;
	public CursorMouse pipetaCursor;
	public CursorMouse pipetaReagentCursor;
	public Slider pipetaValue;
	public Text pipetaValueText;
	//

	//water
	public bool selectWater;
	public CursorMouse waterCursor;
	public float amountSelectedWater;
	public Slider waterValue;
	public Text waterValueText;
	public float porcentErrorWater;
	//

	[System.Serializable]
	public class Hint{
		public Text hintText;
		public string[] hintMessages;

		public void ShowHint(int idHint){
			if(hintMessages[idHint] != null)
				hintText.text = hintMessages[idHint];
		}
		public void HideHint(){
			hintText.text = "";
		}
	}

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

	

	// Use this for initialization
	public void Start () {
		cameraState.gameObject.SetActive(false);

		RefreshInteractiveItens ();

		DesactiveInteractObjects ();

	}
	
	protected override void UpdateState ()
	{

	}

	private void RefreshInteractiveItens(){
		tools = GetComponentsInChildren<ButtonObject> ();

		foreach(ButtonObject t in tools){
			t.GetComponent<Transform>().parent.GetComponent<Canvas>().worldCamera = cameraState;
		}
		
	}

	void Update(){
		base.Update();

		if(!canRun)
			return;

		if(Input.GetKeyDown(KeyCode.Escape)){
			if(selectSpatula || selectPipeta || selectWater){
				UnselectAll();
			}
			else{
				interactBox.GetComponent<BoxCollider>().enabled = true;
				gameController.ChangeState(0);
				FadeScript.instance.ShowFade();
				hints.HideHint();
				amountSelectedSpatula = 0;
			}

		}

		if(uiManager.alertDialog.IsShowing() || !canClickTools){
			DesactiveInteractObjects();
		}
		else{
			ActiveInteractObjects();

		}

	

	}

	private void DesactiveInteractObjects(){
		foreach(ButtonObject t in tools){
			t.GetComponent<Transform>().parent.GetComponent<Canvas>().enabled = false;
		}
	}

	private void ActiveInteractObjects(){
		foreach(ButtonObject t in tools){
			t.GetComponent<Transform>().parent.GetComponent<Canvas>().enabled = true;
		}
	}

	public override void OnStartRun ()
	{
        cameraState.gameObject.SetActive(true);
        cameraState.depth = 2;
        HudText.SetText("");
		CloseSpatulaDialog(false);
		CloseOptionDialogReagent();
		CloseOptionDialogGlass();
		CloseOptionDialogWater ();
		CloseOptionDialogPipeta ();
		CloseOptionDialogBalance ();
		CloseOptionDialogGlassTable ();

	}
	
	public override void OnStopRun ()
	{
        cameraState.depth = -1;
        cameraState.gameObject.SetActive(false);
		DesactiveInteractObjects ();

	}

	//metodo temporario na ausencia do inventario
	public void CallPutBequer(){
		if(gameController.totalBackers > 0){
			OpenOptionDialogGlass();
		}
		else{
            uiManager.alertDialog.ShowAlert("Sem Bequer no inventario!");
		}
	}

	//metodo temporario na ausencia do inventario
	public void CallPutReagent(){
		if(gameController.haveReagentNaCl){
			if(HaveGlassInTable() || HaveGlassInEquipament()){

				if(selectSpatula){
					OpenSpatulaDialog(false);
					//temporario.
					lastReagentName = "NaCl";
				}
				else{
                    uiManager.alertDialog.ShowAlert("Selecione a espatula para manusear o reagente");
				}
				
			}
			else {
                uiManager.alertDialog.ShowAlert("Voce nao tem recipente na bancada ou equipamento");
			}
		}
		else{
            uiManager.alertDialog.ShowAlert("Sem Reagente no inventario!");
		}
	}



	public void OpenOptionDialogBalance(){
		optionDialogBalance.SetActive(true);
		canClickTools = false;
	}
	
	public void CloseOptionDialogBalance(){
		optionDialogBalance.SetActive(false);
		canClickTools = true;
	}


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


	public void OpenOptionDialogWater(){
		optionDialogWater.SetActive(true);
		canClickTools = false;
		DeselectWater ();
		waterValue.value = 0;
	}
	
	public void CloseOptionDialogWater(){
		optionDialogWater.SetActive(false);
		canClickTools = true;
	} 

	public void OpenOptionDialogPipeta(){
		optionDialogPipeta.SetActive(true);
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
			textExactValue.gameObject.SetActive(true);
			textVariableValue.gameObject.SetActive(false);
		}
		else{
			textExactValue.gameObject.SetActive(false);
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


	public void PutGlassInTable(bool realocate){
		if(positionGlass1.childCount > 0 && 
		   positionGlass2.childCount > 0 &&
		   positionGlass3.childCount > 0)
        {
            uiManager.alertDialog.ShowAlert("A Bancada esta cheia!");
			CloseOptionDialogGlassTable();
		}
		else{
			if(!realocate){

					if(positionGlass1.childCount == 0){

						//Temporariamente esta pegando do gamecontroller, mas tem que pegar do inventario esses dados
						GameObject tempGlass = Instantiate(gameController.selectedGlassWare.gameObject, positionGlass1.position, gameController.selectedGlassWare.transform.rotation) as GameObject;
						tempGlass.transform.SetParent(positionGlass1,false);
						tempGlass.transform.localPosition = Vector3.zero;
						tempGlass.GetComponent<Glassware>().SetStateInUse(this);
						gameController.totalBackers--;
					}
					else if(positionGlass2.childCount == 0){
						//Temporariamente esta pegando do gamecontroller, mas tem que pegar do inventario esses dados
						GameObject tempGlass = Instantiate(gameController.selectedGlassWare.gameObject, positionGlass2.position, gameController.selectedGlassWare.transform.rotation) as GameObject;
						tempGlass.transform.SetParent(positionGlass2,false);
						tempGlass.transform.localPosition = Vector3.zero;
						tempGlass.GetComponent<Glassware>().SetStateInUse(this);
						gameController.totalBackers--;

					}
					else{

						//Temporariamente esta pegando do gamecontroller, mas tem que pegar do inventario esses dados
						GameObject tempGlass = Instantiate(gameController.selectedGlassWare.gameObject, positionGlass3.position, gameController.selectedGlassWare.transform.rotation) as GameObject;
						tempGlass.transform.SetParent(positionGlass3,false);
						tempGlass.transform.localPosition = Vector3.zero;
						tempGlass.GetComponent<Glassware>().SetStateInUse(this);
						gameController.totalBackers--;

					}


				CloseOptionDialogGlass();
			}
			else{

				if(lastGlassWareSelected.transform.parent == positionGlass1 ||
				   lastGlassWareSelected.transform.parent == positionGlass2 ||
				   lastGlassWareSelected.transform.parent == positionGlass3)
                {
                       uiManager.alertDialog.ShowAlert("Esse recipiente ja esta na bancada");
				}
				else{

					if(lastGlassWareSelected.transform.parent == positionGlassEquipament){
						lastGlassWareSelected.transform.SetParent(null);
						GetComponent<BalanceController>().RemoveObjectInEquipament(lastGlassWareSelected.gameObject);

					}
					else{
						lastGlassWareSelected.transform.SetParent(null);
					}



					if(positionGlass1.childCount == 0){
						GameObject tempGlass = lastGlassWareSelected.gameObject;
						tempGlass.transform.SetParent(positionGlass1,true);
						tempGlass.transform.localPosition = Vector3.zero;
						tempGlass.GetComponent<Glassware>().SetStateInUse(this);
					}
					else if(positionGlass2.childCount == 0){
						GameObject tempGlass = lastGlassWareSelected.gameObject;
						tempGlass.transform.SetParent(positionGlass2,true);
						tempGlass.transform.localPosition = Vector3.zero;
						tempGlass.GetComponent<Glassware>().SetStateInUse(this);

					}
					else if(positionGlass3.childCount == 0){

						GameObject tempGlass = lastGlassWareSelected.gameObject;
						tempGlass.transform.SetParent(positionGlass3,true);
						tempGlass.transform.localPosition = Vector3.zero;
						tempGlass.GetComponent<Glassware>().SetStateInUse(this);

						
					}

				}
				CloseOptionDialogGlassTable();

			}

				

				
		}


		RefreshInteractiveItens ();

	}

	public void PutGlassInEquip(bool realocate){
		if(positionGlassEquipament.childCount > 0){
            uiManager.alertDialog.ShowAlert("O equipamento ja tem um recipiente!");
		}
		else{
			if(!realocate){
				//Temporariamente esta pegando do gamecontroller, mas tem que pegar do inventario esses dados
				GameObject tempGlass = Instantiate(gameController.selectedGlassWare.gameObject, positionGlassEquipament.position, gameController.selectedGlassWare.transform.rotation) as GameObject;
				tempGlass.transform.SetParent(positionGlassEquipament,false);
				tempGlass.transform.localPosition = Vector3.zero;
				gameController.totalBackers--;
				GetComponent<BalanceController>().AddObjectInEquipament(tempGlass);
				tempGlass.GetComponent<Glassware>().SetStateInUse(this);
			}
			else{

				if(lastGlassWareSelected.transform.parent == positionGlassEquipament){
                    uiManager.alertDialog.ShowAlert("O equipamento ja Esta na bancada");
				}
				else{

					GameObject tempGlass = lastGlassWareSelected.gameObject;
					tempGlass.transform.SetParent(positionGlassEquipament,false);
					tempGlass.transform.localPosition = Vector3.zero;
					GetComponent<BalanceController>().AddObjectInEquipament(tempGlass);
					tempGlass.GetComponent<Glassware>().SetStateInUse(this);
				}
				
			}
				
		}
		
		CloseOptionDialogGlass();
		CloseOptionDialogGlassTable ();
		RefreshInteractiveItens ();
	}

	public void RemoveGlass(bool inInventory){

		if(inInventory){
			//metodo temporario pela a ausencia do inventario
			gameController.totalBackers--;
		}

		CloseOptionDialogGlass();
		
	}



	public bool HaveGlassInTable(){
		if(positionGlass1.childCount > 0 || 
		   positionGlass2.childCount > 0 ||
		   positionGlass3.childCount > 0){
			return true;
		}
		return false;
	}

	public bool HaveGlassInEquipament(){
		if(positionGlassEquipament.childCount > 0){
			return true;
		}

		return false;
	}

	//Spatule//////////////////////////////////////////////////////////////
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

	public void SelectTypeSpatula(int typeNumber){
		typeSpatulaSelected = typeNumber;

	}

	public void DefineAmountSpatule(bool useToRemove){
	
		if(usePrecision){
			CloseSpatulaDialog(false);

		}
		else{
			float currentError = amountSelectedSpatula*porcentErrorSpatula/100;
			if((int)(Time.time) % 2 == 0)
				amountSelectedSpatula -= currentError;
			else
				amountSelectedSpatula += currentError;

			CloseSpatulaDialog(false);
			if(!useToRemove){
				spatulaReagentCursor.CursorEnter();
				hints.ShowHint(0);
			}
		}

		if(useToRemove){
			lastGlassWareSelected.RemoveSolid(amountSelectedSpatula);

		}
	}

	//Spatule//////////////////////////////////////////////////////////////


	//Water//////////////////////////////////////////////////////////////
	public void DefineAmountWater(){

		float currentError = amountSelectedWater*porcentErrorWater/100;
		if((int)(Time.time) % 2 == 0)
			amountSelectedWater -= currentError;
		else
			amountSelectedWater += currentError;


		lastGlassWareSelected.AddLiquid (amountSelectedWater);
		amountSelectedWater = 0;
		CloseOptionDialogWater ();
	}

	public void SetAmountWater(){
		amountSelectedWater = waterValue.value;
		waterValueText.text = amountSelectedWater.ToString ();
	}
	//Water//////////////////////////////////////////////////////////////

	//Pipeta//////////////////////////////////////////////////////////////
	public void DefineAmountPipeta(){
		CloseOptionDialogPipeta ();
		if (amountSelectedPipeta > 0) {
			pipetaReagentCursor.CursorEnter ();
			selectPipeta = true;
			lastGlassWareSelected.RemoveLiquid (amountSelectedPipeta);
		}
}
	
	public void SetAmountPipeta(){
		amountSelectedPipeta = pipetaValue.value;
		pipetaValueText.text = amountSelectedPipeta.ToString ();
	}
	//Pipeta//////////////////////////////////////////////////////////////


	public void SelectSpatula(){
		UnselectAll ();
		selectSpatula = true;
		spatulaCursor.CursorEnter();
		hints.ShowHint(1);
	}

	public void SelectPipeta(){
		UnselectAll ();
		selectPipeta = true;
		pipetaCursor.CursorEnter();
		hints.ShowHint(2);
	}

	public void SelectWater(){
		UnselectAll ();
		selectWater = true;
		waterCursor.CursorEnter();
		hints.ShowHint(3);
	}

	public void DeselectPipeta(){
		selectPipeta = false;
		pipetaCursor.CursorExit();
		pipetaReagentCursor.CursorExit ();
		hints.HideHint();
		amountSelectedPipeta = 0;
	}

	public void DeselectWater(){
		selectWater = false;
		waterCursor.CursorExit();
		hints.HideHint();
		amountSelectedPipeta = 0;
	}

	public void DeselectSpatula(){
		selectSpatula = false;
		spatulaCursor.CursorExit();
		spatulaReagentCursor.CursorExit ();
		hints.HideHint();
		amountSelectedSpatula = 0;
	}

	private void UnselectAll(){
		DeselectPipeta ();
		DeselectWater ();
		DeselectSpatula ();
	}

	public void ClickGlass(GameObject glassClick){

		Glassware glass = glassClick.GetComponent<Glassware> ();
		lastGlassWareSelected = glassClick.GetComponent<Glassware> ();

		if (selectWater) {

			OpenOptionDialogWater();

		}
		else if(selectPipeta){

			if(amountSelectedPipeta > 0){
				glass.AddLiquid(amountSelectedPipeta);
				DeselectPipeta();

			}
			else if(glass.liquid.activeSelf == true){
				OpenOptionDialogPipeta();
			}

		}
		else if(selectSpatula){

			if((float)(glass.GetComponent<Rigidbody>().mass) == (float)(glass.mass) && amountSelectedSpatula == 0){
                uiManager.alertDialog.ShowAlert("Esse recipiente nao tem reagente solido");				
			}
			else{ 

				if((float)(glass.GetComponent<Rigidbody>().mass) != (float)(glass.mass) && 
			        GetComponent<BalanceController>().GetGlassInEquipament() == glass && 
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


}

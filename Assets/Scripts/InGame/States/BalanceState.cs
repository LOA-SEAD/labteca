using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
//! State for Precision Scale.
/*! This state has all the behaviour that controls the precision scale and it's UI.
 */
// TODO: mudar nome da classe de Balance ("Equilibrio") para Scale >.<
public  class BalanceState : GameStateBase {

    // camera and interactive area
    public Camera cameraState;                  /*!< Camera for this state. */
    public GameObject interactBox;              /*!< BoxCollider that allows the Player to enter this state. */
    // glassware positioning
    public Transform positionGlass1;            /*!< Position of first Glassware. */
    public Transform positionGlass2;            /*!< Position of second Glassware. */
    public Transform positionGlass3;            /*!< Position of third Glassware. */
    public Transform positionGlassEquipament;   /*!< Position of Glassware on the Precision Scale. */

	public AudioSource soundBeaker;
    // UI
    // TODO: mudanca em como a UI funciona dentro da balanca
    /* Sugestao: talvez faca mais sentido os dialogs estarem dentro dos objetos em si, exemplo: o dialog da pisseta
     * ser um filho da pisseta ou seu prefab estar atrelado ao script da pisseta e ele eh passado por referencia para
     * uma funcao aqui do BalanceState (ou de um UI_Manager).
     */
    public UI_Manager uiManager;                /*!< The UI Manager Game Object. */
    public GameObject optionDialogGlass;        /*!< Dialog. */
    public GameObject optionDialogGlassTable;   /*!< Dialog. */
    public GameObject optionDialogReagent;      /*!< Dialog. */
    public GameObject optionDialogSpatula;      /*!< Dialog. */
    public GameObject optionDialogWater;        /*!< Dialog. */
    public GameObject optionDialogPipeta;       /*!< Dialog. */
    public GameObject optionDialogBalance;      /*!< Dialog. */

	public Hint hints;

	private ButtonObject[] tools;
	private bool canClickTools;

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

    //! Hint of what the player should do next.
    /*! Display a text message telling the player what he can/should do. */
    // TODO: nao consegui fazer com que essa funcao fosse executada durante o jogo, teoricamente eh uma ajuda ao jogador. Verificar.
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
		cameraState.gameObject.SetActive(false);
		RefreshInteractiveItens ();
		DesactiveInteractObjects ();
	}
	
	protected override void UpdateState ()
	{

	}

    // TODO: se for alterado o modo de interacao com objetos na cena para Raycast, isso provavelmente vai ter de ser alterado.
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

    //! Actions for when the State ends.
    /*! Disable the Camera inside the state, deactivate. */
	public override void OnStopRun ()
	{
        cameraState.depth = -1;
        cameraState.gameObject.SetActive(false);
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

	//TODO: metodo temporario na ausencia do inventario
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

    //TODO: todos esses OpenDialogX e CloseDialogX podem ser uma funcao cada, ou ateh mesmo uma funcao que use parametros.
    /* Sugestao: Se cada objeto tiver seu proprio dialog, chamar essas funcoes pelo Raycast e passar o dialog como parametro pra ser exibido,
     * se achar melhor usar uma funcao soh com dois parametros: (GameObject dialog, bool show), tambem pode funcionar.
     * 
     * Alguns metodos possuem verificacoes que sao feitas na hora de abrir o dialog ou durante sua exibicao, essas coisas podem ser feitas em scripts
     * dentro dos proprios objetos, modularizando o codigo, sendo que o BalanceState fica apenas como gerenciador dos valores ou algo assim. 
     * Os dialogs nao precisariam nem serem implementados aqui, basta usar o UI_Manager -> especializar os scripts e nao fazer um codigo macarronico.
     */

    // ------------------------------------- comeca aqui ---------------------------------------------------------------------
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
    // ------------------------------------- termina aqui ---------------------------------------------------------------------

    //! Put the Glassware on the table.
    /*! Verifiy each position available and let the Player choose an available, if any, position to put the glassware. */
    // TODO: Revisar este codigo maroto aqui, tem coisas muito identicas que poderiam ser funcoes menores, ou talvez feito de maneira melhor?
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

					//TODO: Temporariamente esta pegando do gamecontroller, mas tem que pegar do inventario esses dados
					GameObject tempGlass = Instantiate(gameController.selectedGlassWare.gameObject, positionGlass1.position, gameController.selectedGlassWare.transform.rotation) as GameObject;
					tempGlass.transform.SetParent(positionGlass1,false);
					tempGlass.transform.localPosition = Vector3.zero;
					tempGlass.GetComponent<Glassware>().SetStateInUse(this);
					gameController.totalBeakers--;
				}
				else if(positionGlass2.childCount == 0){						//TODO: Temporariamente esta pegando do gamecontroller, mas tem que pegar do inventario esses dados
					GameObject tempGlass = Instantiate(gameController.selectedGlassWare.gameObject, positionGlass2.position, gameController.selectedGlassWare.transform.rotation) as GameObject;
					tempGlass.transform.SetParent(positionGlass2,false);
					tempGlass.transform.localPosition = Vector3.zero;
					tempGlass.GetComponent<Glassware>().SetStateInUse(this);
					gameController.totalBeakers--;

				}
				else{
				
					//TODO: Temporariamente esta pegando do gamecontroller, mas tem que pegar do inventario esses dados
					GameObject tempGlass = Instantiate(gameController.selectedGlassWare.gameObject, positionGlass3.position, gameController.selectedGlassWare.transform.rotation) as GameObject;
					tempGlass.transform.SetParent(positionGlass3,false);
					tempGlass.transform.localPosition = Vector3.zero;
					tempGlass.GetComponent<Glassware>().SetStateInUse(this);
					gameController.totalBeakers--;

				}
				soundBeaker.Play ();
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
		if(positionGlass1.childCount > 0 || 
		   positionGlass2.childCount > 0 ||
		   positionGlass3.childCount > 0){
			return true;
		}
		return false;
	}

    //! Verify if there is any Glassware on the equipment.
    // TODO: refatorar Equipament para Equipment. 
	public bool HaveGlassInEquipament(){
		if(positionGlassEquipament.childCount > 0){
			return true;
		}
		return false;
	}

    // TODO: Codigo para controle da espatula, talvez modularizar e colocar o script no GameObject da espatula
	//Spatule//////////////////////////////////////////////////////////////
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


		lastGlassWareSelected.AddLiquid (amountSelectedWater);
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
			lastGlassWareSelected.RemoveLiquid (amountSelectedPipeta);
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

    // TODO: nao ha nenhuma referencia disso em outro codigo, tambem nao achei dentro da cena onde esta utilizando.
    //! Click Glass.
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

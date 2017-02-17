using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class LIAState : GameStateBase {
	
	// camera and interactive area
	//public Camera cameraState;                  /*!< Camera for this state. */
	public GameObject interactiveCanvas;		//Interactive canvas of the state

	public GameObject checkCompoundClassCanvas;	//CheckBox Canvas for compound class check
	public GameObject checkWhatCompoundCanvas;	//TextBox Canvas for checking what compound
	public GameObject checkMolarityValueCanvas;	//TextBox canvas for molarity value check
	public GameObject checkGlasswareCanvas;		//Interactive canvas for glassware check
	private int checkBoxSelected;				//Variable to hold the checkbox that was selected

	public ProgressController progressController; //Instance of ProgressController
	
	public GameObject correctAnswer;			//Canvas with the answer for the correct results
	public GameObject wrongAnswer;				//Canvas with the answer for the wrong results   TODO:Take this out. It's only here for SBGames

	public Light LIALight;
	public Image glassware,solid,liquid;

	public string currentIndex;

	private bool up;
	private InventoryManager inventory;
	private Sprite originalSprite;
	public void Start () {
		cameraState.gameObject.SetActive(false);
		interactiveCanvas.SetActive (false);
		inventory = GameObject.Find ("InventoryManager").GetComponent<InventoryManager> ();
		originalSprite = glassware.sprite;

		correctAnswer.SetActive (false);
		wrongAnswer.SetActive (false);

		ResultVerifier.GetInstance ();
		progressController = GameObject.Find ("ProgressController").GetComponent<ProgressController> ();
	}
	
	protected override void UpdateState ()
	{
		
	}

	public bool ReceiveProduct(ItemInventoryBase item,GameObject obj){
		if (currentIndex.Length == 0) {
			if (obj != null) {
				currentIndex = item.index;
				glassware.sprite = item.icon.sprite;
				if(item.solid.IsActive()){
					solid.sprite = item.solid.sprite;
					solid.color = new Color32(255,255,255,255);
				}
				if(item.liquid.IsActive()){
					liquid.sprite = item.liquid.sprite;
					liquid.color = new Color32(255,255,255,255);
				}
				return true;
			} else {
				Debug.Log ("n recebi");
				return false;
			}
		} else {
			gameController.sendAlert("Já tem um produto");
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
			gameController.sendAlert("Não há nenhum produto");
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
		if(Input.GetKeyDown(KeyCode.E)){
			ExitState();
		}
	}
	
	//! Actions for when the State starts.
	// Set the Camera inside the state to be Active, overlaying the Main Camera used at InGameState,
    // close all dialogs that might be enabled.
	public override void OnStartRun ()
	{
		GameObject.Find ("InventoryManager").GetComponent<InventoryManager> ().changeList (0);
		cameraState.gameObject.SetActive(true);
		cameraState.depth = 2;
		interactiveCanvas.SetActive (true);
		HudText.SetText("");
	}
	
	//! Actions for when the State ends.
	/*! Disable the Camera inside the state, deactivate. */
	public override void OnStopRun ()
	{
		this.RetrieveProduct ();
		interactBox.GetComponent<BoxCollider>().enabled = true;

		gameController.closeAlert ();
		LIALight.intensity = 0f;
		cameraState.depth = -1;
		cameraState.gameObject.SetActive(false);
		interactiveCanvas.SetActive (false);
	}

	public override void ExitState(){
		gameController.ChangeState(0);
	}

	//! Called when the verification action is triggered
	// Calls upon the ResultVerifier class to compare the content of the Glassware with the
	// expected result for the actual phase
	public void VerifyPhase() {
		Debug.Log ("Pressing button");
		if (currentIndex.Length > 0) { //Case there is a Glassware being verified
			if ((GameObject.Find (currentIndex).GetComponent<Glassware> ()).content != null) { //Case there is a content
				//TODO: The parameter is always 0 as it's checking only the first phase. It needs to change for the correct value when phases are changing correctly
				if (ResultVerifier.GetInstance ().VerifyResult (0, (GameObject.Find (currentIndex).GetComponent<Glassware> ()).content)) { //Case Product is correct

					//gameController.sendAlert ("Resultado correto! Parabéns!");
					correctAnswer.SetActive(true);

				} else {	//Case Product is NOT correct
					//gameController.sendAlert ("Resultado incorreto.");
					wrongAnswer.SetActive(true);
				}
			} else {	//Case the Glassware is empty (it won't actually happen)
				gameController.sendAlert ("A vidraria está vazia!");
			}
		} else {	//Case there's no Glassware selected
			gameController.sendAlert("Não há nenhum produto para verificar!");
		}
	}

	/// <summary>
	/// Called when the verification action is triggered
	/// </summary>
	/// Calls upon the ResultVerifier class to compare the content of the Glassware with the
	/// expected result for the actual phase
	public void VerifyPhase(int overload) {
		switch (progressController.StepType) {
		case TypeOfStep.CompoundClass:
			checkCompoundClassCanvas.SetActive (false);
			ResultVerifier.GetInstance().VerifyCheckBox(checkBoxSelected);
			break;
		case TypeOfStep.WhatCompound:
			checkWhatCompoundCanvas.SetActive(false);
			//ResultVerifier.GetInstance().VerifyTextBox();
			break;
		case TypeOfStep.MolarityCheck:
			checkMolarityValueCanvas.SetActive(false);
			//ResultVerifier.GetInstance().VerifyTextBox();
			break;
		case TypeOfStep.GlasswareCheck:
			checkGlasswareCanvas.SetActive(false);
			ResultVerifier.GetInstance().VerifyGlassware(GameObject.Find (currentIndex).GetComponent<Glassware>());
			break;
		}
	}

	/// <summary>
	/// Sets in a variable which checkbox was selected.
	/// </summary>
	/// <returns>The index of the box.</returns>
	public void TextBoxAnswer(int answer) {
		checkBoxSelected = answer;
	}

	//Ending animation TODO:sThis was created as a finisher for the SBGames Version
	public void EndGame() {
		FadeScript.instance.FadeIn ();

		#if UNITY_EDITOR
		Application.LoadLevel ("Menu");
		#else
		Application.LoadLevel ("Menu");
		#endif
	}

	/// <summary>
	/// Sets the verification interface based on the type of the current step.
	/// </summary>
	/// <param name="type">The type of the current step</param>
	public void SetVerificationInterface(TypeOfStep type){ //TODO: check in the future if it should receive this indeed, or just use ProgressController.StepType;
		switch (type) {
		case TypeOfStep.CompoundClass:
			checkCompoundClassCanvas.SetActive (true);
			break;
		case TypeOfStep.WhatCompound:
			checkWhatCompoundCanvas.SetActive(true);
			break;
		case TypeOfStep.MolarityCheck:
			checkMolarityValueCanvas.SetActive(true);
			break;
		case TypeOfStep.GlasswareCheck:
			checkGlasswareCanvas.SetActive(true);
			break;
		}
	}
}

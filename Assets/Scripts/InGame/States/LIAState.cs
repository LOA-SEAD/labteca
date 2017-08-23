using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class LIAState : GameStateBase {
	
	// camera and interactive area
	//public Camera cameraState;                  /*!< Camera for this state. */
	public GameObject interactiveCanvas;		//Interactive canvas of the state

	public GameObject checkCompoundClassCanvas;	//CheckBox Canvas for compound class check
	private int checkBoxSelected;				//Variable to hold the checkbox that was selected
	public GameObject checkWhatCompoundCanvas;	//TextBox Canvas for checking what compound
	public Text whatCompoundAnswer;				//Text of WhatCompoundCanvas answer
	public GameObject checkMolarityValueCanvas;	//TextBox canvas for molarity value check
	public Text molarityAnswer;					//Text of MolarityCheckCanvas answer
	public GameObject checkGlasswareCanvas;		//Interactive canvas for glassware check
	public Button verifyButton;

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

		verifyButton.interactable = false;
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
				verifyButton.interactable = true;
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
			verifyButton.interactable = false;
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
		if(Input.GetKeyDown(KeyCode.Escape)){
			ExitState();
		}
	}
	
	//! Actions for when the State starts.
	// Set the Camera inside the state to be Active, overlaying the Main Camera used at InGameState,
    // close all dialogs that might be enabled.
	public override void OnStartRun ()
	{
		//Opens inventory TODO: is it still going to be always open?
		GameObject.Find ("InventoryManager").GetComponent<InventoryManager> ().changeList (0);
		cameraState.gameObject.SetActive(true);
		cameraState.depth = 2;

		Debug.Log ("Starting LIA state");
		//Set active the interaction canvas accordingly to the type of step
		switch (progressController.StepType) {
		case TypeOfStep.CompoundClass:
			checkCompoundClassCanvas.SetActive (true);
			checkCompoundClassCanvas.GetComponentInChildren<ToggleGroup>().SetAllTogglesOff();
			break;
		case TypeOfStep.WhatCompound:
			whatCompoundAnswer.text = "";
			GameObject.Find ("GameController").GetComponent<HUDController> ().LockKeys (true);
			checkWhatCompoundCanvas.SetActive(true);
			break;
		case TypeOfStep.MolarityCheck:
			molarityAnswer.text = "";
			GameObject.Find ("GameController").GetComponent<HUDController> ().LockKeys (true);
			checkMolarityValueCanvas.SetActive(true);
			break;
		case TypeOfStep.GlasswareCheck:
			checkGlasswareCanvas.SetActive(true);
			break;
		}
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
		//interactiveCanvas.SetActive (false); TODO:Take this out
		this.CloseAll ();
	}

	/// <summary>
	/// Closes all the interactive canvas.
	/// </summary>
	public void CloseAll() {
		checkCompoundClassCanvas.SetActive (false);
		checkGlasswareCanvas.SetActive (false);
		checkMolarityValueCanvas.SetActive (false);
		checkWhatCompoundCanvas.SetActive (false);
		GameObject.Find ("GameController").GetComponent<HUDController> ().LockKeys (false);
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
	/// The method is called when the verification action is triggered
	/// </summary>
	/// Calls upon the ResultVerifier class to compare the content of the Glassware with the
	/// expected result for the actual phase.
	/// The call is made by the different methods associated with the different types of steps
	public void VerifyStep() {
		bool complete = false;

		switch (progressController.StepType) {
		case TypeOfStep.CompoundClass:
			checkCompoundClassCanvas.SetActive (false);
			complete = ResultVerifier.GetInstance().VerifyCheckBox(checkBoxSelected);
			break;
		case TypeOfStep.WhatCompound:
			string compoundAnswer = whatCompoundAnswer.text;
			checkWhatCompoundCanvas.SetActive(false);
			GameObject.Find ("GameController").GetComponent<HUDController> ().LockKeys (false);
			complete = ResultVerifier.GetInstance().VerifyTextBox(compoundAnswer);
			break;
		case TypeOfStep.MolarityCheck:
			string molAnswer = molarityAnswer.text;
			checkMolarityValueCanvas.SetActive(false);
			GameObject.Find ("GameController").GetComponent<HUDController> ().LockKeys (false);
			complete = ResultVerifier.GetInstance().VerifyTextBox(molAnswer);
			break;
		case TypeOfStep.GlasswareCheck:
			if (currentIndex.Length > 0) { //Case there is a Glassware being verified
				checkGlasswareCanvas.SetActive(false);
				complete = ResultVerifier.GetInstance().VerifyGlassware(GameObject.Find (currentIndex).GetComponent<Glassware>());
			}
			break;
		}
		//Completes the step or shows the wrong-answer animation
		//TODO: Add verification is currentIndex.Length > 0
		if (complete == true) {
			progressController.CompleteStep();
		} else {
			progressController.WrongAnswer();
		}
	}

	/// <summary>
	/// Sets in a variable which checkbox was selected.
	/// </summary>
	/// <returns>The index of the box.</returns>
	public void CheckBoxAnswer(int answer) {
		Debug.Log ("Opçao selecionada = " + answer);
		checkBoxSelected = answer;
	}

	//Ending animation TODO:This was created as a finisher for the SBGames Version
	public void EndGame() {
		FadeScript.instance.FadeIn ();

		#if UNITY_EDITOR
		Application.LoadLevel ("Menu");
		#else
		Application.LoadLevel ("Menu");
		#endif
	}
}

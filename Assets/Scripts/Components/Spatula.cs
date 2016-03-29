using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

//! Controls the spatulas
/*! Defines if the spatulas are being used, how much they are holding
 *	and integrates the interaction boxes. */

public class Spatula : MonoBehaviour {

	private float pinchesHeld; // Mass being held in the spatule [g]

	//Interaction boxes
	public UI_Manager uiManager;	/*!< The UI Manager Game Object. */
	

	//Interaction box
	public GameObject interactionBoxSpatula;	//Interaction box
	public Text spatulaValueText;				//Text showing the selected value
	public float pinchesSelected;				//Pinches selected during the interaction

	/*	public Button confirmAddButton;		is the spatula going to remove and add reagents?
	 *	public Button confirmRemoveButton;
	 */
	public int checkboxStartpoint;				//Startpoint for the interval that defines the amount of pinches
	public int checkboxEndpoint; 				//Endpoint for the interval that defines the amount of pinches

	public bool usingPrecision;					//If checked, the amount does not vary. Depends on the glassware being on the scale

	//For the cursor
	public Texture2D spatula_CursorTexture;
	public Texture2D filledSpatula_CursorTexture;
	public Vector2 hotSpot = Vector2.zero;
	
	public ReagentsBaseClass reagentInSpatula; //Reagent being held by the pipette

	// Use this for initialization
	void Start () {
		interactionBoxSpatula.SetActive (false);
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	//! Holds the events for when the interactive spatula on the Workbench is clicked
	void OnClick() {
		MouseState currentState = CursorManager.GetCurrentState ();
		
		switch (currentState) {
		case MouseState.ms_default: 		//Default -> Spatula: prepares the spatula for use
			CursorManager.SetMouseState(MouseState.ms_spatula);
			CursorManager.SetNewCursor(spatula_CursorTexture, hotSpot);
			break;
		case MouseState.ms_pipette: 		//Pipette -> Spatula: change to spatula state
			CursorManager.SetMouseState(MouseState.ms_spatula);
			CursorManager.SetNewCursor(spatula_CursorTexture, hotSpot);
			break;
		case MouseState.ms_filledPipette: 	// Filled Spatula -> Spatula: nothing
			break;
		case MouseState.ms_spatula: 		// Spatula -> Spatula: put back the spatula
			CursorManager.SetMouseState(MouseState.ms_default);
			CursorManager.SetCursorToDefault();
			break;
		case MouseState.ms_filledSpatula: 	// Filled Spatula -> Spatula: nothing
			break;
		case MouseState.ms_washBottle: 		// Washe Bottle -> Spatula: change to spatula state
			CursorManager.SetMouseState(MouseState.ms_spatula);
			CursorManager.SetNewCursor(spatula_CursorTexture, hotSpot);
			break;
		case MouseState.ms_glassStick:		// Glass Stick -> Spatula: change to spatula state
			CursorManager.SetMouseState(MouseState.ms_spatula);
			CursorManager.SetNewCursor(spatula_CursorTexture, hotSpot);
			break;
		case MouseState.ms_usingTool:  		// Unable to click somewhere else TODO:is it necessary?
			break;
		}
	}

	//! Close the interaction box
	public void CloseInteractionBox(){
		interactionBoxSpatula.SetActive(false);
	}
	//! Open the interaction box
	public void OpenInteractionBox() {
		interactionBoxSpatula.SetActive (true);
		//CursorManager.SetDefaultCursor ();
		/*
		 * DEFINE HOW TO BLOCK CLICKS OUTSIDE 
		 */
	}
	
	//! Used by the checkboxes on the canvas to set the range.
	/*! The startpoint and endpoint come to the fill/unfill spatula. The values HAVE to be defined on the canvas! */
	public void SetInterval (int startpoint, int endpoint) {
		checkboxStartpoint = startpoint;
		checkboxEndpoint = endpoint;
	}

	//! Is called on the event of a empty spatula clicking on a solid reagent
	/*! If the spatula is not using precision (on a scale, for example), then the amount taken by it is
	 	a value in a set interval. Otherwise, the value should be chosen with a slider?TODO*/
	public void SelectingPinches(bool increase) {
		/*
		 * CODE FILLING THE SPATULA
		 */
		/*if(precision)
		 *	use slider?
		 *else
		 *	use intervals
		 *
		 */
		 if(pinchesSelected < 0)
		 	pinchesSelected = 0;

		if (!usingPrecision) { //If the vessel is NOT on a scale

			if(increase)
				pinchesSelected += Random.Range(checkboxStartpoint, checkboxEndpoint);
			else
				pinchesSelected -= Random.Range(checkboxStartpoint, checkboxEndpoint);


			if(pinchesSelected < 0)
		 		pinchesSelected = 0;

			spatulaValueText.text = pinchesSelected.ToString();
		}
	}

	//! Uses the spatula to hold pinches of a solid reagent
	public void FillSpatula (ReagentsBaseClass reagent) {
		CloseInteractionBox();

		if(pinchesSelected > 0) {
			CursorManager.SetMouseState (MouseState.ms_filledSpatula);
			CursorManager.SetNewCursor (filledSpatula_CursorTexture, hotSpot);

			reagentInSpatula = reagent;
		}

		pinchesHeld = pinchesSelected;
		pinchesSelected = 0;
	}

	//! Unloads the spatula into a proper vessel
	/*! Called when the filled spatula clicks on a valid vessel for the reagent.
	 	In this overload, the vessel is a glassware */
	public void UnfillSpatula(bool usingPrecision, Glassware glassware) {
		/*
		 * CODE TO PUT THE CONTENT INTO THE GLASSWARE
		 */

		pinchesHeld = 0;
		reagentInSpatula = null;

		CursorManager.SetMouseState (MouseState.ms_default);
		CursorManager.SetCursorToDefault();
	}

	//! Unloads the spatula into a proper vessel
	/*! Called when the filled spatula clicks on a valid vessel for the reagent.
	 	In this overload, the vessel the pot of reagent used to get the pinches before */
	public void UnfillSpatula(/*ReagentsBaseClass reagentPot*/) {
	
		pinchesHeld = 0;
		reagentInSpatula = null;

		CursorManager.SetMouseState (MouseState.ms_default);
		CursorManager.SetCursorToDefault();
	}
}

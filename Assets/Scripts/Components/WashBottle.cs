using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

//! Controls the wash botlle
/*! Defines if the wash bottle is being used, and defines how much water they
 * 	are going to pour, integrating the interaction boxes . */

public class WashBottle : MonoBehaviour {


	//Interaction box and components
	public GameObject interactionBoxWashBottle;	//Interaction box
	public Slider boxSlider;  					//Interaction box's slider
	public Text washBottleValueText; 			//Text showing the slider's value
	public float volumeSelected;				//Amount selected in the slider

	//For the cursor
	public Texture2D washBottle_CursorTexture;
	public Vector2 hotSpot = Vector2.zero;

	// Use this for initialization
	void Start () {
		interactionBoxWashBottle.SetActive (false);
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	//! Holds the events for when the interactive wash bottle on the Workbench is clicked
	void OnClick() {
		MouseState currentState = CursorManager.GetCurrentState ();
		
		switch (currentState) {
		case MouseState.ms_default: 		//Default -> Wash Bottle: prepares the washe bottle for use
			CursorManager.SetMouseState(MouseState.ms_washBottle);
			CursorManager.SetNewCursor(washBottle_CursorTexture, hotSpot);
			break;
		case MouseState.ms_pipette: 		//Pipette -> Wash Bottle: change to wash bottle state
			CursorManager.SetMouseState(MouseState.ms_washBottle);
			CursorManager.SetNewCursor(washBottle_CursorTexture, hotSpot);
			break;
		case MouseState.ms_filledPipette: 	// Filled Spatula -> Wash Bottle: nothing
			break;
		case MouseState.ms_spatula: 		// Spatula -> Wash Bottle: change to wash bottle state
			CursorManager.SetMouseState(MouseState.ms_washBottle);
			CursorManager.SetNewCursor(washBottle_CursorTexture, hotSpot);
			break;
		case MouseState.ms_filledSpatula: 	// Filled Spatula -> Wash Bottle: nothing
			break;
		case MouseState.ms_washBottle: 		// Wash Bottle -> Wash Bottle: put back the wash bottle
			CursorManager.SetMouseState(MouseState.ms_default);
			CursorManager.SetCursorToDefault();
			break;
		case MouseState.ms_glassStick:		// Glass Stick -> Wash Bottle: change to wash bottle state
			CursorManager.SetMouseState(MouseState.ms_washBottle);
			CursorManager.SetNewCursor(washBottle_CursorTexture, hotSpot);
			break;
		case MouseState.ms_usingTool:  		// Unable to click somewhere else TODO:is it necessary?
			break;
		}
	}

	//! Close the interaction box
	public void CloseInteractionBox(){
		interactionBoxWashBottle.SetActive(false);
	}
	//! Open the interaction box
	public void OpenInteractionBox() {
		interactionBoxWashBottle.SetActive (true);
		//CursorManager.SetDefaultCursor ();
		/*
		 * DEFINE HOW TO BLOCK CLICKS OUTSIDE 
		 */
	}

	//! Set value of volume currently set by the slider.
	public void VolumeOnSlider(){ //BasicallyDone
		volumeSelected = boxSlider.value;
		washBottleValueText.text = volumeSelected.ToString ();
	}
	
	//! Pours the water into the vessel (mostly glasswares.
	public void PourWater() { //BasicallyDone
		CloseInteractionBox ();
		
		if (volumeSelected > 0.0f) {
			/*
			 * CODE MAKING GLASSWARE RECEIVE THE WATER
			 */
		}

		volumeSelected = 0.0f;
	}

}

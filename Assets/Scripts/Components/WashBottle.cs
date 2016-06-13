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

	public Compound Water = new Compound ("Water", "H2O", false, 18.01f, 1.0f, 1.0f, 1.0f, null, null, 7.0f, 1.0f, 1.0f, 1.0f, 1.0f, 0.0f, null); // = new Compound();			//Defining H2O reagent

	public Glassware interactingGlassware; 		//Glassware which the wash bottle is interacting with

	// Use this for initialization
	void Start () {
		interactionBoxWashBottle.SetActive (false);

	/*	Water.Name = "H2O";
		Water.IsSolid = false;
		Water.MolarMass = 18.01f;
		Water.Density = 1.0f;
		Water.Polarizability = 1.0f;
		Water.Conductibility = 1.0f;
		Water.Solubility = 1.0f;
		Water.irSpecter = null;
		Water.flameSpecter = null;
		Water.uvSpecter = null;
		Water.color = Color.blue;
		//Water.texture = null;
		Water.PH = 7.0f;
		Water.Turbidity = 1.0f;
		Water.Refratometer = 1.0f;
		Water.hplc = null;*/
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	//! Holds the events for when the interactive wash bottle on the Workbench is clicked
	public void OnClick() {
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

	public void OnStopRun() {
		CloseInteractionBox ();

		//TODO: Hide cursor
	}

	//! Close the interaction box
	//	Also resets all the values
	public void CloseInteractionBox(){
		interactionBoxWashBottle.SetActive(false);
		boxSlider.value = 0.0f;
		volumeSelected = 0.0f;
		interactingGlassware = null;
		CursorManager.SetMouseState(MouseState.ms_default);
		CursorManager.SetCursorToDefault();
	}
	//! Open the interaction box
	public void OpenInteractionBox(float maxSliderVolume) {
		interactionBoxWashBottle.SetActive (true);
		interactionBoxWashBottle.GetComponentInChildren<Slider> ().maxValue = maxSliderVolume;
		//CursorManager.SetDefaultCursor ();
		/*
		 * DEFINE HOW TO BLOCK CLICKS OUTSIDE 
		 */
	}

	//! The wash bottle is being put to work
	public void ActivateWashBottle(float valueForSlider, Glassware glassware) {
		this.OpenInteractionBox (valueForSlider);
		interactingGlassware = glassware;
	}

	//! Set value of volume currently set by the slider.
	public void VolumeOnSlider(){ //BasicallyDone
		volumeSelected = boxSlider.value;
		washBottleValueText.text = volumeSelected.ToString ();
	}
	
	//! Pours the water into the vessel (mostly glasswares).
	public void PourWater() { //BasicallyDone
		if (volumeSelected > 0.0f) {
			//interactingGlassware.PourLiquid(volumeSelected, volumeSelected * Water.Density, Water);
			interactingGlassware.IncomingReagent(Water, volumeSelected);
		}
		CloseInteractionBox ();
	}
}

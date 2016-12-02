using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

//! Controls the wash botlle
/*! Defines if the wash bottle is being used, and defines how much water they
 * 	are going to pour, integrating the interaction boxes . */

public class WashBottle : WorkbenchInteractive {


	//Interaction box and components
	public GameObject interactionBoxWashBottle;	//Interaction box
	public Slider boxSlider;  					//Interaction box's slider
	public Text washBottleValueText; 			//Text showing the slider's value
	public float volumeSelected;				//Amount selected in the slider
	public GameObject fillUpWithWater; 		//Interaction box for the WashBottle with precision glasses

	//For the cursor
	public Texture2D washBottle_CursorTexture;
	public Vector2 hotSpot;

	public static Compound Water = new Compound ("Water", "H2O", false, 18.01f, 1.0f, 1.0f, 1.0f, null, null, 7.0f, 1.0f, 1.0f, 1.0f, 1.0f, 0.0f, null, false); // = new Compound();			//Defining H2O reagent

	public Glassware interactingGlassware; 		//Glassware which the wash bottle is interacting with

	// Use this for initialization
	void Start () {
		hoverName = "Pisseta";
		interactionBoxWashBottle.SetActive (false);

		Water.MolarMass = Compound.waterMolarMass;
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
	public override void OnClick() {
		if (GameObject.Find ("GameController").GetComponent<GameController> ().GetCurrentState () !=
			GameObject.Find ("GameController").GetComponent<GameController> ().gameStates [0]) {
			MouseState currentState = CursorManager.GetCurrentState ();
		
			switch (currentState) {
			case MouseState.ms_default: 		//Default -> Wash Bottle: prepares the washe bottle for use
				OnStartInteraction ();
				break;
			case MouseState.ms_pipette: 		//Pipette -> Wash Bottle: change to wash bottle state
				OnStartInteraction ();
				break;
			case MouseState.ms_filledPipette: 	// Filled Spatula -> Wash Bottle: nothing
				break;
			case MouseState.ms_spatula: 		// Spatula -> Wash Bottle: change to wash bottle state
				OnStartInteraction ();
				break;
			case MouseState.ms_filledSpatula: 	// Filled Spatula -> Wash Bottle: nothing
				break;
			case MouseState.ms_washBottle: 		// Wash Bottle -> Wash Bottle: put back the wash bottle
				CursorManager.SetMouseState (MouseState.ms_default);
				CursorManager.SetCursorToDefault ();
				break;
			case MouseState.ms_interacting:  	// Unable to click somewhere else
				break;
			}
		}
	}

	public void OnStartInteraction() {
		CursorManager.SetMouseState(MouseState.ms_washBottle);
		CursorManager.SetNewCursor(washBottle_CursorTexture, hotSpot);
	}

	public void OnStopRun() {
		CloseInteractionBox ();
	}

	//! Close the interaction box
	//	Also resets all the values
	public void CloseInteractionBox(){
		interactionBoxWashBottle.SetActive(false);
		boxSlider.value = 0.0f;
		volumeSelected = 0.0f;
		interactingGlassware = null;

		fillUpWithWater.SetActive (false);

		if (CursorManager.GetCurrentState () == MouseState.ms_interacting) {
			CursorManager.SetMouseState(MouseState.ms_default);
			CursorManager.SetCursorToDefault();
		}
	}
	//! Open the interaction box
	public void OpenInteractionBox(float maxSliderVolume) {
		interactionBoxWashBottle.SetActive (true);
		interactionBoxWashBottle.GetComponentInChildren<Slider> ().maxValue = maxSliderVolume;
		CursorManager.SetMouseState(MouseState.ms_interacting);
	}

	//! Open the interaction box for precision Glassware
	public void OpenPrecisionGlassBox() {
		fillUpWithWater.SetActive (true);
		CursorManager.SetMouseState(MouseState.ms_interacting);
	}

	//! The wash bottle is being put to work
	public void ActivateWashBottle(float volumeLeft, Glassware glassware) {
		interactingGlassware = glassware;
		if (interactingGlassware.precisionGlass) {
			this.OpenPrecisionGlassBox();
		} else {
			this.OpenInteractionBox (volumeLeft);
		}
	}

	//! Set value of volume currently set by the slider.
	public void VolumeOnSlider(){ //BasicallyDone
		volumeSelected = boxSlider.value;
		washBottleValueText.text = volumeSelected.ToString ();
	}
	
	//! Pours the water into the vessel (mostly glasswares).
	public void PourWater() { //BasicallyDone
		if (interactingGlassware.precisionGlass) {
			interactingGlassware.IncomingReagent(Water, interactingGlassware.maxVolume - interactingGlassware.GetLiquidVolume());
		} else {
			if (volumeSelected > 0.0f) {
				//interactingGlassware.PourLiquid(volumeSelected, volumeSelected * Water.Density, Water);
				interactingGlassware.IncomingReagent (Water, volumeSelected);
			}
		}
		CloseInteractionBox ();
	}
}

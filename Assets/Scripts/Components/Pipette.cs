﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

//! Controls the pipette
/*! Defines if the pipettes are being used, how much they are holding
 *	and integrates the interaction boxes. */
//TODO: Needs to add the volumetric pipette
public class Pipette : MonoBehaviour {

	private float volumeHeld;			//Volume being held by the pipette [ml]
	private float maxVolume;			//Max volume the pipette can hold [ml]
	public bool graduated;				//Knows if the pipette being used is graduated or volumetric

	public UI_Manager uiManager;		// The UI Manager Game Object.

	//Interaction boxes to chose between graduated pipettes or volumetric pipettes
	public GameObject boxToChoosePipette;		//Interaction box to choose the pipette

	//Interaction box for graduated pipette
	public GameObject boxGraduatedFilling;		//Interaction box
	public Slider boxSlider;  					//Interaction box's slider
	public Text pipetteValueText; 				//Text showing the slider's value
	public float volumeSelected;				//Amount selected in the slider
	//Interaction box to unfill graduated pipette
	public GameObject boxGraduatedUnfilling;	//Interaction box to unfill pipette
	public Slider u_boxSlider;  				//Unfilling box's slider
	public Text u_pipetteValueText; 			//Text showing the slider's value
	public float u_volumeSelected;				//Amount selected in the slider

	//Interaction box for volumetric pipette
	//public GameObject boxVolumetricPipette;		//Interaction box for the volumetric pipette
	//public float volumetricPipetteChosen;		//Volume chosen in the interaction
	/*
	 */

	//For the cursor
	public Texture2D pipette_CursorTexture;
	public Texture2D filledPipette_CursorTexture;
	public Vector2 hotSpot = Vector2.zero;
	
	public ReagentsLiquidClass reagentInPipette; //Reagent being held by the pipette

	public Glassware interactingGlassware;		 //Glassware which the pipette is interacting with
	public ReagentsLiquidClass interactingReagent; //Reagent which the pipette is interacting with

	//! Use this for initialization
	void Start () {
		boxGraduatedFilling.SetActive (false);
	}
	
	//! Update is called once per frame
	void Update () {

	}

	//! Holds the events for when the interactive pipette on the Workbench is clicked
	public void OnClick() {
		MouseState currentState = CursorManager.GetCurrentState ();

		switch (currentState) {
		case MouseState.ms_default: 		//Default -> Pipette: prepares the pipette for use
			//ChoosePipetteBox
			OpenSelectingBox();
			break;
		case MouseState.ms_pipette: 		//Pipette -> Pipette: put back the pipette
			CursorManager.SetMouseState(MouseState.ms_default);
			CursorManager.SetCursorToDefault();
			break;
		case MouseState.ms_filledPipette: 	// Filled Pipette -> Pipette: nothing
			break;
		case MouseState.ms_spatula: 		// Spatula -> Piepette: change to pipette state
			Spatula spatula = GameObject.Find ("GameController").GetComponent<GameController> ().GetCurrentState ().GetComponent<WorkBench> ().spatula;
			spatula.OpenChooseBox();
			/*CursorManager.SetMouseState(MouseState.ms_pipette);
			CursorManager.SetNewCursor(pipette_CursorTexture, hotSpot);*/
			break;
		case MouseState.ms_filledSpatula: 	// Filled Spatula -> Pipette: nothing
			break;
		case MouseState.ms_washBottle: 		// Wash Bottle -> Pipette: change to pipette state
			CursorManager.SetMouseState(MouseState.ms_pipette);
			CursorManager.SetNewCursor(pipette_CursorTexture, hotSpot);
			break;
		case MouseState.ms_glassStick:		// Glass Stic -> Pipette: change to pipette state
			GlassStick glassStick = GameObject.Find ("GameController").GetComponent<GameController> ().GetCurrentState ().GetComponent<WorkBench> ().glassStick;
			//glassStick.OpenInteractiveBox();
			/*CursorManager.SetMouseState(MouseState.ms_pipette);
			CursorManager.SetNewCursor(pipette_CursorTexture, hotSpot);*/
			break;
		case MouseState.ms_usingTool:  		// Unable to click somewhere else
			break;
		}
	}

	//! Close the interaction box
	public void CloseInteractionBox(){
		boxToChoosePipette.SetActive (false);
		boxGraduatedFilling.SetActive(false);
		boxGraduatedUnfilling.SetActive (false);
		
		interactingGlassware = null;
		interactingReagent = null;
		volumeSelected = 0.0f;
	}

	/* CHOOSING INTERACTION */

	//! Open the interaction box to choose the pipette
	public void OpenSelectingBox() {
		maxVolume = 0.0f;
		graduated = false;
		boxToChoosePipette.SetActive (true);
	}

	//! Opens the box where the choice of pipette is done
	public void SelectingVolumetricPipette(float volume) {
		maxVolume = volume;	
		graduated = false;
	}
	
	//! For the button that chooses the graduated pipette
	public void SelectingGraduatedPipette(float volume) {
		maxVolume = volume;
		graduated = true;
	}
	
	//! For the button that chooses the volumetric pipette
	//	The volume is given as a checkbox parameter
	public void ChoosePipette() {
		CursorManager.SetMouseState(MouseState.ms_pipette);
		CursorManager.SetNewCursor(pipette_CursorTexture, hotSpot);

		CloseInteractionBox ();
	}

	/* END OF CHOOSING INTERACTION */

	/* GRADUATED PIPETTE */
		//FILLING INTERACTION

	//! Open the interaction box to fill the pipette
	//	Also defines the maximum value for the slider
	//	This case is to get liquid from a reagent
	public void OpenGraduatedFillingBox(ReagentsLiquidClass reagent) {

		boxGraduatedFilling.SetActive (true);
		boxGraduatedFilling.GetComponentInChildren<Slider> ().maxValue = maxVolume;

		volumeSelected = 0.0f;
		interactingReagent = reagent;

	
		//CursorManager.SetDefaultCursor ();
		/*
		 * DEFINE HOW TO BLOCK CLICKS OUTSIDE 
		 */
	}
	//	This case is to get liquid from a glassware
	public void OpenGraduatedFillingBox(float volumeAvailable, Glassware glassware) {
		
		volumeSelected = 0.0f;
		
		//interactingReagent = glassware;TODO: Get the reagent from the glassware
		boxGraduatedFilling.SetActive (true);
		if(volumeAvailable < maxVolume)
			boxGraduatedFilling.GetComponentInChildren<Slider> ().maxValue = volumeAvailable;
		else
			boxGraduatedFilling.GetComponentInChildren<Slider> ().maxValue = maxVolume;
		
		//CursorManager.SetDefaultCursor ();
		/*
		 * DEFINE HOW TO BLOCK CLICKS OUTSIDE 
		 */
	}

	//! Set value of volume currently set by the slider.
	public void VolumeOnSlider(){ //BasicallyDone
		volumeSelected = boxSlider.value;
		pipetteValueText.text = volumeSelected.ToString ();
	}

	//! Use the pipette to hold the selected volume.
	public void FillGraduatedPipette() { //BasicallyDone
		
		if (volumeSelected > 0.0f) {
			//TODO: Keeping this for checking. Will be removed as soon as the Glassware part is implemented
			/*if (!(lastItemSelected.GetComponent<Glassware> () == null)) //Only removes from the last selected object if it's a glassware
				lastItemSelected.GetComponent<Glassware>().RemoveLiquid (amountSelectedPipeta);*/
			CursorManager.SetMouseState (MouseState.ms_filledPipette);//pipetaReagentCursor.CursorEnter ();
			CursorManager.SetNewCursor (filledPipette_CursorTexture, hotSpot);
			
			reagentInPipette = interactingReagent;
		}


		volumeHeld = volumeSelected;
		CloseInteractionBox ();
	}
	
		//UNFILLING INTERACTION

	//! Open the interaction box to unfill the pipette
	//	Also defines the maximum value for the slider
	//	Case to unfill pipette into a glassware
	public void OpenGraduatedUnfillingBox(float volumeAvailable, Glassware glassware) {
		boxGraduatedUnfilling.SetActive(true);
		u_volumeSelected = 0.0f;

		//Defining volume on slider
		if(volumeAvailable < volumeHeld)
			boxGraduatedUnfilling.GetComponentInChildren<Slider>().maxValue = volumeAvailable;
		else
			boxGraduatedUnfilling.GetComponentInChildren<Slider>().maxValue = volumeHeld;

		interactingGlassware = glassware;
	}
	//! Open the interaction box to unfill the pipette
	//	Also defines the maximum value for the slider
	//	Case to unfill pipette into a reagent pot
	public void OpenGraduatedUnfillingBox(ReagentsLiquidClass reagentPot) {
		boxGraduatedUnfilling.SetActive(true);
		u_volumeSelected = 0.0f;

		//Defining volume on slider
		boxGraduatedUnfilling.GetComponentInChildren<Slider>().maxValue = volumeHeld;

		interactingGlassware = null;
	}
	
	//! Set value of volume currently set by the slider.
	//	Unfilling variation
	public void u_VolumeOnSlider(){ //BasicallyDone
		u_volumeSelected = u_boxSlider.value;
		u_pipetteValueText.text = volumeSelected.ToString ();
	}
	
	//! Unloads the pipette into a proper vessel
	/*! Called when the filled pipette clicks on a valid vessel for the reagent.
	 	In this overload, the vessel is a glassware */
	public void UnfillGraduatedPipette() {
		/*
		 * CODE PASSING THIS VOLUME AND REAGENT TO THE GLASSWARE
		 * Refreshing method!
		 */
		//glassware.volume = volumeHeld;
		//glassware.reagent = reagentInPipette;

		if (volumeSelected > 0.0f) { //If some liquid is selected, the amount is poured into the glassware
			if (interactingGlassware != null)
				interactingGlassware.PourLiquid (volumeHeld, volumeHeld * reagentInPipette.density, reagentInPipette);//TODO:Needs to treat the case in which the glassware can't receive everything
				
			volumeHeld -= volumeSelected;
		}

		if (volumeHeld <= volumeSelected) { //If all the liquid is taken out of the pipette, the pipette is put down and come back to a default state
			volumeHeld = 0.0f;
			reagentInPipette = null;

			CursorManager.SetMouseState (MouseState.ms_default);
			CursorManager.SetCursorToDefault ();
		}
	
		volumeSelected = 0.0f;
		interactingGlassware = null;

		CloseInteractionBox ();
	}
	//! Unloads the pipette into a proper vessel
	// The vessel being the same reagent used to get the volume */
	//public void UnfillPipette(/*ReagentsLiquidClass reagentPot*/) { //BasicallyDone

/*		volumeHeld = 0.0f;
		reagentInPipette = null;

		CursorManager.SetMouseState (MouseState.ms_default);
		CursorManager.SetCursorToDefault();
	}
*/

	/* END OF GRADUATED PIPETTE */

	public void FillVolumetricPipette(Glassware glassware) {
		if ((glassware.maxVolume - glassware.currentVolume) < maxVolume)
			volumeHeld = glassware.maxVolume - glassware.currentVolume;
		else
			volumeHeld = maxVolume;

		//reagentInPipette = TODO: RECEIVE THE REAGENT FROM GLASSWARE
		if (volumeHeld > 0.0f) {
			CursorManager.SetMouseState (MouseState.ms_filledPipette);
			CursorManager.SetNewCursor (filledPipette_CursorTexture, hotSpot);
		}
	}
	public void FillVolumetricPipette(ReagentsLiquidClass reagent) {
		volumeHeld = maxVolume;

		reagentInPipette = reagent;
		CursorManager.SetMouseState (MouseState.ms_filledPipette);
		CursorManager.SetNewCursor (filledPipette_CursorTexture, hotSpot);
	}

	public void UnfillVolumetricPipette(Glassware glassware) {
		if ((glassware.maxVolume - glassware.currentVolume) < maxVolume) {
			glassware.PourLiquid (glassware.maxVolume - glassware.currentVolume, (glassware.maxVolume - glassware.currentVolume) * reagentInPipette.density, reagentInPipette);
			volumeHeld -= (glassware.maxVolume - glassware.currentVolume);
		} else {
			glassware.PourLiquid (glassware.maxVolume - glassware.currentVolume, (glassware.maxVolume - glassware.currentVolume) * reagentInPipette.density, reagentInPipette);
			volumeHeld = 0.0f;
		}

		if (volumeHeld == 0.0f) {
			/*CursorManager.SetMouseState (MouseState.ms_pipette);
			CursorManager.SetNewCursor (pipette_CursorTexture, hotSpot);*///TODO: DECIDE WHETHER TO GO BACK TO THE DEFAULT OR PIPETTE STATE

			reagentInPipette = null;

			CursorManager.SetMouseState (MouseState.ms_default);
			CursorManager.SetCursorToDefault();
		}
	}
	public void UnfillVolumetricPipette() {
		volumeHeld = 0.0f;
		reagentInPipette = null;

		/*CursorManager.SetMouseState (MouseState.ms_pipette);
			CursorManager.SetNewCursor (pipette_CursorTexture, hotSpot);*///TODO: DECIDE WHETHER TO GO BACK TO THE DEFAULT OR PIPETTE STATE
		
		CursorManager.SetMouseState (MouseState.ms_default);
		CursorManager.SetCursorToDefault();
	}

}

﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

//! Controls the spatulas
/*! Defines if the spatulas are being used, how much they are holding
 *	and integrates the interaction boxes. */

public class Spatula : MonoBehaviour {

	private float volumeHeld;					//Total volume being held in that number of spatulas
	public ReagentsBaseClass reagentInSpatula;	//Reagent being held by the pipette
	//private const float pinchVolume = 0.25f; //Volume of one spatula [ml or cm3]

	//Interaction boxes
	public UI_Manager uiManager;	/*!< The UI Manager Game Object. */
	

	//Interaction box
	public GameObject boxToChooseSpatula;		//Interaction box to choose the spatula
	public GameObject boxToFillSpatula;			//Interaction box to fill spatula
//	public Text f_timesToUseText;				//Text showing the selected number of spatulas for the filling interaction box
	public GameObject boxToUnfillSpatula;		//Interaction box to unfill spatula
//	public Text u_timesToUseText;				//Text showing the selected number of spatulas for the unfilling interaction box
//	public float realVolumeSelected;			//Volume selected during the interaction
//	public float maxVolume;						//Maximum value of spatulas that might be put into the vessel

	/*public int timesToUseSpatula;				//Amount of times that the spatula is going to be used
	public int maxNumberOfSpatulas;				//Maximum number of times the spatula can be used to put reagent into the vessel
	public Text maxNumberText;					//Text showing the maximum number of spatula uses*/
	/*	public Button confirmAddButton;		is the spatula going to remove and add reagents?
	 *	public Button confirmRemoveButton;
	 */
	/*public int checkboxStartpoint;				//Startpoint for the interval that defines the amount of pinches
	public int checkboxEndpoint; 				//Endpoint for the interval that defines the amount of pinches*/

	public float spatulaCapacity;				//Capacity of the spatula being used
	public float capacityError;					//Error associated with the spatula's capacity
	private float error = 0.01f;				//Percentage of error

	public bool usingPrecision;					//If checked, the amount does not vary. Depends on the glassware being on the scale

	//For the cursor
	public Texture2D spatula_CursorTexture;
	public Texture2D filledSpatula_CursorTexture;
	public Vector2 hotSpot = Vector2.zero;


	public Glassware interactingGlassware;		//Glassware which the spatula is interacting with

	// Use this for initialization
	void Start () {
		//boxToFillSpatula.SetActive (false);
		CloseInteractionBox ();
		interactingGlassware = null;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	//! Holds the events for when the interactive spatula on the Workbench is clicked
	public void OnClick() {
		MouseState currentState = CursorManager.GetCurrentState ();
		
		switch (currentState) {
		case MouseState.ms_default: 		//Default -> Spatula: prepares the spatula for use
			OpenChooseBox();
			break;
		case MouseState.ms_pipette: 		//Pipette -> Spatula: change to spatula state
			OpenChooseBox();
			CursorManager.SetMouseState(MouseState.ms_default);
			CursorManager.SetCursorToDefault();
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
			OpenChooseBox();
			CursorManager.SetMouseState(MouseState.ms_default);
			CursorManager.SetCursorToDefault();
			break;
		case MouseState.ms_glassStick:		// Glass Stick -> Spatula: change to spatula state
			OpenChooseBox();
			CursorManager.SetMouseState(MouseState.ms_default);
			CursorManager.SetCursorToDefault();
			break;
		case MouseState.ms_usingTool:  		// Unable to click somewhere else TODO:is it necessary?
			break;
		}
	}

	public void OnStopRun() {
		CloseInteractionBox ();
		spatulaCapacity = 0.0f;
		capacityError = 0.0f;
		volumeHeld = 0.0f;
		reagentInSpatula = null;
		interactingGlassware = null;

		CursorManager.SetMouseState(MouseState.ms_default);
		CursorManager.SetCursorToDefault();
	}

	//! Opens the box where the choice of spatula is made
	//  Is called on the event of a empty spatula clicking on a vessel that holds a solid
	public void OpenChooseBox() {
		spatulaCapacity = 0.0f;
		capacityError = 0.0f;
		boxToChooseSpatula.SetActive(true);
	}

	//! Close the interaction box
	public void CloseInteractionBox(){
		boxToChooseSpatula.SetActive (false);
		boxToFillSpatula.SetActive(false);
		boxToUnfillSpatula.SetActive (false);
	}
	//! Open the interaction box
	public void OpenInteractionBox(bool fill) {
//		realVolumeSelected = 0.0f;

		if(fill)
			boxToFillSpatula.SetActive (true);
		else
			boxToUnfillSpatula.SetActive (true);
		//CursorManager.SetDefaultCursor ();
		/*
		 * DEFINE HOW TO BLOCK CLICKS OUTSIDE 
		 */
	}
	public void OpenInteractionBox(float volumeAvailable, Glassware glassware) {

		/*
		 * OPENS THE RIGHT BOX WHETHER THE GLASSWARE IS ON A SCALE OR NOT
		 */

		boxToUnfillSpatula.SetActive(true);
		
		//Defining maximum amount of spatulas
/*		if (volumeAvailable < realVolumeSelected) {
			maxVolume = volumeAvailable;
			maxNumberOfSpatulas = (int)(volumeAvailable - (volumeAvailable % 1)); //Truncanting value
		}
		else {
			maxVolume = realVolumeSelected;
			maxNumberOfSpatulas = (int)(realVolumeSelected - (realVolumeSelected % 1)); //Trucanting value
		}*/
		
		interactingGlassware = glassware;
	}
	
	//! Used by the checkboxes on the boxToChooseSpatula canvas to set spatula capacity.
	//  There's an error associated with the value. The value HAS to be defined on the canvas!
	public void ChoosingCapacity (float capacity) {
		spatulaCapacity = capacity;
		capacityError = capacity * error;
	}

	//! Consolidate the choice of spatula state, after choosing the capacity
	public void ChooseSpatula() {
		CursorManager.SetMouseState(MouseState.ms_spatula);
		CursorManager.SetNewCursor(spatula_CursorTexture, hotSpot);

		CloseInteractionBox ();
	}

	//! Select the number of times the selected spatula is going to be used
	//  The volume of reagent held by the spatula will be define in a range associated if its capacity
/*	public void SelectingNumberOfSpatulas(bool increase) {
		/*
		 * CODE FILLING THE SPATULA
		 */
		/*if(precision)
		 *	use slider?
		 *else
		 *	use intervals
		 *
		 */
/*		if(realVolumeSelected < 0 || timesToUseSpatula < 0) {		//Can't go below zero
			realVolumeSelected = 0.0f;
			timesToUseSpatula = 0;
		}

		if (!usingPrecision) { //If the vessel is NOT on a scale

			if(increase) {
				timesToUseSpatula++;
				realVolumeSelected += Random.Range(spatulaCapacity - capacityError, spatulaCapacity + capacityError);
			}
			else {
				timesToUseSpatula--;
				realVolumeSelected -= Random.Range(spatulaCapacity - capacityError, spatulaCapacity + capacityError);
			}
			if(realVolumeSelected > maxVolume && interactingGlassware != null)	
				realVolumeSelected = maxVolume;

			if(realVolumeSelected < 0 || timesToUseSpatula < 0) {		//Can't go below zero
		 		realVolumeSelected = 0.0f;
				timesToUseSpatula = 0;
			}
			//timesToUseText.text = realVolumeSelected.ToString();
		}
	}*/

	/*//! Amount of Spatulas chosen
	public void SetUsesOfSpatula() {
		volumeHeld = realVolumeSelected;

	}*/

	//! Uses the spatula to hold a volume of a solid reagent
	public void FillSpatula (ReagentsBaseClass reagent) {
		CloseInteractionBox();

		/*if(realVolumeSelected > 0) {
			CursorManager.SetMouseState (MouseState.ms_filledSpatula);
			CursorManager.SetNewCursor (filledSpatula_CursorTexture, hotSpot);

			reagentInSpatula = reagent;
		}

		spatulasInUse = realVolumeSelected;
		realVolumeSelected = 0;*/

		CursorManager.SetMouseState (MouseState.ms_filledSpatula);
		CursorManager.SetNewCursor (filledSpatula_CursorTexture, hotSpot);
		GameObject.Find ("GameController").GetComponent<GameController>().GetCurrentState().GetComponent<WorkBench>().CannotEndState = true;


		volumeHeld = Random.Range (spatulaCapacity - capacityError, spatulaCapacity + capacityError);
		reagentInSpatula = reagent;
	}

	//! Unloads the spatula into a proper vessel
	//  Called when the filled spatula clicks on a valid vessel for the reagent.
	// 	In this overload, the vessel is a glassware
	public void UnfillSpatula(float volumeAvailable, Glassware glassware) {

		CloseInteractionBox ();

		if (volumeAvailable > volumeHeld) {
			glassware.InsertSolid (volumeHeld, volumeHeld * reagentInSpatula.density, reagentInSpatula);

			CursorManager.SetMouseState (MouseState.ms_default);
			CursorManager.SetCursorToDefault ();

			volumeHeld = 0.0f;
			reagentInSpatula = null;
		}
		else {
			glassware.InsertSolid (volumeAvailable, volumeAvailable * reagentInSpatula.density, reagentInSpatula);
			//TODO: Show something to the player?

			volumeHeld -= volumeAvailable;
		}
		/*if (realVolumeSelected > 0) {
			interactingGlassware.InsertSolid (realVolumeSelected, realVolumeSelected * reagentInSpatula.density, reagentInSpatula);
			spatulasInUse -= realVolumeSelected;
		}
		if (spatulasInUse <= realVolumeSelected) {
			spatulasInUse = 0;
			reagentInSpatula = null;

			CursorManager.SetMouseState (MouseState.ms_default);
			CursorManager.SetCursorToDefault();
		}

		realVolumeSelected = 0.0f;
		interactingGlassware = null;*/
	}

	//! Unloads the spatula into a proper vessel
	//  Called when the filled spatula clicks on a valid vessel for the reagent.
	// 	In this overload, the vessel is the reagent pot or a proper discard place
	public void UnfillSpatula() {
	
		CloseInteractionBox ();

		CursorManager.SetMouseState (MouseState.ms_default);
		CursorManager.SetCursorToDefault();
		GameObject.Find ("GameController").GetComponent<GameController>().GetCurrentState().GetComponent<WorkBench>().CannotEndState = false;

		volumeHeld = 0.0f;
		reagentInSpatula = null;
	}
}

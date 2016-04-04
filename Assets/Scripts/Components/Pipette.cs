using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

//! Controls the pipette
/*! Defines if the pipettes are being used, how much they are holding
 *	and integrates the interaction boxes. */
//TODO: Needs to add the volumetric pipette
public class Pipette : MonoBehaviour {

	private float volumeHeld; // Volume being held by the pipette [ml]


	public UI_Manager uiManager;		// The UI Manager Game Object.

	//Interaction boxes to chose between graduated pipettes or volumetric pipettes
	public GameObject interactionBoxPipette;	//Interaction box
	//Interaction box for graduated pipette
	public Slider boxSlider;  			//Interaction box's slider
	public Text pipetteValueText; 		//Text showing the slider's value
	public float volumeSelected;		//Amount selected in the slider
	//Interaction box for volumetric pipette
	/*
	 */

	//For the cursor
	public Texture2D pipette_CursorTexture;
	public Texture2D filledPipette_CursorTexture;
	public Vector2 hotSpot = Vector2.zero;
	
	public ReagentsLiquidClass reagentInPipette; //Reagent being held by the pipette


	//! Use this for initialization
	void Start () {
		interactionBoxPipette.SetActive (false);
	}
	
	//! Update is called once per frame
	void Update () {

	}

	//! Holds the events for when the interactive pipette on the Workbench is clicked
	void OnClick() {
		MouseState currentState = CursorManager.GetCurrentState ();

		switch (currentState) {
		case MouseState.ms_default: 		//Default -> Pipette: prepares the pipette for use
			CursorManager.SetMouseState(MouseState.ms_pipette);
			CursorManager.SetNewCursor(pipette_CursorTexture, hotSpot);
			break;
		case MouseState.ms_pipette: 		//Pipette -> Pipette: put back the pipette
			CursorManager.SetMouseState(MouseState.ms_default);
			CursorManager.SetCursorToDefault();
			break;
		case MouseState.ms_filledPipette: 	// Filled Pipette -> Pipette: nothing
			break;
		case MouseState.ms_spatula: 		// Spatula -> Piepette: change to pipette state
			CursorManager.SetMouseState(MouseState.ms_pipette);
			CursorManager.SetNewCursor(pipette_CursorTexture, hotSpot);
			break;
		case MouseState.ms_filledSpatula: 	// Filled Spatula -> Pipette: nothing
			break;
		case MouseState.ms_washBottle: 		// Wash Bottle -> Pipette: change to pipette state
			CursorManager.SetMouseState(MouseState.ms_pipette);
			CursorManager.SetNewCursor(pipette_CursorTexture, hotSpot);
			break;
		case MouseState.ms_glassStick:		// Glass Stic -> Pipette: change to pipette state
			CursorManager.SetMouseState(MouseState.ms_pipette);
			CursorManager.SetNewCursor(pipette_CursorTexture, hotSpot);
			break;
		case MouseState.ms_usingTool:  		// Unable to click somewhere else
			break;
		}
	}


	//! Close the interaction box
	public void CloseInteractionBox(){
		interactionBoxPipette.SetActive(false);
	}
	//! Open the interaction box
	public void OpenInteractionBox(float maxSliderVolume) {
		interactionBoxPipette.SetActive (true);
		interactionBoxPipette.GetComponentInChildren<Slider> ().maxValue = maxSliderVolume;
		//CursorManager.SetDefaultCursor ();
		/*
		 * DEFINE HOW TO BLOCK CLICKS OUTSIDE 
		 */
	}

	/*//! The Pipette is being put to work
	public void ActivatePipette(float valueForSlider, Glassware interactingGlass) {
		this.OpenInteractionBox (valueForSlider);

		interactingGlass.PourLiquid(volumeHeld, volumeHeld * reagentInPipette.density, reagentInPipette);

	}*/

	//! Set value of volume currently set by the slider.
	public void VolumeOnSlider(){ //BasicallyDone
		volumeSelected = boxSlider.value;
		pipetteValueText.text = volumeSelected.ToString ();
	}

	//! Use the pipette to hold the selected volume.
	public void FillPipette(ReagentsLiquidClass reagent) { //BasicallyDone
		CloseInteractionBox ();

		if (volumeSelected > 0.0f) {
			//TODO: Keeping this for checking. Will be removed as soon as the Glassware part is implemented
			/*if (!(lastItemSelected.GetComponent<Glassware> () == null)) //Only removes from the last selected object if it's a glassware
				lastItemSelected.GetComponent<Glassware>().RemoveLiquid (amountSelectedPipeta);*/
			CursorManager.SetMouseState (MouseState.ms_filledPipette);//pipetaReagentCursor.CursorEnter ();
			CursorManager.SetNewCursor (filledPipette_CursorTexture, hotSpot);

			reagentInPipette = reagent;
		}

		volumeHeld = volumeSelected;
		volumeSelected = 0.0f;
	}

	//! Unloads the pipette into a proper vessel
	/*! Called when the filled pipette clicks on a valid vessel for the reagent.
	 	In this overload, the vessel is a glassware */
	public void UnfillPipette(Glassware glassware) {
		/*
		 * CODE PASSING THIS VOLUME AND REAGENT TO THE GLASSWARE
		 * Refreshing method!
		 */
		//glassware.volume = volumeHeld;
		//glassware.reagent = reagentInPipette;

		glassware.PourLiquid(volumeHeld, volumeHeld * reagentInPipette.density, reagentInPipette);//TODO:Needs to treat the case in which the glassware can't receive everything

		volumeHeld = 0.0f;
		reagentInPipette = null;

		CursorManager.SetMouseState (MouseState.ms_default);
		CursorManager.SetCursorToDefault();
	}
	//! Unloads the pipette into a proper vessel
	/*! The vessel being the same reagent used to get the volume */
	public void UnfillPipette(/*ReagentsLiquidClass reagentPot*/) { //BasicallyDone

		volumeHeld = 0.0f;
		reagentInPipette = null;

		CursorManager.SetMouseState (MouseState.ms_default);
		CursorManager.SetCursorToDefault();
	}


}

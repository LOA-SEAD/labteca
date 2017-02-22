using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

//! Controls the pipette
/*! Defines if the pipettes are being used, how much they are holding
 *	and integrates the interaction boxes. */
public class Pipette : WorkbenchInteractive {

	public float volumeHeld;			//Volume being held by the pipette [ml]
	private float maxVolume;			//Max volume the pipette can hold [ml]
	public bool graduated;				//Knows if the pipette being used is graduated or volumetric
	private float graduatedError = 0.05f; //Error associated with the graduated pipette

	public UI_Manager uiManager;		// The UI Manager Game Object.

	public ToggleGroup toggle;

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
	
	public Compound reagentInPipette; //Reagent being held by the pipette

	public Glassware interactingGlassware;		 //Glassware which the pipette is interacting with
	public Compound interactingReagent; //Reagent which the pipette is interacting with

	//! Use this for initialization
	void Start () {
		hoverName = "Pipeta";
		boxGraduatedFilling.SetActive (false);
		toggle = boxToChoosePipette.GetComponent<ToggleGroup> ();
	}
	
	//! Update is called once per frame
	void Update () {

	}

	//! Holds the events for when the interactive pipette on the Workbench is clicked
	public override void OnClick() {
		if (GameObject.Find ("GameController").GetComponent<GameController> ().GetCurrentState () !=
			GameObject.Find ("GameController").GetComponent<GameController> ().gameStates [0]) {
			MouseState currentState = CursorManager.GetCurrentState ();

			switch (currentState) {
			case MouseState.ms_default: 		//Default -> Pipette: prepares the pipette for use
			//ChoosePipetteBox
				OnStartInteraction ();
				break;
			case MouseState.ms_pipette: 		//Pipette -> Pipette: put back the pipette
				CursorManager.SetMouseState (MouseState.ms_default);
				CursorManager.SetCursorToDefault ();
				break;
			case MouseState.ms_filledPipette: 	// Filled Pipette -> Pipette: nothing
				break;
			case MouseState.ms_spatula: 		// Spatula -> Piepette: change to pipette state
				OnStartInteraction ();
				break;
			case MouseState.ms_filledSpatula: 	// Filled Spatula -> Pipette: nothing
				break;
			case MouseState.ms_washBottle: 		// Wash Bottle -> Pipette: change to pipette state
				OnStartInteraction ();
				break;
			case MouseState.ms_interacting:  		// Unable to click somewhere else
				break;
			}
		}
	}

	public void OnStartInteraction() {
		OpenSelectingBox();
		CursorManager.SetMouseState(MouseState.ms_interacting);
		CursorManager.SetCursorToDefault();
	}

	public void OnStopRun() {
		CloseInteractionBox ();
		maxVolume = 0.0f;
		volumeHeld = 0.0f;
		boxSlider.value = 0.0f;
		volumeSelected = 0.0f;
		u_boxSlider.value = 0.0f;
		u_volumeSelected = 0.0f;
		interactingReagent = null;
		interactingGlassware = null;

		CursorManager.SetMouseState(MouseState.ms_default);
		CursorManager.SetCursorToDefault();
	}

	//! Close the interaction box
	public void CloseInteractionBox(){
		toggle.SetAllTogglesOff ();

		boxToChoosePipette.SetActive (false);
		boxGraduatedFilling.SetActive(false);
		boxGraduatedUnfilling.SetActive (false);
		
		interactingGlassware = null;
		interactingReagent = null;
		boxSlider.value = 0.0f;
		volumeSelected = 0.0f;
		u_boxSlider.value = 0.0f;
		u_volumeSelected = 0.0f;

		if (CursorManager.GetCurrentState () == MouseState.ms_interacting) {
			CursorManager.SetMouseState(MouseState.ms_default);
			CursorManager.SetCursorToDefault();
		}
	}

	/* CHOOSING INTERACTION */

	//! Open the interaction box to choose the pipette
	public void OpenSelectingBox() {
		maxVolume = 0.0f;
		graduated = false;
		boxToChoosePipette.SetActive (true);
		CursorManager.SetMouseState(MouseState.ms_interacting);
	}

	//! Choose the volumetric pipette for and the volume
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
		if (toggle.AnyTogglesOn ()) {
			CursorManager.SetMouseState (MouseState.ms_pipette);
			CursorManager.SetNewCursor (pipette_CursorTexture, hotSpot);
		}

		CloseInteractionBox ();
	}

	/* END OF CHOOSING INTERACTION */

	/* GRADUATED PIPETTE */
		//FILLING INTERACTION

	//! Open the interaction box to fill the pipette
	//	Also defines the maximum value for the slider
	//	This case is to get liquid from a reagent
	public void OpenGraduatedFillingBox(Compound reagent) { //OK

		boxGraduatedFilling.SetActive (true);
		boxGraduatedFilling.GetComponentInChildren<Slider> ().maxValue = maxVolume;

		volumeSelected = 0.0f;
		interactingReagent = reagent;

		CursorManager.SetMouseState(MouseState.ms_interacting);
	}
	//	This case is to get liquid from a glassware
	public void OpenGraduatedFillingBox(float volumeAvailable, Glassware glassware) {
		
		volumeSelected = 0.0f;

		boxGraduatedFilling.SetActive (true);
		if(volumeAvailable < maxVolume)
			boxGraduatedFilling.GetComponentInChildren<Slider> ().maxValue = volumeAvailable;
		else
			boxGraduatedFilling.GetComponentInChildren<Slider> ().maxValue = maxVolume;

		interactingGlassware = glassware;

		CursorManager.SetMouseState(MouseState.ms_interacting);
	}

	//! Set value of volume currently set by the slider.
	public void VolumeOnSlider(){ //BasicallyDone
		volumeSelected = boxSlider.value;
		pipetteValueText.text = volumeSelected.ToString ();
	}

	//! Use the pipette to hold the selected volume.
	public void FillGraduatedPipette() { //ReagentPot OK | Glassware Ok
		if (volumeSelected > 0.0f) {
			volumeHeld = Random.Range (volumeSelected - graduatedError, volumeSelected + graduatedError);
		}
		if (interactingGlassware != null) {
			if (volumeSelected > 0.0f) {
				/*if (!(lastItemSelected.GetComponent<Glassware> () == null)) //Only removes from the last selected object if it's a glassware
				lastItemSelected.GetComponent<Glassware>().RemoveLiquid (amountSelectedPipeta);*/
				CursorManager.SetMouseState (MouseState.ms_filledPipette);//pipetaReagentCursor.CursorEnter ();
				CursorManager.SetNewCursor (filledPipette_CursorTexture, hotSpot);
				
				GameObject.Find ("GameController").GetComponent<GameController> ().GetCurrentState ().GetComponent<WorkBench> ().cannotEndState = true;
				
				reagentInPipette = (Compound)(interactingGlassware.content as Compound).Clone(volumeHeld);
				
				interactingGlassware.RemoveLiquid(volumeHeld);
			}
		}
		else if (interactingReagent != null) {
			if (volumeSelected > 0.0f) {
				CursorManager.SetMouseState (MouseState.ms_filledPipette);//pipetaReagentCursor.CursorEnter ();
				CursorManager.SetNewCursor (filledPipette_CursorTexture, hotSpot);

				GameObject.Find ("GameController").GetComponent<GameController> ().GetCurrentState ().GetComponent<WorkBench> ().cannotEndState = true;
				reagentInPipette = (Compound)CompoundFactory.GetInstance ().GetCupboardCompound ((interactingReagent.Name)).Clone (volumeHeld);
			}
		} 

		//volumeHeld = volumeSelected;
		CloseInteractionBox ();
	}
	
		//UNFILLING INTERACTION

	//! Open the interaction box to unfill the pipette
	//	Also defines the maximum value for the slider
	//	Case to unfill pipette into a glassware
	public void OpenGraduatedUnfillingBox(float volumeAvailable, Glassware glassware) { //Ok
		boxGraduatedUnfilling.SetActive(true);
		u_volumeSelected = 0.0f;

		//Defining volume on slider
		if(volumeAvailable < volumeHeld)
			boxGraduatedUnfilling.GetComponentInChildren<Slider>().maxValue = volumeAvailable;
		else
			boxGraduatedUnfilling.GetComponentInChildren<Slider>().maxValue = volumeHeld;

		interactingGlassware = glassware;
		interactingReagent = null;
		CursorManager.SetMouseState(MouseState.ms_interacting);
	}
	//! Open the interaction box to unfill the pipette
	//	Also defines the maximum value for the slider
	//	Case to unfill pipette into a reagent pot
	public void OpenGraduatedUnfillingBox(Compound reagentPot) { //Ok
		boxGraduatedUnfilling.SetActive(true);
		u_volumeSelected = 0.0f;

		//Defining volume on slider
		boxGraduatedUnfilling.GetComponentInChildren<Slider>().maxValue = volumeHeld;

		interactingReagent = reagentPot;
		interactingGlassware = null;
		CursorManager.SetMouseState(MouseState.ms_interacting);
	}

	//! Set value of volume currently set by the slider.
	//	Unfilling variation
	public void u_VolumeOnSlider(){ //Ok
		u_volumeSelected = u_boxSlider.value;
		u_pipetteValueText.text = u_volumeSelected.ToString ();
	}
	
	//! Unloads the pipette into a proper vessel
	//  Called when the filled pipette clicks on a valid vessel for the reagent.
	public void UnfillGraduatedPipette() { //ReagentPot Ok | Glassware Ok

		if (u_volumeSelected > 0.0f) { //If some liquid is selected, the amount is poured into the glassware
			bool ok;
			if (interactingGlassware != null) {
				if(u_volumeSelected <= (interactingGlassware.maxVolume - interactingGlassware.GetLiquidVolume())){ //
					//interactingGlassware.PourLiquid (volumeHeld, volumeHeld * reagentInPipette.Density, reagentInPipette);
					ok = interactingGlassware.IncomingReagent(reagentInPipette, u_volumeSelected);
				}
				else {
					/*interactingGlassware.PourLiquid ((interactingGlassware.maxVolume - interactingGlassware.currentVolume),
					                                 (interactingGlassware.maxVolume - interactingGlassware.currentVolume) * reagentInPipette.Density, reagentInPipette);*/
					ok = interactingGlassware.IncomingReagent(reagentInPipette, interactingGlassware.maxVolume - interactingGlassware.GetLiquidVolume());
				}
				if(ok) {
					volumeHeld -= u_volumeSelected;
				}
			}
			else if(interactingReagent != null) {
				volumeHeld -= u_volumeSelected;
			}
		}
		if (volumeHeld <= graduatedError) { //If all the liquid is taken out of the pipette, the pipette is put down and come back to a default state
			volumeHeld = 0.0f;
			reagentInPipette = null;
			
			CursorManager.SetMouseState (MouseState.ms_default);
			CursorManager.SetCursorToDefault ();
			GameObject.Find ("GameController").GetComponent<GameController> ().GetCurrentState ().GetComponent<WorkBench> ().cannotEndState = false;
		} else {
			CursorManager.SetMouseState (MouseState.ms_filledPipette);
			CursorManager.SetNewCursor (filledPipette_CursorTexture, hotSpot);
		}

		u_volumeSelected = 0.0f;
		interactingGlassware = null;
		interactingReagent = null;

		CloseInteractionBox ();
	}
	//! Unloads the pipette into a proper vessel
	// The vessel being the same reagent used to get the volume */
	/*public void UnfillPipette(bool reagent) { //BasicallyDone

		volumeHeld = 0.0f;
		reagentInPipette = null;

		CursorManager.SetMouseState (MouseState.ms_default);
		CursorManager.SetCursorToDefault();
	}*/


	/* END OF GRADUATED PIPETTE */

	public void FillVolumetricPipette(Glassware glassware) { //Ok
		if (glassware.currentVolume < this.maxVolume) { //Case volume on glass < pipette's max volume
			this.volumeHeld = glassware.currentVolume;
			reagentInPipette = (Compound)(glassware.content as Compound).Clone (volumeHeld);
			glassware.RemoveLiquid(glassware.currentVolume);
		} else {
			this.volumeHeld = this.maxVolume;
			this.reagentInPipette = (Compound)(glassware.content as Compound).Clone (volumeHeld);
			glassware.RemoveLiquid (volumeHeld);
		}
		if (volumeHeld > 0.0f) {
			CursorManager.SetMouseState (MouseState.ms_filledPipette);
			CursorManager.SetNewCursor (filledPipette_CursorTexture, hotSpot);
			GameObject.Find ("GameController").GetComponent<GameController>().GetCurrentState().GetComponent<WorkBench>().cannotEndState = true;
		}
	}
	public void FillVolumetricPipette(string reagent) { //OK
		this.volumeHeld = this.maxVolume;

		this.reagentInPipette = (Compound)CompoundFactory.GetInstance ().GetCupboardCompound ((reagent)).Clone (volumeHeld);
		CursorManager.SetMouseState (MouseState.ms_filledPipette);
		CursorManager.SetNewCursor (filledPipette_CursorTexture, hotSpot);

		GameObject.Find ("GameController").GetComponent<GameController>().GetCurrentState().GetComponent<WorkBench>().cannotEndState = true;
	}

	public void UnfillVolumetricPipette(Glassware glassware) { //Ok
		if ((glassware.maxVolume - glassware.GetLiquidVolume()) < this.maxVolume) { //Case volume on pipette > volume available
			float previousVolume = glassware.GetLiquidVolume();
			if(glassware.IncomingReagent (this.reagentInPipette, glassware.maxVolume - glassware.GetLiquidVolume())) {//If poured, takes out the volume from the pipette
				this.volumeHeld -= (glassware.maxVolume - previousVolume);
			}
		} else { //Case volume available in glass > volume on pipette
			if(glassware.IncomingReagent (reagentInPipette, volumeHeld))
				volumeHeld = 0.0f;
		}

		if (volumeHeld == 0.0f) {
			reagentInPipette = null;

			CursorManager.SetMouseState (MouseState.ms_default);
			CursorManager.SetCursorToDefault ();
			GameObject.Find ("GameController").GetComponent<GameController> ().GetCurrentState ().GetComponent<WorkBench> ().cannotEndState = false;
		} else {
			CursorManager.SetMouseState (MouseState.ms_filledPipette);
			CursorManager.SetNewCursor (filledPipette_CursorTexture, hotSpot);
		}
	}
	public void UnfillVolumetricPipette() { //OK
		volumeHeld = 0.0f;
		reagentInPipette = null;
		
		CursorManager.SetMouseState (MouseState.ms_default);
		CursorManager.SetCursorToDefault();
		GameObject.Find ("GameController").GetComponent<GameController>().GetCurrentState().GetComponent<WorkBench>().cannotEndState = false;
	}
}

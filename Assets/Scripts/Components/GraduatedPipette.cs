using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

//! Defines the behaviour for Graduated pipettes
//  Graduated pipettes can be used to hold any volume between 0 and its maximum value.
//  Has a bigger error associated to the value than the volumetric pipettes
public class GraduatedPipette : Pipette {

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


	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}


	//! Holds the events for when the interactive pipette on the Workbench is clicked
	public override void OnClick() {
		MouseState currentState = CursorManager.GetCurrentState ();
		
		switch (currentState) {
		case MouseState.ms_default: 			//Default -> GradPipette: change to grad pipette state
			this.OnStartRun();
			break;
		case MouseState.ms_volPipette:			// VolPipette -> GradPipette : change to grad pipette state
			this.OnStartRun ();
			break;
		case MouseState.ms_filledVolPipette:	// VolPipette -> GradPipette : nothing
			break;
		case MouseState.ms_gradPipette: 		//GradPipette -> GradPipette: put back the pipette
			CursorManager.SetMouseState(MouseState.ms_default);
			CursorManager.SetCursorToDefault();
			break;
		case MouseState.ms_filledGradPipette: 	// Filled GradPipette -> Pipette: nothing
			break;
		case MouseState.ms_spatula: 			// Spatula -> GradPipette: change to grad pipette state
			this.OnStartRun ();
			break;
		case MouseState.ms_filledSpatula: 		// Filled Spatula -> GradPipette: nothing
			break;
		case MouseState.ms_washBottle: 			// Wash Bottle -> GradPipette: change to grad pipette state
			this.OnStartRun ();
			break;
		case MouseState.ms_usingTool:  			// Unable to click somewhere else
			break;
		}
	}


	public override void OnStartRun() {
		maxVolume = 300.0f;

		CursorManager.SetMouseState(MouseState.ms_gradPipette);
		CursorManager.SetNewCursor(pipette_CursorTexture, hotSpot);
	}
	public override void OnStopRun() {
		CloseInteractionBox ();

		this.maxVolume = 0.0f;
		this.volumeHeld = 0.0f;
		this.boxSlider.value = 0.0f;
		this.volumeSelected = 0.0f;
		this.u_boxSlider.value = 0.0f;
		this.u_volumeSelected = 0.0f;
		this.interactingReagent = null;
		this.interactingGlassware = null;

		CursorManager.SetMouseState(MouseState.ms_default);
		CursorManager.SetCursorToDefault();
	}

	//! Close the interaction box
	public override void CloseInteractionBox(){
		/*toggle.SetAllTogglesOff ();
		
		boxToChoosePipette.SetActive (false);
		boxGraduatedFilling.SetActive(false);
		boxGraduatedUnfilling.SetActive (false);
		
		interactingGlassware = null;
		interactingReagent = null;
		boxSlider.value = 0.0f;
		volumeSelected = 0.0f;
		u_boxSlider.value = 0.0f;
		u_volumeSelected = 0.0f;
		transform.GetComponentInParent<WorkBench>().UnblockClicks();*/
	}
	
	//FILLING INTERACTION

	//! Treats how the interaction with different recipients would be.
	public override void FillingInteraction(WorkbenchInteractive interactive) {
		if (interactive is ReagentPot) {
			this.OpenGraduatedFillingBox ((interactive as ReagentPot).reagent);
		} else if (interactive is Glassware) {
			this.OpenGraduatedFillingBox((interactive as Glassware).currentVolume , interactive as Glassware);
		}
	}

	//! Open the interaction box to fill the pipette
	//	Also defines the maximum value for the slider
	//	This case is to get liquid from a reagent
	public void OpenGraduatedFillingBox(Compound reagent) { //OK
		
		boxGraduatedFilling.SetActive (true);
		boxGraduatedFilling.GetComponentInChildren<Slider> ().maxValue = maxVolume;
		
		volumeSelected = 0.0f;
		interactingReagent = reagent;
		
		transform.GetComponentInParent<WorkBench>().BlockClicks();
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
		
		transform.GetComponentInParent<WorkBench>().BlockClicks();
	}
	
	//! Set value of volume currently set by the slider.
	public void VolumeOnSlider(){ //BasicallyDone
		volumeSelected = boxSlider.value;
		pipetteValueText.text = volumeSelected.ToString ();
	}
	
	//! Use the pipette to hold the selected volume.
	public override void FillPipette() { //ReagentPot OK | Glassware Ok
		if (volumeSelected > 0.0f) {
			volumeHeld = Random.Range (volumeSelected - pipetteError, volumeSelected + pipetteError);
		}
		if (interactingGlassware != null) {
			if (volumeSelected > 0.0f) {
				/*if (!(lastItemSelected.GetComponent<Glassware> () == null)) //Only removes from the last selected object if it's a glassware
				lastItemSelected.GetComponent<Glassware>().RemoveLiquid (amountSelectedPipeta);*/
				CursorManager.SetMouseState (MouseState.ms_filledVolPipette);//pipetaReagentCursor.CursorEnter ();
				CursorManager.SetNewCursor (filledPipette_CursorTexture, hotSpot);
				
				GameObject.Find ("GameController").GetComponent<GameController> ().GetCurrentState ().GetComponent<WorkBench> ().cannotEndState = true;
				
				reagentInPipette = (Compound)(interactingGlassware.content as Compound).Clone(volumeHeld);
				
				interactingGlassware.RemoveLiquid(volumeHeld);
			}
		}
		else if (interactingReagent != null) {
			if (volumeSelected > 0.0f) {
				CursorManager.SetMouseState (MouseState.ms_filledVolPipette);//pipetaReagentCursor.CursorEnter ();
				CursorManager.SetNewCursor (filledPipette_CursorTexture, hotSpot);
				
				GameObject.Find ("GameController").GetComponent<GameController> ().GetCurrentState ().GetComponent<WorkBench> ().cannotEndState = true;
				reagentInPipette = (Compound)CompoundFactory.GetInstance ().GetCompound ((interactingReagent.Name)).Clone (volumeHeld);
			}
		} 
		
		//volumeHeld = volumeSelected;
		CloseInteractionBox ();
	}
	
	//UNFILLING INTERACTION

	//! Treats how the interaction with different recipients would be.
	public override void UnfillingInteraction(WorkbenchInteractive interactive) {
		if (interactive is ReagentPot) {
			this.OpenGraduatedUnfillingBox((interactive as ReagentPot).reagent);
		} else if (interactive is Glassware) {
			this.OpenGraduatedUnfillingBox ((interactive as Glassware).maxVolume - (interactive as Glassware).currentVolume, (interactive as Glassware));
		}
	}

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
		transform.GetComponentInParent<WorkBench>().BlockClicks();
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
		transform.GetComponentInParent<WorkBench>().BlockClicks();
	}
	
	//! Set value of volume currently set by the slider.
	//	Unfilling variation
	public void u_VolumeOnSlider(){ //Ok
		u_volumeSelected = u_boxSlider.value;
		u_pipetteValueText.text = u_volumeSelected.ToString ();
	}
	
	//! Unloads the pipette into a proper vessel
	//  Called when the filled pipette clicks on a valid vessel for the reagent.
	public override void UnfillPipette() { //ReagentPot Ok | Glassware Ok
		
		if (u_volumeSelected > 0.0f) { //If some liquid is selected, the amount is poured into the glassware
			bool ok;
			if (interactingGlassware != null) {
				if(u_volumeSelected <= (interactingGlassware.maxVolume - interactingGlassware.currentVolume)){ //
					//interactingGlassware.PourLiquid (volumeHeld, volumeHeld * reagentInPipette.Density, reagentInPipette);
					ok = interactingGlassware.IncomingReagent(reagentInPipette, u_volumeSelected);
				}
				else {
					/*interactingGlassware.PourLiquid ((interactingGlassware.maxVolume - interactingGlassware.currentVolume),
					                                 (interactingGlassware.maxVolume - interactingGlassware.currentVolume) * reagentInPipette.Density, reagentInPipette);*/
					ok = interactingGlassware.IncomingReagent(reagentInPipette, interactingGlassware.maxVolume - interactingGlassware.currentVolume);
				}
				if(ok)
					volumeHeld -= u_volumeSelected;
			}
			else if(interactingReagent != null) {
				volumeHeld -= u_volumeSelected;
			}
		}
		
		if (volumeHeld <= u_volumeSelected) { //If all the liquid is taken out of the pipette, the pipette is put down and come back to a default state
			volumeHeld = 0.0f;
			reagentInPipette = null;
			
			CursorManager.SetMouseState (MouseState.ms_default);
			CursorManager.SetCursorToDefault ();
			GameObject.Find ("GameController").GetComponent<GameController> ().GetCurrentState ().GetComponent<WorkBench> ().cannotEndState = false;
		}
		
		u_volumeSelected = 0.0f;
		interactingGlassware = null;
		interactingReagent = null;
		
		CloseInteractionBox ();
	}

}

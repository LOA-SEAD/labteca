using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

//! Defines the behaviour for Volumetric Pipettes
//  Volumetric pipettes have a pre-set maximum value, and should only be used for that value.
//  They have a muuuch smaller error than Graduated pipettes when used correctly,
//	but quite some error otherwise
public class VolumetricPipette : Pipette {

	//Interaction boxes to chose the volumes for volumetric pipettes
	public GameObject boxToChoosePipette; //Interaction box to choose the pipette
	public ToggleGroup toggle;
	
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
		case MouseState.ms_default: 			//Default -> VolPipette: change to grad pipette state
			this.OnStartRun();
			break;
		case MouseState.ms_volPipette:			// VolPipette -> VolPipette : put back the vol pipette
			CursorManager.SetMouseState(MouseState.ms_default);
			CursorManager.SetCursorToDefault();
			break;
		case MouseState.ms_filledVolPipette:	// VolPipette -> VolPipette : nothing
			break;
		case MouseState.ms_gradPipette: 		//GradPipette -> VolPipette: change to grad pipette state
			this.OnStartRun ();
			break;
		case MouseState.ms_filledGradPipette: 	// Filled GradPipette -> VolPipette: nothing
			break;
		case MouseState.ms_spatula: 		// Spatula -> VolPipette: change to vol pipette state
			this.OnStartRun();
			break;
		case MouseState.ms_filledSpatula: 	// Filled Spatula -> VolPipette: nothing
			break;
		case MouseState.ms_washBottle: 		// Wash Bottle -> VolPipette: change to vol pipette state
			this.OnStartRun();
			break;
		case MouseState.ms_glassStick:		// Glass Stic -> VolPipette: change to vol pipette state
			this.OnStartRun();
			break;
		case MouseState.ms_usingTool:  		// Unable to click somewhere else
			break;
		}
	}

	public override void OnStartRun () {
		maxVolume = 0.0f;

		//Opening the interaction box to choose the pipette
		boxToChoosePipette.SetActive(true);
		transform.GetComponentInParent<WorkBench>().BlockClicks();
	}
	public override void OnStopRun() {
		CloseInteractionBox ();
		/*
		maxVolume = 0.0f;
		volumeHeld = 0.0f;
		boxSlider.value = 0.0f;
		volumeSelected = 0.0f;
		u_boxSlider.value = 0.0f;
		u_volumeSelected = 0.0f;
		interactingReagent = null;
		interactingGlassware = null;
		*/
		CursorManager.SetMouseState(MouseState.ms_default);
		CursorManager.SetCursorToDefault();
	}

	//! Close the interaction box
	public override void CloseInteractionBox(){
		toggle.SetAllTogglesOff ();
		boxToChoosePipette.SetActive (false);
		
		transform.GetComponentInParent<WorkBench>().UnblockClicks();
	}

	/* CHOOSING INTERACTION */

	//! Choose the volumetric pipette for and the volume
	//  The parameter is the volume associated with each toggle box
	public void SelectingVolumetricPipette(float volume) {
		maxVolume = volume;	
	}

	//! For the button that chooses the volumetric pipette
	public void ChoosePipette() {
		if (toggle.AnyTogglesOn ()) {
			CursorManager.SetMouseState (MouseState.ms_volPipette);
			CursorManager.SetNewCursor (pipette_CursorTexture, hotSpot);
		}
		CloseInteractionBox ();
	}


	public override void FillPipette ()
	{

	}
	public void FillPipette(Glassware glassware) {
		if (glassware.currentVolume < maxVolume) { //Case volume on glass < pipette's max volume
			volumeHeld = glassware.currentVolume;
			reagentInPipette = (Compound)(glassware.content as Compound).Clone (volumeHeld);
			glassware.RemoveLiquid(glassware.currentVolume);
		} else {
			volumeHeld = maxVolume;
			reagentInPipette = (Compound)(glassware.content as Compound).Clone (volumeHeld);
			glassware.RemoveLiquid (volumeHeld);
		}
		if (volumeHeld > 0.0f) {
			CursorManager.SetMouseState (MouseState.ms_filledVolPipette);
			CursorManager.SetNewCursor (filledPipette_CursorTexture, hotSpot);
			GameObject.Find ("GameController").GetComponent<GameController>().GetCurrentState().GetComponent<WorkBench>().cannotEndState = true;
		}
	}
	public void FillPipette(string reagent) {
		volumeHeld = maxVolume;
		
		reagentInPipette = (Compound)CompoundFactory.GetInstance ().GetCompound ((reagent)).Clone (volumeHeld);
		CursorManager.SetMouseState (MouseState.ms_filledVolPipette);
		CursorManager.SetNewCursor (filledPipette_CursorTexture, hotSpot);
		
		GameObject.Find ("GameController").GetComponent<GameController>().GetCurrentState().GetComponent<WorkBench>().cannotEndState = true;
		
	}
	
	public void UnfillPipette(Glassware glassware) {
		if ((glassware.maxVolume - glassware.currentVolume) < this.maxVolume) { //Case volume on pipette > volume available
			float previousVolume = glassware.currentVolume;
			if(glassware.IncomingReagent (reagentInPipette, glassware.maxVolume - glassware.currentVolume)) //If poured, takes out the volume from the pipette
				volumeHeld -= (glassware.maxVolume - previousVolume);
		} else { //Case volume available in glass > volume on pipette
			if(glassware.IncomingReagent (reagentInPipette, volumeHeld))
				volumeHeld = 0.0f;
		}
		
		if (volumeHeld == 0.0f) {
			reagentInPipette = null;
			
			CursorManager.SetMouseState (MouseState.ms_default);
			CursorManager.SetCursorToDefault();
			GameObject.Find ("GameController").GetComponent<GameController>().GetCurrentState().GetComponent<WorkBench>().cannotEndState = false;
		}
	}
	public override void UnfillPipette() {
		volumeHeld = 0.0f;
		reagentInPipette = null;
		
		CursorManager.SetMouseState (MouseState.ms_default);
		CursorManager.SetCursorToDefault();
		GameObject.Find ("GameController").GetComponent<GameController>().GetCurrentState().GetComponent<WorkBench>().cannotEndState = false;
	}

}

using UnityEngine;
using System.Collections;

//! Declaration of information for liquid reagents
/*! The liquids have all the attributes from solids, plus a couple more (Reason why it was implemented like this)*/

public class ReagentsLiquidClass : ReagentsBaseClass {

	public float ph;
	public float turbidity;
	public float refratometer;
	public Texture2D hplc;  //High-Performance liquid chromatography

	//! Holds the events for when the interactive liquid reagent on the Workbench is clicked
	public void OnClick(){		
		MouseState currentState = CursorManager.GetCurrentState ();
		Pipette pipette = GameObject.Find ("GameController").GetComponent<GameController> ().GetCurrentState ().GetComponent<WorkBench> ().pipette;
			
		switch (currentState) {
		case MouseState.ms_default: 		//Default -> Liquid Reagent: open interaction box to take the reagent to the inventory?
			//
			break;
		case MouseState.ms_pipette: 		//Pipette -> Liquid Reagent: fill the pipette with the reagent clicked
			if(pipette.graduated)
				pipette.OpenGraduatedFillingBox(this);
			else
				pipette.FillVolumetricPipette(this);
			break;
		case MouseState.ms_filledPipette: 	// Filled Spatula -> Liquid Reagent: put back the content if it is the same reagent
			if(pipette.reagentInPipette == this) {
				if(pipette.graduated)
					pipette.OpenGraduatedUnfillingBox(this);
				else
					pipette.UnfillVolumetricPipette();
			}
			break;
		case MouseState.ms_spatula: 		// Spatula -> Liquid Reagent: nothing
			break;
		case MouseState.ms_filledSpatula: 	// Filled Spatula -> Liquid Reagent: nothing
			break;
		case MouseState.ms_washBottle: 		// Wash Bottle -> Liquid Reagent: nothing
			break;
		case MouseState.ms_glassStick:		// Glass Stick -> Liquid Reagent: nothing
			break;
		case MouseState.ms_usingTool:  		// Unable to click somewhere else TODO:is it necessary?
			break;
		}
	}
}
using UnityEngine;
using System.Collections;

public class ReagentPot : ItemToInventory {
	
	public Reagent reagent;
	public bool isSolid;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	//! Holds the events for when the interactive reagent pot on the Workbench is clicked
	public void OnClick(){
		MouseState currentState = CursorManager.GetCurrentState ();
		Spatula spatula = GameObject.Find ("GameController").GetComponent<GameController> ().GetCurrentState ().GetComponent<WorkBench> ().spatula;
		Pipette pipette = GameObject.Find ("GameController").GetComponent<GameController> ().GetCurrentState ().GetComponent<WorkBench> ().pipette;

		if (isSolid) {
			switch (currentState) {
			case MouseState.ms_default: 		//Default -> Solid Reagent: open interaction box to take the reagent to the inventory?
				//
				break;
			case MouseState.ms_pipette: 		//Pipette -> Solid Reagent: nothing
				break;
			case MouseState.ms_filledPipette: 	// Filled Spatula -> Solid Reagent: nothing
				break;
			case MouseState.ms_spatula: 		// Spatula -> Solid Reagent: fill the spatula with the reagent clicked
				//spatula.OpenInteractionBox(true);
				spatula.FillSpatula (this.reagent.Clone ());
				break;
			case MouseState.ms_filledSpatula: 	// Filled Spatula -> Solid Reagent: put back the content if it is the same reagent
				if (spatula.reagentInSpatula.name == this.reagent.GetName())
					spatula.UnfillSpatula ();
				//else
				//	GameObject.Find("GameController").GetComponent<GameController>().GetCurrentState().GetComponent<WorkBench>().differentReagentErrorBox.SetActive(true);
				break;
			case MouseState.ms_washBottle: 		// Wash Bottle -> Solid Reagent: nothing
				break;
			case MouseState.ms_glassStick:		// Glass Stick -> Solid Reagent: nothing
				break;
			case MouseState.ms_usingTool:  		// Unable to click somewhere else TODO:is this necessary?
				break;
			}
		}
		else {
			switch (currentState) {
			case MouseState.ms_default: 		//Default -> Liquid Reagent: open interaction box to take the reagent to the inventory?
				//
				break;
			case MouseState.ms_pipette: 		//Pipette -> Liquid Reagent: fill the pipette with the reagent clicked
				if(pipette.graduated)
					pipette.OpenGraduatedFillingBox(this.reagent);
				else
					pipette.FillVolumetricPipette(this.reagent.Clone ());
				break;
			case MouseState.ms_filledPipette: 	// Filled Spatula -> Liquid Reagent: put back the content if it is the same reagent
				if(pipette.reagentInPipette.name == this.reagent.name) {
					if(pipette.graduated)
						pipette.OpenGraduatedUnfillingBox(this.reagent);
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
}

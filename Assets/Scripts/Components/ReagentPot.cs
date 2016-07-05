using UnityEngine;
using System.Collections;

public class ReagentPot : ItemToInventory {
	
	public Reagent reagent = new Reagent();
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
		Spatula spatula = (GameObject.Find ("GameController").GetComponent<GameController> ().GetCurrentState () as WorkBench).spatula;
		Pipette pipette = (GameObject.Find ("GameController").GetComponent<GameController> ().GetCurrentState () as WorkBench).pipette;

		if (isSolid) {
			switch (currentState) {
			case MouseState.ms_default: 		//Default -> Solid Reagent: open interaction box to take the reagent to the inventory?
				(GameObject.Find ("GameController").GetComponent<GameController> ().GetCurrentState () as WorkBench).stateUIManager.OpenOptionDialog(this);
				break;
			case MouseState.ms_pipette: 		//Pipette -> Solid Reagent: nothing
				break;
			case MouseState.ms_filledPipette: 	// Filled Spatula -> Solid Reagent: nothing
				break;
			case MouseState.ms_spatula: 		// Spatula -> Solid Reagent: fill the spatula with the reagent clicked
				//spatula.OpenInteractionBox(true);
				//spatula.FillSpatula ((Compound)this.reagent.Clone ());
				if(reagent.FumeHoodOnly) { //In case it should be use at the Fume Hood
					if(GameObject.Find("GameController").GetComponent<GameController>().currentStateIndex != 4) { // If it is the FumeHoodState
						GameObject.Find("GameController").GetComponent<GameController>().sendAlert("Este reagente libera gases prejudiciais.\nDirija-se a capela");
					}
					else { //Sub-case: being use at the fume hood.
						spatula.FillSpatula (reagent.Name);
					}
				} else { //Case: doesn't need fume hood
					spatula.FillSpatula (reagent.Name);
				}
				break;
			case MouseState.ms_filledSpatula: 	// Filled Spatula -> Solid Reagent: put back the content if it is the same reagent
				if (spatula.reagentInSpatula.Name == this.reagent.Name)
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
				(GameObject.Find ("GameController").GetComponent<GameController> ().GetCurrentState () as WorkBench).stateUIManager.OpenOptionDialog(this);
				break;
			case MouseState.ms_pipette: 		//Pipette -> Liquid Reagent: fill the pipette with the reagent clicked
				if(reagent.FumeHoodOnly) { //In case it should be use at the Fume Hood
					if(GameObject.Find("GameController").GetComponent<GameController>().currentStateIndex != 4) { // If it is the FumeHoodState
						GameObject.Find("GameController").GetComponent<GameController>().sendAlert("Este reagente libera gases prejudiciais.\nDirija-se a capela");
					}
					else { //Sub-case: being use at the fume hood.
						if(pipette.graduated)
							pipette.OpenGraduatedFillingBox(this.reagent);
						else
							pipette.FillVolumetricPipette(this.reagent.Name);
					}
				} else { //Case: doesn't need fume hood
					if(pipette.graduated)
						pipette.OpenGraduatedFillingBox(this.reagent);
					else
						pipette.FillVolumetricPipette(this.reagent.Name);
				}
				break;
			case MouseState.ms_filledPipette: 	// Filled Spatula -> Liquid Reagent: put back the content if it is the same reagent
				if(pipette.reagentInPipette.Name == this.reagent.Name) {
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
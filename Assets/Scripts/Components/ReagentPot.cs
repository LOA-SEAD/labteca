using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class ReagentPot : ItemToInventory {
	
	public Reagent reagent = new Reagent();
	public RectTransform infoCanvas;
	public bool isSolid;
	public List<Text> tabValues;

	// Use this for initialization
	void Start () {
		tabValues[0].text = reagent.Name;
		tabValues[1].text = reagent.MolarMass + " g/mol";
		tabValues[2].text = reagent.Density+ " g/ml";
		tabValues[3].text = CompoundFactory.GetInstance().GetCompound(reagent.Name).Purity*100+ "%";
		tabValues[4].text = reagent.Solubility+ " g/1g";
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	//! Holds the events for when the interactive reagent pot on the Workbench is clicked
	public void OnClick(){
		MouseState currentState = CursorManager.GetCurrentState ();
		Spatula spatula = (GameObject.Find ("GameController").GetComponent<GameController> ().GetCurrentState () as WorkBench).spatula;
		VolumetricPipette volPipette = (GameObject.Find ("GameController").GetComponent<GameController> ().GetCurrentState () as WorkBench).volumetricPipette;
		GraduatedPipette gradPipette = (GameObject.Find ("GameController").GetComponent<GameController> ().GetCurrentState () as WorkBench).graduatedPipette;

		switch (currentState) {
		case MouseState.ms_default: 		//Default -> Solid Reagent: open interaction box to take the reagent to the inventory?
			(GameObject.Find ("GameController").GetComponent<GameController> ().GetCurrentState () as WorkBench).stateUIManager.OpenOptionDialog(this);
			break;
		case MouseState.ms_volPipette: 		//Pipette -> Solid Reagent: nothing
			if(!(reagent.IsSolid)) {
				if(FumeHoodVerification()) { //In case it should be use at the Fume Hood
					volPipette.FillPipette(this.reagent.Name);
				}
			}
			break;
		case MouseState.ms_filledVolPipette: 	// Filled Pipette -> Solid Reagent: nothing
			if(!(reagent.IsSolid)) {
				if(volPipette.reagentInPipette.Name == this.reagent.Name) {
					volPipette.UnfillingInteraction();
				}
			}
			break;
		case MouseState.ms_gradPipette: 		//Pipette -> Solid Reagent: nothing
			if(!(reagent.IsSolid)) {
				if(FumeHoodVerification()) { //In case it should be use at the Fume Hood
					gradPipette.FillingInteraction(this);
				}
			}
			break;
		case MouseState.ms_filledGradPipette: 	// Filled Pipette -> Solid Reagent: nothing
			if(!(reagent.IsSolid)) {
				if(gradPipette.reagentInPipette.Name == this.reagent.Name) {
					gradPipette.UnfillingInteraction();
				}
			}
			break;
		case MouseState.ms_spatula: 		// Spatula -> Solid Reagent: fill the spatula with the reagent clicked
			//spatula.OpenInteractionBox(true);
			//spatula.FillSpatula ((Compound)this.reagent.Clone ());
			if(reagent.IsSolid) {
				if(FumeHoodVerification()){
					spatula.FillSpatula (reagent.Name);
				}
			}
			break;
		case MouseState.ms_filledSpatula: 	// Filled Spatula -> Solid Reagent: put back the content if it is the same reagent
			if(reagent.IsSolid) {
				if (spatula.reagentInSpatula.Name == this.reagent.Name)
					spatula.UnfillSpatula ();
			}
			//else
			//	GameObject.Find("GameController").GetComponent<GameController>().GetCurrentState().GetComponent<WorkBench>().differentReagentErrorBox.SetActive(true);
			break;
		case MouseState.ms_washBottle: 		// Wash Bottle -> Solid Reagent: nothing
			break;
		case MouseState.ms_usingTool:  		// Unable to click somewhere else TODO:is this necessary?
			break;
		}
	}

	private bool FumeHoodVerification() {
		if (this.reagent.FumeHoodOnly) {
			if (GameObject.Find ("GameController").GetComponent<GameController> ().currentStateIndex != 4) { // If it is the FumeHoodState
				GameObject.Find ("GameController").GetComponent<GameController> ().sendAlert ("Este reagente libera gases prejudiciais.\nDirija-se à capela");
				return false;
			} else { //Sub-case: being use at the fume hood.
				return true;
			}
		} else { //Case: doesn't need fume hood
			return true;
		}
	}
}
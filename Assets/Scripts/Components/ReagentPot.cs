using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class ReagentPot : ItemToInventory {
	
	public Reagent reagent = new Reagent();
	public RectTransform infoCanvas;
	public bool isSolid;
	public List<Text> tabValues;

	public GameObject potMesh;
	private Color defaultColour;

	// Use this for initialization
	void Start () {
		tabValues[0].text = reagent.Name;
		tabValues[1].text = reagent.MolarMass + " g/mol";
		tabValues[2].text = reagent.Density+ " g/ml";
		tabValues[3].text = CompoundFactory.GetInstance().GetCupboardCompound(reagent.Name).Purity*100+ "%";
		tabValues[4].text = reagent.Solubility+ " g/1g";

		defaultColour = potMesh.GetComponent<Renderer>().material.color;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	//! Holds the events for when the interactive reagent pot on the Workbench is clicked
	public void OnClick(){
		if (GameObject.Find ("GameController").GetComponent<GameController>().GetCurrentState () !=
		    GameObject.Find ("GameController").GetComponent<GameController>().gameStates [0]) {
			MouseState currentState = CursorManager.GetCurrentState ();
			Spatula spatula = (GameObject.Find ("GameController").GetComponent<GameController> ().GetCurrentState () as WorkBench).spatula;
			Pipette pipette = (GameObject.Find ("GameController").GetComponent<GameController> ().GetCurrentState () as WorkBench).pipette;

			switch (currentState) {
			case MouseState.ms_default: 		//Default -> Solid Reagent: open interaction box to take the reagent to the inventory?
				(GameObject.Find ("GameController").GetComponent<GameController> ().GetCurrentState () as WorkBench).stateUIManager.OpenOptionDialog (this);
				break;
			case MouseState.ms_pipette: 		//Pipette -> Solid Reagent: nothing
				if (!isSolid) {
					if (reagent.FumeHoodOnly) { //In case it should be use at the Fume Hood
						if (GameObject.Find ("GameController").GetComponent<GameController> ().currentStateIndex != 4) { // If it is the FumeHoodState
							GameObject.Find ("GameController").GetComponent<GameController> ().sendAlert ("Este reagente libera gases prejudiciais.\nDirija-se à capela");
						} else { //Sub-case: being use at the fume hood.
							if (pipette.graduated)
								pipette.OpenGraduatedFillingBox (this.reagent);
							else
								pipette.FillVolumetricPipette (this.reagent.Name);
						}
					} else { //Case: doesn't need fume hood
						if (pipette.graduated)
							pipette.OpenGraduatedFillingBox (this.reagent);
						else
							pipette.FillVolumetricPipette (this.reagent.Name);
					}
				}
				break;
			case MouseState.ms_filledPipette: 	// Filled Spatula -> Solid Reagent: nothing
				if (pipette.reagentInPipette.Name == this.reagent.Name) {
					if (pipette.graduated)
						pipette.OpenGraduatedUnfillingBox (this.reagent);
					else
						pipette.UnfillVolumetricPipette ();
				}
				break;
			case MouseState.ms_spatula: 		// Spatula -> Solid Reagent: fill the spatula with the reagent clicked
			//spatula.OpenInteractionBox(true);
			//spatula.FillSpatula ((Compound)this.reagent.Clone ());
				if (isSolid) {
					if (reagent.FumeHoodOnly) { //In case it should be use at the Fume Hood
						if (GameObject.Find ("GameController").GetComponent<GameController> ().currentStateIndex != 4) { // If it is the FumeHoodState
							GameObject.Find ("GameController").GetComponent<GameController> ().sendAlert ("Este reagente libera gases prejudiciais.\nDirija-se à capela");
						} else { //Sub-case: being use at the fume hood.
							spatula.FillSpatula (reagent.Name);
						}
					} else { //Case: doesn't need fume hood
						spatula.FillSpatula (reagent.Name);
					}
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
			case MouseState.ms_interacting:  		// Unable to click somewhere else TODO:is this necessary?
				break;
			}
		}
	}

	//! Handles actions when on hovering the mouse
	//  Shows the infoCanvas (setting its position correctly), and makes the object glow when interaction is possible
	public void OnHoverIn() {
		if (GameObject.Find ("GameController").GetComponent<GameController> ().GetCurrentState () !=
			GameObject.Find ("GameController").GetComponent<GameController> ().gameStates [0]) {
			infoCanvas.gameObject.SetActive (true);

			//Glowing state machine
			if (CursorManager.GetCurrentState () != MouseState.ms_interacting) {
				switch (CursorManager.GetCurrentState ()) {
				/*case MouseState.ms_default: 		//Default -> Pot
					potMesh.renderer.material.color = Color.white;
					break;*/
				case MouseState.ms_pipette: 		//Pipette -> Pot
					if (!isSolid) {
						//glow
						potMesh.GetComponent<Renderer>().material.color = Color.white;
					}
					break;
				case MouseState.ms_filledPipette: 	// Filled Pipette -> Pot.
					if (!isSolid && (GameObject.Find ("GameController").GetComponent<GameController> ().GetCurrentState () as WorkBench).pipette.reagentInPipette.Formula == this.reagent.Formula) {
						//glow
						potMesh.GetComponent<Renderer>().material.color = Color.white;
					}
					break;
				case MouseState.ms_spatula: 		// Spatula -> Pot.
					if (isSolid) {
						potMesh.GetComponent<Renderer>().material.color = Color.white;
					}
					break;
				case MouseState.ms_filledSpatula: 	// Filled Spatula -> Pot.
					if (isSolid && (GameObject.Find ("GameController").GetComponent<GameController> ().GetCurrentState () as WorkBench).spatula.reagentInSpatula.Formula == this.reagent.Formula) {
						//glow
						potMesh.GetComponent<Renderer>().material.color = Color.white;
					}
					break;
				default:
					break;		
				}
			}
		}
	}
	//! Handles actions when hovering stops
	public void OnHoverOut() {
		if (GameObject.Find ("GameController").GetComponent<GameController> ().GetCurrentState () !=
			GameObject.Find ("GameController").GetComponent<GameController> ().gameStates [0]) {
			infoCanvas.gameObject.SetActive (false);

			potMesh.GetComponent<Renderer>().material.color = defaultColour;
		}
	}
}
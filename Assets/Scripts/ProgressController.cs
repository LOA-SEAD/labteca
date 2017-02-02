using UnityEngine;
using System.Collections;
using System.Collections.Generic;

// Enum to indicate the types of verification
public enum TypeOfStep {
	none,				//There's no verification yet
	CompoundClass,		//The class of the compound will be checked
	WhatCompound,		//What compound is in the glassware
	MolarityCheck,		//The amount of compound will be asked
	Diluting			//A dilution will be done
};


// The class is responsible for every phase transition, and every step within the phase
// It sends the information for the ResultVerifier class for which interface to set and what to check,
// and also help defines the number of tabs in the tablet
public class ProgressController : MonoBehaviour {

	TypeOfStep stepType;
	TypeOfStep StepType { get; set; }

	private string customPhaseDir = "Assets/Resources/customPhase.json";

	private Dictionary<int, Dictionary<string, string>> currentPhase;
	private Dictionary<string, string> currentStep;
	private int numberOfSteps;
	private int actualStep;


	// Use this for initialization
	void Start () {
		stepType = TypeOfStep.none;
	}

	void Awake() {


		stepType = TypeOfStep.none;
	}

	/*void SceneLoadOnStart() {
		if (GameObject.Find ("ProgressController") != null) {
		}
	}*/

	// Load the scene according to what was selected on the Menu
	public void LoadScene() {
	}

	// Start CustomkMode, triggering all the other entities to read the needed information.
	private void StartCustomMode() {
		currentPhase = PhasesSaver.LoadPhases ("Assets/Resources/customPhases.json");
		NewPhase();

		numberOfSteps = currentPhase.Count;
		actualStep = 0;


		/*switch(currentPhase[0].["typeOfStep"] = "0")
		StepType = TypeOfStep*/
		NewPhase();

	}
	// Called to load a new phase
	private void NewPhase(){
		//Check how to start the phase
		//if(currentPhase[0].glasswareStart == "1")
		/*
		 * Instaciar vidraria
		 * Adionar reagente a vidraria
		 * Adicionar vidraria ao inventario
		 */
		//else
		/*
		 * Pote de reagente
		 */
		
		//Add steps to Experiment Menu on Tablet

		//NewStep();
	}
	// Called to load the subsequent step of a phase
	private void NewStep(){
		//ResultVerifier.SetVerificationSteps(TypeOfStep, Dic<string, string>));

		//Play starting dialogue according to type of quest, if needed
	}

	// Is called when a step is completed, transiting to the next step, or next phase
	public void CompleteStep() {
		//Write on .json the answers

		//if(numberOfSteps == actualStep)
		/*
		 * PhaseTransition();
		 */
		//else
		/*
		 * Play ending dialogue accoding to step, if needed;
		 * actualStep++;
		 * NewStep();
		 */
	}

	// Makes the transition to the next phase
	private void PhaseTransition(){
		//if(customMode)
		/*
		 * EndGame
		 */
		//else
		/*
		 * LoadNextPhase();
		 */
	}

	/* CREATION TEST
	void FixedUpdate() {
		if(Input.GetKey(KeyCode.L)) {
			GameObject bequer = Instantiate ((GameObject.Find ("GameController").GetComponent<GameController> ().gameStates [3] as GetGlasswareState).glasswareList [0].gameObject) as GameObject;
			if((GameObject.Find ("GameController").GetComponent<GameController> ().gameStates [1] as WorkBench).TryPutIntoPosition(bequer)) {
				JSONEdit lu = new JSONEdit("C:/Users/Thiago/Desktop/test/test.json");
				Compound reagentAcc = new Compound();
				
				reagentAcc.Name = lu.GetString (0, "name");
				reagentAcc.Formula = lu.GetString (0, "formula");
				reagentAcc.IsSolid = false;
				reagentAcc.MolarMass = lu.GetFloat (0, "molarMass");
				reagentAcc.Molarity = lu.GetFloat (0, "molarity");
				reagentAcc.Density = lu.GetFloat (0, "density");
				reagentAcc.Polarizability = lu.GetFloat (0, "polarizability");
				reagentAcc.PH = lu.GetFloat (0, "pH");
				reagentAcc.Conductibility = lu.GetFloat (0, "conductibility");
				reagentAcc.Turbidity = lu.GetFloat (0, "turbidity");
				reagentAcc.Refratometer = lu.GetFloat (0, "refraction");
				reagentAcc.compoundColor = new Color32(0,255,255,130);
				
				
				bequer.GetComponent<Glassware> ().IncomingReagent (reagentAcc.Clone (lu.GetFloat(0, "volume")) as Compound, lu.GetFloat(0, "volume"));
			}
			else {
				Destroy (bequer);
			}
			Debug.Log("Printing");
			Debug.Log((bequer.GetComponent<Glassware>().content as Compound).Formula);
			Debug.Log((bequer.GetComponent<Glassware>().content as Compound).Molarity);
			Debug.Log((bequer.GetComponent<Glassware>().content as Compound).Density);
			Debug.Log((bequer.GetComponent<Glassware>().content as Compound).Formula);
			Debug.Log((bequer.GetComponent<Glassware>().content as Compound).Formula);
		}
	}*/
}

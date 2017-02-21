﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// Enum to indicate the types of verification
/// </summary>
public enum TypeOfStep {
	none = 0,				//There's no verification yet
	CompoundClass = 1,		//The class of the compound will be checked
	WhatCompound = 2,		//What compound is in the glassware
	MolarityCheck = 3,		//The amount of compound will be asked
	GlasswareCheck = 4		//It will check the glassware as a whole
};

/// <summary>
/// The class is responsible for every phase transition, and every step within the phase
/// It sends the information for the ResultVerifier class for which interface to set and what to check,
/// and also help defines the number of tabs in the tablet
/// </summary>
public class ProgressController : MonoBehaviour {

	private TypeOfStep stepType;
	public TypeOfStep StepType { get{ return stepType; }}

	private string customPhaseDir = "Assets/Resources/customPhase.json";
	private Dictionary<string,string> phaseDefinitions;
	private bool customMode;

	// The dictionary for phases uses the index as a key to access the step dictionary, which has all values for the current step.
	// The values of the steps are all in string, the conversion has to be done when comparing the values
	private Dictionary<int, Dictionary<string, string>> currentPhase; // < index of step, Dictionary< name of variable, variable's value as a string > >
	private Dictionary<string, string> currentStep;
	private int numberOfSteps;
	private int actualStep;

	// Use this for initialization
	void Start () {
		stepType = TypeOfStep.none;
	}

	void Awake() {
	}

	void OnLevelWasLoaded(int lvlNum) {
		//For testing
		#if UNITY_EDITOR
			if(Application.loadedLevelName == "DemoLabDev"){
				StartCustomMode();
			}
		#endif
	}

	/*void SceneLoadOnStart() {
		if (GameObject.Find ("ProgressController") != null) {
		}
	}*/

	/// <summary>
	/// Starts the campaign mode.
	/// </summary>
	public void StartCampaignMode() {
	}
	
	/// <summary>
	/// Start CustomMode, triggering all the other entities to read the needed information.
	/// </summary>
	private void StartCustomMode() {
		bool glasswareStart = false;
		currentPhase = PhasesSaver.LoadPhases (customPhaseDir);
		phaseDefinitions = PhasesSaver.GetPhaseLibrary (customPhaseDir);

		numberOfSteps = currentPhase.Count;
		actualStep = 0;

		this.NewPhase (bool.Parse (phaseDefinitions ["glasswareStart"]));
	}

	/// <summary>
	/// The method is called to load a new phase
	/// </summary>
	/// <param name="glasswareStart">Glassware start.</param>
	private void NewPhase(bool glasswareStart){
		//Check how to start the phase

		if (glasswareStart) {
			GameObject bequer = Instantiate ((GameObject.Find ("GameController").GetComponent<GameController> ().gameStates [3] as GetGlasswareState).glasswareList [0].gameObject) as GameObject;
			Compound compound = new Compound ();
			compound = CompoundFactory.GetInstance ().GetProduct (phaseDefinitions ["compoundFormula"]);

			/*
			 * Changes on properties according to float.Parse(phaseDefinitions["molarity"])
			 */
				bequer.GetComponent<Glassware> ().IncomingReagent (compound.Clone (float.Parse(phaseDefinitions ["volume"])) as Compound, float.Parse(phaseDefinitions ["volume"]));
		} else {
			// Cupboard test
			/*
			 * Pote de reagente
			 */
		
		}

		//Add steps to Experiment Menu on Tablet


		NewStep();
	}

	/// <summary>
	/// It is called to load the subsequent step of a phase
	/// </summary>
	private void NewStep(){
		actualStep++;
		stepType = (TypeOfStep) int.Parse(currentStep ["stepType"]);
		ResultVerifier.GetInstance().SetVerificationStep(StepType, currentStep);

		//Play starting dialogue according to type of quest, if needed
		switch (StepType) {
		case TypeOfStep.CompoundClass:
			//compoundClassStartAnimation.Show();
			break;
		case TypeOfStep.WhatCompound:
			//whatCompoundStartAnimation.Show();
			break;
		case TypeOfStep.MolarityCheck:
			//molarityCheckStartAnimation.Show();
			break;
		case TypeOfStep.GlasswareCheck:
			//glasswareCheckStartAnimation.Show();
			break;
		}
	}
	
	/// <summary>
	/// It is called when a step is completed, transiting to the next step, or next phase
	/// </summary>
	public void CompleteStep() {
		//Write on .json the answers
		if (numberOfSteps == actualStep) {
			this.PhaseTransition ();
		} else {
		   /*
		 	* Play ending dialogue accoding to step, if needed;
		 	*/
			this.NewStep ();
		}
	}

	/// <summary>
	/// The method is called when a wrong answer is given.
	/// It triggers the animation for wrong answer, and writes the wrong answer that was given.
	/// </summary>
	public void WrongAnswer(){
		//wrongAnswerCanvas.SetActive(true);
	}


	/// <summary>
	/// It makes the transition to the next phase
	/// </summary>
	private void PhaseTransition() {
		if (customMode) { 
			/* FinalAnimation.Show()
			 * EndGame
			 */

			#if UNITY_EDITOR
			Application.LoadLevel ("Menu");
			#else
			Application.LoadLevel ("Menu");
			#endif

		} else {
			/*
			 * LoadNextPhase();
			 */
		}
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

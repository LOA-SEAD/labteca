using UnityEngine;
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

	private string customPhaseDirectory = "customPhase";
	private Dictionary<string,string> phaseDefinitions;
	private bool customMode;

	public CustomModeAnimationsController cutsceneController; //Cutscene controller associated with the CustomMode

	// The dictionary for phases uses the index as a key to access the step dictionary, which has all values for the current step.
	// The values of the steps are all in string, the conversion has to be done when comparing the values
	private Dictionary<int, Dictionary<string, string>> currentPhase; // < index of step, Dictionary< name of variable, variable's value as a string > >
	private int actualPhase;
	public int ActualPhase { get { return actualPhase; } }
	//private Dictionary<string, string> currentStep;
	private int numberOfSteps;
	private int actualStep;
	public int ActualStep { get { return actualStep; } }

	// Use this for initialization
	void Start () {
		//stepType = TypeOfStep.none;
	}

	void Awake() {
		stepType = TypeOfStep.none;
		actualPhase = 0;
		//For testing
		if(Application.loadedLevelName == "DemoLabDev"){
			StartCustomMode();
			cutsceneController = GameObject.Find ("CustomModeAnimations").GetComponent<CustomModeAnimationsController> ();
			GameObject.Find ("Handbook Menu").GetComponent<HandbookMenu> ().RefreshScroll ();
		}
	}

	void OnLevelWasLoaded(int lvlNum) {
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
		customMode = true;
		currentPhase = PhasesSaver.LoadPhases (customPhaseDirectory);
		phaseDefinitions = PhasesSaver.GetPhaseLibrary (customPhaseDirectory);

		for(int i = 0; i < currentPhase.Count; i++) {
			Debug.Log ("PC.CurrentPhase " + i + " = " + currentPhase[i]["typeOfStep"]);
		}

		numberOfSteps = currentPhase.Count;

		this.NewPhase (bool.Parse (phaseDefinitions ["glasswareStart"]));
	}

	/// <summary>
	/// The method is called to load a new phase
	/// </summary>
	/// <param name="glasswareStart">Glassware start.</param>
	private void NewPhase(bool glasswareStart){
		//Check how to start the phase
		actualStep = 0;
		if (glasswareStart) {
			GameObject bequer = Instantiate ((GameObject.Find ("GameController").GetComponent<GameController> ().gameStates [3] as GetGlasswareState).glasswareList [0].gameObject) as GameObject;
			if((GameObject.Find ("GameController").GetComponent<GameController> ().gameStates [1] as WorkBench).TryPutIntoPosition(bequer)) {
				Compound compound = new Compound ();
				compound = CompoundFactory.GetInstance ().GetCompound (phaseDefinitions ["compoundFormula"]);

				/*
				 * Changes on properties according to float.Parse(phaseDefinitions["molarity"])
				 */
				bequer.GetComponent<Glassware> ().IncomingReagent (compound.Clone (float.Parse(phaseDefinitions ["volume"])) as Compound, float.Parse(phaseDefinitions ["volume"]));
				GameObject.Find("InventoryManager").GetComponent<InventoryManager>().AddProductToInventory(bequer.gameObject);
			}
		} else {
			// Cupboard test
			/*
			 * Pote de reagente
			 */
		
		}

		//Add steps to Experiment Menu on Tablet
		GameObject.Find ("Tablet").GetComponentInChildren<NotesState> ().LoadNotes ();
		GameObject.Find ("Experiments Menu").GetComponent<ExperimentMenu> ().RefreshScroll (1); //TODO: RETHINK ABOUT ALL THIS STEP-PHASE CONSISTENCY ON JOURNAL
		NewStep();
	}

	/// <summary>
	/// It is called to load the subsequent step of a phase
	/// </summary>
	private void NewStep(){
		stepType = (TypeOfStep) int.Parse(currentPhase[actualStep]["typeOfStep"]);
		ResultVerifier.GetInstance().SetVerificationStep(StepType, currentPhase[actualStep]);

		//GameObject.Find ("Experiments Menu").GetComponent<ExperimentMenu> ().ActivatePhaseTab (actualPhase);
		//GameObject.Find ("Journal State").GetComponent<JournalController> ().ActivateStepTab (actualPhase, actualStep);

		if (StepType == TypeOfStep.CompoundClass) {
			Debug.Log ("1 = CompoundClass");
		}
		if (StepType == TypeOfStep.WhatCompound) {
			Debug.Log ("2 = WhatCompound");
		}
		if (StepType == TypeOfStep.MolarityCheck) {
			Debug.Log ("3 = MolarityCheck");
		}
		if (StepType == TypeOfStep.GlasswareCheck) {
			Debug.Log ("4 = GlasswareCheck");
		}

		//Play starting dialogue according to type of quest, if needed
		cutsceneController.PlayTransitionCutscene ();
	}
	
	/// <summary>
	/// It is called when a step is completed, transiting to the next step, or next phase
	/// </summary>
	public void CompleteStep() {
		Debug.Log ("Completing step");
		//Write on .json the answers
		if ((numberOfSteps - 1) == actualStep) {
			cutsceneController.PlayEndingScene();
			//Debug.Log("PhaseTransition will be called");
			//this.PhaseTransition ();
		} else {
			GameObject.Find("JournalUIItem"+actualStep).GetComponent<JournalUIItem>().checkItem();
			actualStep++;
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
		cutsceneController.PlayWrongAnswerScene ();
	}


	/// <summary>
	/// It makes the transition to the next phase
	/// </summary>
	public void PhaseTransition() {
		Debug.Log ("PhaseTransition");
		if (customMode) { 
			/* FinalAnimation.Show()
			 * EndGame
			 */
			Debug.Log ("Phase Transition for custom mode");

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

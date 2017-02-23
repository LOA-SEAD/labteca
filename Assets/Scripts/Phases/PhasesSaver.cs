﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

//! I/O class for the Phases.
// The phases have all variables. If any variable was not assigned, the value will be saved as "null"
// and ignored in the comparison.
public class PhasesSaver {

	private static TextEdit text = new TextEdit("Assets/Resources/phases.txt");
	
	//Loads the reactions from a .txt file, and returns a dictionary of phases
	/*public static Dictionary<int, Dictionary<string, string>> LoadPhases()
	{
		TextEdit textLoad = new TextEdit ("Assets/Resources/phases.txt");
		
		int numberOfPhases = text.GetInt ("numberOfPhases");

		Dictionary<int, Dictionary<string, string>> Phases = new Dictionary<int, Dictionary<string, string>>();
		
		if (numberOfPhases > 0) 
		{
			for (int i = 0; i < numberOfPhases; i++) 
			{
				Dictionary<string, string> phaseValues = new Dictionary<string, string>();

				phaseValues["productFormula"] = textLoad.GetString ("productFormula" + i.ToString()); //TODO: Needs testing
				phaseValues["molarity"] = textLoad.GetString ("molarity" + i.ToString());
				phaseValues["minVolume"] = textLoad.GetString ("minVolume" + i.ToString());
				phaseValues["density"] = textLoad.GetString ("density" + i.ToString());
				phaseValues["turbidity"] = textLoad.GetString ("turbidity" + i.ToString());
				phaseValues["conductibility"] = textLoad.GetString ("conductibility" + i.ToString());


				Phases.Add(i, phaseValues);
			}
		}
		return Phases;
	}*/

	//Loads the phase from a .json file, and returns a dictionary of phases
	public static Dictionary<int, Dictionary<string, string>> LoadPhases () {
		JSONEdit jsonEditor = new JSONEdit("Assets/Resources/phases.json");
		
		Dictionary<int, Dictionary<string, string>> phase = new Dictionary<int, Dictionary<string, string>> ();
		
		for (int i = 0; i < jsonEditor.NumberOfObjects(); i++) {
			Dictionary<string, string> stepValues = new Dictionary<string, string> ();
			
			//stepValues["typeOfStep"] = josnEdito.GetString(i, "typeOfStep");

			stepValues["productFormula"] = jsonEditor.GetString(i, "productFormula");
			stepValues["molarity"] = jsonEditor.GetString(i, "molarity");
			stepValues["minVolume"] = jsonEditor.GetString(i, "minVolume");
			stepValues["maxError"] = jsonEditor.GetString(i, "maxError");
			stepValues["density"] = jsonEditor.GetString(i, "density");
			stepValues["turbidity"] = jsonEditor.GetString(i, "turbidity");
			stepValues["conductibility"] = jsonEditor.GetString(i, "conductibility");
			
			phase.Add (i, stepValues);
		}
		
		return phase;
	}

	//Loads the phase from a .json file, and returns a dictionary of phases
	public static Dictionary<int, Dictionary<string, string>> LoadPhases (string directory) {
		JSONEdit jsonEditor = new JSONEdit(directory);
		
		Dictionary<int, Dictionary<string, string>> phase = new Dictionary<int, Dictionary<string, string>> ();

		Debug.Log ("Number of steps = " + jsonEditor.NumberOfObjects ());
		for (int i = 0; i < jsonEditor.NumberOfObjects(); i++) {
			phase.Add (i, PhasesSaver.LoadStep(jsonEditor, i));
			Debug.Log("Adicionando step = " + phase[i]["typeOfStep"]);
		}
		return phase;
	}

	//Load the step, putting it into a Dictironary
	private static Dictionary<string, string> LoadStep(JSONEdit jsonEditor, int i) {
		
		Dictionary<string, string> step = new Dictionary<string, string> ();
			
		step.Add("typeOfStep", jsonEditor.GetString(i, "typeOfStep"));

		switch (step ["typeOfStep"]) {
		case "1":
			step.Add("option1", jsonEditor.GetString(i, "option1"));
			step.Add("option2", jsonEditor.GetString(i, "option2"));
			step.Add("option3", jsonEditor.GetString(i, "option3"));
			step.Add("option4", jsonEditor.GetString(i, "option4"));
			step.Add("option5", jsonEditor.GetString(i, "option5"));
			step.Add("correctAnswer", jsonEditor.GetString(i, "correctAnswer"));
			break;
		case "2":
			step.Add("correctAnswer", jsonEditor.GetString(i, "correctAnswer")); //Compound Formula
			break;
		case "3":
			step.Add("compoundFormula", jsonEditor.GetString(i, "compoundFormula"));
			step.Add("correctAnswer", jsonEditor.GetString(i, "correctAnswer")); //Molarity
			step.Add("maxError", jsonEditor.GetString(i, "maxError"));
			break;
		case "4":
			step.Add("compoundFormula", jsonEditor.GetString(i, "compoundFormula"));
			step.Add("molarity", jsonEditor.GetString(i, "molarity"));
			step.Add("minVolume", jsonEditor.GetString(i, "minVolume"));
			step.Add("maxError", jsonEditor.GetString(i, "maxError"));
			break;
		}
						
		return step;
	}

	public static Dictionary<string, string> GetPhaseLibrary(string directory) {
		JSONEdit jsonEditor = new JSONEdit(directory);
		
		Dictionary<string, string> library = new Dictionary<string, string> ();

		library.Add("glasswareStart", jsonEditor.GetMainValue("glasswareStart"));
		library.Add("compoundFormula", jsonEditor.GetMainValue("compoundFormula"));
		library.Add("volume", jsonEditor.GetMainValue("volume"));
		library.Add("molarity", jsonEditor.GetMainValue("molarity"));

		return library;
	}
}

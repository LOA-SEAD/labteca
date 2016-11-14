using UnityEngine;
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

	//Loads the reactions from a .json file, and returns a dictionary of phases
	public static Dictionary<int, Dictionary<string, string>> LoadPhases () {

		//SimpleJSON.JSONNode phases = JSONEdit.Read ("Assets/Resources/phases.json");
		JSONEdit jsonEditor = new JSONEdit("Assets/Resources/phases.json");

		Dictionary<int, Dictionary<string, string>> allPhases = new Dictionary<int, Dictionary<string, string>> ();
	
		for (int i = 0; i < jsonEditor.NumberOfObjects(); i++) {
			Dictionary<string, string> phaseValues = new Dictionary<string, string> ();

			phaseValues["productFormula"] = jsonEditor.GetString(i, "productFormula");
			phaseValues["molarity"] = jsonEditor.GetString(i, "molarity");
			phaseValues["minVolume"] = jsonEditor.GetString(i, "minVolume");
			phaseValues["maxError"] = jsonEditor.GetString(i, "maxError");
			phaseValues["density"] = jsonEditor.GetString(i, "density");
			phaseValues["turbidity"] = jsonEditor.GetString(i, "turbidity");
			phaseValues["conductibility"] = jsonEditor.GetString(i, "conductibility");

			allPhases.Add (i, phaseValues);
		}

		return allPhases;
	}
}

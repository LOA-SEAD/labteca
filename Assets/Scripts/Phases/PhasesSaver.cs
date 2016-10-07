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

		SimpleJSON.JSONNode phases = JSONEdit.Read ("Assets/Resources/phases.json");

		Dictionary<int, Dictionary<string, string>> allPhases = new Dictionary<int, Dictionary<string, string>> ();
	
		for (int i = 0; i < phases["phases"].Count; i++) {
			Dictionary<string, string> phaseValues = new Dictionary<string, string> ();

			phaseValues["productFormula"] = phases["phases"][i]["productFormula"].Value;
			phaseValues["molarity"] = phases["phases"][i]["molarity"].Value;
			phaseValues["minVolume"] = phases["phases"][i]["minVolume"].Value;
			phaseValues["maxError"] = phases["phases"][i]["maxError"].Value;
			phaseValues["density"] = phases["phases"][i]["density"].Value;
			phaseValues["turbidity"] = phases["phases"][i]["turbidity"].Value;
			phaseValues["conductibility"] = phases["phases"][i]["conductibility"].Value;

			allPhases.Add (i, phaseValues);
		}

		return allPhases;
	}
}

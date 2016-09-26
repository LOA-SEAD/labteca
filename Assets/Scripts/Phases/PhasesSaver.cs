using UnityEngine;
using System.Collections;
using System.Collections.Generic;

//! I/O class for the Phases.
// The phases have all variables. If any variable was not assigned, the value will be saved as "null"
// and ignored in the comparison.
public class PhasesSaver {

	private static TextEdit text = new TextEdit("Assets/Resources/phases.txt");
	
	//Loads the reactions from a file, and returns a dictionary of phases
	public static Dictionary<int, Dictionary<string, string>> LoadPhases()
	{
		TextAsset loadText = Resources.Load("phases") as TextAsset;
		
		TextEdit textLoad = new TextEdit(loadText);
		
		int numberOfPhases = text.GetInt ("numberOfPhases");

		Dictionary<int, Dictionary<string, string>> Phases = new Dictionary<int, Dictionary<string, string>>();
		
		if (numberOfPhases > 0) 
		{
			for (int i = 0; i < numberOfPhases; i++) 
			{
				Dictionary<string, string> phaseValues = new Dictionary<string, string>();

				phaseValues["productFormula"] = textLoad.GetString ("productFormula" + i.ToString()); //TODO: Needs testing

				/*
				READS ALL THE VALUES.

				phaseValues.Add(textLoad.GetString([]what is before the "="[], i.ToString());
				*/
			}
		}
		return Phases;
	}
}

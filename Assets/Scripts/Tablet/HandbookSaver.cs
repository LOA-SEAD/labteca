﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class HandbookSaver : MonoBehaviour {

	private static Dictionary<string, Dictionary<string, string>> handbookReagents;

	/// <summary>
	/// Returns the handbook of reagents.
	/// </summary>
	/// If not yet created, the method invokes the LoadHandbook method
	public static Dictionary<string, Dictionary<string, string>> GetHandbook() {
		if (handbookReagents != null) {
			return handbookReagents;
		} else {
			handbookReagents = LoadHandbook();
			return GetHandbook();
		}
	}

	//Loads the phase from a .json file, and returns a dictionary of phases
	private static Dictionary<string, Dictionary<string, string>> LoadHandbook () {
		JSONEditor jsonEditor = new JSONEditor ("handbook", true);
		
		Dictionary<string, Dictionary<string, string>> handbook = new Dictionary<string, Dictionary<string, string>> ();

		for (int i = 0; i < jsonEditor.NumberOfObjects(); i++) {
			handbook.Add (jsonEditor.GetString (i, "formula"), HandbookSaver.LoadReagentValues(jsonEditor, i));
		}
		return handbook;
	}
	
	//Load the values of eache reagent, putting it into a Dictionary
	private static Dictionary<string, string> LoadReagentValues(JSONEditor jsonEditor, int i) {
		
		Dictionary<string, string> reagentValues = new Dictionary<string, string> ();

		reagentValues.Add("formula", 		jsonEditor.GetString(i, "formula"));
		reagentValues.Add("name", 			jsonEditor.GetString(i, "name"));
		/*reagentValues.Add("density",		jsonEditor.GetString(i, "density"));
		reagentValues.Add("pH", 			jsonEditor.GetString(i, "pH"));
		reagentValues.Add("conductibility", jsonEditor.GetString(i, "conductibility"));
		reagentValues.Add("turbidity", 		jsonEditor.GetString(i, "turbidity"));
		reagentValues.Add("polarizability", jsonEditor.GetString(i, "polarizability"));
		reagentValues.Add("NaConcentration",jsonEditor.GetString(i, "NaConcentration"));
		reagentValues.Add("KConcentration", jsonEditor.GetString(i, "KConcentration"));
		reagentValues.Add("LiConcentration",jsonEditor.GetString(i, "LiConcentration"));*/
		reagentValues.Add("description", 	jsonEditor.GetString (i, "description"));
		
		return reagentValues;
	}
}

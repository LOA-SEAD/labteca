using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PhasesSaver {

	private static TextEdit text = new TextEdit("Assets/Resources/phases.txt");
	
	//Loads the reactions from a file, and returns a dictionary
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
				/*
				reaction.name = textLoad.GetString("name" + i.ToString ());
				
				reaction.reagent1 = textLoad.GetString("aName" + i.ToString ());
				reaction.reagent2 = textLoad.GetString("bName" + i.ToString ());
				reaction.mainProduct = textLoad.GetString("cName" + i.ToString ());
				reaction.subProduct = textLoad.GetString("dName" + i.ToString ());
				
				reaction.stoichiometryR1 = textLoad.GetInt("aMultiply" + i.ToString());
				reaction.stoichiometryR2 = textLoad.GetInt("bMultiply" + i.ToString());
				reaction.stoichiometryMainProduct = textLoad.GetInt("cMultiply" + i.ToString());
				reaction.stoichiometrySubProduct = textLoad.GetInt("dMultiply" + i.ToString());
				
				Phases.Add(reaction.name, reaction);

				NEEDS A WAY OF READING EACH LINE SEPARETLY, AND GETTING ONLY WHAT COMES BEFORE THE "=" SO THAT WILL BE THE KEY FOR THE VALUE.

				phaseValues.Add(textLoad.GetString([]what is before the "="[], i.ToString());
				*/
			}
		}
		return Phases;
	}
}

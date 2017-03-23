using UnityEngine;
using System.Collections;
using System.Collections.Generic;

//! Saves a Reaction and it's properties in a text file.

public class ReactionsSaver {

	//Loads the reactions from a file, and returns a dictionary
	public static Dictionary<string, ReactionClass> LoadReactions()
	{
		JSONEditor json = new JSONEditor("reactions");
		
		int numberOfReactions = json.NumberOfObjects ();
		
		Dictionary<string, ReactionClass> Reagents = new Dictionary<string, ReactionClass>();
		
		if (numberOfReactions > 0) 
		{
			for (int i = 0; i < numberOfReactions; i++) 
			{
				ReactionClass reaction = new ReactionClass();

				reaction.name = json.GetString(i,"name");

				reaction.reagent1 = json.GetString(i, "aName");
				reaction.reagent2 = json.GetString(i, "bName");
				reaction.mainProduct = json.GetString(i, "cName");
				reaction.subProduct = json.GetString(i, "dName");

				reaction.stoichiometryR1 = json.GetInt(i, "aMultiply");
				reaction.stoichiometryR2 = json.GetInt(i, "bMultiply");
				reaction.stoichiometryMainProduct = json.GetInt(i, "cMultiply");
				reaction.stoichiometrySubProduct = json.GetInt(i, "dMultiply");

				Reagents.Add(reaction.name, reaction);
			}
		}
		return Reagents;
	}
}

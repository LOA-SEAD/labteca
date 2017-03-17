using UnityEngine;
using System.Collections;
using System.Collections.Generic;

//! Saves a Reaction and it's properties in a text file.

public class ReactionsSaver 
{
	private static TextEdit text = new TextEdit("Assets/Resources/reactions.txt");
	//Method for saving the reaction made in the tab "Reacao/Criar reacao" in the unity editor
	public static void SaveReactionsFromEditor(string name, int aMultiply, string aName,
	                                           				int bMultiply, string bName,
	                                           				int cMultiply, string cName,	
	                                           				int dMultiply, string dName)
	{
		
		Dictionary<string, ReactionClass> reactions = LoadReactions();
		
		ReactionClass reaction = new ReactionClass();

		reaction.name = name;

		reaction.reagent1 = aName;
		reaction.reagent2 = bName;
		reaction.mainProduct = cName;
		reaction.subProduct = dName;

		reaction.stoichiometryR1 = aMultiply;
		reaction.stoichiometryR2 = bMultiply;
		reaction.stoichiometryMainProduct = cMultiply;
		reaction.stoichiometrySubProduct = dMultiply;
		
		if (!reactions.ContainsKey(reaction.name)) 
		{
			reactions.Add(reaction.name, reaction);
			SaveReactions(reactions);
			Debug.Log ("Reaçao Salva Com Sucesso!");
		} 
		else 
		{
			Debug.Log ("Reaçao ja inserido na base de dados!");	
		}
	}

	//Saves the reactions from a dictionary to the text file
	public static void SaveReactions(Dictionary<string, ReactionClass> reactions)
	{
		text.ClearFile ();
		
		text.SetInt ("numberOfReactions", reactions.Count);
		
		int counter = 0;
		foreach (ReactionClass reaction in reactions.Values)
		{
			text.SetString("name" + counter.ToString(), reaction.name);

			text.SetString("aName" + counter.ToString(), reaction.reagent1);
			text.SetString("bName" + counter.ToString(), reaction.reagent2);
			text.SetString("cName" + counter.ToString(), reaction.mainProduct);
			text.SetString("dName" + counter.ToString(), reaction.subProduct);

			text.SetInt("aMultiply" + counter.ToString(), reaction.stoichiometryR1);
			text.SetInt("bMultiply" + counter.ToString(), reaction.stoichiometryR2);
			text.SetInt("cMultiply" + counter.ToString(), reaction.stoichiometryMainProduct);
			text.SetInt("dMultiply" + counter.ToString(), reaction.stoichiometrySubProduct);
			
			counter++;
		}
	}
	//Loads the reactions from a file, and returns a dictionary
	public static Dictionary<string, ReactionClass> LoadReactions()
	{
		JSONEditor json = JSONEdit.NewJSONEditor("Assets/Resources/reactions.txt"); //new JSONEdit("Assets/Resources/reactions.txt");
		
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

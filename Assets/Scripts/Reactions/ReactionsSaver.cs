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

		reaction.aName = aName;
		reaction.bName = bName;
		reaction.cName = cName;
		reaction.dName = dName;

		reaction.aMultipler = aMultiply;
		reaction.bMultipler = bMultiply;
		reaction.cMultipler = cMultiply;
		reaction.dMultipler = dMultiply;
		
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

			text.SetString("aName" + counter.ToString(), reaction.aName);
			text.SetString("bName" + counter.ToString(), reaction.bName);
			text.SetString("cName" + counter.ToString(), reaction.cName);
			text.SetString("dName" + counter.ToString(), reaction.dName);

			text.SetInt("aMultiply" + counter.ToString(), reaction.aMultipler);
			text.SetInt("bMultiply" + counter.ToString(), reaction.bMultipler);
			text.SetInt("cMultiply" + counter.ToString(), reaction.cMultipler);
			text.SetInt("dMultiply" + counter.ToString(), reaction.dMultipler);
			
			counter++;
		}
	}
	//Loads the reactions from a file, and returns a dictionary
	public static Dictionary<string, ReactionClass> LoadReactions()
	{
		TextAsset loadText = Resources.Load("reactions") as TextAsset;
		
		TextEdit textLoad = new TextEdit(loadText);
		
		int numberOfReactions = text.GetInt ("numberOfReactions");
		
		Dictionary<string, ReactionClass> Reagents = new Dictionary<string, ReactionClass>();
		
		if (numberOfReactions > 0) 
		{
			for (int i = 0; i < numberOfReactions; i++) 
			{
				ReactionClass reaction = new ReactionClass();

				reaction.name = textLoad.GetString("name" + i.ToString ());

				reaction.aName = textLoad.GetString("aName" + i.ToString ());
				reaction.bName = textLoad.GetString("bName" + i.ToString ());
				reaction.cName = textLoad.GetString("cName" + i.ToString ());
				reaction.dName = textLoad.GetString("dName" + i.ToString ());

				reaction.aMultipler = textLoad.GetInt("aMultiply" + i.ToString());
				reaction.bMultipler = textLoad.GetInt("bMultiply" + i.ToString());
				reaction.cMultipler = textLoad.GetInt("cMultiply" + i.ToString());
				reaction.dMultipler = textLoad.GetInt("dMultiply" + i.ToString());

				Reagents.Add(reaction.name, reaction);
			}
		}
		return Reagents;
	}
}

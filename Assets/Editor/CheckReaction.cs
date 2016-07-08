using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

public class CheckReaction : EditorWindow 
{
	private int indexOfReactions;
	private string selectedReaction = "";
	private string selectedReactionLastInteraction = "";
	private bool selected = false;
	private ReactionClass reaction;


	private Vector2 scrollPosition;

	[MenuItem ("Reacao/Checar Reacao")]
	static void CreateWindow() 
	{
		CheckReaction window = EditorWindow.GetWindow(typeof(CheckReaction),true,"Checar Reacao") as CheckReaction;
		window.minSize = new Vector2(600f,200f);
		window.maxSize = new Vector2(600f,200f);
	}
	
	void OnGUI()
	{
		EditorGUILayout.BeginVertical();
		scrollPosition = EditorGUILayout.BeginScrollView(scrollPosition, GUILayout.Width(0), GUILayout.Height(0));

		EditorGUILayout.LabelField("Checar Reacao:");

		Dictionary<string, ReactionClass> reactions = ReactionsSaver.LoadReactions();

		string[] names = new string[reactions.Count];

		int counter = 0;
		foreach (string name in reactions.Keys) 
		{
			names[counter] = name;
			counter++;
		}

		indexOfReactions = EditorGUILayout.Popup(indexOfReactions,names);

		selectedReaction = names[indexOfReactions];

		if (selectedReaction != selectedReactionLastInteraction) 
		{
			selected = false;
		}

		selectedReactionLastInteraction = selectedReaction;

		if(!selected)
		{
			if(GUILayout.Button("Checar"))
			{
				selected = true;
				reaction = reactions[names[indexOfReactions]];
			}
		}
	
		EditorGUILayout.Space();
		EditorGUILayout.Space();

		if(selected)
		{
			EditorGUILayout.LabelField("Nome: " + reaction.name);

			EditorGUILayout.Space();

			string bString = "";
			if(reaction.stoichiometryR2 != 0)
			{
				bString = " + " + reaction.stoichiometryR2.ToString() + " " + reaction.reagent2;
			}

			string dString = "";
			if(reaction.stoichiometrySubProduct != 0)
			{
				dString = " + " + reaction.stoichiometrySubProduct.ToString() + " " + reaction.subProduct;
			}

			EditorGUILayout.LabelField(reaction.stoichiometryR1.ToString() + " " + reaction.reagent1 + bString + " = " +  reaction.stoichiometryMainProduct.ToString() + " " + reaction.mainProduct + dString);
		}

		EditorGUILayout.EndScrollView();
		EditorGUILayout.EndVertical();
	}
}

using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

public class DeleteReaction : EditorWindow 
{
	private int indexOfReactions;
	private string selectedReaction = "";
	private string selectedReactionLastInteraction = "";
	private bool selected = false;
	private ReactionClass reaction;

	private bool deleteSelected = false;

	private Vector2 scrollPosition;

	[MenuItem ("Reacao/Deletar Reacao")]
	static void CreateWindow() 
	{
		DeleteReaction window = EditorWindow.GetWindow(typeof(DeleteReaction),true,"Deletar Reacao") as DeleteReaction;
		window.minSize = new Vector2(600f,200f);
		window.maxSize = new Vector2(600f,200f);
	}
	
	void OnGUI()
	{
		EditorGUILayout.BeginVertical();
		scrollPosition = EditorGUILayout.BeginScrollView(scrollPosition, GUILayout.Width(0), GUILayout.Height(0));

		EditorGUILayout.LabelField("Deletar Reacao:");

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

			EditorGUILayout.Space();

			if(!deleteSelected)
			{
				if(GUILayout.Button("DELETAR!"))
				{
					deleteSelected = true;
				}
			}
			else
			{
				EditorGUILayout.LabelField("Deletar?");
				if(GUILayout.Button("Sim"))
				{
					Dictionary<string, ReactionClass> allReactions = ReactionsSaver.LoadReactions();
					
					allReactions.Remove(names[indexOfReactions]);
//					ReactionsSaver.SaveReactions(allReactions);
					
					Debug.Log("Reaçao " + names[indexOfReactions] + " Removida com sucesso!");
					
					this.Close();
				}
				if(GUILayout.Button("Nao"))
				{
					this.Close();
				}
			}
		}

		EditorGUILayout.EndScrollView();
		EditorGUILayout.EndVertical();
	}
}

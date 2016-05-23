using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

public class DeleteComponent : EditorWindow 
{
	private int indexOfComponent;
	private string selectedComponent = "";
	private string selectedComponentLastInteraction = "";
	private bool selected = false;
	private Compound reagent;

	private bool deleteSelected = false;

	private Vector2 scrollPosition;

	[MenuItem ("Reagente/Deletar Reagente")]
	static void CreateWindow() 
	{
		DeleteComponent window = EditorWindow.GetWindow(typeof(DeleteComponent),true,"Deletar Reagente") as DeleteComponent;
		window.minSize = new Vector2(300f,500f);
		window.maxSize = new Vector2(300f,500f);
	}
	
	void OnGUI()
	{
		EditorGUILayout.BeginVertical();
		scrollPosition = EditorGUILayout.BeginScrollView(scrollPosition, GUILayout.Width(0), GUILayout.Height(0));

		EditorGUILayout.LabelField("Deletar Reagente:");

		Dictionary<string, Compound> reagents = ComponentsSaver.LoadReagents();

		string[] names = new string[reagents.Count];

		int counter = 0;
		foreach (string name in reagents.Keys) 
		{
			names[counter] = name;
			counter++;
		}

		indexOfComponent = EditorGUILayout.Popup(indexOfComponent,names);

		selectedComponent = names[indexOfComponent];

		if (selectedComponent != selectedComponentLastInteraction) 
		{
			selected = false;
		}

		selectedComponentLastInteraction = selectedComponent;

		if(!selected)
		{
			if(GUILayout.Button("Checar"))
			{
				selected = true;
				deleteSelected = false;
				reagent = reagents[names[indexOfComponent]] as Compound;
			}
		}

		if(selected)
		{
			EditorGUILayout.LabelField("Nome: " + reagent.name);
			EditorGUILayout.LabelField("Massa Molar : " + reagent.molarMass.ToString());
			EditorGUILayout.LabelField("Densidade : " + reagent.density.ToString());
			EditorGUILayout.LabelField("Polaridade: " + reagent.polarizability.ToString());
			EditorGUILayout.LabelField("Condutividade: " + reagent.conductibility.ToString());
			EditorGUILayout.LabelField("Solubilidade: " + reagent.solubility.ToString());

			if(!reagent.isSolid) {
				EditorGUILayout.LabelField("Ph : " + (reagent as Compound).pH.ToString());
				EditorGUILayout.LabelField("Turbilidade: " + (reagent as Compound).turbidity.ToString());
				EditorGUILayout.LabelField("Refratometro: " + (reagent as Compound).refratometer.ToString());

				if((reagent as Compound).hplc != null) {
					EditorGUILayout.LabelField("HPLC = " + (reagent as Compound).hplc.ToString());
				} else {
					EditorGUILayout.LabelField ("HPLC = ");
				}
			}
	
			if(reagent.irSpecter != null) {
				EditorGUILayout.LabelField("Espectro IR : " + reagent.irSpecter.name);
			} else {
				EditorGUILayout.LabelField("Espectro IR : ");
			}

			if(reagent.flameSpecter != null) {
				EditorGUILayout.LabelField("Espectro de Chama: " + reagent.flameSpecter.name);
			} else {
				EditorGUILayout.LabelField("Espectro de Chama: ");
			}
			
			if(reagent.uvSpecter != null) {
				EditorGUILayout.LabelField("Espectro UV : " + reagent.uvSpecter.name);
			} else {
				EditorGUILayout.LabelField("Espectro UV : ");
			}

		/*	if(reagent.texture != null)
			{
				EditorGUILayout.LabelField("Textura: " + reagent.texture.name);
			}
			else
			{
				EditorGUILayout.LabelField("Textura: ");
			}*/
			//EditorGUILayout.ColorField("Cor: ", reagent.color);

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
					Dictionary<string, Compound> allReagents = ComponentsSaver.LoadReagents();

					allReagents.Remove(names[indexOfComponent]);
					ComponentsSaver.SaveReagents(allReagents);

					Debug.Log("Reagente " + names[indexOfComponent] + " Removido com sucesso!");

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

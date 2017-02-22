using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

public class CheckProduct : EditorWindow 
{
	private int indexOfComponent;
	private string selectedComponent = "";
	private string selectedComponentLastInteraction = "";
	private bool selected = false;
	private Compound product;
	
	
	private Vector2 scrollPosition;
	
	[MenuItem ("Produto/Checar Produto")]
	static void CreateWindow() 
	{
		CheckProduct window = EditorWindow.GetWindow(typeof(CheckProduct),true,"Checar Produto") as CheckProduct;
		window.minSize = new Vector2(300f,500f);
		window.maxSize = new Vector2(300f,500f);
	}
	
	void OnGUI()
	{
		EditorGUILayout.BeginVertical();
		scrollPosition = EditorGUILayout.BeginScrollView(scrollPosition, GUILayout.Width(0), GUILayout.Height(0));
		
		EditorGUILayout.LabelField("Checar Produto:");
		
		Dictionary<string, Compound> products = CompoundFactory.GetInstance().Collection;
		string[] names = new string[products.Count];
		
		int counter = 0;
		foreach (string name in products.Keys) 
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
				product = products[names[indexOfComponent]];
			}
		}
		
		if(selected)
		{
			EditorGUILayout.LabelField("Nome: " + product.Name);
			EditorGUILayout.LabelField("Formula :" + product.Formula);
			EditorGUILayout.LabelField("Massa Molar : " + product.MolarMass.ToString());
			EditorGUILayout.LabelField("Densidade : " + product.Density.ToString());
			EditorGUILayout.LabelField("Pureza : " + product.Purity.ToString());
			EditorGUILayout.LabelField("Polaridade: " + product.Polarizability.ToString());
			EditorGUILayout.LabelField("Condutividade: " + product.Conductibility.ToString());
			EditorGUILayout.LabelField("Solubilidade: " + product.Solubility.ToString());
			
			if(!product.IsSolid) {
				EditorGUILayout.LabelField("Ph : " + (product as Compound).PH.ToString());
				EditorGUILayout.LabelField("Turbilidade: " + (product as Compound).Turbidity.ToString());
				EditorGUILayout.LabelField("Refratometro: " + (product as Compound).Refratometer.ToString());
				EditorGUILayout.LabelField("Espectro de Chama: " + product.FlameSpecter.ToString());
				
				if((product as Compound).hplc != null) {
					EditorGUILayout.LabelField("HPLC : " + (product as Compound).hplc);
				} else {
					EditorGUILayout.LabelField("HPLC : ");
				}
				
			}
			
			if(product.uvSpecter != null) {
				EditorGUILayout.LabelField("Espectro UV : " + product.uvSpecter.name);
			} else {
				EditorGUILayout.LabelField("Espectro UV : ");
			}
			
			if(product.irSpecter != null) {
				EditorGUILayout.LabelField("Espectro IR : " + product.irSpecter.name);
			} else {
				EditorGUILayout.LabelField("Espectro IR : ");
			}
			
			
			
			/*if(product.texture != null)
			{
				EditorGUILayout.LabelField("Textura: " + product.texture.name);
			}
			else
			{
				EditorGUILayout.LabelField("Textura: ");
			}
			EditorGUILayout.ColorField("Cor: ", product.color);*/
		}
		
		EditorGUILayout.EndScrollView();
		EditorGUILayout.EndVertical();
	}
}
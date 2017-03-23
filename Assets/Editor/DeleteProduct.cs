using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

public class DeleteProduct : EditorWindow 
{
	private int indexOfProduct;
	private string selectedProduct = "";
	private string selectedProductLastInteraction = "";
	private bool selected = false;
	private Compound product;
	
	private bool deleteSelected = false;
	
	private Vector2 scrollPosition;
	
	[MenuItem ("Produto/Deletar Produto")]
	static void CreateWindow() 
	{
		DeleteProduct window = EditorWindow.GetWindow(typeof(DeleteProduct),true,"Deletar Produto") as DeleteProduct;
		window.minSize = new Vector2(300f,500f);
		window.maxSize = new Vector2(300f,500f);
	}
	
	void OnGUI()
	{
		EditorGUILayout.BeginVertical();
		scrollPosition = EditorGUILayout.BeginScrollView(scrollPosition, GUILayout.Width(0), GUILayout.Height(0));
		
		EditorGUILayout.LabelField("Deletar Produto:");
		
		Dictionary<string, Compound> products = CompoundFactory.GetInstance ().Collection;
		
		string[] names = new string[products.Count];
		
		int counter = 0;
		foreach (string name in products.Keys) 
		{
			names[counter] = name;
			counter++;
		}
		
		indexOfProduct = EditorGUILayout.Popup(indexOfProduct,names);
		
		selectedProduct = names[indexOfProduct];
		
		if (selectedProduct != selectedProductLastInteraction) 
		{
			selected = false;
		}
		
		selectedProductLastInteraction = selectedProduct;
		
		if(!selected)
		{
			if(GUILayout.Button("Checar"))
			{
				selected = true;
				deleteSelected = false;
				product = products[names[indexOfProduct]] as Compound;
			}
		}
		
		if(selected)
		{
			EditorGUILayout.LabelField("Nome: " + product.Name);
			EditorGUILayout.LabelField("Massa Molar : " + product.MolarMass.ToString());
			EditorGUILayout.LabelField("Densidade : " + product.Density.ToString());
			EditorGUILayout.LabelField("Purity : " + product.Purity.ToString());
			EditorGUILayout.LabelField("Polaridade: " + product.Polarizability.ToString());
			EditorGUILayout.LabelField("Condutividade: " + product.Conductibility.ToString());
			EditorGUILayout.LabelField("Solubilidade: " + product.Solubility.ToString());
			
			if(!product.IsSolid) {
				EditorGUILayout.LabelField("Ph : " + (product as Compound).PH.ToString());
				EditorGUILayout.LabelField("Turbilidade: " + (product as Compound).Turbidity.ToString());
				EditorGUILayout.LabelField("Refratometro: " + (product as Compound).Refratometer.ToString());
				EditorGUILayout.LabelField("Espectro de Chama: " + product.FlameSpecter);
				
				if((product as Compound).hplc != null) {
					EditorGUILayout.LabelField("HPLC = " + (product as Compound).hplc.ToString());
				} else {
					EditorGUILayout.LabelField ("HPLC = ");
				}
			}
			
			if(product.irSpecter != null) {
				EditorGUILayout.LabelField("Espectro IR : " + product.irSpecter.name);
			} else {
				EditorGUILayout.LabelField("Espectro IR : ");
			}
			
			if(product.uvSpecter != null) {
				EditorGUILayout.LabelField("Espectro UV : " + product.uvSpecter.name);
			} else {
				EditorGUILayout.LabelField("Espectro UV : ");
			}
			
			/*	if(product.texture != null)
			{
				EditorGUILayout.LabelField("Textura: " + product.texture.name);
			}
			else
			{
				EditorGUILayout.LabelField("Textura: ");
			}*/
			
			EditorGUILayout.ColorField("Cor: ", product.compoundColor);
			
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
					Dictionary<string, Compound> allProducts = CompoundFactory.GetInstance().CupboardCollection;
					
					allProducts.Remove(names[indexOfProduct]);
//					ComponentsSaver.SaveProducts(allProducts);
					
					Debug.Log("Produto " + names[indexOfProduct] + " Removido com sucesso!");
					
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
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

//! Saves the component information from editor.
/*!
 * Contains three methods that saves the liquidReagent from editor in a dictionary and in a file  
 * and load the dictionary from file.
 */

public class ComponentsSaver : MonoBehaviour
{
	//! Saves the component information in a Dictionary. 
	public static void SaveReagentFromEditor(string name,string formula ,bool isSolid, float molarMass, float purity, float density, float pH, float polarizability, 
	                                         Texture2D uvSpecter, Texture2D irSpecter, float flameSpecter, float conductibility, 
	                                         float solubility, float turbidity, Texture2D hplc, float refratometer, bool fumeHoodOnly, Texture2D texture, Color32 color)
	{

		Dictionary<string, Compound> reagents = LoadCupboardCompounds ();


		Compound reagent = new Compound();
		reagent.IsSolid = isSolid ;
		reagent.Name = name;
		reagent.Formula = formula;
		reagent.MolarMass = molarMass;
		reagent.Purity = purity;
		reagent.Density = density;
		reagent.Polarizability  = polarizability;
		reagent.irSpecter = irSpecter;
		reagent.Conductibility = conductibility;
		reagent.Solubility = solubility;
		reagent.FumeHoodOnly = fumeHoodOnly;
		reagent.compoundColor = color;

		if(!isSolid) {
			reagent.PH = pH;
			reagent.uvSpecter = uvSpecter;
			reagent.FlameSpecter = flameSpecter;
			reagent.Turbidity = turbidity;
			reagent.hplc = hplc;
			reagent.Refratometer = refratometer;
		}

		if (!reagents.ContainsKey(reagent.Name)) 
		{
			reagents.Add(reagent.Name, reagent);
			SaveReagents(reagents);
			Debug.Log ("Reagente Salvo Com Sucesso!");
		} 
		else 
		{
			reagent.Name += "(2)";
			reagents.Add(reagent.Name, reagent);
			SaveReagents(reagents);
			Debug.Log ("Reagente Salvo Com Sucesso!");

		}
	}

	//! Saves the new reagent in a file 
	public static void SaveReagents(Dictionary<string, Compound> reagents)
	{
		TextEdit text = new TextEdit("Assets/Resources/components.txt");

		text.ClearFile ();

		text.SetInt ("numberOfReagents", reagents.Count);

		int counter = 0;
		foreach (Compound reagent in reagents.Values)
		{
			text.SetString("name" + counter.ToString(), reagent.Name);
			text.SetString("formula" + counter.ToString(), reagent.Formula);
			text.SetBool("isSolid" + counter.ToString(), reagent.IsSolid);
			text.SetFloat("molarMass" + counter.ToString(), reagent.MolarMass);
			text.SetFloat ("purity" + counter.ToString(), reagent.Purity);
			text.SetFloat("density" + counter.ToString(), reagent.Density);
			text.SetFloat("polarizability" + counter.ToString(), reagent.Polarizability);
			text.SetInt("compoundColorR" + counter.ToString(), reagent.compoundColor.r);
			text.SetInt("compoundColorG" + counter.ToString(), reagent.compoundColor.g);
			text.SetInt("compoundColorB" + counter.ToString(), reagent.compoundColor.b);

			if(reagent.irSpecter != null)
			{
				text.SetString("irSpecter" + counter.ToString(), reagent.irSpecter.name);
			}
			else
			{
				text.SetString("irSpecter" + counter.ToString(), "");
			}

			text.SetFloat("conductibility" + counter.ToString(), reagent.Conductibility);
			text.SetFloat("solubility" + counter.ToString(), reagent.Solubility);
			text.SetBool ("fumeHoodOnly" + counter.ToString (), reagent.FumeHoodOnly);
			//!This saves only what is related to liquids
			if (!reagent.IsSolid) {  
				text.SetFloat("pH" + counter.ToString(), reagent.PH);
				text.SetFloat("turbidity" + counter.ToString(), reagent.Turbidity);
				text.SetFloat("refratometer" + counter.ToString(), reagent.Refratometer);
				text.SetFloat("flameSpecter" + counter.ToString(), reagent.FlameSpecter);

				if(reagent.uvSpecter != null) {
					text.SetString("uvSpecter" + counter.ToString(), reagent.uvSpecter.name);
				} else {
					text.SetString("uvSpecter" + counter.ToString(), "");
				}
				if(reagent.hplc != null) {
					text.SetString("hplc" + counter.ToString(), reagent.hplc.name);
				} else {
					text.SetString("hplc" + counter.ToString(), "");
				}
			}

			counter++;
		}
	}

	/*public static Dictionary<string, Compound> LoadReagents()
	{
		TextEdit textLoad = new TextEdit("Assets/Resources/components.txt");

		int numberOfReagents = textLoad.GetInt ("numberOfReagents");

		Dictionary<string, Compound> reagents = new Dictionary<string, Compound>();

		if (numberOfReagents > 0) 
		{
			for (int i = 0; i < numberOfReagents; i++) 
			{

				Compound reagentAcc = new Compound();

				reagentAcc.Name = textLoad.GetString ("name" + i.ToString ());
				reagentAcc.Formula = textLoad.GetString ("formula" + i.ToString ());
				reagentAcc.IsSolid = textLoad.GetBool("isSolid" + i.ToString());
				reagentAcc.MolarMass = textLoad.GetFloat ("molarMass" + i.ToString ());
				reagentAcc.Purity = textLoad.GetFloat ("purity" + i.ToString ());
				reagentAcc.Density = textLoad.GetFloat ("density" + i.ToString ());
				reagentAcc.Polarizability = textLoad.GetFloat ("polarizability" + i.ToString ());
				reagentAcc.compoundColor = new Color32((byte)textLoad.GetInt("compoundColorR" + i.ToString ()),
				                                       (byte)textLoad.GetInt("compoundColorG" + i.ToString ()),
				                                       (byte)textLoad.GetInt("compoundColorB" + i.ToString ()),
				                                       (byte)255);


				if (!string.IsNullOrEmpty (textLoad.GetString ("irSpecter" + i.ToString ()))) 
				{
					reagentAcc.irSpecter = Resources.Load<Texture2D> ("specter/" + textLoad.GetString ("irSpecter" + i.ToString ()));
				} 
				else 
				{
						reagentAcc.irSpecter = null;
				}

				reagentAcc.Conductibility = textLoad.GetFloat ("conductibility" + i.ToString ());
				reagentAcc.Solubility = textLoad.GetFloat ("solubility" + i.ToString ());
				reagentAcc.FumeHoodOnly = textLoad.GetBool("fumeHoodOnly" + i.ToString());

				//reagentAcc.color = new Color (textLoad.GetFloat ("colorR"+ i.ToString ()), textLoad.GetFloat ("colorG"+ i.ToString ()), textLoad.GetFloat ("colorB"+ i.ToString ()), textLoad.GetFloat ("colorA"+ i.ToString ()));

				//!Gets the liquid-related variables
				if(!textLoad.GetBool("isSolid" + i.ToString())) {

					reagentAcc.PH = textLoad.GetFloat ("ph" + i.ToString ());
					reagentAcc.Turbidity = textLoad.GetFloat ("turbidity" + i.ToString ());
					reagentAcc.Refratometer = textLoad.GetFloat ("refratometer" + i.ToString ());
					reagentAcc.FlameSpecter = textLoad.GetFloat ("flameSpecter" + i.ToString ());
					if (!string.IsNullOrEmpty (textLoad.GetString ("uvSpecter" + i.ToString ()))) {
						reagentAcc.uvSpecter = Resources.Load<Texture2D> ("specter/" + textLoad.GetString ("uvSpecter" + i.ToString ()));
					} else {
						reagentAcc.uvSpecter = null;
					}

					if (!string.IsNullOrEmpty (textLoad.GetString ("hplc" + i.ToString ()))) {
						reagentAcc.hplc = Resources.Load<Texture2D> ("specter/" + textLoad.GetString ("hplc" + i.ToString ()));
					} else {
						reagentAcc.hplc = null;
					}
				}

				reagentAcc.Molarity = ((reagentAcc.Purity * reagentAcc.Density) / reagentAcc.MolarMass) * 1000; // number of mols / volume
				Debug.Log (reagentAcc.Formula + ".Molarity = " + reagentAcc.Molarity);
				reagents.Add(reagentAcc.Name, reagentAcc);
			}
		}
		return reagents;
	}*/
	//! Reads/Loads the dictionary from .json file.
	public static Dictionary<string, Compound> LoadCupboardCompounds()
	{
		JSONEditor jsonEditor = new JSONEditor("compounds");
		
		Dictionary<string, Compound> reagents = new Dictionary<string, Compound>();
		
		for (int i = 0; i < jsonEditor.NumberOfObjects(); i++) 
		{
				
			Compound reagentAcc = new Compound();
			
			reagentAcc.Name = jsonEditor.GetString (i, "name");
			reagentAcc.Formula = jsonEditor.GetString (i, "formula");
			reagentAcc.IsSolid = jsonEditor.GetBool(i, "isSolid");
			reagentAcc.MolarMass = jsonEditor.GetFloat (i, "molarMass");
			reagentAcc.Purity = jsonEditor.GetFloat (i, "purity");
			reagentAcc.Density = jsonEditor.GetFloat (i, "density");
			reagentAcc.Polarizability = jsonEditor.GetFloat (i, "polarizability");
			reagentAcc.compoundColor = new Color32((byte)jsonEditor.GetInt(i, "compoundColorR"),
			                                       (byte)jsonEditor.GetInt(i, "compoundColorG"),
			                                       (byte)jsonEditor.GetInt(i, "compoundColorB"),
			                                       (byte)255);
			
			
			if (!string.IsNullOrEmpty (jsonEditor.GetString (i, "irSpecter"))) 
			{
				reagentAcc.irSpecter = Resources.Load<Texture2D> ("specter/" + jsonEditor.GetString (i, "irSpecter"));
			} 
			else 
			{
				reagentAcc.irSpecter = null;
			}
				
			reagentAcc.Conductibility = jsonEditor.GetFloat (i, "conductibility" + i.ToString ());
			reagentAcc.Solubility = jsonEditor.GetFloat (i, "solubility");
			reagentAcc.FumeHoodOnly = jsonEditor.GetBool(i, "fumeHoodOnly");
				
			//reagentAcc.color = new Color (jsonEditor.GetFloat ("colorR"+ i.ToString ()), jsonEditor.GetFloat ("colorG"+ i.ToString ()), jsonEditor.GetFloat ("colorB"+ i.ToString ()), jsonEditor.GetFloat ("colorA"+ i.ToString ()));
				
			//!Gets the liquid-related variables
			if(!jsonEditor.GetBool(i, "isSolid")) {				
				reagentAcc.PH = jsonEditor.GetFloat (i, "ph");
				reagentAcc.Turbidity = jsonEditor.GetFloat (i, "turbidity");
				reagentAcc.Refratometer = jsonEditor.GetFloat (i, "refratometer");
				reagentAcc.FlameSpecter = jsonEditor.GetFloat (i, "flameSpecter");
				if (!string.IsNullOrEmpty (jsonEditor.GetString (i, "uvSpecter"))) {
					reagentAcc.uvSpecter = Resources.Load<Texture2D> ("specter/" + jsonEditor.GetString (i, "uvSpecter"));
				} else {
					reagentAcc.uvSpecter = null;
				}
				
				if (!string.IsNullOrEmpty (jsonEditor.GetString (i, "hplc"))) {
					reagentAcc.hplc = Resources.Load<Texture2D> ("specter/" + jsonEditor.GetString (i, "hplc"));
				} else {
					reagentAcc.hplc = null;
				}
			}
				
			reagentAcc.Molarity = ((reagentAcc.Purity * reagentAcc.Density) / reagentAcc.MolarMass) * 1000; // number of mols / volume
//			Debug.Log (reagentAcc.Formula + ".Molarity = " + reagentAcc.Molarity);
			reagents.Add(reagentAcc.Name, reagentAcc);
		}
		return reagents;
	}
	
	/* -------------PRODUCT------------- */
	
	//! Saves the Product information in a Dictionary. 
	public static void SaveProductFromEditor(string name,string formula ,bool isSolid, float molarMass, float purity, float density, float pH, float polarizability, 
	                                         Texture2D uvSpecter, Texture2D irSpecter, float flameSpecter, float conductibility, 
	                                         float solubility, float turbidity, Texture2D hplc, float refratometer, bool fumeHoodOnly, Texture2D texture, Color32 color)
	{
		
		Dictionary<string, Compound> products = LoadBackgroundCompounds ();
		
		
		Compound product = new Compound();
		product.IsSolid = isSolid ;
		product.Name = name;
		product.Formula = formula;
		product.MolarMass = molarMass;
		product.Purity = purity;
		product.Density = density;
		product.Polarizability  = polarizability;
		product.irSpecter = irSpecter;
		product.Conductibility = conductibility;
		product.Solubility = solubility;
		product.FumeHoodOnly = fumeHoodOnly;
		product.compoundColor = color;
		
		if(!isSolid) {
			product.PH = pH;
			product.uvSpecter = uvSpecter;
			product.FlameSpecter = flameSpecter;
			product.Turbidity = turbidity;
			product.hplc = hplc;
			product.Refratometer = refratometer;
		}
		
		if (!products.ContainsKey(product.Name)) 
		{
			products.Add(product.Name, product);
			SaveProducts(products);
			Debug.Log ("Produto Salvo Com Sucesso!");
		} 
		else 
		{
			product.Name += "(2)";
			products.Add(product.Name, product);
			SaveProducts(products);
			Debug.Log ("Produto Salvo Com Sucesso!");
			
		}
	}

	//! Saves the new product in a file 
	public static void SaveProducts(Dictionary<string, Compound> products)
	{
		TextEdit text = new TextEdit("Assets/Resources/products.txt");
		
		text.ClearFile ();
		
		text.SetInt ("numberOfProducts", products.Count);
		
		int counter = 0;
		foreach (Compound product in products.Values)
		{
			text.SetString("name" + counter.ToString(), product.Name);
			text.SetString("formula" + counter.ToString(), product.Formula);
			text.SetBool("isSolid" + counter.ToString(), product.IsSolid);
			text.SetFloat("molarMass" + counter.ToString(), product.MolarMass);
			text.SetFloat ("purity" + counter.ToString(), product.Purity);
			text.SetFloat("density" + counter.ToString(), product.Density);
			text.SetFloat("polarizability" + counter.ToString(), product.Polarizability);
			text.SetInt("compoundColorR" + counter.ToString(), product.compoundColor.r);
			text.SetInt("compoundColorG" + counter.ToString(), product.compoundColor.g);
			text.SetInt("compoundColorB" + counter.ToString(), product.compoundColor.b);
			
			
			if(product.irSpecter != null)
			{
				text.SetString("irSpecter" + counter.ToString(), product.irSpecter.name);
			}
			else
			{
				text.SetString("irSpecter" + counter.ToString(), "");
			}
			
			text.SetFloat("conductibility" + counter.ToString(), product.Conductibility);
			text.SetFloat("solubility" + counter.ToString(), product.Solubility);
			text.SetBool ("fumeHoodOnly" + counter.ToString (), product.FumeHoodOnly);
			/*
			text.SetFloat("colorR" + counter.ToString(), product.color.r);
			text.SetFloat("colorG" + counter.ToString(), product.color.g);
			text.SetFloat("colorB" + counter.ToString(), product.color.b);
			text.SetFloat("colorA" + counter.ToString(), product.color.a);
			*/
			//!This saves only what is related to liquids
			if (!product.IsSolid) {  
				text.SetFloat("pH" + counter.ToString(), product.PH);
				text.SetFloat("turbidity" + counter.ToString(), product.Turbidity);
				text.SetFloat("refratometer" + counter.ToString(), product.Refratometer);
				text.SetFloat("flameSpecter" + counter.ToString(), product.FlameSpecter);
				
				if(product.uvSpecter != null) {
					text.SetString("uvSpecter" + counter.ToString(), product.uvSpecter.name);
				} else {
					text.SetString("uvSpecter" + counter.ToString(), "");
				}
				if(product.hplc != null) {
					text.SetString("hplc" + counter.ToString(), product.hplc.name);
				} else {
					text.SetString("hplc" + counter.ToString(), "");
				}
			}
			
			counter++;
		}
	}
	
	//! Reads/Loads the dictionary from file.
	public static Dictionary<string, Compound> LoadBackgroundCompounds()
	{
		JSONEditor jsonEditor = new JSONEditor("products");
		
		Dictionary<string, Compound> products = new Dictionary<string, Compound>();

		for (int i = 0; i < jsonEditor.NumberOfObjects(); i++) 
		{
			
			Compound productAcc = new Compound();
			
			productAcc.Name = jsonEditor.GetString (i, "name");
			productAcc.Formula = jsonEditor.GetString (i, "formula");
			productAcc.IsSolid = jsonEditor.GetBool(i, "isSolid" + i.ToString());
			productAcc.MolarMass = jsonEditor.GetFloat (i, "molarMass");
			productAcc.Purity = jsonEditor.GetFloat (i, "purity");
			productAcc.Density = jsonEditor.GetFloat (i, "density");
			productAcc.Polarizability = jsonEditor.GetFloat (i, "polarizability");
			
			if (!string.IsNullOrEmpty (jsonEditor.GetString (i, "irSpecter"))) 
			{
				productAcc.irSpecter = Resources.Load<Texture2D> ("specter/" + jsonEditor.GetString (i, "irSpecter"));
			} 
			else 
			{
				productAcc.irSpecter = null;
			}
			
			productAcc.Conductibility = jsonEditor.GetFloat (i, "conductibility");
			productAcc.Solubility = jsonEditor.GetFloat (i, "solubility");
			productAcc.FumeHoodOnly = jsonEditor.GetBool(i, "fumeHoodOnly");
			
			productAcc.compoundColor = new Color32((byte)jsonEditor.GetInt(i, "compoundColorR"),
			                                       (byte)jsonEditor.GetInt(i, "compoundColorG"),
			                                       (byte)jsonEditor.GetInt(i, "compoundColorB"),
			                                       (byte)255);
			
			//!Gets the liquid-related variables
			if(!jsonEditor.GetBool(i, "isSolid")) {
				
				productAcc.PH = jsonEditor.GetFloat (i, "ph");
				productAcc.Turbidity = jsonEditor.GetFloat (i, "turbidity");
				productAcc.Refratometer = jsonEditor.GetFloat (i, "refratometer");
				productAcc.FlameSpecter = jsonEditor.GetFloat (i, "flameSpecter");
				if (!string.IsNullOrEmpty (jsonEditor.GetString (i, "uvSpecter"))) {
					productAcc.uvSpecter = Resources.Load<Texture2D> ("specter/" + jsonEditor.GetString (i, "uvSpecter"));
				} else {
					productAcc.uvSpecter = null;
				}
					
				if (!string.IsNullOrEmpty (jsonEditor.GetString (i, "hplc"))) {
					productAcc.hplc = Resources.Load<Texture2D> ("specter/" + jsonEditor.GetString (i, "hplc"));
				} else {
					productAcc.hplc = null;
				}
			}
				
			productAcc.Molarity = ((productAcc.Purity * productAcc.Density) / productAcc.MolarMass); // number of mols / volume
			Debug.Log (productAcc.Formula + ".Molarity = " + productAcc.Molarity);
			products.Add(productAcc.Formula, productAcc);
		}

		Dictionary<string, Compound> cupboard = ComponentsSaver.LoadCupboardCompounds();

		foreach (string name in cupboard.Keys) {
			products.Add(cupboard[name].Formula, cupboard[name]);
		}

		return products;
	}

}

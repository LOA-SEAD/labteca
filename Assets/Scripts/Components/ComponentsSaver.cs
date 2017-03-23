using UnityEngine;
using System.Collections;
using System.Collections.Generic;

//! Saves the component information from editor.
/*!
 * Contains three methods that saves the liquidReagent from editor in a dictionary and in a file  
 * and load the dictionary from file.
 */

public class ComponentsSaver : MonoBehaviour  {

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

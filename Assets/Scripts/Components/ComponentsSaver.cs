using UnityEngine;
using System.Collections;
using System.Collections.Generic;

//! Saves the component information from editor.
/*!
 * Contains three methods that saves the liquidReagent from editor in a dictionary and in a file  
 * and load the dictionary from file.
 */

public class ComponentsSaver
{
	//! Saves the component information in a Dictionary. 
	public static void SaveReagentFromEditor(string name,string formula ,bool isSolid, int molarMass, float density, float pH, float polarizability, 
	                                         Texture2D uvSpecter, Texture2D irSpecter, Texture2D flameSpecter, float conductibility, 
	                                         float solubility, float turbidity, Texture2D hplc, float refratometer, Texture2D texture, Color color)
	{

		Dictionary<string, Compound> reagents = LoadReagents ();


		Compound reagent = new Compound();
		reagent.IsSolid = isSolid ;
		reagent.Name = name;
		reagent.Formula = formula;
		reagent.MolarMass = molarMass;
		reagent.Density = density;
		reagent.Polarizability  = polarizability;
		reagent.irSpecter = irSpecter;
		reagent.Conductibility = conductibility;
		reagent.Solubility = solubility;

		if(!isSolid) {
			reagent.PH = pH;
			reagent.uvSpecter = uvSpecter;
			reagent.flameSpecter = flameSpecter;
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
			text.SetFloat("density" + counter.ToString(), reagent.Density);
			text.SetFloat("polarizability" + counter.ToString(), reagent.Polarizability);


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
			/*
			text.SetFloat("colorR" + counter.ToString(), reagent.color.r);
			text.SetFloat("colorG" + counter.ToString(), reagent.color.g);
			text.SetFloat("colorB" + counter.ToString(), reagent.color.b);
			text.SetFloat("colorA" + counter.ToString(), reagent.color.a);
			*/
			//!This saves only what is related to liquids
			if (!reagent.IsSolid) {  
				text.SetFloat("pH" + counter.ToString(), reagent.PH);
				text.SetFloat("turbidity" + counter.ToString(), reagent.Turbidity);
				text.SetFloat("refratometer" + counter.ToString(), reagent.Refratometer);

				if(reagent.flameSpecter != null) {
					text.SetString("flameSpecter" + counter.ToString(), reagent.flameSpecter.name);
				} else {
					text.SetString("flameSpecter" + counter.ToString(), "");
				}
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

	//! Reads/Loads the dictionary from file.
	public static Dictionary<string, Compound> LoadReagents()
	{
		TextAsset loadText = Resources.Load("components") as TextAsset;

		TextEdit textLoad = new TextEdit(loadText);

		//Debug.Log (loadText);

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
				reagentAcc.MolarMass = textLoad.GetInt ("molarMass" + i.ToString ());
				reagentAcc.Density = textLoad.GetFloat ("density" + i.ToString ());
				reagentAcc.Polarizability = textLoad.GetFloat ("polarizability" + i.ToString ());

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
			

				//reagentAcc.color = new Color (textLoad.GetFloat ("colorR"+ i.ToString ()), textLoad.GetFloat ("colorG"+ i.ToString ()), textLoad.GetFloat ("colorB"+ i.ToString ()), textLoad.GetFloat ("colorA"+ i.ToString ()));

				//!Gets the liquid-related variables
				if(!textLoad.GetBool("isSolid" + i.ToString())) {

					reagentAcc.PH = textLoad.GetFloat ("ph" + i.ToString ());
					reagentAcc.Turbidity = textLoad.GetFloat ("turbidity" + i.ToString ());
					reagentAcc.Refratometer = textLoad.GetFloat ("refratometer" + i.ToString ());
					if (!string.IsNullOrEmpty (textLoad.GetString ("uvSpecter" + i.ToString ()))) {
						reagentAcc.uvSpecter = Resources.Load<Texture2D> ("specter/" + textLoad.GetString ("uvSpecter" + i.ToString ()));
					} else {
						reagentAcc.uvSpecter = null;
					}

					if (!string.IsNullOrEmpty (textLoad.GetString ("flameSpecter" + i.ToString ()))) {
						reagentAcc.flameSpecter = Resources.Load<Texture2D> ("specter/" + textLoad.GetString ("flameSpecter" + i.ToString ()));
					} else {
						reagentAcc.flameSpecter = null;
					}

					if (!string.IsNullOrEmpty (textLoad.GetString ("hplc" + i.ToString ()))) {
						reagentAcc.hplc = Resources.Load<Texture2D> ("specter/" + textLoad.GetString ("hplc" + i.ToString ()));
					} else {
						reagentAcc.hplc = null;
					}
				}

				reagents.Add(reagentAcc.Name, reagentAcc);
			}
		}
		return reagents;
	}
}

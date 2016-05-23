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
	public static void SaveReagentFromEditor(string name, bool isSolid, int molarMass, float density, float pH, float polarizability, 
	                                         Texture2D uvSpecter, Texture2D irSpecter, Texture2D flameSpecter, float conductibility, 
	                                         float solubility, float turbidity, Texture2D hplc, float refratometer, Texture2D texture, Color color)
	{

		Dictionary<string, Compound> reagents = LoadReagents ();


		Compound reagent = new Compound();
		reagent.isSolid = isSolid;
		reagent.name = name;
		reagent.molarMass = molarMass;
		reagent.density = density;
		reagent.polarizability = polarizability;
		reagent.irSpecter = irSpecter;
		reagent.conductibility = conductibility;
		reagent.solubility = solubility;

		if(!isSolid) {
			reagent.pH = pH;
			reagent.uvSpecter = uvSpecter;
			reagent.flameSpecter = flameSpecter;
			reagent.turbidity = turbidity;
			reagent.hplc = hplc;
			reagent.refratometer = refratometer;
		}

		if (!reagents.ContainsKey(reagent.name)) 
		{
			reagents.Add(reagent.name, reagent);
			SaveReagents(reagents);
			Debug.Log ("Reagente Salvo Com Sucesso!");
		} 
		else 
		{
			reagent.name += "(2)";
			reagents.Add(reagent.name, reagent);
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
			text.SetString("name" + counter.ToString(), reagent.name);
			text.SetBool("isSolid" + counter.ToString(), reagent.isSolid);
			text.SetFloat("molarMass" + counter.ToString(), reagent.molarMass);
			text.SetFloat("density" + counter.ToString(), reagent.density);
			text.SetFloat("polarizability" + counter.ToString(), reagent.polarizability);


			if(reagent.irSpecter != null)
			{
				text.SetString("irSpecter" + counter.ToString(), reagent.irSpecter.name);
			}
			else
			{
				text.SetString("irSpecter" + counter.ToString(), "");
			}

			text.SetFloat("conductibility" + counter.ToString(), reagent.conductibility);
			text.SetFloat("solubility" + counter.ToString(), reagent.solubility);
			/*
			text.SetFloat("colorR" + counter.ToString(), reagent.color.r);
			text.SetFloat("colorG" + counter.ToString(), reagent.color.g);
			text.SetFloat("colorB" + counter.ToString(), reagent.color.b);
			text.SetFloat("colorA" + counter.ToString(), reagent.color.a);
			*/
			//!This saves only what is related to liquids
			if (!reagent.isSolid) {  
				text.SetFloat("pH" + counter.ToString(), reagent.pH);
				text.SetFloat("turbidity" + counter.ToString(), reagent.turbidity);
				text.SetFloat("refratometer" + counter.ToString(), reagent.refratometer);

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

				reagentAcc.name = textLoad.GetString ("name" + i.ToString ());
				reagentAcc.isSolid = textLoad.GetBool("isSolid" + i.ToString());
				reagentAcc.molarMass = textLoad.GetInt ("molarMass" + i.ToString ());
				reagentAcc.density = textLoad.GetFloat ("density" + i.ToString ());
				reagentAcc.polarizability = textLoad.GetFloat ("polarizability" + i.ToString ());

				if (!string.IsNullOrEmpty (textLoad.GetString ("irSpecter" + i.ToString ()))) 
				{
					reagentAcc.irSpecter = Resources.Load<Texture2D> ("specter/" + textLoad.GetString ("irSpecter" + i.ToString ()));
				} 
				else 
				{
						reagentAcc.irSpecter = null;
				}

				reagentAcc.conductibility = textLoad.GetFloat ("conductibility" + i.ToString ());
				reagentAcc.solubility = textLoad.GetFloat ("solubility" + i.ToString ());
			

				//reagentAcc.color = new Color (textLoad.GetFloat ("colorR"+ i.ToString ()), textLoad.GetFloat ("colorG"+ i.ToString ()), textLoad.GetFloat ("colorB"+ i.ToString ()), textLoad.GetFloat ("colorA"+ i.ToString ()));

				//!Gets the liquid-related variables
				if(!textLoad.GetBool("isSolid" + i.ToString())) {

					reagentAcc.pH = textLoad.GetFloat ("ph" + i.ToString ());
					reagentAcc.turbidity = textLoad.GetFloat ("turbidity" + i.ToString ());
					reagentAcc.refratometer = textLoad.GetFloat ("refratometer" + i.ToString ());
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

				reagents.Add(reagentAcc.name, reagentAcc);
			}
		}
		return reagents;
	}
}

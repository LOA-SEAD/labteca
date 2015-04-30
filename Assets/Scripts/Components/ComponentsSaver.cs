using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ComponentsSaver
{
	public static void SaveReagentFromEditor(string name, int molarMass, float density, float ph, float polarizibility, 
	                                         Texture2D uvSpecter, Texture2D irSpecter, Texture2D flameSpecter, float condutibility, 
	                                         float solubility, float turbility, Texture2D hplc, float refratometer, Texture2D texture, Color color)
	{

		Dictionary<string, ReagentsLiquidClass> reagents = LoadReagents ();

		ReagentsLiquidClass reagent = new ReagentsLiquidClass ();
		reagent.name = name;
		reagent.molarMass = molarMass;
		reagent.density = density;
		reagent.ph = ph;
		reagent.polarizibility = polarizibility;
		reagent.uvSpecter = uvSpecter;
		reagent.irSpecter = irSpecter;
		reagent.flameSpecter = flameSpecter;
		reagent.condutibility = condutibility;
		reagent.solubility = solubility;
		reagent.turbility = turbility;
		reagent.hplc = hplc;
		reagent.refratometer = refratometer;
		reagent.texture = texture;
		reagent.color = color;

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


	public static void SaveReagents(Dictionary<string, ReagentsLiquidClass> liquidReagents)
	{
		TextEdit text = new TextEdit("Assets/Resources/componentsLiquids.txt");

		text.ClearFile ();

		text.SetInt ("numberOfReagents", liquidReagents.Count);

		int counter = 0;
		foreach (ReagentsLiquidClass reagent in liquidReagents.Values)
		{
			text.SetString("name" + counter.ToString(), reagent.name);
			text.SetInt("molarMass" + counter.ToString(), reagent.molarMass);
			text.SetFloat("density" + counter.ToString(), reagent.density);
			text.SetFloat("ph" + counter.ToString(), reagent.ph);
			text.SetFloat("polarizibility" + counter.ToString(), reagent.polarizibility);

			if(reagent.uvSpecter != null)
			{
				text.SetString("uvSpecter" + counter.ToString(), reagent.uvSpecter.name);
			}
			else
			{
				text.SetString("uvSpecter" + counter.ToString(), "");
			}
			if(reagent.irSpecter != null)
			{
				text.SetString("irSpecter" + counter.ToString(), reagent.irSpecter.name);
			}
			else
			{
				text.SetString("irSpecter" + counter.ToString(), "");
			}
			if(reagent.flameSpecter != null)
			{
				text.SetString("flameSpecter" + counter.ToString(), reagent.flameSpecter.name);
			}
			else
			{
				text.SetString("flameSpecter" + counter.ToString(), "");
			}

			text.SetFloat("condutibility" + counter.ToString(), reagent.condutibility);
			text.SetFloat("solubility" + counter.ToString(), reagent.solubility);
			text.SetFloat("turbility" + counter.ToString(), reagent.turbility);

			if(reagent.hplc != null)
			{
				text.SetString("hplc" + counter.ToString(), reagent.hplc.name);
			}
			else
			{
				text.SetString("hplc" + counter.ToString(), "");
			}

			text.SetFloat("refratometer" + counter.ToString(), reagent.refratometer);

			if(reagent.texture != null)
			{
				text.SetString("texture" + counter.ToString(), reagent.texture.name);
			}
			else
			{
				text.SetString("texture" + counter.ToString(), "");
			}
			text.SetFloat("colorR" + counter.ToString(), reagent.color.r);
			text.SetFloat("colorG" + counter.ToString(), reagent.color.g);
			text.SetFloat("colorB" + counter.ToString(), reagent.color.b);
			text.SetFloat("colorA" + counter.ToString(), reagent.color.a);

			counter++;
		}
	}

	public static Dictionary<string, ReagentsLiquidClass> LoadReagents()
	{
		TextAsset loadText = Resources.Load("componentsLiquids") as TextAsset;

		TextEdit textLoad = new TextEdit(loadText);

		//Debug.Log (loadText);

		int numberOfReagents = textLoad.GetInt ("numberOfReagents");

		Dictionary<string, ReagentsLiquidClass> liquidReagents = new Dictionary<string, ReagentsLiquidClass>();

		if (numberOfReagents > 0) 
		{
			for (int i = 0; i < numberOfReagents; i++) 
			{
				ReagentsLiquidClass reagentAcc = new ReagentsLiquidClass();

				reagentAcc.name = textLoad.GetString ("name" + i.ToString ());
				reagentAcc.molarMass = textLoad.GetInt ("molarMass" + i.ToString ());
				reagentAcc.density = textLoad.GetFloat ("density" + i.ToString ());
				reagentAcc.ph = textLoad.GetFloat ("ph" + i.ToString ());
				reagentAcc.polarizibility = textLoad.GetFloat ("polarizibility" + i.ToString ());
				if (!string.IsNullOrEmpty (textLoad.GetString ("uvSpecter" + i.ToString ()))) 
				{
					reagentAcc.uvSpecter = Resources.Load<Texture2D> ("specter/" + textLoad.GetString ("uvSpecter" + i.ToString ()));
				} 
				else 
				{
						reagentAcc.uvSpecter = null;
				}
				if (!string.IsNullOrEmpty (textLoad.GetString ("irSpecter" + i.ToString ()))) 
				{
					reagentAcc.irSpecter = Resources.Load<Texture2D> ("specter/" + textLoad.GetString ("irSpecter" + i.ToString ()));
				} 
				else 
				{
						reagentAcc.irSpecter = null;
				}
				if (!string.IsNullOrEmpty (textLoad.GetString ("flameSpecter" + i.ToString ()))) 
				{
					reagentAcc.flameSpecter = Resources.Load<Texture2D> ("specter/" + textLoad.GetString ("flameSpecter" + i.ToString ()));
				} 
				else 
				{
						reagentAcc.flameSpecter = null;
				}
				reagentAcc.condutibility = textLoad.GetFloat ("condutibility" + i.ToString ());
				reagentAcc.solubility = textLoad.GetFloat ("solubility" + i.ToString ());
				reagentAcc.turbility = textLoad.GetFloat ("turbility" + i.ToString ());

				if (!string.IsNullOrEmpty (textLoad.GetString ("hplc" + i.ToString ()))) 
				{
					reagentAcc.hplc = Resources.Load<Texture2D> ("specter/" + textLoad.GetString ("hplc" + i.ToString ()));
				} 
				else 
				{
					reagentAcc.hplc = null;
				}

				reagentAcc.refratometer = textLoad.GetFloat ("refratometer" + i.ToString ());


				if (!string.IsNullOrEmpty (textLoad.GetString ("texture" + i.ToString ()))) 
				{
					reagentAcc.texture = Resources.Load<Texture2D>("specter/" + textLoad.GetString ("texture" + i.ToString ()));
				} 
				else 
				{
					reagentAcc.texture = null;
				}

				reagentAcc.color = new Color (textLoad.GetFloat ("colorR"+ i.ToString ()), textLoad.GetFloat ("colorG"+ i.ToString ()), textLoad.GetFloat ("colorB"+ i.ToString ()), textLoad.GetFloat ("colorA"+ i.ToString ()));

				liquidReagents.Add(reagentAcc.name, reagentAcc);
			}
		}
		return liquidReagents;
	}
}

﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MachineBehaviour : MonoBehaviour 
{
	public enum MACHINES
	{
		PHMETER,
		CONDUTIVIMETER,
		SPCTROPHOTOMETER_UV,
		SPCTROPHOTOMETER_IR,
		SPCTROPHOTOMETER_FLAME,
		POLARIMETER,
		HPLC,
		REFRATOMETER,
		TURBIDOMETER
	}

	public MACHINES myType;

	private Texture2D setupTexture;
	private float setupFloat;

	private Texture2D resultTexture;
	private float resultFloat;

	public TextMesh textResult;
	public Renderer rendererResult;


	public float errorAmplitude;
	public int errorPrecision;

	private bool allowShowFloat = false;
	private bool allowShowTexture = false;

	public float timeToUpdateValue;
	private float timeToUpdateValueAcc;


	public string actualReagent;
	public float actualConcentration;

	public float timeToShowPrintText;
	private float timeToShowPrintTextAcc;

	// Update is called once per frame
	void Update () 
	{
		if (allowShowFloat) 
		{
			if(Time.time - timeToUpdateValueAcc > timeToUpdateValue)
			{
				string errorStringForm = "0";
				if(errorPrecision > 0)
				{
					errorStringForm = "0.";
					for (int i = 0; i < errorPrecision; i++) 
					{
						errorStringForm += "0";
					}
				}

				textResult.text = applyErrorInFloat(resultFloat).ToString(errorStringForm);
				timeToUpdateValueAcc = Time.time;
			}
		}

		if (allowShowTexture) 
		{
			if(rendererResult != null)
			{
				rendererResult.material.mainTexture = resultTexture;
			}
			allowShowTexture = false;
		}

		if (Time.time - timeToShowPrintTextAcc > timeToShowPrintText)
		{
			timeToShowPrintTextAcc = Mathf.Infinity;
			HudText.EraseText();
		}
	}

	public void Setup(string reagent, float concentration)
	{
		Dictionary<string, ReagentsLiquidClass> allReagents = ComponentsSaver.LoadReagents ();

		if (!allReagents.ContainsKey(reagent))
		{
			Debug.LogWarning("Reagent not seted in database");
			return;
		}

		ReagentsLiquidClass realReagent = allReagents [reagent];

		allowShowFloat = false;
		allowShowTexture = false;

		bool showFloat = false;

		switch (myType) 
		{
			default:
			{
				setupFloat = 0f;
				setupTexture = null; // TODO: do something!
				allowShowFloat = false;
				allowShowTexture = false;
			}
			break;

			case MACHINES.CONDUTIVIMETER:
			{
				setupFloat = realReagent.condutibility;
				showFloat = true;

				InventoryController	inventory = FindObjectOfType (typeof(InventoryController)) as InventoryController;
			}
			break;

			case MACHINES.SPCTROPHOTOMETER_UV:
			{
				setupTexture = realReagent.uvSpecter;
				allowShowTexture = true;

				InventoryController	inventory = FindObjectOfType (typeof(InventoryController)) as InventoryController;

			}
			break;

			case MACHINES.SPCTROPHOTOMETER_IR:
			{
				setupTexture = realReagent.irSpecter;
				allowShowTexture = true;

				InventoryController	inventory = FindObjectOfType (typeof(InventoryController)) as InventoryController;

			}
			break;

			case MACHINES.TURBIDOMETER:
			{
				setupFloat = realReagent.turbility;
				showFloat = true;

				InventoryController	inventory = FindObjectOfType (typeof(InventoryController)) as InventoryController;

			}
			break;
		}

		if (showFloat) 
		{
			resultFloat *= concentration;

			string errorStringForm = "0";
			if (errorPrecision > 0) 
			{
				errorStringForm = "0.";
				for (int i = 0; i < errorPrecision; i++) 
				{
					errorStringForm += "0";
				}
			}

			textResult.text = applyErrorInFloat (resultFloat).ToString (errorStringForm);
		}

		if (allowShowTexture) 
		{
			setupTexture = ChartGenerator.GenerateWithTextureConectingDots(setupTexture, concentration, 0.8f);
			HudText.SetText("O grafico foi Impresso!");
			PrinterUse.SendFileToPrinter(setupTexture);
			timeToShowPrintTextAcc = Time.time;
		}
	}


	public void Use(string reagent, float concentration)
	{
		Dictionary<string, ReagentsLiquidClass> allReagents = ComponentsSaver.LoadReagents ();
		
		if (!allReagents.ContainsKey(reagent))
		{
			Debug.LogWarning("Reagent not seted in database");
			return;
		}
		
		ReagentsLiquidClass realReagent = allReagents [reagent];

		switch (myType) 
		{
			case MACHINES.PHMETER:
			{
				resultFloat = realReagent.ph;
				allowShowFloat = true;
				allowShowTexture = false;
			}
			break;
			
			case MACHINES.CONDUTIVIMETER:
			{
				resultFloat = realReagent.condutibility - setupFloat;
				allowShowFloat = true;
				allowShowTexture = false;
			}
			break;
				
			case MACHINES.SPCTROPHOTOMETER_UV:
			{
				resultTexture = realReagent.uvSpecter;
				allowShowFloat = false;
				allowShowTexture = true;
			}
			break;
			
			case MACHINES.SPCTROPHOTOMETER_IR:
			{
				resultTexture = realReagent.irSpecter;
				allowShowFloat = false;
				allowShowTexture = true;
			}
			break;
			
			case MACHINES.SPCTROPHOTOMETER_FLAME:
			{
				resultTexture = Resources.Load<Texture2D>("specter/grafico_sem_fitting");
				//resultTexture = realReagent.flameSpecter; //TODO: some shit goes on here
				allowShowFloat = false;
				allowShowTexture = true;
			}
			break;
			
			case MACHINES.POLARIMETER:
			{
				resultFloat = realReagent.polarizibility;
				allowShowFloat = true;
				allowShowTexture = false;
			}
			break;
				
			case MACHINES.HPLC:
			{
				resultTexture = realReagent.hplc;
				allowShowFloat = false;
				allowShowTexture = true;
			}
			break;
			
			case MACHINES.REFRATOMETER:
			{
				resultFloat = realReagent.refratometer;
				allowShowFloat = true;
				allowShowTexture = false;
			}
			break;

			case MACHINES.TURBIDOMETER:
			{
				resultFloat = realReagent.turbility - setupFloat;
				allowShowFloat = true;
				allowShowTexture = false;
			}
			break;
		}

		InventoryController	inventory = FindObjectOfType (typeof(InventoryController)) as InventoryController;


		if (allowShowTexture)
		{
			resultTexture = ChartGenerator.GenerateWithTextureConectingDots(resultTexture, concentration, 0.8f);
			PrinterUse.SendFileToPrinter(resultTexture);

			HudText.SetText("O grafico foi Impresso!");
			timeToShowPrintTextAcc = Time.time;
		}

		if (allowShowFloat) 
		{
			resultFloat *= concentration;		
		}
	}


	private float applyErrorInFloat(float realValue)
	{
		float value = Mathf.Round (realValue * Mathf.Pow (10f, errorPrecision) + (Random.Range(-1f,1f) * errorAmplitude));
		return value / Mathf.Pow (10f, errorPrecision);
	}


	public void ButtonSetup()
	{
		Setup (actualReagent, actualConcentration);
	}

	public void ButtonUse()
	{
		Use (actualReagent, actualConcentration);
	}
}

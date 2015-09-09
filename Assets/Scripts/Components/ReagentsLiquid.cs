﻿using UnityEngine;
using System.Collections;

//! Enable and Disable ReagentsLiquid.
/*! Changes the colors of the reagent liquid when is enable or desable.
 * Mass and volume of liquid and actions by clicking on an liquid. */
public class ReagentsLiquid : MonoBehaviour 
{
	public float concentration;
	public float mass;
	public float volume;
	
	public float alphaValueWhenDisable;

	private InventoryController inventory;
	private Vector3 originalPosition;

	public ReagentsLiquidClass info;

	private bool _inInventory = false;

	public bool inInventory {
		get {
			return _inInventory;
		}
		set {
			_inInventory = value;
		}
	}

	//! Returns the inventory object and its position.
	/*! Script instance is being loaded. */
	void Awake()
	{
		inventory = FindObjectOfType (typeof(InventoryController)) as InventoryController;
		originalPosition = this.transform.position;
	}

	//! Calculates the mass and the volume of liquid.
	// Use this for initialization. 
	void Start () 
	{
		if (info != null) 
		{
			if (mass != 0f)
			{
				mass = info.density * volume;		
			}

			if (volume != 0f) 
			{
				volume = mass / info.density;		
			}
		}
	}

	//! Changes (Enable) the color object when collide with other colliders
	/*! Returns the first instantiated Material assigned to the renderer and creat a new object with certain color. */
	public void Enable()
	{
		this.collider.enabled = true;
		foreach (Transform child in this.transform) 
		{
			if(child.renderer != null)
			{
				child.renderer.material.color = new Color(child.renderer.material.color.r, child.renderer.material.color.g, child.renderer.material.color.b, 1f);
			}
		}
	}

	//! Changes (Disable) the color object when collide with other colliders
	/*! Returns the first instantiated Material assigned to the renderer and creat a new object with certain color. */
	public void Disable()
	{
		this.collider.enabled = false;
		foreach (Transform child in this.transform) 
		{
			if(child.renderer != null)
			{
				child.renderer.material.color = new Color(child.renderer.material.color.r, child.renderer.material.color.g, child.renderer.material.color.b, alphaValueWhenDisable);
			}
		}
	}

	//! Actions by clicking on an item.
	/*! If the application is "lab_workBench", remove reagent liquid from inventory, else if the liquid is in inventory, set the name and the concentration. */
	public void MsgMouseDown()
	{
		if(Application.loadedLevelName == "lab_workBench")
		{
			WorkBench wb = FindObjectOfType(typeof(WorkBench)) as WorkBench;
			Enable();
			inventory.RemoveReagentLiquid(this);
			wb.useSlot(this, null);
		}
		else
		{
			MachineBehaviour machine = FindObjectOfType (typeof(MachineBehaviour)) as MachineBehaviour;

			if (machine != null && inventory) 
			{
				if(machine.textResult != null)
				{
					machine.textResult.text = "waiting";
				}
				machine.actualReagent = info.name;
				machine.actualConcentration = concentration;

			}
		}
	}

	//! Loads reagentsLiquid
	public void GetInfo(string reagent)
	{
		info = ComponentsSaver.LoadReagents()[reagent];
	}
}

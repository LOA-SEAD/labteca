using UnityEngine;
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

	public Compound info;

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
				mass = info.Density * volume;		
			}

			if (volume != 0f) 
			{
				volume = mass / info.Density;
			}
		}
	}

	//! Changes (Enable) the color object when collide with other colliders
	/*! Returns the first instantiated Material assigned to the renderer and creat a new object with certain color. */
	public void Enable()
	{
		this.GetComponent<Collider>().enabled = true;
		foreach (Transform child in this.transform) 
		{
			if(child.GetComponent<Renderer>() != null)
			{
				child.GetComponent<Renderer>().material.color = new Color(child.GetComponent<Renderer>().material.color.r, child.GetComponent<Renderer>().material.color.g, child.GetComponent<Renderer>().material.color.b, 1f);
			}
		}
	}

	//! Changes (Disable) the color object when collide with other colliders
	/*! Returns the first instantiated Material assigned to the renderer and creat a new object with certain color. */
	public void Disable()
	{
		this.GetComponent<Collider>().enabled = false;
		foreach (Transform child in this.transform) 
		{
			if(child.GetComponent<Renderer>() != null)
			{
				child.GetComponent<Renderer>().material.color = new Color(child.GetComponent<Renderer>().material.color.r, child.GetComponent<Renderer>().material.color.g, child.GetComponent<Renderer>().material.color.b, alphaValueWhenDisable);
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
//			wb.useSlot(this, null); //TODO descomentar isso, fazendo as alteraçoes necessarias no workbench
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
				machine.actualReagent = info.Formula;
				machine.actualConcentration = concentration;

			}
		}
	}

	//! Loads reagentsLiquid
	public void GetInfo(string reagent)
	{
		info = CompoundFactory.GetInstance().GetCupboardCompound(reagent) as Compound;
	}
}

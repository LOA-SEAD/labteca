using UnityEngine;
using System.Collections;

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

	void Awake()
	{
		inventory = FindObjectOfType (typeof(InventoryController)) as InventoryController;
		originalPosition = this.transform.position;
	}

	// Use this for initialization
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
	
	// Update is called once per frame
	void Update () 
	{
		
	}

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
				inventory.DisactiveAllReagentsLiquid();
			}
		}
	}

	public void GetInfo(string reagent)
	{
		info = ComponentsSaver.LoadReagents()[reagent];
	}
}

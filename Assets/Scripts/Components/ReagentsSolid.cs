using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ReagentsSolid : MonoBehaviour 
{
	public string name;
	public float totalMass;

	public string liquidName;

	public float alphaValueWhenDisable;

	private List<GameObject> unitys = new List<GameObject>();

	private InventoryController inventory;
	private Vector3 originalPosition;

	public bool solidSource;

	void Awake()
	{
		inventory = FindObjectOfType (typeof(InventoryController)) as InventoryController;
		originalPosition = this.transform.position;
	}

	// Use this for initialization
	void Start () 
	{

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
		if(Application.loadedLevelName == "Balance" && solidSource)
		{
			BalanceController balance = FindObjectOfType(typeof(BalanceController)) as BalanceController;

			balance.SelectedSolid();

			balance.solidSelected = this;

			inventory.DisactiveAllCharts();
			inventory.DisactiveAllGlassware();
			inventory.DisactiveAllReagentsLiquid();
			inventory.DisactiveAllReagentsSolid();
		}

		if (Application.loadedLevelName == "lab_workBench" && !solidSource) 
		{
			WorkBench wb = FindObjectOfType (typeof(WorkBench)) as WorkBench;
			Enable ();
			inventory.RemoveReagentSolid (this);
			wb.useSlot (null, this);
		} 

	}
}

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

//! Enable and Disable ReagentsSolid.
/*! Changes the colors of the reagent solid when is enable or desable. */

public class ReagentsSolid : MonoBehaviour 
{
	public string name;

	public float totalMass;
	public string liquidName;
	private List<GameObject> unitys = new List<GameObject>();

	public float alphaValueWhenDisable;

	private InventoryController inventory;
	private Vector3 originalPosition;  /*!< 3D positions and directions around. */

	public bool solidSource;

	//! Returns the inventory object and its position.
	/*! Script instance is being loaded. */
	void Awake()
	{
		inventory = FindObjectOfType (typeof(InventoryController)) as InventoryController;
		originalPosition = this.transform.position;
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

	//! Changes (Diable) the color object when collide with other colliders
	/*! Returns the first instantiated Material assigned to the renderer and creat a new object with alphaValueWhenDisable. */
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

	//! There is no code (it's commented)
	/*! //! Actions by clicking on an item.
	/*! If the application is "Scale" and solidSource is true, desactives all (charts, glassware, reagents) 
	 * If the application is "lab_workBench", remove reagent solid from inventory. */
	public void MsgMouseDown()
	{
//		if(Application.loadedLevelName == "Balance" && solidSource)
//		{
//			BalanceController balance = FindObjectOfType(typeof(BalanceController)) as BalanceController;
//
//			balance.SelectedSolid();
//
//			balance.solidSelected = this;
//
//			inventory.DisactiveAllCharts();
//			inventory.DisactiveAllGlassware();
//			inventory.DisactiveAllReagentsLiquid();
//			inventory.DisactiveAllReagentsSolid();
//		}
//
//		if (Application.loadedLevelName == "lab_workBench" && !solidSource) 
//		{
//			WorkBench wb = FindObjectOfType (typeof(WorkBench)) as WorkBench;
//			Enable ();
//			inventory.RemoveReagentSolid (this);
//			wb.useSlot (null, this);
//		} 

	}
}

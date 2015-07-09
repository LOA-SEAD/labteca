using UnityEngine;
using System.Collections;
using System.Collections.Generic;

//! Enable and Disable ReagentsSolid.
/*! Changes the colors of the reagent solid when is enable or desable. */
//TODO: Testar para saber exatamente como funciona.
// Algumas coisas do ReagentsSolid e ReagentLiquid sao iguais. Talvez seja melhor criar uma classe Reagents e herdar Solid e Liquid. Elimina hardcode.
public class ReagentsSolid : MonoBehaviour 
{
	public string name;
	public float totalMass;

	public string liquidName;

	public float alphaValueWhenDisable;

	private List<GameObject> unitys = new List<GameObject>();

	private InventoryController inventory;
	private Vector3 originalPosition;  /*!< 3D positions and directions around. */

	public bool solidSource;

	//! Script instance is being loaded.
	/*! Returns the first active loaded object of Type InventoryController and set position of this object.*/
	void Awake()
	{
		inventory = FindObjectOfType (typeof(InventoryController)) as InventoryController;
		originalPosition = this.transform.position;
	}

	// Use this for initialization
	/*void Start () 
	{

	}
	
	// Update is called once per frame
	void Update () 
	{
	
	}*/

	//! Enable collide with other colliders
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

	//! Disable collide with other colliders
	/*! Returns the first instantiated Material assigned to the renderer and creat a new object with alphaValueWhenDisable. */
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

	//! There is no code (it's commented)
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

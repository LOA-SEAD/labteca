using UnityEngine;
using System.Collections;

public class InventorySelectorInMachineScene : MonoBehaviour 
{

	private InventoryController inventory;
	private MachineBehaviour machine;

	public bool allowGlassware;
	public bool allowLiquids;
	public bool allowSolids;
	public bool allowCharts;

	// Use this for initialization
	void Start () 
	{
		inventory = FindObjectOfType (typeof(InventoryController)) as InventoryController;
		machine = FindObjectOfType (typeof(MachineBehaviour)) as MachineBehaviour;

//		if (!allowGlassware) 
//		{
//			inventory.DisactiveAllGlassware();
//		}
//		else
//		{
//			/inventory.ActiveAllGlassware();
//		}
//
//		if (!allowLiquids) 
//		{
//			inventory.DisactiveAllReagentsLiquid();
//		}
//		else
//		{
//			inventory.ActiveAllReagentsLiquid();
//		}
//
//		if (!allowSolids) 
//		{
//			inventory.DisactiveAllReagentsSolid();
//		}
//		else
//		{
//			inventory.ActiveAllReagentsSolid();
//		}
//
//		if (!allowCharts) 
//		{
//			inventory.DisactiveAllCharts();
//		}
//		else
//		{
//			inventory.ActiveAllCharts();
//		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}

using UnityEngine;
using System.Collections;

// TODO: Este codigo provavelmente eh da versao antiga e esta obsoleto, nao sera mais utilizado. Verificar.	
public class InventorySelectorInMachineScene : MonoBehaviour 
{

	private InventoryController inventory;
	private MachineBehaviour machine;

	public bool allowGlassware;
	public bool allowLiquids;
	public bool allowSolids;
	public bool allowCharts;

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

}

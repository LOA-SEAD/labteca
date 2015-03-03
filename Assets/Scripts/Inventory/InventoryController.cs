using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class InventoryController : MonoBehaviour 
{
	public Dictionary<ReagentsSolid,GameObject> reagentsSolid = new Dictionary<ReagentsSolid, GameObject>();
	public Dictionary<ReagentsLiquid,GameObject> reagentsLiquid = new Dictionary<ReagentsLiquid, GameObject>();
	public Dictionary<Glassware,GameObject> glassware = new Dictionary<Glassware, GameObject>();
	public Dictionary<Chart,GameObject> chart = new Dictionary<Chart, GameObject>();

	public GameObject[] slotPositions;

	public GameObject selectedItem;

	private bool _showingChart = false;

	public Animator fade;

	public bool showingChart 
	{
		get 
		{
			return _showingChart;
		}
		set 
		{
			_showingChart = value;
		}
	}

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () 
	{
	
	}

	public void UpdateInventory()
	{
		int counter = 0;
		foreach (GameObject item in glassware.Values) 
		{
			item.transform.position = slotPositions[counter].transform.position;
			counter++;
		}

		foreach (GameObject item in reagentsSolid.Values) 
		{
			item.transform.position = slotPositions[counter].transform.position;
			counter++;
		}

		foreach (GameObject item in reagentsLiquid.Values) 
		{
			item.transform.position = slotPositions[counter].transform.position;
			counter++;
		}

		foreach (GameObject item in chart .Values) 
		{
			item.transform.position = slotPositions[counter].transform.position;
			counter++;
		}
	}
		
	//TODO: remove hard code!
	public void AddReagentLiquid(ReagentsLiquid reagent)
	{
		Debug.Log ("added " + reagent.info.name + " in inventory!");
		reagent.gameObject.layer = 10;
		foreach (Transform child in reagent.transform) 
		{
			child.gameObject.layer = 10;
		}
		reagentsLiquid.Add (reagent, reagent.gameObject);
		UpdateInventory ();
	}

	public void AddReagentSolid(ReagentsSolid reagent)
	{
		Debug.Log ("added " + reagent.name + " in inventory!");

		reagent.gameObject.layer = 10;
		foreach (Transform child in reagent.transform) 
		{
			child.gameObject.layer = 10;
		}
		reagentsSolid.Add (reagent, reagent.gameObject);
		UpdateInventory ();
	}

	public void AddGlassware(Glassware glass)
	{
		Debug.Log ("added glass in inventory!");

		glass.gameObject.layer = 10;
		foreach (Transform child in glass.transform) 
		{
			child.gameObject.layer = 10;
		}
		glassware.Add (glass, glass.gameObject);
		UpdateInventory ();
	}

	public void AddChart(Chart graph)
	{
		graph.gameObject.layer = 10;
		foreach (Transform child in graph.transform) 
		{
			child.gameObject.layer = 10;
		}
		chart.Add (graph, graph.gameObject);
		UpdateInventory ();
	}

	public void RemoveReagentLiquid(ReagentsLiquid reagent)
	{
		if (reagentsLiquid.ContainsKey (reagent)) 
		{
			reagent.gameObject.layer = 0;
			foreach (Transform child in reagent.transform) 
			{
				child.gameObject.layer = 0;
			}
			reagentsLiquid.Remove(reagent);
			UpdateInventory ();
		}
	}
	
	public void RemoveReagentSolid(ReagentsSolid reagent)
	{
		if (reagentsSolid.ContainsKey (reagent)) 
		{
			reagent.gameObject.layer = 0;
			foreach (Transform child in reagent.transform) 
			{
				child.gameObject.layer = 0;
			}
			reagentsSolid.Remove(reagent);
			UpdateInventory ();
		}
	}
	
	public void RemoveGlassware(Glassware glass)
	{
		if (glassware.ContainsKey (glass)) 
		{
			glass.gameObject.layer = 0;
			foreach (Transform child in glass.transform) 
			{
				child.gameObject.layer = 0;
			}
			glassware.Remove(glass);
			UpdateInventory ();
		}
	}

	public void RemoveChart(Chart graph)
	{
		if (chart.ContainsKey (graph)) 
		{
			graph.gameObject.layer = 0;
			foreach (Transform child in graph.transform) 
			{
				child.gameObject.layer = 0;
			}
			chart.Remove(graph);
			UpdateInventory ();
		}
	}

	public void ActiveAllGlassware()
	{
		foreach (Glassware item in glassware.Keys) 
		{
			item.Enable();
		}
	}

	public void ActiveAllReagentsSolid()
	{
		foreach (ReagentsSolid item in reagentsSolid.Keys) 
		{
			item.Enable();
		}
	}

	public void ActiveAllReagentsLiquid()
	{
		foreach (ReagentsLiquid item in reagentsLiquid.Keys) 
		{
			item.Enable();
		}
	}

	public void ActiveAllCharts()
	{
		foreach (Chart item in chart.Keys) 
		{
			item.Enable();
		}
	}

	public void DisactiveAllGlassware()
	{
		foreach (Glassware item in glassware.Keys) 
		{
			item.Disable();
		}
	}
	
	public void DisactiveAllReagentsSolid()
	{
		foreach (ReagentsSolid item in reagentsSolid.Keys) 
		{
			item.Disable();
		}
	}
	
	public void DisactiveAllReagentsLiquid()
	{
		foreach (ReagentsLiquid item in reagentsLiquid.Keys) 
		{
			item.Disable();
		}
	}

	public void DisactiveAllCharts()
	{
		foreach (Chart item in chart.Keys) 
		{
			item.Disable();
		}
	}

	public void ShowFade(){
		fade.SetTrigger("fade");
	}
}

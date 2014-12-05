using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PrinterUse : MonoBehaviour 
{
	public KeyCode keyToUse;

	private static bool allowGetData = false;

	private static List<Texture2D> pressSheets = new List<Texture2D>();

	private InventoryController inventory;

	public Chart chartPrefab;

	// Use this for initialization
	void Start () 
	{
		inventory = FindObjectOfType (typeof(InventoryController)) as InventoryController;
	}
	
	// Update is called once per frame
	void Update () {

	}

	void OnTriggerEnter(Collider other)
	{
		if (other.tag == "Player" && allowGetData) 
		{
			HudText.SetText("Aperte " + keyToUse.ToString() + " para pegar as folhas da impressora.");
		}
	}

	void OnTriggerStay(Collider other)
	{
		if (other.tag == "Player" && allowGetData) 
		{
			if(Input.GetKeyDown(keyToUse))
			{
				HudText.EraseText();
				allowGetData = false;

				foreach (Texture2D item in pressSheets) 
				{
					Chart graph = Instantiate(chartPrefab) as Chart;
					graph.transform.parent = inventory.transform;
					graph.SetChart(item);
					graph.gameObject.layer = 10;
					inventory.AddChart(graph);
				}

				pressSheets.Clear();
			}
		}
	}

	void OnTriggerExit(Collider other)
	{
		if (other.tag == "Player" && allowGetData) 
		{
			HudText.EraseText();
		}
	}


	public static void SendFileToPrinter(Texture2D file)
	{
		allowGetData = true;
		pressSheets.Add (file);
	}
}

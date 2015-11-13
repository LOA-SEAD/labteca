using UnityEngine;
using System.Collections;
using System.Collections.Generic;

//! Interaction with the printer (LIA)
/*!
 *  Contains four methods that recognizes the interaction with Player and Printer.
 * LIA communicates with the player through the printer.
 * Machines that use the printer: Espectrofotometria UV-Vis.
 */

public class PrinterUse : MonoBehaviour 
{
	public KeyCode keyToUse; /*!< Capture keyboard input */

	public bool allowGetData = false;

	private static List<Texture2D> pressSheets = new List<Texture2D>(); /*!< List of Texture2D*/

	private InventoryController inventory; /*!< InventoryController object. */

	public Chart chartPrefab; /*!< Chart object. */

	// Use this for initialization
	//! Returns the first loaded object of InventoryController.
	/*! */
	void Start () 
	{
		inventory = FindObjectOfType (typeof(InventoryController)) as InventoryController;
	}

	//! Is called when the collider other enters the trigger.
	/*! This message is sent to the trigger collider and the rigidbody 
	 * that the trigger collider belongs to, and the rigidbody that touches the trigger.*/
	void OnTriggerEnter(Collider other)
	{
		Debug.Log (other.name);
		if (other.name == "Player" && allowGetData) 
		{
			HudText.SetText("Aperte " + keyToUse.ToString() + " para pegar as folhas da impressora.");
		}
	}
	//! Is called almost all the frames for every collider other that is touching the trigger.
	/*! This message is sent to the trigger and the collider that touches the the trigger. */
	void OnTriggerStay(Collider other)
	{
		if (other.tag == "Player" && allowGetData) 
		{
			if(Input.GetKeyDown(keyToUse))
			{
				HudText.EraseText();
				allowGetData = false;

				//! Add the chart in inventory
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
	//! The collider other has stopped touching the trigger.
	void OnTriggerExit(Collider other)
	{
		if (other.tag == "Player" && allowGetData) 
		{
			HudText.EraseText();
		}
	}

	//! Send the file to printer
	public void SendFileToPrinter(Texture2D file)
	{
		allowGetData = true;
		pressSheets.Add (file);
	}
}

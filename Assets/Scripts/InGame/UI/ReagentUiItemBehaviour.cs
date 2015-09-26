using UnityEngine;
using System.Collections;
using UnityEngine.UI;

//! Set a reagent and add to the inventory.
/*! */

public class ReagentUiItemBehaviour : MonoBehaviour {

	public Text nameReagent;

	private AnyObjectInstantiation reagent;
	private InventoryManager inventoryManager;

	void Start () {
		inventoryManager = GameObject.Find ("InventoryManager").GetComponent<InventoryManager> ();
	}

	/*// Update is called once per frame
	void Update () {
	
	}*/


	public void SetReagent(string name){

		nameReagent.text = name;
	
	}

	//! Add the reagent clicked to the inventory
	public void AddToInventory(){
		//Debug.Log ("Add " + nameReagent.text);
		reagent = GameObject.Find ("GetReagents").GetComponent<GetReagentState>().ReagentInstantiation(nameReagent.text);
		inventoryManager.AddItemToInventory (reagent);

	}

}

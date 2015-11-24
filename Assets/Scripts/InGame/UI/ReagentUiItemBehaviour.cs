using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

//! Set a reagent and add to the inventory.
/*! */

public class ReagentUiItemBehaviour : MonoBehaviour {

	public Text nameReagent;

	public ReagentsBaseClass prefabReagent;

	private InventoryManager inventoryManager;

	void Start () {
		inventoryManager = GameObject.Find ("InventoryManager").GetComponent<InventoryManager> ();
	}

	/*// Update is called once per frame
	void Update () {
	
	}*/


	public void SetReagent(string name, ReagentsBaseClass r){

		nameReagent.text = name;

		prefabReagent = r;

		//Copies the ReagentClass component
		if (r.isSolid) {
			if (UnityEditorInternal.ComponentUtility.CopyComponent (r.GetComponent<ReagentsBaseClass> ())) {
				if (UnityEditorInternal.ComponentUtility.PasteComponentValues (prefabReagent)) {
				}
			}
		} else {
			if (UnityEditorInternal.ComponentUtility.CopyComponent (r.GetComponent<ReagentsLiquidClass> ())) {
				if (UnityEditorInternal.ComponentUtility.PasteComponentValues (prefabReagent)) {
				}
			}
		}
	}

	//! Add the reagent clicked to the inventory
	public void AddToInventory(){
		//Debug.Log ("Add " + nameReagent.text);
		//inventoryManager.AddItemToInventory (GameObject.Find ("GetReagents").GetComponent<GetReagentState>().ReagentInstantiation(nameReagent.text));
		ReagentsBaseClass reagent;
		Dictionary<string, ReagentsBaseClass> reagentDictionary = ComponentsSaver.LoadReagents ();
		reagentDictionary.TryGetValue (nameReagent.text, out reagent);
		Debug.Log (reagent.name);
		inventoryManager.AddReagentToInventory (prefabReagent,reagent);
	}

}

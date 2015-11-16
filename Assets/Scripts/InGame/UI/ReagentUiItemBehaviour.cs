﻿using UnityEngine;
using System.Collections;
using UnityEngine.UI;

//! Set a reagent and add to the inventory.
/*! */

public class ReagentUiItemBehaviour : MonoBehaviour {

	public Text nameReagent;

	private ReagentsBaseClass prefabReagent;

	private InventoryManager inventoryManager;

	void Start () {
		inventoryManager = GameObject.Find ("InventoryManager").GetComponent<InventoryManager> ();
	}

	/*// Update is called once per frame
	void Update () {
	
	}*/


	public void SetReagent(string name, ReagentsBaseClass r){

		nameReagent.text = name;

		//Copies the ReagentClass component
		if (r.isSolid) {
			if (UnityEditorInternal.ComponentUtility.CopyComponent (r.GetComponent<ReagentsBaseClass> ())) {
				if (UnityEditorInternal.ComponentUtility.PasteComponentValues (prefabReagent)) {
				}
			}
		} else {
			if (UnityEditorInternal.ComponentUtility.CopyComponent (r.GetComponent<ReagentsBaseClass> ())) {
				if (UnityEditorInternal.ComponentUtility.PasteComponentValues (prefabReagent)) {
				}
			}
		}
	}

	//! Add the reagent clicked to the inventory
	public void AddToInventory(){
		//Debug.Log ("Add " + nameReagent.text);
		//inventoryManager.AddItemToInventory (GameObject.Find ("GetReagents").GetComponent<GetReagentState>().ReagentInstantiation(nameReagent.text));
		inventoryManager.AddReagentToInventory (prefabReagent);
	}

}

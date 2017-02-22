using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

//! Set a reagent and add to the inventory.
/*! */

public class ReagentUiItemBehaviour : MonoBehaviour {

	public Text nameReagent;
	public List<Sprite> bg;
	public ReagentPot prefabReagentPot;
	private InventoryManager inventoryManager;

	void Start () {
		inventoryManager = GameObject.Find ("InventoryManager").GetComponent<InventoryManager> ();
	}

	/*// Update is called once per frame
	void Update () {
	
	}*/


	public void SetReagent(string name, GameObject r){

		nameReagent.text = name;

		prefabReagentPot = r.GetComponent<ReagentPot>();

		if (r.GetComponent<ReagentPot> ().isSolid)
			gameObject.GetComponent<Image>().sprite = bg[0];
		else
			gameObject.GetComponent<Image>().sprite = bg[1];
		

		//Copies the ReagentClass component
		/*if (r.isSolid) {
			if (UnityEditorInternal.ComponentUtility.CopyComponent (r.GetComponent<ReagentsBaseClass> ())) {
				if (UnityEditorInternal.ComponentUtility.PasteComponentValues (prefabReagent)) {
				}
			}
		} else {
			if (UnityEditorInternal.ComponentUtility.CopyComponent (r.GetComponent<ReagentsLiquidClass> ())) {
				if (UnityEditorInternal.ComponentUtility.PasteComponentValues (prefabReagent)) {
				}
			}
		}*/
	}

	//! Add the reagent clicked to the inventory
	public void AddToInventory(){
		//Debug.Log ("Add " + nameReagent.text);
		//inventoryManager.AddItemToInventory (GameObject.Find ("GetReagents").GetComponent<GetReagentState>().ReagentInstantiation(nameReagent.text));
		Compound reagent;
		reagent = CompoundFactory.GetInstance ().GetCupboardCompound (nameReagent.text);
		inventoryManager.AddReagentToInventory (prefabReagentPot,reagent);
	}

}

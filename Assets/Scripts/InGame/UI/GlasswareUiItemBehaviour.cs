﻿using UnityEngine;
using System.Collections;
using UnityEngine.UI;

//! Set a Glassware and add to inventory.

public class GlasswareUiItemBehaviour : MonoBehaviour {

	public Text nameGlass;

	private Glassware prefabGlassware;

	private int glasswareIndex;  //Position in the glasswareList in GetGlasswareState.cs

	private AnyObjectInstantiation glassware;
	private InventoryManager inventoryManager;

	// Use this for initialization
	void Start () {
		inventoryManager = GameObject.Find ("InventoryManager").GetComponent<InventoryManager> ();
	}
	
	// Update is called once per frame
	/*void Update () {
	
	}*/

	public void SetGlass(Glassware glass, int index){

		nameGlass.text = glass.name;

		prefabGlassware = glass;
	
		glasswareIndex = index;
	}

	/*! Add the glassware to the inventory.*/
	public void AddToInventory(){
		//This is how it is now//inventoryManager.AddItemToInventory (GameObject.Find ("GetGlassware").GetComponent<GetGlasswareState>().GlasswareInstantiation(glasswareIndex));
		inventoryManager.AddGlasswareToInventory (GameObject.Find ("GetGlassware").GetComponent<GetGlasswareState>().GlasswareInstantiation(glasswareIndex)); //This is working
		//Maybe should be like this? inventoryManager.AddGlasswareToInventory (glasswareIndex);

	}



}

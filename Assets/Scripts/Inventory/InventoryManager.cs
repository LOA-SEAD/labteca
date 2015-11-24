﻿using UnityEngine;
using System.Collections;

//! Manages all 'types' of Inventory Controller.
/*! It has information of all of the four types of InventoryController ( Solid, Liquid, Glassware and Others ). */
public class InventoryManager : MonoBehaviour {

    // need to be set on the inspector with the correct game object containing each InventoryController
	public InventoryController SolidReagents;       /*!< InventoryController for Solid Reagents. */
    public InventoryController LiquidReagents;      /*!< InventoryController for Liquid Reagents. */
    public InventoryController Glassware;           /*!< InventoryController for Glassware. */
    public InventoryController Others;              /*!< InventoryController for Others. */

    private AnyObjectInstantiation selectedItem = null;     /*!< Current selected item (game object). */
    private ItemStackableBehavior selectedUIItem = null;    /*!< Current selected item from inventory UI. */


	//TODO: Remover depois!!!
	public GameObject bequerPrefab;
	///////APAGA AQUI/////////////

    //! Does the operation on the inventory (insert, remove or destroy).
    /*! Based on the string passed as parameter, takes the selectedItem or selectedUIItem and does the operation. */
	void operateOnInventory(string op){
        // Insertion
		if(string.Compare(op,"insert") == 0 && selectedItem != null){
            switch(selectedItem.getItemType())
            {
                case ItemType.Solids:
                    SolidReagents.AddItem(selectedItem);
                    break;

                case ItemType.Liquids:
                    LiquidReagents.AddItem(selectedItem);
                    break;

                case ItemType.Glassware:
                    Glassware.AddItem(selectedItem);
                    break;

                case ItemType.Others:
                    Others.AddItem(selectedItem);
                    break;

                default:
                    Debug.LogError("ItemType not found!");
                    break;
            }
		}
        // Removal
		else if(string.Compare(op,"remove") == 0 && selectedUIItem != null)
        {
            switch (selectedUIItem.getObject().getItemType())
            {
                case ItemType.Solids:
                    SolidReagents.RemoveItem(selectedUIItem);
                    break;

                case ItemType.Liquids:
                    LiquidReagents.RemoveItem(selectedUIItem);
                    break;

                case ItemType.Glassware:
                    Glassware.RemoveItem(selectedUIItem);
                    break;

                case ItemType.Others:
                    Others.RemoveItem(selectedUIItem);
                    break;

                default:
                    Debug.LogError("ItemType not found!");
                    break;
            }
		}
        // Destruction
        else if (string.Compare(op, "destroy") == 0 && selectedUIItem != null)
        {
            switch (selectedUIItem.getObject().getItemType())
            {
                case ItemType.Solids:
                    SolidReagents.DestroyItem(selectedUIItem);
                    break;

                case ItemType.Liquids:
                    LiquidReagents.DestroyItem(selectedUIItem);
                    break;

                case ItemType.Glassware:
                    Glassware.DestroyItem(selectedUIItem);
                    break;

                case ItemType.Others:
                    Others.DestroyItem(selectedUIItem);
                    break;

                default:
                    Debug.LogError("ItemType not found!");
                    break;
            }
        }

        selectedItem = null;
	}

    // TODO: Remover estes botoes de teste e utilizar as funcoes nos devidos locais.
    // Ex.: Quando o objeto eh clicado dentro do laboratorio.
	void Update () 
    {
        if(Input.GetKeyDown(KeyCode.A))
        {
			operateOnInventory("insert");
		}
		if(Input.GetKeyDown(KeyCode.S))
        {
			operateOnInventory("remove");
		}
        if(Input.GetKeyDown(KeyCode.D))
        {
            operateOnInventory("destroy");
        }
	}

    //! Set the selected Item.
    public void setSelectedItem(AnyObjectInstantiation i)
    {
        this.selectedItem = i;
    }

    //! Set the selected Item at the inventory UI.
    public void setSelectedUIItem(ItemStackableBehavior i)
    {
        this.selectedUIItem = i;
    }

	//! Add the item to inventory
	public void AddItemToInventory(AnyObjectInstantiation item) {
		Debug.Log (item.gameObject.name);
		setSelectedItem (item);
		operateOnInventory ("insert");
	}
	
	//! Remove from inventory
	public void RemoveItemFromInventory(AnyObjectInstantiation item) {
		setSelectedItem (item);
		operateOnInventory ("remove");
	}

	//-------------LeMigue for Glassware------------------------
	public void AddGlasswareToInventory(Glassware gl) {
		Glassware.GetComponentInChildren<InventoryContent> ().addNewGlasswareUI (gl);
		//Need to save "gl" within the InventoryItemGlassware informations
		//!(It is not actually saving any information right now, only creating the GameObject inside the inventory with the proper name)
	}

	//Written in a temporary way!!!!!!
	public void RemoveGlasswareFromInventory() {
		//Destroy the button (this will be used when the item is being put in the workbench slot)
		GameController gameController = GameObject.Find ("GameController").GetComponent<GameController> ();
		GameObject glasswareInScene = Instantiate(bequerPrefab) as GameObject;// = Instantiate the glassware saved in the informations

		//This here will be done using the PutGlassInTable() method from the WorkBench.
//		glasswareInScene.transform.position = gameController.gameStates [gameController.currentStateIndex].GetComponent<FumeHoodState> ().positionGlass1.transform.position;
		//gameController.GetCurrentState ().GetComponentInParent<WorkBench> ().PutGlassOnTable (true);
	}

	public void DestroyGlasswareFromInvenory() {

	}
	//-------------END OF LeMigue for Glassware-------------------------

	//-------------LeMigue for Reagents------------------------
	public void AddReagentToInventory(ReagentsBaseClass reagent, ReagentsBaseClass r ) {
		if(reagent.isSolid)
			SolidReagents.GetComponentInChildren<InventoryContent> ().addNewReagentsUI (reagent,r);
		if(!reagent.isSolid)
			LiquidReagents.GetComponentInChildren<InventoryContent> ().addNewReagentsUI (reagent,r);
	}
	
	public void RemoveReagentFromInventory() {
		//Destroy the button (this will be used when the item is being put in the workbench slot)
	}
	//-------------END OF LeMigue for Reagents-------------------------
}

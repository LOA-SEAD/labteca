using UnityEngine;
using System.Collections;
using UnityEngine.UI;

//! Base Class to be used to define an object to work with inventory.
/*!
 * This Class is inherited by other classes and is used to define the base for any object that will be used
 * by any inventory.
 */
public class ItemInventoryBase : MonoBehaviour {
	
	public string name;
	public string index;
	public Glassware gl;
	public string reagent;
    public Image icon;                         /*!< Icon that represents this object */
    public bool stackable;                      /*!< If can be stacked. */
    public ItemType itemType;                   /*!< Enum to set this item type: 'solids', 'liquids', 'glassware' and 'others'. */
	public Text underText;
	
	public ItemToInventory itemBeingHeld;
	public GameObject physicalObject;

    //! Empty Constructor
	public ItemInventoryBase (){}

/*	public ItemInventoryBase(Component item)
	{
		Debug.Log ("constructor called");
		if (item.GetComponent<ReagentsBaseClass>()!=null) {
			reagent = item.GetComponent<ReagentsBaseClass>();
			if(reagent.isSolid)
				itemType=ItemType.Solids;
			else
				itemType=ItemType.Liquids;
		}
		if(item.GetComponent<Glassware>()!=null){
			gl = item.GetComponent<Glassware>();
			itemType=ItemType.Glassware;
		}
	}

	public void Info(Component item)
	{
		Debug.Log ("constructor called");
		if (item.GetComponent<ReagentsBaseClass>()!=null) {
			reagent = item.GetComponent<ReagentsBaseClass>();
			if(reagent.isSolid)
				itemType=ItemType.Solids;
			else
				itemType=ItemType.Liquids;
		}
		if(item.GetComponent<Glassware>()!=null){
			gl = item.GetComponent<Glassware>();
			itemType=ItemType.Glassware;
		}
	}
*/
	public void copyData(ItemInventoryBase item)
	{
		 this.name=item.name;
		 this.gl=item.gl;
		 this.reagent = item.reagent;
		 this.icon=item.icon;         
		 this.stackable=item.stackable;   
		 this.itemType=item.itemType;
		 this.itemBeingHeld=item.itemBeingHeld;
		 this.index = item.index;
		 this.underText = item.underText;
	}

	public void addReagent(Compound r){
		reagent = r.name;
		if(r.isSolid)
			itemType=ItemType.Solids;
		else
			itemType=ItemType.Liquids;
	}

	public void addGlassware(Glassware g){
		if(g!=null){
			gl = g;
			itemType=ItemType.Glassware;
		}
	}

    //! Constructor using ItemInventoryBase
    /*public ItemInventoryBase(ItemInventoryBase i)
    {
        this.currentPosition = i.currentPosition;
        this.icon = i.icon;
        this.amountItem = i.amountItem;
        this.stackable = i.stackable;
    }

    //! Set the current variables to i variables.
    public void copyItemBase(ItemInventoryBase i){
        this.currentPosition = i.currentPosition;
        this.icon = i.icon;
        this.amountItem = i.amountItem;
        this.stackable = i.stackable;    
    }*/

    //! Get this object type.
    /*! Returns the enum type that this object represents. */
    public ItemType getItemType()
    {
        return this.itemType;
    }

	public void refreshState(){
		GameStateBase currentState = GameObject.Find("GameController").GetComponent<GameController>().GetCurrentState();
		if (currentState is InGameState)
			disableButton ();
		else {
			if(currentState.gameObject.GetComponent<WorkBench>()!=null){
				enableButton();
				//actionText.text="Utilizar";
			}else if(currentState is GetInterface){
				GetInterface interfaceTest = currentState as GetInterface;
				if(interfaceTest.isGlassware()&&gameObject.GetComponent<AnyObjectInstantiation>().itemType==ItemType.Glassware){
					enableButton();
					//actionText.text="Remover";
				}
				if(!interfaceTest.isGlassware()&&(gameObject.GetComponent<AnyObjectInstantiation>().itemType==ItemType.Liquids||gameObject.GetComponent<AnyObjectInstantiation>().itemType==ItemType.Solids)){
					enableButton();
					//actionText.text="Remover";
				}
			}
		}
	}
	
	public void infoButtonClick(){
		
	}
	
	public void actionButtonClick(){
		GameStateBase currentState = GameObject.Find("GameController").GetComponent<GameController>().GetCurrentState();
		if (currentState.GetComponent<WorkBench> () != null)
			CallWorkbenchToTable ();
		else {
			gameObject.GetComponentInParent<InventoryContent>().removeItemUI(gameObject.GetComponent<ItemStackableBehavior>());
		}
		
		
	}
	
	public void disableButton(){
		/*actionButton.interactable=false;
		actionText.color = new Color (actionText.color.r, actionText.color.g, actionText.color.b, 128/256f);
		actionText.text = "Inativo";*/
	}
	
	public void enableButton(){
		/*actionButton.interactable=true;
		actionText.color = new Color (actionText.color.r, actionText.color.g, actionText.color.b, 1);*/
	}
	
	public void HoldItem(ItemToInventory item) {
		itemBeingHeld = item;
	}
	
	//! Calls the method in the workbench that will put the object on the table
	/*! This method should be used as the onClick effect for the button. It calls the method that will put the
	 * 	item on the table */
	public void CallWorkbenchToTable() {
		//GameObject tempItem = Instantiate (itemBeingHeld.gameObject) as GameObject;
		//gameController.GetCurrentState ().GetComponent<WorkBench> ().PutItemFromInventory (tempItem);
		//GameObject.Find("GameController").GetComponent<GameController>().GetCurrentState().GetComponent<WorkBench> ().PutItemFromInventory (itemBeingHeld,gameObject.GetComponent<ReagentsBaseClass>());
	}
}

//! Enum that defines the Item Type.
/*! Define the four types of objects: 'solids', 'liquids', 'glassware' and 'others'. */
public enum ItemType {
    Solids,         /*!< Enum Solids type. */	
    Liquids,        /*!< Enum Liquids type. */
    Glassware,      /*!< Enum Glassware type. */
    Others          /*!< Enum Others type. */
}
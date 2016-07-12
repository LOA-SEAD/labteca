using UnityEngine;
using System.Collections;
using System.Collections.Generic;
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
	public Image solid;                         /*!< Icon that represents this object */
	public Image liquid;                         /*!< Icon that represents this object */
    public bool stackable;                      /*!< If can be stacked. */
    public ItemType itemType;                   /*!< Enum to set this item type: 'solids', 'liquids', 'glassware' and 'others'. */
	public Text underText;

	public Transform posTab;
	
	public ItemToInventory itemBeingHeld;
	public bool physicalObject;

    //! Empty Constructor
	public ItemInventoryBase (){
	}

	void Start(){
	}

	public void copyData(ItemInventoryBase item)
	{
		 this.name=item.name;
		 this.gl=item.gl;
		 this.reagent = item.reagent;        
		 this.stackable=item.stackable;   
		 this.itemType=item.itemType;
		 this.itemBeingHeld=item.itemBeingHeld;
		 this.index = item.index;
		 this.underText = item.underText;
		this.physicalObject = item.physicalObject;
	}

	public void addReagent(Compound r){
		reagent = r.Name;
		if(r.IsSolid)
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
    //! Get this object type.
    /*! Returns the enum type that this object represents. */
    public ItemType getItemType()
    {
        return this.itemType;
    }

	public void HoldItem(ItemToInventory item) {
		itemBeingHeld = item;
	}

}

//! Enum that defines the Item Type.
/*! Define the four types of objects: 'solids', 'liquids', 'glassware' and 'others'. */
public enum ItemType {
    Solids,         /*!< Enum Solids type. */	
    Liquids,        /*!< Enum Liquids type. */
    Glassware,      /*!< Enum Glassware type. */
    Others,          /*!< Enum Others type. */
	Null			/*!< Enum Null type. */
}
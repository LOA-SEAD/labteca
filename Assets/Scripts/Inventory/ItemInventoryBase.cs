using UnityEngine;
using System.Collections;
using UnityEngine.UI;

//! Base Class to be used to define an object to work with inventory.
/*!
 * This Class is inherited by other classes and is used to define the base for any object that will be used
 * by any inventory.
 */
public class ItemInventoryBase : MonoBehaviour {

	public GameObject objectReceived;
	public string name;
	public Glassware gl;
	public ReagentsBaseClass reagent;
    public Sprite icon;                         /*!< Icon that represents this object */
    public bool stackable;                      /*!< If can be stacked. */
    public ItemType itemType;                   /*!< Enum to set this item type: 'solids', 'liquids', 'glassware' and 'others'. */

    //! Empty Constructor
	public ItemInventoryBase (){}

	public ItemInventoryBase(Component item)
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
}

//! Enum that defines the Item Type.
/*! Define the four types of objects: 'solids', 'liquids', 'glassware' and 'others'. */
public enum ItemType {
    Solids,         /*!< Enum Solids type. */	
    Liquids,        /*!< Enum Liquids type. */
    Glassware,      /*!< Enum Glassware type. */
    Others          /*!< Enum Others type. */
}
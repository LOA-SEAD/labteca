using UnityEngine;
using System.Collections;
using UnityEngine.UI;

//! Base Class to be used to define an object to work with inventory.
/*!
 * This Class is inherited by other classes and is used to define the base for any object that will be used
 * by any inventory.
 */
public abstract class ItemInventoryBase : MonoBehaviour {

	public int currentPosition;                 /*!< Current position at the inventory interface. */
    public Sprite icon;                         /*!< Icon that represents this object */
    public float amountItem;                    /*!< Amount of 'this' item. */
    public bool stackable;                      /*!< If can be stacked. */
    public ItemType itemType;                   /*!< Enum to set this item type: 'solids', 'liquids', 'glassware' and 'others'. */
    protected InventoryController inventory;    /*!< Inventory that this object belongs. */

    //! Empty Constructor
    public ItemInventoryBase()
    {

    }

    //! Constructor using ItemInventoryBase
    public ItemInventoryBase(ItemInventoryBase i)
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
    }

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
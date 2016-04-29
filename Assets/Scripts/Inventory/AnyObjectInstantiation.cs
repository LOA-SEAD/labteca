using UnityEngine;
using System.Collections;

//! Used to define a GameObject as an object that can be used by the inventory.
/*!
 *  This class inherits the ItemInventoryBase that is used to allow this GameObject to be accessed and
 *  stored by any inventory inside the game.
 */
public class AnyObjectInstantiation : ItemInventoryBase {

    private InventoryManager inventoryManager;
    
    //! Tries to find the inventoryManager inside object InventoryManager
    void Start()
    {
        inventoryManager = GameObject.Find("InventoryManager").GetComponent<InventoryManager>();
        if(inventoryManager == null)
        {
            Debug.LogError("AnyObjectInstantiation : couldn't find 'InventoryManager'.");
        }
    }

    //! Link the object where this script is attached to inventoryManager.
    // This method is used in buttons and it calls the setSelectedItem from inventoryManager using the object
    //  that this script is attached as reference.
    public void btnItem()
    {
        Debug.Log("Item " + this.name + " selected of " + this.getItemType() + " type");
        //inventoryManager.setSelectedItem(this);
    }

    //! Set the inventory where this object belongs.
    //! Each inventory has an InventoryController, this method sets the inventory inherited from ItemInventoryBase
    //   to the targetInventory. 
    public void SetInventory(InventoryController targetInventory)
    {
        //this.inventory = targetInventory;
    }

    //! Get the inventory that is attached to this object.
    //! Returns an InventoryController that is the inventory attached to this object. 
    public InventoryController GetInventory()
    {
		return null;
    }

    //! Set the current position at the inventory grid.
    // <summary>
    /// Sets the current position.
    /// </summary>
    /// <param name="pos">Position.</param> The inventory interface uses a grid layout for positioning and this variable stores that position. //
    public void setCurrentPosition(int pos)
    {
        //this.currentPosition = pos;
    }

    //! Get the current position at the inventory grid.
    // Returns the current position that this object is allocated at the inventory grid layout. */
    public int getCurrentPosition()
    {
		return 0;
    }

}

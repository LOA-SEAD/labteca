using UnityEngine;
using System.Collections;

//! Stackable Behavior for Items.
/*!
 *  Used at the InventoryContent to link the AnyObjectInstantiation and the Inventory Interface.
 */
public class ItemStackableBehavior : MonoBehaviour {

    private InventoryManager inventoryManager;      /*!< Inventory Manager on current scene. */
    public AnyObjectInstantiation linkedObject;     /*!< Linked Object */

    void Start()
    {
        inventoryManager = GameObject.Find("InventoryManager").GetComponent<InventoryManager>();
        if (inventoryManager == null)
            Debug.LogError("ItemStackableBehavior: couldn't find 'InventoryManager'.");
    }

    //! Set linked object to AnyObjectInstantiation.
    public void setObject(AnyObjectInstantiation i)
    {
        this.linkedObject = i;
    }

    //! Get linked object.
    public AnyObjectInstantiation getObject()
    {
        return this.linkedObject;
    }

    //! Items inside the inventory interface.
    /*! Each item is also a button that can be selected, this method is used to set the selected UI item. */
    public void btnItemUI()
    {
        //inventoryManager.setSelectedUIItem(this.gameObject.GetComponent<ItemStackableBehavior>()); 
        
        Debug.Log("Item " + this.name + 
            " selected in " + linkedObject.GetInventory().transform.parent.transform.parent.name +
            " at position " + linkedObject.getCurrentPosition());
    }
}

using UnityEngine;
using System.Collections;

public class AnyObjectInstantiation : ItemInventoryBase {

    private InventoryManager inventoryManager;
    
    void Start()
    {
        inventoryManager = GameObject.Find("Main Camera").GetComponent<InventoryManager>();
    }

    public void btnItem()
    {
        Debug.Log("Item " + this.name + " selected of " + this.getItemType() + " type");
        inventoryManager.setSelectedItem(this);
    }

    public void SetInventory(InventoryController targetInventory)
    {
        this.inventory = targetInventory;
    }

    public InventoryController GetInventory()
    {
        return this.inventory;
    }

    public void setCurrentPosition(int pos)
    {
        this.currentPosition = pos;
    }

    public int getCurrentPosition()
    {
        return this.currentPosition;
    }

}

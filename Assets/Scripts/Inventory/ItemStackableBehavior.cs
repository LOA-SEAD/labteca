using UnityEngine;
using System.Collections;

public class ItemStackableBehavior : MonoBehaviour {


    private InventoryManager inventoryManager;
    public AnyObjectInstantiation linkedObject;

    void Start()
    {
        inventoryManager = GameObject.Find("Main Camera").GetComponent<InventoryManager>();
    }

    public void setObject(AnyObjectInstantiation i)
    {
        this.linkedObject = i;
    }

    public AnyObjectInstantiation getObject()
    {
        return this.linkedObject;
    }

    public void btnItemUI()
    {
        Debug.Log("Item " + this.name + 
            " selected in " + linkedObject.GetInventory().transform.parent.transform.parent.name +
            " at position " + linkedObject.getCurrentPosition());

        inventoryManager.setSelectedUIItem(this.gameObject.GetComponent<ItemStackableBehavior>());
    }
}

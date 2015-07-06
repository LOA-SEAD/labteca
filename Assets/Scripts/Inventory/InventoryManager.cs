using UnityEngine;
using System.Collections;

public class InventoryManager : MonoBehaviour {

	public InventoryController SolidReagents;
	public InventoryController LiquidReagents;
	public InventoryController Glassware;
	public InventoryController Others;

    private AnyObjectInstantiation selectedItem = null;
    private ItemStackableBehavior selectedUIItem = null;

	void operateOnInventory(string op){

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

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
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

    public void setSelectedItem(AnyObjectInstantiation i)
    {
        this.selectedItem = i;
    }
    public void setSelectedUIItem(ItemStackableBehavior i)
    {
        this.selectedUIItem = i;
    }
}

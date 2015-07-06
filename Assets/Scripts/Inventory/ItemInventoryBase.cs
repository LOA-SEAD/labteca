using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public abstract class ItemInventoryBase : MonoBehaviour {

	public int currentPosition;
	public Sprite icon;
	public float amountItem;
	public bool stackable;
    public ItemType itemType;
	protected InventoryController inventory;

    // empty constructor
    public ItemInventoryBase()
    {

    }

    public ItemInventoryBase(ItemInventoryBase i)
    {
        this.currentPosition = i.currentPosition;
        this.icon = i.icon;
        this.amountItem = i.amountItem;
        this.stackable = i.stackable;
    }

    public void copyItemBase(ItemInventoryBase i){
        this.currentPosition = i.currentPosition;
        this.icon = i.icon;
        this.amountItem = i.amountItem;
        this.stackable = i.stackable;    
    }

    public ItemType getItemType()
    {
        return this.itemType;
    }
}

public enum ItemType {
    Solids,
    Liquids,
    Glassware,
    Others
}
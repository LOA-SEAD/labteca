using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class InventoryContent : MonoBehaviour {

    public GameObject prefabItem;
    private int rows = 2, columns = 6, maxItems;
    private float x_Offset, y_Offset;
    private RectTransform contentRect, prefabRect;
    private int lastItemPos = 0;
    private List<int> freedPos = new List<int>();
    
    void Start()
    {
        contentRect = GetComponent<RectTransform>();
        prefabRect = prefabItem.GetComponent<RectTransform>();
        columns = (int)(contentRect.rect.width / prefabRect.rect.width);
        rows = (int)(contentRect.rect.height / prefabRect.rect.height);
        x_Offset = (contentRect.rect.width - (columns * prefabRect.rect.width))/(columns + 1);
        y_Offset = (contentRect.rect.height - (rows * prefabRect.rect.height)) / (rows + 1);
    }

    public void addNewItemUI(AnyObjectInstantiation item)
    {  
        int itemPos;
        if (freedPos.Count == 0)
            itemPos = lastItemPos;
        else
        {
            itemPos = freedPos[0];
            freedPos.Remove(itemPos);
        }

        // set prefab ItemIventoryBase
        item.setCurrentPosition(itemPos);
        //prefabItem.GetComponent<AnyObjectInstantiation>().copyItemBase(item);

        Debug.Log("Adding " + item.name 
            + " to " + itemPos 
            + " at " + transform.parent.transform.parent.transform.parent.name);
        
        // find x and y for position
        float x = (itemPos % columns) * (prefabRect.rect.width + x_Offset);
        float y = -1 * (y_Offset + (itemPos / columns) * (prefabRect.rect.height + y_Offset));

        // add content size to fit more items
        if ((itemPos % columns) == 0 && (itemPos % rows) == 0 && itemPos > columns)
        {
            contentRect.sizeDelta = new Vector2(
                contentRect.rect.width,
                contentRect.rect.height + prefabRect.rect.height + y_Offset);
        }
        
        // set position
        Vector3 currentPos = new Vector3(x, y);

        // instantiate Item
        GameObject tempItem = Instantiate(prefabItem.gameObject,
                                          currentPos,
                                          prefabItem.transform.rotation) as GameObject;

        // next position on inventory grid
        lastItemPos++;

        // set new item parent to scroll rect content
        tempItem.GetComponent<Image>().sprite = item.icon;
        tempItem.transform.SetParent(contentRect.transform, false);
        tempItem.name = item.name + "_" +itemPos;

        tempItem.GetComponent<ItemStackableBehavior>().setObject(item);
    }

    public void removeItemUI(ItemStackableBehavior itemUI)
    {
        Debug.Log("Removing " + itemUI.name
            + " from " + itemUI.getObject().getCurrentPosition()
            + " at " + transform.parent.transform.parent.transform.parent.name);

        freedPos.Add(itemUI.getObject().getCurrentPosition());
        freedPos.Sort();
        
        Debug.Log("Freed: " + freedPos[0]);
        
        lastItemPos--;

        Destroy(itemUI.gameObject);
    }


}

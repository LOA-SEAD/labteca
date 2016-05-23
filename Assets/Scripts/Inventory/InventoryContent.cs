using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

//! Work as Inventory interface controller.
/*!
 *  Defines where the new object will be instantiated inside the inventory grid layout, controls the last used position, 
 *  RectTransform contentRect size, free positions and number of rows and columns.
 */
public class InventoryContent : MonoBehaviour {

    public GameObject prefabItem;                       /*!< Prefab that will be put inside the content. */
    private int rows = 2000000, columns = 1, maxItems;        /*!< Number of rows, columns and maxItems. */
    private float x_Offset, y_Offset;                   /*!< X and Y offset. */
    private RectTransform contentRect, prefabRect;      /*!< Scrollrect contentRect and prefabRect. */
    private int lastItemPos = 0;                        /*!< Last item position inside the inventory. */
    private List<int> freedPos = new List<int>();       /*!< List of free positions before last item position. */
    
    //! Start all private variables
    void Start()
    {
        contentRect = GetComponent<RectTransform>();
        prefabRect = prefabItem.GetComponent<RectTransform>();
        columns = (int)(contentRect.rect.width / prefabRect.rect.width);    /*!< Columns = contentRect.width / prefabRect.width */
        rows = (int)(contentRect.rect.height / prefabRect.rect.height);     /*!< Rows = contentRect.height / prefabRect.height */
        //x_Offset = (contentRect.rect.width - (columns * prefabRect.rect.width))/(columns + 1); 
        y_Offset = 10f;
		//contentRect.transform.localPosition = new Vector3 (170, 0, 0);
        // verify for error
        if (contentRect == null)
            Debug.LogError("InventoryContent : couldnt get component RectTranform for 'contentRect'.");
        if (prefabRect == null)
            Debug.LogError("InventoryContent : couldnt get component RectTransform for 'prefabRect'.");
    }

    //! Add New Item to Inventory UI
    /*! Receives an item, defines it's position inside the grid layout, instantiates a new prefab with the item's properties
     *  at the position defined.*/
    public void addNewItemUI(AnyObjectInstantiation item)
    {  
        // if there is freedPosition, use it. If NOT, use the last item position.
        //prefabItem.GetComponent<AnyObjectInstantiation>().copyItemBase(item);

        Debug.Log("Adding " + item.name 
            + " to " + 1
            + " at " + transform.parent.transform.parent.transform.parent.name);
        
        // find x and y for position
		float y = (prefabRect.rect.height + y_Offset) * lastItemPos ;
		
		// set position
		Vector3 currentPos = new Vector3(1f, -60-110*(lastItemPos));


        // add content size to fit more items

		contentRect.sizeDelta = new Vector2(
			1f, // width doesnt change
			120 + (prefabRect.rect.height + y_Offset) * lastItemPos);

        // instantiate Item
        GameObject tempItem = Instantiate(prefabItem.gameObject,
                                          currentPos,
                                          prefabItem.transform.rotation) as GameObject;

        // next position on inventory grid
        lastItemPos++;

        // set new item parent to scroll rect content
        //tempItem.GetComponent<Image>().sprite = item.icon;
        tempItem.transform.SetParent(contentRect.transform, false);
        tempItem.name = item.name + "_" +1;

        tempItem.GetComponent<ItemStackableBehavior>().setObject(item);
    }

	/*//-----------------LeMigue para Glassware-----------------------
	public void addNewGlasswareUI(Glassware item)
	{  
		// if there is freedPosition, use it. If NOT, use the last item position.
		//prefabItem.GetComponent<AnyObjectInstantiation>().copyItemBase(item);
		
		Debug.Log("Adding " + item.name 
		          + " to " + 1
		          + " at " + transform.parent.transform.parent.transform.parent.name);
		
		// find x and y for position
		float y = (prefabRect.rect.height + y_Offset) * lastItemPos ;
		
		// set position
		Vector3 currentPos = new Vector3(1f, -60-110*(lastItemPos));
		
		
		// add content size to fit more items
		
		contentRect.sizeDelta = new Vector2(
			1f, // width doesnt change
			120 + (prefabRect.rect.height + y_Offset) * lastItemPos);
		
		// instantiate Item
		GameObject tempItem = Instantiate(prefabItem.gameObject,
		                                  currentPos,
		                                  prefabItem.transform.rotation) as GameObject;
		
		// next position on inventory grid
		lastItemPos++;
		
		// set new item parent to scroll rect content
		//tempItem.GetComponent<Image>().sprite = item.icon;
		tempItem.transform.SetParent(contentRect.transform, false);
		tempItem.name = item.name + "_" +1;
		tempItem.GetComponent<InventoryItem> ().itemBeingHeld = item;
		tempItem.GetComponent<InventoryItem> ().nameText.text = item.name;
		//tempItem.GetComponent<ItemStackableBehavior>().setObject(item);
	}
	//-----------------END OF LeMigue para Glassware-----------------------

	//-----------------LeMigue para Reagents-----------------------
	public void addNewReagentsUI(Compound item, Compound data)
	{  
		// if there is freedPosition, use it. If NOT, use the last item position.
		//prefabItem.GetComponent<AnyObjectInstantiation>().copyItemBase(item);
		
		Debug.Log("Adding " + item.name 
		          + " to " + 1
		          + " at " + transform.parent.transform.parent.transform.parent.name);
		
		// find x and y for position
		float y = (prefabRect.rect.height + y_Offset) * lastItemPos ;
		
		// set position
		Vector3 currentPos = new Vector3(1f, -60-110*(lastItemPos));
		
		
		// add content size to fit more items
		
		contentRect.sizeDelta = new Vector2(
			1f, // width doesnt change
			120 + (prefabRect.rect.height + y_Offset) * lastItemPos);
		
		// instantiate Item
		GameObject tempItem = Instantiate(prefabItem.gameObject,
		                                  currentPos,
		                                  prefabItem.transform.rotation) as GameObject;
		
		// next position on inventory grid
		lastItemPos++;
		
		// set new item parent to scroll rect content
		//tempItem.GetComponent<Image>().sprite = item.icon;
		tempItem.transform.SetParent(contentRect.transform, false);
		tempItem.name = data.name + "_" +1;
		tempItem.GetComponent<InventoryItem> ().nameText.text = data.name;
		if (data.isSolid)
			tempItem.AddComponent<Compound> ();
		else
			tempItem.AddComponent<Compound> ();
		tempItem.GetComponent<Compound> ().setValues (data);
		tempItem.GetComponent<InventoryItem> ().HoldItem(item);
		//tempItem.GetComponent<ItemStackableBehavior>().setObject(item);
	}
	//-----------------END OF LeMigue para Reagents-----------------------*/
	
	//! Remove Item from Inventory UI
	/*! Using the selected itemUI as reference, delete it from the UI and add it's position into the freedPos queue.*/
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

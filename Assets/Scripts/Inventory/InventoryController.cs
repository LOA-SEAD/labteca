using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

//! This is the controller for the Inventory.
/*!
 *  It contains a list of all objects that are associated with it, composing the inventory. 
 */
public class InventoryController : MonoBehaviour {

    public List<AnyObjectInstantiation> itens = new List<AnyObjectInstantiation>(); /*!< List of itens of type AnyObjectInstantiation. */
    public GameObject confirmationBox;  /*!< Prefab of confirmation box. */

    private AnyObjectInstantiation tempItem;
    private ItemStackableBehavior tempItemUI;
	private bool requestAddItem;
    private bool requestRemoveItem;
    private InventoryContent content;

	// Get InventoryContent in child.
	void Start () 
    {
        content = GetComponentInChildren<InventoryContent>();
        if (content == null)
            Debug.LogError("Content not found in " + this.transform.parent.transform.parent.name);
    }
	
    // TODO: Verificar a real necessidade de colocar as funcoes dentro Update sendo chamadas atraves de variaveis booleanas.
	void Update () 
    {
        // Check if an Item needs to be added or removed. 
		if(requestAddItem)
        {
			requestAddItem = false;
//            content.addNewItemUI(tempItem);
            tempItem = null;
		}
        if(requestRemoveItem)
        {
            requestRemoveItem = false;
            content.removeItemUI(tempItemUI);
            tempItem = null;
        }
	}

    //! Add Item to inventory.
    /*! Receives and item and defines this inventory to it, also add this item to the interface using InventoryContent 
     * and disable the original object from the scene. */
    public void AddItem(AnyObjectInstantiation item)
    {
        item.SetInventory(this);
        tempItem = item;
        itens.Add(tempItem);
        requestAddItem = true;
        // item on scene will disappear
        item.gameObject.SetActive(false);
    }

    //! Remove Item from inventory.
    /*! Receives the item from the interface, remove it from the interface, from the List of itens 
     * and enables the original GameObject again.*/
    public void RemoveItem(ItemStackableBehavior itemUI)
    {        
        tempItemUI = itemUI;
        itens.Remove(itemUI.getObject());
        requestRemoveItem = true;
        itemUI.getObject().gameObject.SetActive(enabled);
    }

    //! Destroy Item from the game.
    /*! Receives and item from the UI, removes it from the list and then destroy it from the scene. */
    public void DestroyItem(ItemStackableBehavior itemUI)
    {
        AnyObjectInstantiation obj = itemUI.getObject();
        RemoveItem(itemUI);
        Destroy(obj.gameObject);
    }

    //TODO: Remover codigo abaixo fazendo as alteracoes para o script do inventario acima.

    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    //  Methods from Old Script!  ------- start here --------
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    public Dictionary<ReagentsSolid, GameObject> reagentsSolid = new Dictionary<ReagentsSolid, GameObject>();
    public Dictionary<ReagentsLiquid, GameObject> reagentsLiquid = new Dictionary<ReagentsLiquid, GameObject>();
    public Dictionary<Glassware, GameObject> glassware = new Dictionary<Glassware, GameObject>();
    public Dictionary<Chart, GameObject> chart = new Dictionary<Chart, GameObject>();
    private bool _showingChart = false;
    public GameObject[] slotPositions;
    public GameObject selectedItem;

    public void UpdateInventory()
    {
        int counter = 0;
        foreach (GameObject item in glassware.Values)
        {
            item.transform.position = slotPositions[counter].transform.position;
            counter++;
        }

        foreach (GameObject item in reagentsSolid.Values)
        {
            item.transform.position = slotPositions[counter].transform.position;
            counter++;
        }

        foreach (GameObject item in reagentsLiquid.Values)
        {
            item.transform.position = slotPositions[counter].transform.position;
            counter++;
        }

        foreach (GameObject item in chart.Values)
        {
            item.transform.position = slotPositions[counter].transform.position;
            counter++;
        }
    }
    public bool showingChart
    {
        get
        {
            return _showingChart;
        }
        set
        {
            _showingChart = value;
        }
    }

    public void AddReagentLiquid(ReagentsLiquid reagent)
    {
        Debug.Log("added " + reagent.info.Name + " in inventory!");
        reagent.gameObject.layer = 10;
        foreach (Transform child in reagent.transform)
        {
            child.gameObject.layer = 10;
        }
        reagentsLiquid.Add(reagent, reagent.gameObject);
        UpdateInventory();
    }

    public void AddReagentSolid(ReagentsSolid reagent)
    {
        Debug.Log("added " + reagent.name + " in inventory!");

        reagent.gameObject.layer = 10;
        foreach (Transform child in reagent.transform)
        {
            child.gameObject.layer = 10;
        }
        reagentsSolid.Add(reagent, reagent.gameObject);
        UpdateInventory();
    }

    public void AddGlassware(Glassware glass)
    {
        Debug.Log("added glass in inventory!");

        glass.gameObject.layer = 10;
        foreach (Transform child in glass.transform)
        {
            child.gameObject.layer = 10;
        }
        glassware.Add(glass, glass.gameObject);
        UpdateInventory();
    }

    public void AddChart(Chart graph)
    {
        graph.gameObject.layer = 10;
        foreach (Transform child in graph.transform)
        {
            child.gameObject.layer = 10;
        }
        chart.Add(graph, graph.gameObject);
        UpdateInventory();
    }

    public void RemoveReagentLiquid(ReagentsLiquid reagent)
    {
        if (reagentsLiquid.ContainsKey(reagent))
        {
            reagent.gameObject.layer = 0;
            foreach (Transform child in reagent.transform)
            {
                child.gameObject.layer = 0;
            }
            reagentsLiquid.Remove(reagent);
            UpdateInventory();
        }
    }

    public void RemoveReagentSolid(ReagentsSolid reagent)
    {
        if (reagentsSolid.ContainsKey(reagent))
        {
            reagent.gameObject.layer = 0;
            foreach (Transform child in reagent.transform)
            {
                child.gameObject.layer = 0;
            }
            reagentsSolid.Remove(reagent);
            UpdateInventory();
        }
    }

    public void RemoveGlassware(Glassware glass)
    {
        if (glassware.ContainsKey(glass))
        {
            glass.gameObject.layer = 0;
            foreach (Transform child in glass.transform)
            {
                child.gameObject.layer = 0;
            }
            glassware.Remove(glass);
            UpdateInventory();
        }
    }

    public void RemoveChart(Chart graph)
    {
        if (chart.ContainsKey(graph))
        {
            graph.gameObject.layer = 0;
            foreach (Transform child in graph.transform)
            {
                child.gameObject.layer = 0;
            }
            chart.Remove(graph);
            UpdateInventory();
        }
    }
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    //  Methods from Old Script!  ------- end here --------
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    

}

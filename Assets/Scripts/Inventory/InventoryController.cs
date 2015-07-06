using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class InventoryController : MonoBehaviour {

    public List<AnyObjectInstantiation> itens = new List<AnyObjectInstantiation>();
    public GameObject confirmationBox;

    private AnyObjectInstantiation tempItem;
    private ItemStackableBehavior tempItemUI;
	private bool requestAddItem;
    private bool requestRemoveItem;

    private InventoryContent content;

	// Use this for initialization
	void Start () 
    {
        content = GetComponentInChildren<InventoryContent>();
        if (content == null)
            Debug.LogError("Content not found in " + this.transform.parent.transform.parent.name);
    }
	
	// Update is called once per frame
	void Update () 
    {
		if(requestAddItem)
        {
			requestAddItem = false;
            content.addNewItemUI(tempItem);
            tempItem = null;
		}
        if(requestRemoveItem)
        {
            requestRemoveItem = false;
            content.removeItemUI(tempItemUI);
            tempItem = null;
        }
	}

    public void AddItem(AnyObjectInstantiation item)
    {
        item.SetInventory(this);
        tempItem = item;
        itens.Add(tempItem);
        requestAddItem = true;
        // item on scene will disappear
        item.gameObject.SetActive(false);
    }

    public void RemoveItem(ItemStackableBehavior itemUI)
    {        
        tempItemUI = itemUI;
        itens.Remove(itemUI.getObject());
        requestRemoveItem = true;
        itemUI.getObject().gameObject.SetActive(enabled);
    }

    public void DestroyItem(ItemStackableBehavior itemUI)
    {
        AnyObjectInstantiation obj = itemUI.getObject();
        RemoveItem(itemUI);
        Destroy(obj.gameObject);
    }

    //TODO: remove hard code!
    // methods from old script

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
        Debug.Log("added " + reagent.info.name + " in inventory!");
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


}

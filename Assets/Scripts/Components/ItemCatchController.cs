using UnityEngine;
using System.Collections;

public class ItemCatchController : MonoBehaviour 
{
	public enum ItemType
	{
		GLASSWARE,
		REAGENT_SOLID,
		REAGENT_LIQUID
	}

	public ItemType itensType;

	public Transform[] placeHolders;
	public GameObject[] objectPrefabs;
	public GameObject buttonPrefab;
	public string[] itensText;

	private InventoryController inventory;

	// Use this for initialization
	void Start () 
	{
		inventory = FindObjectOfType (typeof(InventoryController)) as InventoryController;

		for (int i = 0; i < placeHolders.Length; i++) 
		{
			GameObject obj = Instantiate(buttonPrefab, placeHolders[i].position, placeHolders[i].rotation) as GameObject;
			obj.GetComponentInChildren<TextMesh>().text = itensText[i];
			obj.GetComponent<SendMessegeMouseButonWithParameter>().parameter = i;
			obj.GetComponent<SendMessegeMouseButonWithParameter>().target = this.gameObject;
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void ButtonPressed(int index)
	{
		switch (itensType) 
		{
			case ItemType.GLASSWARE:
			{
				GameObject item = Instantiate(objectPrefabs[index]) as GameObject;
				inventory.AddGlassware(item.GetComponent<Glassware>());
				item.transform.parent = inventory.transform;
			}
			break;

			case ItemType.REAGENT_LIQUID:
			{
				GameObject item = Instantiate(objectPrefabs[index]) as GameObject;
				inventory.AddReagentLiquid(item.GetComponent<ReagentsLiquid>());
				item.transform.parent = inventory.transform;
			}
			break;

			case ItemType.REAGENT_SOLID:
			{
				GameObject item = Instantiate(objectPrefabs[index]) as GameObject;
				inventory.AddReagentSolid(item.GetComponent<ReagentsSolid>());
				item.transform.parent = inventory.transform;
			}		
			break;
		}
	}
}

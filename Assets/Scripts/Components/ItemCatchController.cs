using UnityEngine;
using System.Collections;

//! Controls the selected itens (glassware and reagents).
/*! */

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
	//!
	/*! */
	//TODO: Nao entendi o porque do Start()
	void Start () 
	{
		inventory = FindObjectOfType (typeof(InventoryController)) as InventoryController;
		//Entendi que esta fazendo atribuiçoes para o obj, mas nao sei exatamente como funciona.
		for (int i = 0; i < placeHolders.Length; i++) 
		{
			GameObject obj = Instantiate(buttonPrefab, placeHolders[i].position, placeHolders[i].rotation) as GameObject;
			obj.GetComponentInChildren<TextMesh>().text = itensText[i];
			obj.GetComponent<SendMessageMouseButtonWithParameter>().parameter = i;
			obj.GetComponent<SendMessageMouseButtonWithParameter>().target = this.gameObject;
		}
	}
	
	// Update is called once per frame
	/*void Update () {
	
	}*/

	//! Add selected itensType in inventory (glassware, reagent liquid and reagent solid).
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

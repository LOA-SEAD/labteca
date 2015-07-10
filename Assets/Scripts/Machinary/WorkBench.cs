using UnityEngine;
using System.Collections;

//! Puts the reagents in slots and mix the reagents.
/*! */
//TODO: Testar para saber como funciona.
public class WorkBench : MonoBehaviour {

	public GameObject slot0;
	public GameObject slot1;

	private string slot0Type;
	private string slot1Type;

	public GameObject slot0Position;
	public GameObject slot1Position;
	public GameObject finalSlotPosition;

	public GameObject buttonMix;

	public GameObject resultPrefab;
	public Vector3 resultPrefabScale;


	public bool resultMake = false;
	
	public Vector3 inventoryScale;
	
	public Vector3 inventoryColliderSize;
	public Vector3 inventoryColliderPosition;


	private InventoryController inventory;
	// Use this for initialization
	//! Deactivates buttonMix and returns object of type inventory.
	/*! */
	void Start () 
	{
		buttonMix.SetActive (false);
		inventory = FindObjectOfType (typeof(InventoryController)) as InventoryController;
	}
	
	// Update is called once per frame
	//! Activates/Deactivates buttonMix
	void Update () 
	{
		if(slot0 != null && slot1 != null)
		{
			buttonMix.SetActive(true);
		}
		else
		{
			buttonMix.SetActive(false);
		}
	}

	//! Puts the reagents in slots.
	//*! */
	public void useSlot(ReagentsLiquid liquid, ReagentsSolid solid)
	{
		if(slot0 == null)
		{
			if(liquid != null)
			{
				slot0 = liquid.gameObject;
				slot0.transform.position = slot0Position.transform.position;
				slot0Type = "liquid";
			}
			if(solid != null)
			{
				slot0 = solid.gameObject;
				slot0.transform.position = slot0Position.transform.position;
				slot0Type = "solid";
			}

			slot0.transform.localScale = resultPrefabScale;
		}
		else if(slot1 == null)
		{
			if(liquid != null)
			{
				slot1 = liquid.gameObject;
				slot1.transform.position = slot0Position.transform.position;
				slot1Type = "liquid";
			}
			if(solid != null)
			{
				slot1 = solid.gameObject;
				slot1.transform.position = slot0Position.transform.position;
				slot1Type = "solid";
			}

			slot1.transform.localScale = resultPrefabScale;
		}
		else
		{
			Debug.LogWarning("all slots used");
		}
	}
	//!Mix the reagents.
	/*! */
	public void Mix()
	{
		if((slot0Type == "liquid" && slot1Type == "solid" )||(slot1Type == "liquid" && slot0Type == "solid" ))
		{
			ReagentsLiquid result;
			GameObject resultGo = Instantiate(resultPrefab) as GameObject;
			result = resultGo.GetComponent<ReagentsLiquid>();
			//result.transform.position = finalSlotPosition.transform.position;
			result.transform.localScale = resultPrefabScale;


			if(slot0Type == "liquid")
			{
				result.volume = slot0.GetComponent<ReagentsLiquid>().volume;
			}
			else
			{
				result.volume = slot1.GetComponent<ReagentsLiquid>().volume;
			}



			float mass;

			if(slot0Type == "solid")
			{
				result.GetInfo(slot0.GetComponent<ReagentsSolid>().liquidName);
				mass = slot0.GetComponent<ReagentsSolid>().totalMass;
				Debug.Log(slot0.GetComponent<ReagentsSolid>().liquidName);
			}
			else
			{
				result.GetInfo(slot1.GetComponent<ReagentsSolid>().liquidName);
				mass = slot1.GetComponent<ReagentsSolid>().totalMass;
			}

			result.concentration = (mass/result.info.molarMass);//*result.volume;

			resultMake = true;


			Destroy(slot0);
			Destroy(slot1);

			result.transform.parent = inventory.transform;
			result.transform.localScale = inventoryScale;
			BoxCollider col = result.collider as BoxCollider;
			col.size = inventoryColliderSize;
			col.center = inventoryColliderPosition;

			inventory.AddReagentLiquid(result);
		}
	}
}

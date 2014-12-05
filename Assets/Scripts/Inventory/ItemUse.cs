using UnityEngine;
using System.Collections;

public class ItemUse : MonoBehaviour 
{
	public KeyCode keyToUse;
	public string objectName;

	private bool allowChangeScene = false;

	private InventoryController inventory;

	private ReagentsLiquid reagent;
	private ReagentsSolid reagentSolid;

	public bool isSolid;

	public string solidName;

	private Vector3 originalScale;
	public Vector3 inventoryScale;

	private Vector3 originalColliderSize;
	private Vector3 originalColliderPosition;

	public Vector3 inventoryColliderSize;
	public Vector3 inventoryColliderPosition;

	// Use this for initialization
	void Start () 
	{
		inventory = FindObjectOfType (typeof(InventoryController)) as InventoryController;

		if(!isSolid)
		{
			reagent = this.GetComponent<ReagentsLiquid> ();
			reagent.GetInfo(objectName);
		}
		else
		{
			reagentSolid = this.GetComponent<ReagentsSolid>();
			reagentSolid.name = solidName;
			reagentSolid.liquidName = objectName;
		}

		originalScale = this.transform.localScale;
		BoxCollider col = this.collider as BoxCollider;
		originalColliderPosition = col.center;
		originalColliderSize = col.size;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnTriggerEnter(Collider other)
	{
		if (other.tag == "Player") 
		{
			allowChangeScene = true;
			if(isSolid)
			{
				HudText.SetText("Aperte " + keyToUse.ToString() + " para pegar " + solidName + ".");
			}
			else
			{
				HudText.SetText("Aperte " + keyToUse.ToString() + " para pegar " + objectName + ".");
			}
		}
	}

	void OnTriggerStay(Collider other)
	{
		if (other.tag == "Player") 
		{
			if(Input.GetKeyDown(keyToUse) && allowChangeScene)
			{
				HudText.EraseText();

				this.transform.localScale = inventoryScale;

				this.collider.isTrigger = false;
				BoxCollider col = this.collider as BoxCollider;
				col.size = inventoryColliderSize;
				col.center = inventoryColliderPosition;


				this.transform.parent = inventory.gameObject.transform;
				DontDestroyOnLoad(this.gameObject);

				if(!isSolid)
				{
					inventory.AddReagentLiquid(reagent);
					reagent.inInventory = true;
				}
				else
				{
					inventory.AddReagentSolid(reagentSolid);
				}
			}
		}
	}

	void OnTriggerExit(Collider other)
	{
		if (other.tag == "Player") 
		{
			allowChangeScene = false;
			HudText.EraseText();
		}
	}
}

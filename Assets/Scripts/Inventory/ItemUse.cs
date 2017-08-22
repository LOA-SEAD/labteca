using UnityEngine;
using System.Collections;

//! Allow any Game Object to be set as Item.
/*!
 *  This makes any object inside the scene to become an Item that will be used with the inventory and 
 *  the machinary / other items / so the experiments can be done.
 */
public class ItemUse : MonoBehaviour 
{
	public KeyCode keyToUse;
	public string objectName;

	private bool allowChangeScene = false;

    // TODO: Ha alteracoes necessarias para utilizar novo sistema de inventario - esse codigo eh para o sistema "antigo"
	private InventoryController inventory;  // utilizar inventory manager

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

	void Start () 
	{
        // TODO: alterar para utilizar o inventory manager
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

        // TODO: verificar se isso realmente eh necessario, me parece redundante -> hardcode
		originalScale = this.transform.localScale;
		BoxCollider col = this.GetComponent<Collider>() as BoxCollider;
		originalColliderPosition = col.center;
		originalColliderSize = col.size;
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

				this.GetComponent<Collider>().isTrigger = false;
				BoxCollider col = this.GetComponent<Collider>() as BoxCollider;
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

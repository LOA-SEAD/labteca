using UnityEngine;
using System.Collections;

public class Glassware : MonoBehaviour 
{
	public float volume;
	public float uncalibrateVolume;
	public float mass;

	public float alphaValueWhenDisable;
	public float alphaValueWhenEnable;
	public bool calibrate;

	private InventoryController inventory;
	private Vector3 originalPosition;

	public Vector3 balancePosition;


	private float buttonTimeout;
	void Awake()
	{
		inventory = FindObjectOfType (typeof(InventoryController)) as InventoryController;
		originalPosition = this.transform.position;
	}

	// Use this for initialization
	void Start () 
	{
		this.rigidbody.mass = mass;
	}
	
	// Update is called once per frame
	void Update () 
	{
	
	}

	public void Enable()
	{
		this.collider.enabled = true;
		foreach (Transform child in this.transform) 
		{
			if(child.renderer != null)
			{
				child.renderer.material.color = new Color(child.renderer.material.color.r, child.renderer.material.color.g, child.renderer.material.color.b, alphaValueWhenEnable);
			}
		}
	}
	
	public void Disable()
	{
		this.collider.enabled = false;
		foreach (Transform child in this.transform) 
		{
			if(child.renderer != null)
			{
				child.renderer.material.color = new Color(child.renderer.material.color.r, child.renderer.material.color.g, child.renderer.material.color.b, alphaValueWhenDisable);
			}
		}
	}

	public void MsgMouseDown()
	{
		if (Time.time - buttonTimeout < 0.1f) 
		{
			return;
		}

		if (inventory.selectedItem != this.gameObject) 
		{
			inventory.selectedItem = this.gameObject;
			if(Application.loadedLevelName == "Balance")
			{
				this.transform.parent = null;
				this.transform.position = balancePosition;
				inventory.glassware.Remove (this);
				inventory.DisactiveAllGlassware ();
				inventory.ActiveAllReagentsSolid ();
			}
			else
			{
				this.transform.position = originalPosition;
				inventory.glassware.Remove (this);
				inventory.DisactiveAllGlassware ();
				inventory.ActiveAllReagentsLiquid ();
				inventory.ActiveAllReagentsSolid ();
			}
		} 
		else 
		{
			inventory.selectedItem = null;
			inventory.glassware.Add (this, this.gameObject);
			inventory.ActiveAllGlassware ();
			inventory.UpdateInventory ();
		}

		buttonTimeout = Time.time;
	}
}

using UnityEngine;
using System.Collections;

//! Calculates the chart data. 
/*!
 * Contains six methods that "calculates" the chart data, enable/desable the object
 * and a message button.
 * 
 */

// TODO: Testar para ver como funciona.

public class Chart : MonoBehaviour 
{
	public GameObject infoChart; /*!< New gameObject. */
	public float alphaValueWhenDisable;
	public Vector3 positionOnSelected;
	public Vector3 scaleOnSelected;


	private bool selected;
	private Vector3 originalScale;
	private Vector3 originalPosition;

	private bool growing;
	private bool notGrowing;

	public float timeToGrow;
	private float timeToGrowAcc;

	private InventoryController inventory; /*!< InventoryController object. */

	// Use this for initialization
	//! Returns the first loaded object of InventoryController with original scale and position.
	/*! */
	void Start () 
	{
		inventory = FindObjectOfType (typeof(InventoryController)) as InventoryController;

		originalScale = this.transform.localScale;
		originalPosition = this.transform.localPosition;
	}
	
	// Update is called once per frame
	//! Linearly interpolates between two vectors (Growing and notGrowing).
	/*! This is most commonly used to find a point some fraction of the way along a line between two endpoints (timeToGrow and timeToGrowAcc). */
	void Update () 
	{
		if (growing) 
		{
			this.transform.localPosition = Vector3.Lerp(originalPosition, positionOnSelected, (Time.time - timeToGrowAcc)/timeToGrow);
			this.transform.localScale = Vector3.Lerp(originalScale, scaleOnSelected, (Time.time - timeToGrowAcc)/timeToGrow);
			if((Time.time - timeToGrowAcc)/timeToGrow > 1f)
			{
				growing = false;
			}
		}

		if (notGrowing) 
		{
			this.transform.localPosition = Vector3.Lerp(positionOnSelected, originalPosition, (Time.time - timeToGrowAcc)/timeToGrow);
			this.transform.localScale = Vector3.Lerp(scaleOnSelected, originalScale, (Time.time - timeToGrowAcc)/timeToGrow);
			if((Time.time - timeToGrowAcc)/timeToGrow > 1f)
			{
				notGrowing = false;
			}
		}
	}

	//! Set the chart
	/*! */
	public void SetChart(Texture2D chart)
	{
		infoChart.renderer.material.mainTexture = chart;
	}

	//! Enable infoChart 
	/*! */
	public void Enable()
	{
		this.collider.enabled = true;
		this.renderer.material.color = new Color(this.renderer.material.color.r, this.renderer.material.color.g, this.renderer.material.color.b, 1f);
		infoChart.renderer.material.color = this.renderer.material.color;
	}

	//! Disable infoChart 
	/*! */
	public void Disable()
	{
		this.collider.enabled = false;
		this.renderer.material.color = new Color(this.renderer.material.color.r, this.renderer.material.color.g, this.renderer.material.color.b, alphaValueWhenDisable);
		infoChart.renderer.material.color = this.renderer.material.color;
	}

	//!  
	/*! */
	public void msgButtonDown()
	{
		if(!inventory.showingChart)
		{
			if (!selected) 
			{
				inventory.showingChart = true;
				selected = true;
				growing = true;
				notGrowing = false;
				timeToGrowAcc = Time.time;
			} 
		}
		else
		{
			if (selected)
			{
				inventory.showingChart = false;
				selected = false;
				growing = false;
				notGrowing = true;
				timeToGrowAcc = Time.time;
			}
		}
	}
}

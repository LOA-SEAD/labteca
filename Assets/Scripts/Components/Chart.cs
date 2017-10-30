using UnityEngine;
using System.Collections;

//! Calculates the chart data. 
/*!
 * Contains six methods that "calculates" the chart data, enable/desable the object
 * and a message button.
 * 
 */

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
	//! Returns the first loaded object of InventoryController with local scale and position.
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

	//! Set the infoChart
	/*! */
	public void SetChart(Texture2D chart)
	{
		infoChart.GetComponent<Renderer>().material.mainTexture = chart;
	}

	//! Enable infoChart 
	/*! Changes the color */
	public void Enable()
	{
		this.GetComponent<Collider>().enabled = true;
		this.GetComponent<Renderer>().material.color = new Color(this.GetComponent<Renderer>().material.color.r, this.GetComponent<Renderer>().material.color.g, this.GetComponent<Renderer>().material.color.b, 1f);
		infoChart.GetComponent<Renderer>().material.color = this.GetComponent<Renderer>().material.color;
	}

	//! Disable infoChart 
	/*! Changes the color */
	public void Disable()
	{
		this.GetComponent<Collider>().enabled = false;
		this.GetComponent<Renderer>().material.color = new Color(this.GetComponent<Renderer>().material.color.r, this.GetComponent<Renderer>().material.color.g, this.GetComponent<Renderer>().material.color.b, alphaValueWhenDisable);
		infoChart.GetComponent<Renderer>().material.color = this.GetComponent<Renderer>().material.color;
	}

	//! Sets the variables when is showing/not showing the chart.
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

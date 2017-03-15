using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

//! Scale Controller
//  This is the controller for the Scale, it has all the methods that allows the Scale to work with 
//  the Glassware, calculating its mass - together with its content - and displaying the value.

public class ScaleController : EquipmentControllerBase 
{
    public float valueToShow;  		// Mass to be shown on scale. 
	private float taredValue;		// Tared value

	public float previousMass;		// Value to control the flicking effect
	private bool changed;			// Used to control the flicking effect on display.

	public TextMesh scaleText;    // Text display of Scale.

	public float timeConstant;		// Maximum time of flicking effect
	private float timeElapsed;

	public GameObject activeMass;   // List of GameObjects that composes the mass.

    public WorkBench workbench;		// BalanceState component.

    // Awake happens before all Start()
	void Awake()
	{
	}

	void Start () {
		taredValue = 0.00f;
		previousMass = 0;
		timeElapsed = 0;
	}

	void Update () {
		RefreshEquipment ();

		if (changed) {
			scaleText.text = EquipmentValueToString(UnityEngine.Random.Range(-1f+timeElapsed/timeConstant,1f-timeElapsed/timeConstant) + valueToShow);
			timeElapsed += Time.fixedDeltaTime;
			if(timeConstant<timeElapsed){
				timeElapsed = 0;
				changed = false;
				scaleText.text = EquipmentValueToString(valueToShow);
			}
		} 
	}

	/// <summary>
	/// Converts the float value to formated text for the equipment.
	/// </summary>
	/// <returns>Formated string.</returns>
	/// <param name="value">Float value to be converted.</param>
	private string EquipmentValueToString(float value){
		string txt;
		txt = String.Format("{0:F2}", value);

		return txt;
	}

	/// <summary>
	/// Set up the taring value of the scale.
	/// </summary>
	public void SetTaredValue() {
		if (activeMass != null) {
			taredValue = activeMass.GetComponent<Glassware> ().GetMass ();
		} else {
			taredValue = 0.00f;
		}
		workbench.GetComponentInChildren<StateUIManager> ().CloseAll ();
	}

    /// <summary>
    /// Resets the scale, setting the tared value to 0.0.
    /// </summary>
	public void ResetScale()
	{
		taredValue = 0.00f;
		RefreshEquipment ();
		workbench.GetComponentInChildren<StateUIManager> ().CloseAll ();
	}

    //! Get Glassware that is on Scale.
	public Glassware GetGlassInEquipament(){
		Glassware glassToReturn = null;

		if(activeMass.GetComponent<Glassware>() != null){
			glassToReturn = activeMass.GetComponent<Glassware>();
		}
		return glassToReturn;
	}

    //! Add a GameObject to be measured on Scale.
	public override void AddObjectInEquipament(GameObject objectToAdd){
		activeMass = objectToAdd;
		RefreshEquipment();
	}

    //! Remove a GameObject from being measured on Scale.
	public override void RemoveObjectInEquipament(GameObject objectToRemove){
		activeMass = null;
		RefreshEquipment();
	}

    //! Update Real Mass to the mass of all GameObjects on ActiveMass.
	private void RefreshEquipment(){
		if (workbench.IsRunning ()) {
			valueToShow = 0.00f;
			if (activeMass != null) {
				valueToShow = activeMass.GetComponent<Glassware> ().GetMass ();		
			}
			
			valueToShow -= taredValue;

			if(previousMass!=valueToShow){
				previousMass = valueToShow;
				changed = true;
			}
		}
	}

}

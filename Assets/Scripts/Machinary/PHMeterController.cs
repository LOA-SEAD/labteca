using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

//! Phmeter Controller
/*! This is the controller for the PHmeter, it has all the methods that allows the Phmeter to work with 
 * the Glassware and Reagents, calculating their PH displaying it.
 */
public class PHMeterController : EquipmentControllerBase {

	public float ph;

	public float valueToShow;			// Mass to be shown on scale. 	

	public float previousValue;			// Value to controle the flicking effect
	bool changed;						// Used to control the flicking effect on display.
	bool equipmentOn;
	bool measure;

	public TextMesh phmeterText;		// Text display of Scale.

	public float timeConstant;			// Maximum time of flicking effect
	private float timeElapsed;

	public GameObject activeGlassware;	// List of GameObjects that composes the mass.
	
	public WorkBench workbench;			// BalanceState component.


	void Awake()
	{
		// PlayerPrefs.SetFloat ("setupBalance", 0);
	}

	void Start () 
	{
		ph = 0;
		timeElapsed = 0;
	}


	void Update () 
	{
	
		RefreshEquipament ();

		if (changed) {
			phmeterText.text = PhmeterTextToString(UnityEngine.Random.Range(-1f,1f) + ph);
			timeElapsed += Time.fixedDeltaTime;
			if(timeConstant<timeElapsed){
				timeElapsed = 0;
				changed = false;
				phmeterText.text = PhmeterTextToString(ph);
			}
		} 

	}

	private string PhmeterTextToString(float value){
		string txt;
		txt = String.Format("{0:F2}", value);
		
		return txt;
	}


	public void ResetPhmeter()
	{
		PlayerPrefs.SetFloat ("setupBalance", 0);
		RefreshEquipament ();

	}


	//! Get Glassware that is on Phmeter.
	public Glassware GetGlassInEquipament(){
		Glassware glassToReturn = null;
		
		if(activeGlassware.GetComponent<Glassware>() != null){
			glassToReturn = activeGlassware.GetComponent<Glassware>();
		}
		return glassToReturn;
	}

	//! Add a GameObject to be measured on the equipment.
	public override void AddObjectInEquipament(GameObject objectToAdd){
		activeGlassware = objectToAdd;
		RefreshEquipament();
	}
	
	//! Remove a GameObject from being measure by the equipment
	public override void RemoveObjectInEquipament(GameObject objectToRemove){
		activeGlassware = null;
		RefreshEquipament();
	}

	//! Update Real Mass to the mass of all GameObjects on ActiveMass.
	private void RefreshEquipament(){
		if (workbench.IsRunning () && activeGlassware!=null && equipmentOn && measure) {
			valueToShow = 0.00f;
			if (activeGlassware != null) {
				valueToShow = activeGlassware.GetComponent<Glassware> ().GetPH ();
			}

			if(previousValue !=  valueToShow){
				previousValue = valueToShow;
				changed = true;
			}
			//measure=false;
		}
	}

	/// <summary>
	/// Triggers the events when the power button pressed.
	/// </summary>
	public void OnClickPower(){
		if (!equipmentOn) {
			phmeterText.text = "--.--";
			equipmentOn = true;
		}
	}

	/// <summary>
	/// Triggers the events when the power button pressed.
	/// </summary>
	public void OnClickMeasure(){
		if (equipmentOn)
			phmeterText.text = PhmeterTextToString (0.00f);
			measure = true;
	}
}
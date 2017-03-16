using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class PolarimeterController : EquipmentControllerBase {
	
	public float valueToShow;			// Value to be shown on scale. 	
	
	public float previousValue;			// Value to control the flicking effect
	bool changed;						// Used to control the flicking effect on display.
	bool equipmentOn;
	bool measure;
	
	public TextMesh equipmentText;		// Text display of Scale.
	
	public float timeConstant;			// Maximum time of flicking effect
	private float timeElapsed;
	
	public GameObject activeGlassware;	// List of GameObjects that composes the mass.
	
	public WorkBench workbench;			// BalanceState component.
	
	void Start () {
		timeElapsed = 0;
		equipmentOn = true;
		measure = true;
	}
	
	void Update () {
		RefreshEquipament ();
		
		if (changed) {
			equipmentText.text = EquipmentTextToString(UnityEngine.Random.Range(-1f+timeElapsed/timeConstant,1f-timeElapsed/timeConstant) + valueToShow);
			timeElapsed += Time.fixedDeltaTime;
			if(timeConstant<timeElapsed){
				timeElapsed = 0;
				changed = false;
				equipmentText.text = EquipmentTextToString(valueToShow);
			}
		}
	}
	
	/// <summary>
	/// Converts the float value to formated text for the equipment.
	/// </summary>
	/// <returns>Formated string.</returns>
	/// <param name="value">Float value to be converted.</param>
	private string EquipmentTextToString(float value){
		string txt;
		txt = String.Format("{0:F2}", value);
		
		return txt;
	}
	
	/// <summary>
	/// Resets the equipment.
	/// </summary>
	public void ResetEquipment() {
		RefreshEquipament ();
	}

	/// <summary>
	/// Gets the glassware currently in the equipment position.
	/// </summary>
	/// <returns>The glassware currently in equipment.</returns>
	public Glassware GetGlassInEquipament(){
		Glassware glassToReturn = null;
		
		if(activeGlassware.GetComponent<Glassware>() != null){
			glassToReturn = activeGlassware.GetComponent<Glassware>();
		}
		return glassToReturn;
	}
	
	/// <summary>
	/// Adds a glassware to be measured by the equipment.
	/// </summary>
	/// <param name="objectToAdd">Object to be added.</param>
	public override void AddObjectInEquipament(GameObject objectToAdd){
		activeGlassware = objectToAdd;
		RefreshEquipament();
	}
	
	/// <summary>
	/// Removes a GameObject from the equipment position.
	/// </summary>
	/// <param name="objectToRemove">Object to be removed.</param>
	public override void RemoveObjectInEquipament(GameObject objectToRemove){
		activeGlassware = null;
		RefreshEquipament();
	}
	
	//! Update Real Mass to the mass of all GameObjects on ActiveMass.
	/// <summary>
	/// Refreshs the equipament, checking if the flicking effect will be triggered.
	/// </summary>
	private void RefreshEquipament(){
		if (workbench.IsRunning () && equipmentOn && measure) {
			if (activeGlassware != null) {
				valueToShow = activeGlassware.GetComponent<Glassware> ().GetPH (); //TODO: Change to correct property
			}
			else{
				valueToShow = 0.0f;
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
			equipmentText.text = "--.--";
			equipmentOn = true;
		}
	}
	
	/// <summary>
	/// Triggers the events when the power button pressed.
	/// </summary>
	public void OnClickMeasure(){
		if (equipmentOn && !measure) {
			equipmentText.text = EquipmentTextToString (0.00f);
			measure = true;
		}
	}
}

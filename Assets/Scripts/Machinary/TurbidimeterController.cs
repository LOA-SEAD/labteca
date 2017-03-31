using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

//! Turbidimeter Controller
/*! This is the controller for the Turbidimeter, it has all the methods that allows the equipment to work with 
 * the Glassware and Reagents, calculating their turbidity and displaying it.
 */
public class TurbidimeterController : EquipmentControllerBase {

	public float valueToShow;			// Value to be shown on display. 	
	
	public float previousValue;			// Value to control the flicking effect
	bool changed;						// Used to control the flicking effect on display.
	bool equipmentOn;
	bool measure;
	
	public TextMesh displayText;		// Text display of equipment.
	
	public float timeConstant;			// Maximum time of flicking effect
	private float timeElapsed;
	
	public GameObject activeGlassware;	// Glassware that is being measured
	public Glassware cubeta;			// Glassware to be used on the equipment


	public WorkBench workbench;			// BalanceState component.

	void Start () {
		timeElapsed = 0;
	}
	
	
	void Update () {
		RefreshEquipament ();
		
		if (changed) {
			displayText.text = EquipmentTextToString(UnityEngine.Random.Range(-1f+timeElapsed/timeConstant,1f-timeElapsed/timeConstant) + valueToShow);
			timeElapsed += Time.fixedDeltaTime;
			if(timeConstant<timeElapsed){
				timeElapsed = 0;
				changed = false;
				displayText.text = EquipmentTextToString(valueToShow);
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
				valueToShow = activeGlassware.GetComponent<Glassware> ().GetPH ();
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
	/// Correctly prepares the glassware to be analyzed, already putting it on the equipment position.
	/// </summary>
	/// <param name="glass">The glassware to be prepared.</param>
	public void PrepareGlassware(Glassware glass) {
		Compound sample = (glass.content as Compound).Clone (50) as Compound;
		glass.RemoveLiquid (50);

		cubeta.IncomingReagent (sample, 50);
	}
}

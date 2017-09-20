using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class FlameSpectrophotometer : EquipmentControllerBase {

	public float valueToShow;			// Value to be shown on scale. 	
	
	public float previousValue;			// Value to controle the flicking effect
	bool changed;						// Used to control the flicking effect on display.
	bool equipmentOn;
	bool measure;
	
	public TextMesh displayValue;		// Text to display the value being read.
	public TextMesh waveLengthDisplay;	// Text to display the wave length being read.
	
	public float timeConstant;			// Maximum time of flicking effect
	private float timeElapsed;
	
	public GameObject activeGlassware;	// List of GameObjects that composes the mass.
	
	public WorkBench workbench;			// Workbench component

	public enum WaveLengthTypes {		// Wave lengths measured by the photometer
		Na = 0,
		K = 1,
		Li = 2
	};
	public WaveLengthTypes waveMode;	// Actual wave length being measured
	
	void Start () {
		timeElapsed = 0;
		waveMode = WaveLengthTypes.Na;
	}
	
	void Update () {
		RefreshEquipament ();
		
		if (changed) {
			displayValue.text = EquipmentTextToString(UnityEngine.Random.Range(-1f+timeElapsed/timeConstant,1f-timeElapsed/timeConstant) + valueToShow);
			timeElapsed += Time.fixedDeltaTime;
			if(timeConstant<timeElapsed){
				timeElapsed = 0;
				changed = false;
				displayValue.text = EquipmentTextToString(valueToShow);
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
				switch (waveMode) { //TODO: Change for correct values
				case WaveLengthTypes.Na:
					valueToShow = activeGlassware.GetComponent<Glassware> ().GetMolarity();
					break;
				case WaveLengthTypes.K:
					valueToShow = 0.0f;
					break;
				case WaveLengthTypes.Li:
					valueToShow = 0.0f;
					break;
				}
			}
			else{
				valueToShow = 0.0f;
			}
			
			if(previousValue !=  valueToShow){
				previousValue = valueToShow;
				changed = true;
			}
		}
	}
	
	/// <summary>
	/// Triggers the events when the power button pressed.
	/// </summary>
	public void OnClickPower(){
		if (!equipmentOn) {
			displayValue.text = "--.--";
			equipmentOn = true;
		}
	}
	
	/// <summary>
	/// Triggers the events when the power button pressed.
	/// </summary>
	public void OnClickMeasure(){
		if (equipmentOn) {
			if(!measure) {
				displayValue.text = EquipmentTextToString (0.00f);
				waveLengthDisplay.text = "Na";
				measure = true;
			}
			else {
				NextWaveLength();
			}
		}
	}

	/// <summary>
	/// Change for the next the wave length value.
	/// </summary>
	public void NextWaveLength() {
		if (waveMode == WaveLengthTypes.Li) {
			waveMode = WaveLengthTypes.Na;
		} else {
			waveMode++;
		}

		switch (waveMode) {
		case WaveLengthTypes.Na:
			waveLengthDisplay.text = "Na";
			break;
		case WaveLengthTypes.K:
			waveLengthDisplay.text = "K";
			break;
		case WaveLengthTypes.Li:
			waveLengthDisplay.text = "Li";
			break;
		}
	}
}

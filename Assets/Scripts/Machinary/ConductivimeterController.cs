using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using System;

public class ConductivimeterController : EquipmentControllerBase {

	
	public float conductivimeter;
	
	bool changed,equipmentOn,measure;
	
	public TextMesh conductivimeterText;
	
	public float timeConstant;
	private float timeElapsed;
	
	public GameObject activeGlassware;   
	
	public WorkBench workbench;
	
	
	void Awake()
	{
		// PlayerPrefs.SetFloat ("setupBalance", 0);
	}
	
	void Start () 
	{
		conductivimeter = 0;
		timeElapsed = 0;
	}
	
	
	void Update () 
	{
		RefreshEquipament ();
		
		if (changed) {
			conductivimeterText.text = conductivimeterValueToString(UnityEngine.Random.Range(-1f,1f) + conductivimeter);
			timeElapsed += Time.fixedDeltaTime;
			if(timeConstant<timeElapsed){
				timeElapsed = 0;
				changed = false;
				conductivimeterText.text = conductivimeterValueToString(conductivimeter);
			}
		} 
		
	}

	//! Measures the value, updating the display
	private void RefreshEquipament(){
		if (workbench.IsRunning () && activeGlassware!=null && equipmentOn && measure) {
			
			float tempMass = 0.00f;
			
			if (activeGlassware != null){
				tempMass += activeGlassware.GetComponent<Glassware> ().GetConductivity ();
			}
			conductivimeter = tempMass;
			
			if(conductivimeter!=0){
				changed = true;
			}
		}
	}
	// Turns on the equipment, displaying 0.0 as initial value
	public void onClickRun(){		
		conductivimeterText.text = conductivimeterValueToString (0f);
		equipmentOn = true;
	}
	public void onClickMeasure(){
		if(equipmentOn)
			measure = true;
	}

	//Converts the value into string, formatting it
	private string conductivimeterValueToString(float value){
		string txt;
		txt = String.Format("{0:F2}", value);
		
		return txt;
	}

	public void Resetconductivimeter() {
		RefreshEquipament ();
	}
	
	//! Get Glassware that is on conductivimeter.
	public Glassware GetGlassInEquipament(){
		Glassware glassToReturn = null;
		if(activeGlassware.GetComponent<Glassware>() != null){
			glassToReturn = activeGlassware.GetComponent<Glassware>();
		}
		return glassToReturn;
	}

	public override void AddObjectInEquipament(GameObject objectToAdd){
		activeGlassware = objectToAdd;
		RefreshEquipament();
		
	}
	
	//! Remove a GameObject from being measured on Scale.
	public override void RemoveObjectInEquipament(GameObject objectToRemove){
		activeGlassware = null;
		RefreshEquipament();
	}
}

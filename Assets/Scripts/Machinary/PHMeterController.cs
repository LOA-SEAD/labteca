using UnityEngine;
using System.Collections;
using System.Collections.Generic;

//! Phmeter Controller
/*! This is the controller for the PHmeter, it has all the methods that allows the Phmeter to work with 
 * the Glassware and Reagents, calculating their PH displaying it.
 */

using System;


public class PHMeterController : EquipmentControllerBase {

	public float ph;

	bool changed,equipmentOn,measure;

	public TextMesh phmeterText;

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

	//! Set PlayerPrefs "setupBalance" to realMass.
//
//	public void SetupBalance()
//	{
//		PlayerPrefs.SetFloat ("setupBalance", realMass);
//	}
	
	//! Set PlayerPrefs "setupBalance" to zero.

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



	public override void AddObjectInEquipament(GameObject objectToAdd){

		activeGlassware = objectToAdd;
		 RefreshEquipament();

	}
	
	//! Remove a GameObject from being measured on Scale.
	public override void RemoveObjectInEquipament(GameObject objectToRemove){

		activeGlassware = null;
		RefreshEquipament();
	

	}

	  //! Update Real Mass to the mass of all GameObjects on ActiveMass.

	private void RefreshEquipament(){

		if (workbench.IsRunning ()&& activeGlassware!=null && equipmentOn && measure) {

			float tempMass = 0.00f;

			if (activeGlassware != null)
				tempMass += activeGlassware.GetComponent<Glassware> ().GetPH ();
		

			 ph = tempMass - PlayerPrefs.GetFloat ("setupBalance");

			if(ph!=0){
				changed = true;
			}
			measure=false;
		}
	}


	public void onClickRun(){

		phmeterText.text = PhmeterTextToString (0f);
		equipmentOn = true;
	}

	public void onClickMeasure(){
		if(equipmentOn)
			measure = true;
	}


}

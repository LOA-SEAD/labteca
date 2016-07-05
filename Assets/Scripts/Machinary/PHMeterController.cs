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

	bool changed;

	public TextMesh phmeterText;

	public float timeConstant;
	private float timeElapsed;

	public GameObject activeMass;   
	
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
		
		if(activeMass.GetComponent<Glassware>() != null){
			glassToReturn = activeMass.GetComponent<Glassware>();
		}
		return glassToReturn;
	}



	public override void AddObjectInEquipament(GameObject objectToAdd){

		activeMass = objectToAdd;
		RefreshEquipament();

	}
	
	//! Remove a GameObject from being measured on Scale.
	public override void RemoveObjectInEquipament(GameObject objectToRemove){

		activeMass = null;
		RefreshEquipament();
		phmeterText.text = PhmeterTextToString (0f);

	}

	  //! Update Real Mass to the mass of all GameObjects on ActiveMass.

	private void RefreshEquipament(){

		if (workbench.IsRunning ()) {

			float tempMass = 0.00f;

			if (activeMass != null)
				tempMass += activeMass.GetComponent<Glassware> ().GetMass ();	


			 ph = tempMass - PlayerPrefs.GetFloat ("setupBalance");

			if(ph!=0){
				changed = true;
			}
		}
	}


}

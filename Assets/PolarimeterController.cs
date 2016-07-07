using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using System;

public class PolarimeterController : EquipmentControllerBase {


	public float polarity;
	
	bool changed;
	
	public TextMesh polarimeterText;
	
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
		polarity = 0;
		timeElapsed = 0;
	}
	
	
	void Update () 
	{
		RefreshEquipament ();
		
		if (changed) {
			polarimeterText.text = PolarimeterTextoToString(UnityEngine.Random.Range(-1f,1f) + polarity);
			timeElapsed += Time.fixedDeltaTime;
			if(timeConstant<timeElapsed){
				timeElapsed = 0;
				changed = false;
				polarimeterText.text = PolarimeterTextoToString(polarity);
			}
		} 
		
	}
	
	private string PolarimeterTextoToString(float value){
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
	
	public void ResetPolarimeter()
	{
		PlayerPrefs.SetFloat ("setupBalance", 0);
		RefreshEquipament ();
		
	}
	
	//! Get Glassware that is on polaritymeter.
	
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
		polarimeterText.text = PolarimeterTextoToString (0f);
		
	}
	
	//! Update Real Mass to the mass of all GameObjects on ActiveMass.
	
	private void RefreshEquipament(){
		
		if (workbench.IsRunning ()) {
			
			float tempMass = 0.00f;
			
			if (activeGlassware != null)
				tempMass += activeGlassware.GetComponent<Glassware> ().GetPolarity ();
			else
				polarimeterText.text = PolarimeterTextoToString (0f);
			
			
			polarity = tempMass - PlayerPrefs.GetFloat ("setupBalance");
			
			if(polarity!=0){
				changed = true;
			}
		}
	}
	
	
}

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using System;

public class TurbidimeterController : EquipmentControllerBase {

	public float turbidity;
	
	bool changed;
	
	public TextMesh turbitivityText;
	
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
		turbidity = 0;
		timeElapsed = 0;
	}
	
	
	void Update () 
	{
		RefreshEquipament ();
		
		if (changed) {
			turbitivityText.text = TurbidimeterTextToString(UnityEngine.Random.Range(-1f,1f) + turbidity);
			timeElapsed += Time.fixedDeltaTime;
			if(timeConstant<timeElapsed){
				timeElapsed = 0;
				changed = false;
				turbitivityText.text = TurbidimeterTextToString(turbidity);
			}
		} 
		
	}
	
	private string TurbidimeterTextToString(float value){
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
	
	public void ResetTurbidimeter()
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
		turbitivityText.text = TurbidimeterTextToString (0f);
		
	}
	
	//! Update Real Mass to the mass of all GameObjects on ActiveMass.
	
	private void RefreshEquipament(){
		
		if (workbench.IsRunning ()) {
			
			float tempMass = 0.00f;
			
			if (activeGlassware != null)
				tempMass += activeGlassware.GetComponent<Glassware> ().GetTurbidity ();
			else
				turbitivityText.text = TurbidimeterTextToString (0f);
			
			
			turbidity = tempMass - PlayerPrefs.GetFloat ("setupBalance");
			
			if(turbidity!=0){
				changed = true;
			}
		}
	}
	
	
}

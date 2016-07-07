using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using System;

public class EspectofometerController : EquipmentControllerBase {

	
	public float light;  // nao sei se eh o melhor nome
	
	bool changed;
	
	public TextMesh espectofometerText;
	
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
		light = 0;
		timeElapsed = 0;
	}
	
	
	void Update () 
	{
		RefreshEquipament ();
		
		if (changed) {
			espectofometerText.text = EspectofometerTextoToString(UnityEngine.Random.Range(-1f,1f) + light);
			timeElapsed += Time.fixedDeltaTime;
			if(timeConstant<timeElapsed){
				timeElapsed = 0;
				changed = false;
				espectofometerText.text = EspectofometerTextoToString(light);
			}
		} 
		
	}
	
	private string EspectofometerTextoToString(float value){
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
	
	public void ResetEspectofometer()
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
		espectofometerText.text = EspectofometerTextoToString (0f);
		
	}
	
	//! Update Real Mass to the mass of all GameObjects on ActiveMass.
	
	private void RefreshEquipament(){
		
		if (workbench.IsRunning ()) {
			
			float tempMass = 0.00f;
			
			if (activeGlassware != null)
				tempMass += activeGlassware.GetComponent<Glassware> ().GetPolarity ();
			else
				espectofometerText.text = EspectofometerTextoToString (0f);
			
			
			light = tempMass - PlayerPrefs.GetFloat ("setupBalance");
			
			if(light!=0){
				changed = true;
			}
		}
	}
	
	
}

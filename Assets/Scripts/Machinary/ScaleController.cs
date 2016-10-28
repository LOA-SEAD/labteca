using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

//! Scale Controller
//  This is the controller for the Scale, it has all the methods that allows the Scale to work with 
//  the Glassware, calculating its mass - together with its content - and displaying the value.

public class ScaleController : EquipmentControllerBase 
{
    public float valueToShow;  		// Mass to be shown on scale. 
	private float taredValue;		// Tared value

	public float previousMass;		// Value to control the flicking effect
	private bool changed;			// Used to control the flicking effect on display.

	public TextMesh balanceText;    // Text display of Scale.

	public float timeConstant;		// Maximum time of flicling effect
	private float timeElapsed;

	public GameObject activeMass;   // List of GameObjects that composes the mass.

    public WorkBench workbench;       // BalanceState component.

    // Awake happens before all Start()
	void Awake()
	{
	}

	void Start () {
		taredValue = 0.00f;
		previousMass = 0;
		timeElapsed = 0;
	}

	void Update () {
		RefreshEquipment ();

		if (changed) {
			balanceText.text = BalanceTextToString(UnityEngine.Random.Range(-1f+timeElapsed/timeConstant,1f-timeElapsed/timeConstant) + valueToShow);
			timeElapsed += Time.fixedDeltaTime;
			if(timeConstant<timeElapsed){
				timeElapsed = 0;
				changed = false;
				balanceText.text = BalanceTextToString(valueToShow);
			}
		} 
	}

	private string BalanceTextToString(float value){
		string txt;
		txt = String.Format("{0:F2}", value);

		return txt;
	}
    
    //! Set the taring value to the mass value on the scale.
	//	If there's no mass, the taring value becomes zero
	public void SetupBalance() {
		if (activeMass != null) {
			taredValue = activeMass.GetComponent<Glassware> ().GetMass ();
		} else {
			taredValue = 0.00f;
		}
		workbench.GetComponentInChildren<StateUIManager> ().CloseAll ();
	}

    //! Set PlayerPrefs "setupBalance" to zero.
	public void ResetBalance()
	{
		taredValue = 0.00f;
		RefreshEquipment ();
		workbench.GetComponentInChildren<StateUIManager> ().CloseAll ();
	}

    //! Get Glassware that is on Scale.
	public Glassware GetGlassInEquipament(){
		Glassware glassToReturn = null;

		if(activeMass.GetComponent<Glassware>() != null){
			glassToReturn = activeMass.GetComponent<Glassware>();
		}
		return glassToReturn;
	}

    //! Add a GameObject to be measured on Scale.
	public override void AddObjectInEquipament(GameObject objectToAdd){
		activeMass = objectToAdd;
		RefreshEquipment();
	}

    //! Remove a GameObject from being measured on Scale.
	public override void RemoveObjectInEquipament(GameObject objectToRemove){
		activeMass = null;
		RefreshEquipment();
	}

    //! Update Real Mass to the mass of all GameObjects on ActiveMass.
	private void RefreshEquipment(){

		if (workbench.IsRunning ()) {
			valueToShow = 0.00f;
			if (activeMass != null) {
				valueToShow = activeMass.GetComponent<Glassware> ().GetMass ();		
			}
			
			valueToShow -= taredValue;

			if(previousMass!=valueToShow){
				previousMass = valueToShow;
				changed = true;
			}
		}
	}

}

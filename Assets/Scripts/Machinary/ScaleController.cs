using UnityEngine;
using System.Collections;
using System.Collections.Generic;

//! Scale Controller
/*! This is the controller for the Scale, it has all the methods that allows the Scale to work with 
 * the Glassware and Reagents, calculating their mass together and displaying it.
 */
using System;


public class ScaleController : EquipmentControllerBase 
{
	public float previousMass;
    public float realMass;  /*!< Real mass that is on the scale. */

	private bool changed;

	public TextMesh balanceText;    /*!< Text display of Scale. */

	public float timeConstant;
	private float timeElapsed;

	public GameObject activeMass;   /*!< List of GameObjects that composes the mass. */

    public WorkBench workbench;       /*!< BalanceState component. */

    // Awake happens before all Start()
	void Awake()
	{
		PlayerPrefs.SetFloat ("setupBalance", 0);
	}

	void Start () 
	{
		previousMass = 0;
		timeElapsed = 0;
	}

	void Update () 
	{
		RefreshEquipament ();

		if (changed) {
			balanceText.text = BalanceTextToString(UnityEngine.Random.Range(-1f+timeElapsed/timeConstant,1f-timeElapsed/timeConstant) + realMass);
			timeElapsed += Time.fixedDeltaTime;
			if(timeConstant<timeElapsed){
				timeElapsed = 0;
				changed = false;
				balanceText.text = BalanceTextToString(realMass);
			}
		} 
	}

	private string BalanceTextToString(float value){
		string txt;
		txt = String.Format("{0:F2}", value);

		return txt;
	}
    
    //! Set PlayerPrefs "setupBalance" to realMass.
	public void SetupBalance()
	{
		PlayerPrefs.SetFloat ("setupBalance", realMass);
		workbench.GetComponentInChildren<StateUIManager> ().CloseAll ();
	}

    //! Set PlayerPrefs "setupBalance" to zero.
	public void ResetBalance()
	{
		PlayerPrefs.SetFloat ("setupBalance", 0);
		RefreshEquipament ();
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
		RefreshEquipament();
	}

    //! Remove a GameObject from being measured on Scale.
	public override void RemoveObjectInEquipament(GameObject objectToRemove){
		activeMass = null;
		balanceText.text = BalanceTextToString (0f);
		RefreshEquipament();
	}

    //! Update Real Mass to the mass of all GameObjects on ActiveMass.
	private void RefreshEquipament(){

		if (workbench.IsRunning ()) {
			float tempMass = 0.00f;

			if (activeMass != null)
				tempMass += activeMass.GetComponent<Glassware> ().GetMass ();		
			
			realMass = tempMass - PlayerPrefs.GetFloat ("setupBalance");

			if(previousMass!=realMass){
				previousMass = realMass;
				changed = true;
			}
		}
	}

}

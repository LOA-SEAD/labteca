using UnityEngine;
using UnityEngine.UI;
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
	public Transform bucketPosition;	// Default position for the bucket

	public GameObject bucketCanvas;
	public Button bucketButton;			// Button to remove solution from bucket
	public Text destinationText;
	private string equipmentButton = "Colocar na bancada";
	private string workbenchButton = "Colocar no equipamento";
	public Button glasswareButton;		// Button to prepare the bucket

	public WorkBench workbench;			// BalanceState component.

	void Start () {
		timeElapsed = 0;
		equipmentOn = true;
		measure = true;
		destinationText.text = workbenchButton;
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
		if (workbench.positionGlass [0].childCount > 0 && bucketPosition.childCount > 0) {
			if (cubeta.content != null) {
				bucketButton.interactable = true;
				glasswareButton.interactable = false;
			} else {
				bucketButton.interactable = false;
				if (workbench.positionGlass [0].GetComponentInChildren<Glassware> ().content != null) {
					glasswareButton.interactable = true;
				}
			}
		} else {
			bucketButton.interactable = false;
			glasswareButton.interactable = false;
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
	public Glassware GetGlassInEquipment(){
		Glassware glassToReturn = null;
		
		if(activeGlassware != null){
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
		destinationText.text = equipmentButton;
		changed = true;
		RefreshEquipament();
	}

	/// <summary>
	/// Removes a GameObject from the equipment position.
	/// </summary>
	/// <param name="objectToRemove">Object to be removed.</param>
	public override void RemoveObjectInEquipament(GameObject objectToRemove){
		activeGlassware = null;
		destinationText.text = workbenchButton;
		changed = true;
		RefreshEquipament();
	}
	
	//! Update Real Mass to the mass of all GameObjects on ActiveMass.
	/// <summary>
	/// Refreshs the equipament, checking if the flicking effect will be triggered.
	/// </summary>
	private void RefreshEquipament(){
		if (workbench.IsRunning () && equipmentOn && measure) {
			if (activeGlassware != null) {
				valueToShow = activeGlassware.GetComponent<Glassware> ().GetTurbidity ();
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
	/// Correctly prepares the glassware to be analyzed.
	/// </summary>
	/// <param name="glass">The glassware to be prepared.</param>
	public void PrepareGlassware(Glassware glass) {
		float value = Mathf.Min ((glass.content as Compound).Volume, cubeta.maxVolume);
		Compound sample = (glass.content as Compound).Clone (value) as Compound;
		glass.RemoveLiquid (value);

		cubeta.IncomingReagent (sample, value);
	}

	/// <summary>
	/// Returns the volume in the bucket to the glassware to be analyzed.
	/// </summary>
	public void GiveBackReagent() {
		if ((cubeta.content as Compound).Formula == (workbench.positionGlass [0].GetComponentInChildren<Glassware> ().content as Compound).Formula) {
			float value = (cubeta.content as Compound).Volume;
			Compound sample = (cubeta.content as Compound).Clone (value) as Compound;
			cubeta.RemoveLiquid (value);

			if (workbench.positionGlass [0].childCount > 0) {
				workbench.positionGlass [0].GetComponentInChildren<Glassware> ().IncomingReagent (sample, value);
			}
		} else {
			//Send alert
		}
	}

	public void PutBackBucket() {
		cubeta.transform.SetParent(bucketPosition,false);
		cubeta.transform.localPosition = Vector3.zero;
		this.RemoveObjectInEquipament(cubeta.gameObject);
	}
}

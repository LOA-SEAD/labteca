using UnityEngine;
using System.Collections;

//! Base Class for any Controller that defines the equipments' behaviours.
/*! This class is inherited by any Controller. It's mostly used to allow a polymorphism with the Controllers,
 *  so the Game State may be more general within the presence or not of equipments.
 */

public abstract class EquipmentControllerBase : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public abstract void AddObjectInEquipament(GameObject objectToAdd);
	public abstract void RemoveObjectInEquipament(GameObject objectToRemove);
}

using UnityEngine;
using System.Collections;
using System.Collections.Generic;


//! The result of reaction.
/*  Contains all the properties of the compounds, together with
 *  a list of the reagents that were reacted, plus the reagents leftover.
 */

public class Mixture : IPhysicochemical {

	public string name;
	private float volume = 0.0f; //Volume of mixture inside
	private float waterVolume = 0.0f;
	private float realMass;
	private float density;
	private float solubility;

	private float pH;
	private float conductibility;
	private float turbidity;
	private float polarizability;
	private float refratometer;


	//The reagents that resulted on this mixture.
	/* It may also indicate reagent leftovers after the reaction.
	 * The howMuch attribute indicates the amount of leftover compounds, where (howMuch = 0.0f) means there's no leftover of that compound.
	 */
	public List<Reagent> reagents = new List<Reagent>(); //List of reagents inside
	public Compound product = null; //Product of the reaction

	//[System.Serializable] /*!< Lets you embed a class with sub properties in the inspector. */
	//Listing the compounds inside, together with the respective masses.
/*	public class CompoundsInMixture{ //TODO:having another class is really necessary?
		public Compound reagent;
		public float howMuch; //[g]
		
		public CompoundsInMixture(Compound re, float qu) {
			reagent.CopyCompound(re);
			howMuch = qu;
		}
	}
*/
	/*
	public Mixture() {

	}
	*/
	public float GetMass() {
		float resultingMass = 0.0f;

		if (reagents [0] != null)
			resultingMass += reagents [0].GetRealMass ();
		if(reagents[1] != null)
			resultingMass += reagents [1].GetRealMass ();
		if(product != null)
			resultingMass += product.GetRealMass ();

		return resultingMass;
	}

	public void SetName(string _name) { name = _name; }
	public string GetName() { return name; }
	//realMass
	public void SetRealMass(float _mass) { realMass = _mass; }
	public float GetRealMass() { return realMass; }
	//density
	public void SetDensity(float _density) { density = _density; }
	public float GetDensity() { return density; }
	//solubility
	public void SetSolubilitye(float _solubility) { solubility = _solubility; }
	public float GetSolubility() { return solubility; }
	
	
	//pH
	public void SetPh (float _pH) { pH = _pH; }
	public float GetPh() { return pH;}
	//conductibility
	public void SetConductibility (float _conductibility) { conductibility = _conductibility;}
	public float GetConductibility() { return conductibility; }
	//turbidity
	public void SetTurbidity (float _turbidity) { turbidity = _turbidity;}
	public float GetTurbidity() { return turbidity; }
	//polarizability
	public void SetPolarizability (float _polarizability) { polarizability = _polarizability; }
	public float GetPolarizability() { return polarizability; }
	//refratometer
	public void SetRefratometer (float _refratometer) { refratometer = _refratometer; }
	public float GetRefratometer() { return refratometer; } 
}
using UnityEngine;
using System.Collections;
using System.Collections.Generic;


//! The result of reaction.
/*  Contains all the properties of the compounds, together with
 *  a list of the reagents that were reacted, plus the reagents leftover.
 */

public class Mixture : IPhysicochemical {

	private string name;
	public string Name { get{ return name; } set{ name = value; }}
	private float realMass;		//The mass instantiated in the world [g]
	public float RealMass { get { return realMass; } set { realMass = value; } }
	private float density;
	public float Density { get { return density; } set { density = value; } }
	private float solubility;
	public float Solubility { get { return solubility; } set { solubility = value; } }

	private float volume = 0.0f;		//volume instantiated in the world [mL]
	public float Volume { get { return volume; } set { volume = value; } }
	private float waterVolume = 0.0f;
	public float WaterVolume { get { return waterVolume; } set { waterVolume = value; } }


	private float pH;
	public float PH { get { return pH; } set { pH = value; } }
	private float conductibility;
	public float Conductibility { get { return conductibility; } set { conductibility = value; } }
	private float turbidity;
	public float Turbidity { get { return turbidity; } set { turbidity = value; } }
	private float polarizability;
	public float Polarizability { get { return polarizability; } set { polarizability = value; } }
	private float refratometer;
	public float Refratometer { get { return refratometer; } set { refratometer = value; } }


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
			resultingMass += reagents [0].RealMass;
		if(reagents[1] != null)
			resultingMass += reagents [1].RealMass;
		if(product != null)
			resultingMass += product.RealMass;

		return resultingMass;
	}
}
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
	public float RealMass { get { return this.GetMass(); } }
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


	//! The reagents that resulted on this mixture.
	// It may also indicate reagent leftovers after the reaction.
	// The concentration of them is always 1, as the the values will be only available via Get methods
	private Reagent[] leftovers; //List of reagents inside

	//! The product of the reaction
	// The volume of water is taken in consideration only in this Compound, via the concentration value
	private Compound product = null; //Product of the reaction


	public Mixture() {
	}
	public Mixture(Compound _product, Reagent[] _leftovers) {
		this.product = _product;
		this.leftovers = _leftovers;
	}

	//! Return the value of mass
	public float GetMass() {
		float resultingMass = 0.0f;

		if(product != null)
			resultingMass += product.RealMass;
		if (leftovers != null) {
			if (leftovers [0] != null)
				resultingMass += leftovers [0].RealMass;
			if (leftovers [1] != null)
				resultingMass += leftovers [1].RealMass;
		}
		return resultingMass;
	}
}
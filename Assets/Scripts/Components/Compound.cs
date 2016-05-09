using UnityEngine;
using System.Collections;

//! The basis for all the  chemical compounds.
/*	Compounds include reagents and the results of reactions (solutions).
	The compounds are the reagents as "pure" as they can, having all the reference values. */

public class Compound {


	//Returns the mass based on the purity of the reagent.
	public float GetMass() {
		float mass = 0.0f;

		return mass;
		//return purity * molarMass + (1 - purity)*waterMolarMass
	}
}

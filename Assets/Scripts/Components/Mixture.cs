using UnityEngine;
using System.Collections;
using System.Collections.Generic;


//! The result of reaction.
/*  Contains all the properties of the compounds, together with
 *  a list of the reagents that were reacted, plus the reagents leftover.
 */

public class Mixture : Compound {

	//The reagents that resulted on this mixture.
	/* It may also indicate reagent leftovers after the reaction.
	 * The howMuch attribute indicates the amount of leftover compounds, where (howMuch = 0.0f) means there's no leftover of that compound.
	 */
	public List<CompoundsInMixture> reagents = new List<CompoundsInMixture>(); //List of reagents inside
	public Compound product = null; //Product of the reaction
	public float volume = 0.0f;		//Volume of mixture inside

	[System.Serializable] /*!< Lets you embed a class with sub properties in the inspector. */
	//Listing the compounds inside, together with the respective masses.
	public class CompoundsInMixture{ //TODO:having another class is really necessary?
		public Compound reagent;
		public float howMuch; //[g]
		
		public CompoundsInMixture(Compound re, float qu) {
			reagent.CopyCompound(re);
			howMuch = qu;
		}
	}

	/*
	public Mixture() {

	}
	*/
	public float GetMass() {
		float resultingMass = 0.0f;

		if(reagents[0] != null)
			 resultingMass += reagents [0].reagent.GetMass ();
		if(reagents[1] != null)
			resultingMass += reagents [1].reagent.GetMass ();
		if(product != null)
			resultingMass += product.GetMass ();

		return resultingMass;
	}
}
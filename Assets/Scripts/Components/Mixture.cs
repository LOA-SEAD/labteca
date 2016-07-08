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
	public float RealMass { get { return this.GetMass (); } set { } }
	private float density;
	public float Density { get { return density; } set { density = value; } }
	private float solubility;
	public float Solubility { get { return solubility; } set { solubility = value; } }


	private bool isSolid;
	public bool IsSolid { get { return this.isSolid; } set { isSolid = value; } }
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
	/*public Mixture(Compound _product, Reagent[] _leftovers) {
		this.product = _product;
		this.leftovers = _leftovers;
	}*/

	//Creates a Mixture, completing the reaction
	public Mixture(Reagent r1, Reagent r2) {
		ReactionClass reaction = ReactionTable.GetInstance().GetReaction(r1, r2);

		if (reaction != null) {
			//Makes sure the reagents are being dealt in the correct order
			if (reaction.reagent1 != r1.Formula) {
				string aux = reaction.reagent1;
				reaction.reagent1 = reaction.reagent2;
				reaction.reagent2 = aux;

				int auxSt = reaction.stoichiometryR1;
				reaction.stoichiometryR1 = reaction.stoichiometryR2;
				reaction.stoichiometryR2 = auxSt;
			}
			product = CompoundFactory.GetInstance ().GetCompound (reaction.mainProduct);

			this.RealMass = r1.RealMass + r2.RealMass;
			leftovers [0] = r1;
			leftovers [1] = r2;

			//Calculates the limiting reagent
			float limitingFactor1 = (leftovers [0].Molarity * leftovers [0].Volume) / reaction.stoichiometryR1;
			float limitingFactor2 = (leftovers [1].Molarity * leftovers [1].Volume) / reaction.stoichiometryR2;
			if (limitingFactor1 <= limitingFactor2) { //Case: limiting reagent is R1



				//Adding the water into the product's values
				product.Dilute((leftovers[0].RealMass - (leftovers[0].Molarity * (leftovers[0].Volume / 1000) * leftovers[0].MolarMass)) * 1.0f);
				product.Dilute((leftovers[1].RealMass - (leftovers[1].Molarity * (leftovers[1].Volume / 1000) * leftovers[1].MolarMass)) * 1.0f);

				//leftovers[0].Volume
				//product.Dilute(agua r2);

				//consomeTodo(r1);
					//r1.RealMass = 0.0f;


				//consome(r2);
					//r2.RealMass = (massaAgua + massaSolutoConsumido)
					//r2.Molarity = 

				/*

				leftovers [0].RealMass = 0.0f;
				leftovers [0].Molarity = 0.0f;
				leftovers [0].Density = 0.0f;
				leftovers [0].Volume = 0.0f;

				float usedMoles = reaction.stoichiometryR2 / limitingFactor1;
				leftovers [1].RealMass -= usedMoles * leftovers [1].MolarMass;
				leftovers [1].Molarity = (leftovers [1].Molarity * leftovers [1].MolarMass);
				leftovers [1].Density = 0.0f;
				leftovers [1].Volume = 0.0f;*/

			}

			if ((reaction.subProduct != "H2O") || (reaction.subProduct != "")) {
				leftovers [2] = (Reagent)CompoundFactory.GetInstance ().GetCompound (reaction.subProduct);
			}
		} else { //There was no reaction
			product = null;
			leftovers[0] = r1;
			leftovers[1] = r1;

			Name = "UnknownMixture";
			RealMass = leftovers[0].RealMass + leftovers[1].RealMass;


		}
	
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
			if (leftovers [2] != null)
				resultingMass += leftovers [2].RealMass;
		}
		return resultingMass;
	}
	
	public float GetVolume() {
		float resultingVolume = 0.0f;
					
		if(product != null)
			resultingVolume += product.Volume;
		if (leftovers != null) {
			if (leftovers [0] != null)
				resultingVolume += leftovers [0].Volume;
			if (leftovers [1] != null)
				resultingVolume += leftovers [1].Volume;
			if (leftovers [2] != null)
				resultingVolume += leftovers [2].Volume;
		}
		return resultingVolume;
	}

	public void Dilute(Compound water) {

	}
}
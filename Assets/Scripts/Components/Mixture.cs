using UnityEngine;
using System.Collections;
using System.Collections.Generic;


//! The result of reaction.
/*  Contains all the properties of the compounds, together with
 *  a list of the reagents that were reacted, plus the reagents leftover.
 */

public class Mixture : IPhysicochemical { //TODO: Add changes of collections in CompoundFactory

	private string name;
	public string Name { get{ return name; } set{ name = value; }}

	public float RealMass { get { 
		float resultingMass = 0.0f;
			
		if (product != null) {
			resultingMass += product.RealMass;
			Debug.Log (product.Formula + " = " + product.RealMass);
		}
		if (leftovers != null) {
			for (int i = 0; i < leftovers.Count; i++) {
				resultingMass += leftovers[i].RealMass;
				Debug.Log (leftovers[i].Formula + " = " + leftovers[i].RealMass);
			}
		}
		
		resultingMass += waterVolume * Compound.waterDensity;
		return resultingMass;
		} set {}}

	private float density;
	public float Density { get { return density; } set { density = value; } }
	private float solubility;
	public float Solubility { get { return solubility; } set { solubility = value; } }


	private bool isSolid;
	public bool IsSolid { get { return this.isSolid; } set { isSolid = value; } }
	private float volume = 0.0f;		//volume instantiated in the world [mL]

	public float Volume { get { 
		float resultingVolume = 0.0f;
			
		if (product != null) {
			resultingVolume += product.Volume;
		}
		if (leftovers != null) {
			if (leftovers != null) {
				for (int i = 0; i < leftovers.Count; i++) {
					resultingVolume += leftovers[i].Volume;
					Debug.Log (leftovers[i].Formula + " Volume = " + leftovers[i].Volume);
				}
			}
		}
		resultingVolume += waterVolume;
		return resultingVolume;
		//return volume;
		} set{ volume = value; }}

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

	public void ShowMolarity() {
		Debug.Log ("Amostrando de amor a molaridade da mistura");
		if (product != null) {
			Debug.Log (product.Formula + " = " + product.Molarity);
		}
		Debug.Log (leftovers[0].Formula + " = " + leftovers[0].Molarity);
		Debug.Log (leftovers[1].Formula + " = " + leftovers[1].Molarity);
	}

	//! The reagents that resulted on this mixture.
	// It may also indicate reagent leftovers after the reaction.
	// The concentration of them is always 1, as the the values will be only available via Get methods
	[SerializeField]
	//private Compound[] leftovers = new Compound[3]; //List of reagents inside
	private List<Compound> leftovers = new List<Compound>(); //List of reagents inside

	public string Leftover1Name() {
		return leftovers[0].Name;
	}
	public string Leftover1Formula(){
		return leftovers[0].Formula;
	}
	public float Leftover1Molarity () {
		return leftovers[0].Molarity;
	}
	public string Leftover2Name() {
		return leftovers[1].Name;
	}
	public string Leftover2Formula(){
		return leftovers[1].Formula;
	}
	public float Leftover2Molarity () {
		return leftovers[1].Molarity;
	}

	//! The product of the reaction
	// The volume of water is taken in consideration only in this Compound, via the concentration value
	[SerializeField]
	private Compound product = new Compound(); //Product of the reaction

	public string ProductName() {
		if (product != null)
			return product.Name;
		else
			return "";
	}
	public string ProductFormula(){
		if (product != null)
			return product.Formula;
		else
			return "";
	}
	public float ProductMolarity () {
		if (product != null)
			return product.Molarity;
		else
			return -1.0f;
	}

	public Mixture() {
	}
	/*public Mixture(Compound _product, Reagent[] _leftovers) {
		this.product = _product;
		this.leftovers = _leftovers;
	}*/

	//Creates a Mixture, completing the reaction
	public Mixture(Compound r1, Compound r2) {
		ReactionClass reaction = ReactionTable.GetInstance().GetReaction(r1.Formula, r2.Formula);

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

			//Calculates the limiting reagent
			float limitingFactor1 = ( (r1.Molarity / 1000) * r1.Volume) / reaction.stoichiometryR1;
			float limitingFactor2 = ( (r2.Molarity / 1000) * r2.Volume) / reaction.stoichiometryR2;
			Debug.Log ("limit1 =" + limitingFactor1.ToString() + "; limit2 = " + limitingFactor2.ToString());

			float trueLimiting;
			float limitingStoichiometry;
			if (limitingFactor1 <= limitingFactor2) { //Case: limiting reagent is R1
				trueLimiting = limitingFactor1;			
				limitingStoichiometry = reaction.stoichiometryR1;
				//Defining the order of reagents. The first is always the limiting reagent
				leftovers.Add(r1);
				leftovers.Add(r2);
			}
			else { 
				trueLimiting = limitingFactor2;			
				limitingStoichiometry = reaction.stoichiometryR2;

				leftovers.Add(r2);
				leftovers.Add(r1);
			}

			//Calculating mass of product
			//mass = limiting mols consumed * (stoichiometry of product / limiting stoichiometry) * product molar mass
			float productMass = ((trueLimiting * limitingStoichiometry) * reaction.stoichiometryMainProduct / limitingStoichiometry) * product.MolarMass;
			//Calculating total amount of water
			waterVolume = (r1.RealMass - ( (r1.Molarity / 1000) * r1.Volume * r1.MolarMass)) / Compound.waterDensity;
			waterVolume += (r2.RealMass - ( (r2.Molarity / 1000) * r2.Volume * r2.MolarMass)) / Compound.waterDensity;
			if(reaction.subProduct == "H2O") {
				waterVolume += (((trueLimiting * limitingStoichiometry) * (reaction.stoichiometrySubProduct / limitingStoichiometry)) * Compound.waterMolarMass) / Compound.waterDensity;
			}


			//Setting product's molarity
			product.Purity = 1.0f;
			product.Molarity = ((product.Purity * product.Density) / product.MolarMass);
			product.RealMass = productMass;
			this.Volume = r1.Volume + r2.Volume; //TODO:For the time being, the final volume is the sum of both volumes

			//Setting leftovers's values
			leftovers[0].Purity = 1.0f;
			leftovers[0].Molarity = 0.0f;
			leftovers[0].RealMass = 0.0f;
			leftovers[0].Density = 0.0f;
			leftovers[0].Volume = 0.0f;

			float massOfReagent = (leftovers[1].Molarity * (leftovers[1].Volume / 1000) * leftovers[1].MolarMass)
				- (((trueLimiting * limitingStoichiometry) * (reaction.stoichiometryR2 / limitingStoichiometry)) * leftovers[1].MolarMass); // Mass = previous mass of reagent - mass consumed;

			leftovers[1].setValues(CompoundFactory.GetInstance().GetCupboardCompound (leftovers[1].Name));
			leftovers[1].Purity = 1.0f;
			leftovers[1].RealMass = massOfReagent;
			if(leftovers[1].IsSolid) {
				//CHECK FOR PRECIPITATE
				//FOR THE TIME BEING, IT WILL BE DILUTED, AND WON'T CHANGE THE VOLUME
				leftovers[1].Volume = 0.0f;
			}
			else {
				//leftovers[1].Volume = leftovers[1].RealMass * leftovers[1].Density;
				leftovers[1].Volume = 0.0f;
			}

			//Setting Mixture's values
			//this.RealMass = this.GetMass ();
			//this.Volume = this.GetVolume ();
			this.Density  = this.RealMass / this.Volume;
			this.Name = reaction.name;

			//Setting product's values that depends on the mixture's final values
			product.Molarity = (productMass / product.MolarMass) / (this.Volume * 0.001f);
			leftovers[1].Molarity = (leftovers[1].RealMass / leftovers[1].MolarMass) / (this.Volume * 0.001f);

				/*
				 * TODO: VERIFICAR PRECIPITADOS
				 */

			/*if ((reaction.subProduct != "H2O") || (reaction.subProduct != "")) { TODO: VERIFICAR REACTION.SUBPRODUCT E COMPONENTSAVER UTILIZANDO NAME AO INVES DE FORMULA
				leftovers.Add((Compound)CompoundFactory.GetInstance ().GetCompound (reaction.subProduct));
				leftovers[2].Purity = 1.0f;
				leftovers[2].RealMass = (trueLimiting * reaction.stoichiometrySubProduct / limitingStoichiometry) * leftovers[3].MolarMass;
				leftovers[2].Molarity = leftovers[3].RealMass / this.Volume;
				if(leftovers[2].IsSolid) {
					//CHECK FOR PRECIPITATE
					//FOR THE TIME BEING, IT WILL BE DILUTED, AND WON'T CHANGE THE VOLUME
					leftovers[2].Volume = 0.0f;
				}
				else {
					leftovers[2].Volume = leftovers[3].RealMass * leftovers[3].Density;
				}
			}*/
		} else { //There was no reaction
			Debug.Log("falha");
			product = null;

			leftovers.Add (r1);
			leftovers.Add (r2);

			Name = "UnknownMixture";

			float auxVolume = 0.0f;

			//Cases where the physical states are the same, the volumes do not change
			if(leftovers[0].IsSolid && (!leftovers[1].IsSolid)) { //Case: l0 is solid, l1 is liquid
				leftovers[0].Volume = 0.0f;
				this.IsSolid = false;
			}
			else if (leftovers[1].IsSolid && (!leftovers[0].IsSolid)) { //Case: l0 is liquid, l1 is solid
				leftovers[1].Volume = 0.0f;
				this.IsSolid = false;
			} else if (leftovers[0].IsSolid && leftovers[1].IsSolid) {
				this.IsSolid = true;
			}
			else if ((!leftovers[0].IsSolid) && (!leftovers[1].IsSolid)) {
				this.IsSolid = false;
			}
		}
	
	}


	//! Dilutes the mixture based on the amount of water
	//  This overload receives the very Water compound
	public void Dilute(Compound water) {
		
	}

	//! Returns a new instance of the mixture, based on the volume wanted
	//  The portion of each component of the mixture is set based on the ratio of (originalComponentVolume / originalTotalVolume) * volume
	public Mixture Clone (float volume) {
		Mixture mix = new Mixture ();

		mix.product = (Compound)product.Clone ((this.product.Volume / this.Volume) * volume);
		mix.leftovers [0] = (Compound)this.leftovers [0].Clone ((this.leftovers [0].Volume / this.Volume) * volume);
		mix.leftovers [1] = (Compound)this.leftovers [1].Clone ((this.leftovers [1].Volume / this.Volume) * volume);
		
		return mix;
	}
}
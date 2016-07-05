using UnityEngine;
using System.Collections;

//! The reagents are compounds that may take part in reactions.
/*	They can be diluted in water, but they are still treated as the same reagents, with different concentration.
 	Reagent also holds the "React" method */

public class Reagent : Compound {

	//[TODO:mol/L?] -> How pure the reagent is, in terms of amounts of water it was diluted with.
	//private float realMass = purity * molarMass + (1 - purity)*waterMolarMass

 

	/*public Reagent (Compound compound, float _volume, float _concentration) {
		Name = compound.Name;
		this.IsSolid = compound.IsSolid;
		this.MolarMass = compound.MolarMass;
		this.Concentration = compound.Concentration;
		this.Density = compound.Density;
		this.Solubility = compound.Solubility;
		irSpecter = compound.irSpecter;
		uvSpecter = compound.uvSpecter;
		this.PH = compound.PH;
		this.Conductibility = compound.Conductibility;
		this.Turbidity = compound.Turbidity;
		this.Polarizability = compound.Polarizability;
		this.Refratometer = compound.Refratometer;
		flameSpecter = compound.flameSpecter;
		hplc = compound.hplc;
	
		this.Volume = _volume;
		//concentration = _concentration;
	}*/
	public Reagent (Compound r) {
		Name = r.Name;
		Formula = r.Formula;
		this.IsSolid = r.IsSolid;
		this.MolarMass = r.MolarMass;
		this.Molarity = r.Molarity;
		this.Density = r.getDensity();
		this.Solubility = r.Solubility;
		irSpecter = r.irSpecter;
		uvSpecter = r.uvSpecter;
		this.PH = r.PH;
		this.Conductibility = r.Conductibility;
		this.Turbidity = r.Turbidity;
		this.Polarizability = r.Polarizability;
		this.Refratometer = r.Refratometer;
		this.FlameSpecter = r.FlameSpecter;
		hplc = r.hplc;

		this.FumeHoodOnly = r.FumeHoodOnly;

		this.RealMass = r.RealMass;
		this.Volume = r.Volume;
	}

	public Reagent() {
	
	}

	public override object Clone() {
		Reagent newCompound = new Reagent(this as Compound);
		return newCompound;
	}
	public override object Clone(float reagentVolume) {
		Reagent newCompound = new Reagent(this as Compound);
		newCompound.Volume = reagentVolume;
		newCompound.RealMass = newCompound.Density * newCompound.Volume;
		return newCompound;
	}

/*	//! Dilutes the reagent into water
	// 	Takes the reagent Water as a parapeter in order to destroy the component afterwards.
	public void Dilute (Compound water) {
		if (!IsSolid) {
			this.Volume = this.Volume + water.Volume;
			this.RealMass = this.RealMass + water.RealMass;
			this.Molarity = (this.Molarity *  this.Volume) / (this.Volume + water.Volume);
			this.Density = this.RealMass / this.Volume;
		} else {
			this.Volume = water.Volume; //TODO:CHECK WITH TECA.
			this.RealMass = this.RealMass + water.RealMass;
			this.Molarity = (this.Molarity *  this.Volume) / (this.Volume + water.Volume);
			this.Density = this.RealMass / this.Volume;
		}

		water = null;
	}
	public void Dilute (float waterVolume) {
		Debug.Log ("Dilute() called");
		if (!IsSolid) {
			this.Volume = this.Volume + waterVolume;
			this.RealMass = this.RealMass + waterVolume * waterMolarMass;
			this.Molarity = (this.Molarity  *  this.Volume) / (this.Volume + waterVolume);
			this.Density = this.RealMass / this.Volume;
		} else {
			this.Volume = waterVolume; //TODO:CHECK WITH TECA.
			this.RealMass = this.RealMass + waterVolume * waterMolarMass;
			this.Molarity = (this.Molarity *  this.Volume) / (this.Volume + waterVolume);
			this.Density = this.RealMass / this.Volume;
		}
	}
*/
	public Mixture React (Reagent reagent) {

		Compound product = null;


		/*
		 * 
		 */
		return null;
	}
}
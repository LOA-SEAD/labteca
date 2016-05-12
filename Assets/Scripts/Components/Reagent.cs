﻿using UnityEngine;
using System.Collections;

//! The reagents are compounds that may take part in reactions.
/*	They can be diluted in water, but they are still treated as the same reagents, with different concentration.
 	Reagent also holds the "React" method */

public class Reagent : Compound {

	private float concentration; //[TODO:mol/L?] -> How pure the reagent is, in terms of amounts of water it was diluted with.
	//private float realMass = purity * molarMass + (1 - purity)*waterMolarMass
	public float volume;		 //[mL]
 

	public Reagent (Compound compound, float _volume, float _concentration) {
		name = compound.name;
		isSolid = compound.isSolid;
		molarMass = compound.molarMass;
		purity = compound.purity;
		density = compound.density;
		solubility = compound.solubility;
		irSpecter = compound.irSpecter;
		uvSpecter = compound.uvSpecter;
		pH = compound.pH;
		conductibility = compound.conductibility;
		turbidity = compound.turbidity;
		polarizability = compound.polarizability;
		refratometer = compound.refratometer;
		flameSpecter = compound.flameSpecter;
		hplc = compound.hplc;
	
		volume = _volume;
		concentration = _concentration;
	}
	public Reagent() {
	
	}

	//! Dilutes the reagent into water
	// 	Takes the reagent Water as a parapeter in order to destroy the component afterwards.
	public void Dilute (Reagent water) {
		/*
		 * CHANGE CONCENTRATION
		 */


		water = null;
	}

	public void React (Reagent reagent) {


	}
}
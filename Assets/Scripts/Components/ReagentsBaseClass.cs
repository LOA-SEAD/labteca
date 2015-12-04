using UnityEngine;
using System.Collections;

//! Declaration of information for liquid reagents
/*! The liquid class inherits this one, because it has all the same attributes */

public class ReagentsBaseClass : ItemToInventory {

	public string name;
	public bool isSolid;
	public int molarMass;
	public float density;
	public float polarizability;
	public float conductibility;
	public float solubility;
	public Texture2D irSpecter;
	public Texture2D flameSpecter;
	public Texture2D uvSpecter;
	
	public Texture2D texture;
	public Color color;

	public void receiveValues(ReagentsBaseClass r){
		this.name = r.name;
		this.isSolid = r.isSolid;
		this.molarMass = r.molarMass;
		this.density = r.density;
		this.polarizability = r.polarizability;
		this.conductibility = r.conductibility;
		this.solubility = r.solubility;
		this.irSpecter = r.irSpecter;
		this.flameSpecter = r.flameSpecter;
		this.uvSpecter = r.uvSpecter;
		this.texture = r.texture;
		if (!this.isSolid) {
			(this as ReagentsLiquidClass).ph = (r as ReagentsLiquidClass).ph;
			(this as ReagentsLiquidClass).turbidity = (r as ReagentsLiquidClass).turbidity;
			(this as ReagentsLiquidClass).refratometer = (r as ReagentsLiquidClass).refratometer;
		}
	}

	public ReagentsBaseClass getValues(){
		ReagentsBaseClass reg = new ReagentsBaseClass ();
		reg.name = this.name;
		reg.isSolid = this.isSolid;
		reg.molarMass = this.molarMass;
		reg.density = this.density;
		reg.polarizability = this.polarizability;
		reg.conductibility = this.conductibility;
		reg.solubility = this.solubility;
		reg.irSpecter = this.irSpecter;
		reg.flameSpecter = this.flameSpecter;
		reg.uvSpecter = this.uvSpecter;
		reg.texture = this.texture;
		if (!reg.isSolid) {
			(reg as ReagentsLiquidClass).ph = (this as ReagentsLiquidClass).ph;
			(reg as ReagentsLiquidClass).turbidity = (this as ReagentsLiquidClass).turbidity;
			(reg as ReagentsLiquidClass).refratometer = (this as ReagentsLiquidClass).refratometer;
		}

		return reg;
	}
}
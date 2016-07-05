using UnityEngine;
using System.Collections;
using System.Collections.Generic;

//! The basis for all the  chemical compounds.
/*	Compounds include reagents and the results of reactions (solutions).
	The compounds are the reagents as "pure" as they can, having all the reference values. */
[System.Serializable]
public class Compound : IPhysicochemical {

	//Water mass;
	protected const float waterMolarMass = 18.015f;
	//Density of powedered material
	protected const float powderDensity = 1.0f;

	[SerializeField]
	private string name;
	public string Name { get{ return name; } set{ name = value.Clone().ToString(); }}
	[SerializeField]
	private string formula;
	public string Formula { get{ return formula; } set{ formula = value; }}
	private bool isSolid;
	public bool IsSolid { get { return isSolid; } set { isSolid = value; } }
	[SerializeField]
	private float molarMass;
	public float MolarMass { get { return molarMass; } set { molarMass = value; } }
	private float molarity; //The compound's concentration. [mol/L]
	public float Molarity { get { return molarity; } set { molarity = value; } }
	private float purity; //The compound's purity, how it comes from the pot. [0, 1][g/g]
	public float Purity { get { return purity; } set { purity = value; } }
	[SerializeField]
	private float realMass;		//The mass instantiated in the world [g]
	public float RealMass { get { return realMass; } set { realMass = value; } }
	[SerializeField]
	private float volume;		//volume instantiated in the world [mL]
	public float Volume { get { return volume; } set { volume = value; } }
	private float density;
	public float Density { get { return (this.isSolid) ? powderDensity : density; } set { density = value; } }
	private float solubility;
	public float Solubility { get { return solubility; } set { solubility = value; } }
	public Texture2D irSpecter;
	public Texture2D uvSpecter;
	public Color color;

	//For liquid compounds
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
	private float flameSpecter;
	public float FlameSpecter { get { return flameSpecter; } set { flameSpecter = value; } }
	public Texture2D hplc;  //High-Performance liquid chromatography

	private bool fumeHoodOnly;
	public bool FumeHoodOnly { get { return fumeHoodOnly; } set { fumeHoodOnly = value; } }
	/*
	public Texture2D texture;
	 //TODO: is this needed?
	*/
	
	// Returns the value of density
	//!To be used by the Reagent class to clone a Compound correctly
	public float getDensity () { return this.density; }


	//! Constructor for generating a Compound that is yet not used in the real World
	public Compound(string _name,string _formula, bool _isSolid, float _molarMass, float _purity, float _density, float _solubility, Texture2D _irSpecter, Texture2D _uvSpecter,
	                float _pH, float _conducdibility, float _turbidity, float _polarizability, float _refratometer, float _flameSpecter, Texture2D _hplc, bool _fumeHood) {

		Name = _name;
		Formula = _formula;
		isSolid = _isSolid;
		molarMass = _molarMass;
		purity = _purity;
		density = _density;
		solubility = _solubility;
		irSpecter = _irSpecter;
		uvSpecter = _uvSpecter;
		pH = _pH;
		conductibility = _conducdibility;
		turbidity = _turbidity;
		polarizability = _polarizability;
		refratometer = _refratometer;
		flameSpecter = _flameSpecter;
		hplc = _hplc;
		fumeHoodOnly = _fumeHood;
		//volume = 1000.0f; //Supposing 1L as starting value to define the other values
		//realMass = concentration * molarMass + (1 - concentration)*waterMolarMass;
		molarity = ((purity * density) / molarMass); // number of mols / volume


	}

	//! Empty constructor
	public Compound () {
	}

	//! Constructor that copies all the attributes of a previous compound
	public Compound (Compound r) {
		this.Name = r.Name;
		this.Formula = r.Formula;
		this.isSolid = r.isSolid;
		this.molarMass = r.molarMass;
		this.molarity = r.molarity;
		this.purity = r.purity;
		this.density = r.density;
		this.polarizability = r.polarizability;
		this.conductibility = r.conductibility;
		this.solubility = r.solubility;
		this.irSpecter = r.irSpecter;
		this.flameSpecter = r.flameSpecter;
		this.uvSpecter = r.uvSpecter;
		if (!this.isSolid) {
			this.pH = r.pH;
			this.turbidity = r.turbidity;
			this.refratometer = r.refratometer;
		}
		this.fumeHoodOnly = r.FumeHoodOnly;
		this.realMass = r.realMass;
		this.volume = r.volume;
	}

	//! Set all the values to the ones of an existing compound
	public void setValues(Compound r){
		this.Name = r.Name;
		this.Formula = r.Formula;
		this.isSolid = r.isSolid;
		this.molarMass = r.molarMass;
		this.density = r.density;
		this.polarizability = r.polarizability;
		this.conductibility = r.conductibility;
		this.solubility = r.solubility;
		this.irSpecter = r.irSpecter;
		this.flameSpecter = r.flameSpecter;
		this.uvSpecter = r.uvSpecter;
		//this.color = r.color;
		if (!this.isSolid) {
			this.pH = r.pH;
			this.turbidity = r.turbidity;
			this.refratometer = r.refratometer;
		}
		this.fumeHoodOnly = r.FumeHoodOnly;
	}
	
	//! Set all the values to the ones of an existing compound
	/*public void CopyCompound(Compound baseCompound) {

		System.Reflection.FieldInfo[] fields = baseCompound.GetType().GetFields(); 
		foreach (System.Reflection.FieldInfo field in fields)
		{
			field.SetValue(this, field.GetValue(baseCompound));
		}
		/*name = baseCompound.name;
		isSolid = baseCompound.isSolid;
		molarMass = baseCompound.molarMass;
		purity = baseCompound.purity;
		density = baseCompound.density;
		solubility = baseCompound.solubility;
		irSpecter = baseCompound.irSpecter;
		uvSpecter = baseCompound.uvSpecter;
		pH = baseCompound.pH;
		conductibility = baseCompound.conductibility;
		turbidity = baseCompound.turbidity;
		polarizability = baseCompound.polarizability;
		refratometer = baseCompound.refratometer;
		flameSpecter = baseCompound.flameSpecter;
		hplc = baseCompound.hplc;*/
	//}

	public virtual object Clone() {
		return new Compound (this);
	}
	public virtual object Clone(float compoundVolume) {
		Compound newCompound = new Compound(this);
		newCompound.volume = compoundVolume;
		newCompound.realMass = newCompound.Density * newCompound.Volume;
		return newCompound;
	}

	//! Dilutes the reagent into water
	// 	Takes the reagent Water as a parapeter in order to destroy the component afterwards.
	public void Dilute (Compound water) {	//TODO: IMPLEMENTAR PH
		if (!this.IsSolid) {
			this.Volume = this.Volume + water.Volume;
			this.RealMass = this.RealMass + water.RealMass;
			this.Molarity = (this.Molarity *  this.Volume) / (this.Volume + water.Volume);
			this.Density = this.RealMass / this.Volume;
		} else {
			if(this.CheckPrecipitate(water)) { //Case there is precipitation
				this.Volume = water.Volume + ((this.molarMass * this.molarMass * this.volume) - ((this.solubility * water.RealMass) / 100) * powderDensity );
			}
			else { ///Case there is no precipitation
				this.Volume = water.Volume;
			}
			this.RealMass = this.RealMass + water.RealMass;
			this.Molarity = (this.Molarity *  this.Volume) / (this.Volume + water.Volume);
			this.Density = this.RealMass / this.Volume;
			/*
			 * Check if there will be any precipitate
			 */
			this.IsSolid = false;
		}
		
		water = null;
	}

	//! Checks if there would be precipitate on the compound
	//  Returns true if precipitation should happen. False otherwise
	public bool CheckPrecipitate(Compound water) {

		if (this.Solubility < ((this.MolarMass * this.MolarMass * this.Volume) / water.RealMass)) {
			return true;
		} else {
			return false;
		}

	}
	/*public void Dilute (float waterVolume) {
		Debug.Log ("Dilute(float) called");
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
	}*/
}

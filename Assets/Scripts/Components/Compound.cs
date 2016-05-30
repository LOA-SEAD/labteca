using UnityEngine;
using System.Collections;
using System.Collections.Generic;

//! The basis for all the  chemical compounds.
/*	Compounds include reagents and the results of reactions (solutions).
	The compounds are the reagents as "pure" as they can, having all the reference values. */
[System.Serializable]
public class Compound : IPhysicochemical {

	//Water mass;
	public const float waterMolarMass = 18.015f;
	
	private string name;
	public string Name { get{ return name; } set{ name = value; }}
	private bool isSolid;
	public bool IsSolid { get { return isSolid; } set { isSolid = value; } }
	private float molarMass;
	public float MolarMass { get { return molarMass; } set { molarMass = value; } }
	private float molarity;		//Number of mols in the solution [mol/L]
	public float Molarity { get { return molarity; } set { molarity = value; } }
	private float concentration; //The compound's "purity". [0, 1][g/g]
	public float Concentration { get { return concentration; } set { concentration = value; } }
	private float realMass;		//The mass instantiated in the world [g]
	public float RealMass { get { return realMass; } set { realMass = value; } }
	private float volume;		//volume instantiated in the world [mL]
	public float Volume { get { return volume; } set { volume = value; } }
	private float density;
	public float Density { get { return density; } set { density = value; } }
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
	public Texture2D flameSpecter;
	public Texture2D hplc;  //High-Performance liquid chromatography
	/*
	public Texture2D texture;
	 //TODO: are those needed?
	*/
	public Compound(string _name, bool _isSolid, float _molarMass, float _purity, float _density, float _solubility, Texture2D _irSpecter, Texture2D _uvSpecter,
	                float _pH, float _conducdibility, float _turbidity, float _polarizability, float _refratometer, Texture2D _flameSpecter, Texture2D _hplc) {

		Name = _name;
		isSolid = _isSolid;
		molarMass = _molarMass;
		concentration = _purity;
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

		//realMass = purity * molarMass + (1 - purity)*watermolarMass;
	}

	public Compound () {
	}

	public Compound (Compound r) {
		this.Name = r.Name;
		this.isSolid = r.isSolid;
		this.molarMass = r.molarMass;
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

		this.volume = r.volume;
	}

	public void setValues(Compound r){
		this.Name = r.Name;
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
	}

	public void CopyCompound(Compound baseCompound) {

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
	}

	public virtual object Clone() {
		return new Compound (this);
	}
	public virtual object Clone(float compoundvolume) {
		Compound newCompound = new Compound(this);
		newCompound.realMass = this.molarMass / this.density;
		return newCompound;
	}



}

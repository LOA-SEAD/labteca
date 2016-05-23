using UnityEngine;
using System.Collections;
using System.Collections.Generic;

//! The basis for all the  chemical compounds.
/*	Compounds include reagents and the results of reactions (solutions).
	The compounds are the reagents as "pure" as they can, having all the reference values. */
[System.Serializable]
public class Compound : IPhysicochemical {

	//Water mass;
	private const float waterMolarMass = 18.015f;
	
	public string name;
	public bool isSolid;
	public float molarMass;
	public float purity; 	//As pure as the compound may be. [0, 1]
	public float realMass;	//The real mass, considering purity
	public float density;
	public float solubility;
	public Texture2D irSpecter;
	public Texture2D uvSpecter;

	//For liquid compounds
	public float pH;
	public float conductibility;
	public float turbidity;
	public float polarizability;
	public float refratometer;
	public Texture2D flameSpecter;
	public Texture2D hplc;  //High-Performance liquid chromatography
	/*
	public Texture2D texture;
	public Color color; //TODO: are those needed?
	*/
	public Compound(string _name, bool _isSolid, float _molarMass, float _purity, float _density, float _solubility, Texture2D _irSpecter, Texture2D _uvSpecter,
	                float _pH, float _conducdibility, float _turbidity, float _polarizability, float _refratometer, Texture2D _flameSpecter, Texture2D _hplc) {

		name = _name;
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

		realMass = purity * molarMass + (1 - purity)*waterMolarMass;
	}

	public Compound () {
	}

	public Compound (Compound r) {
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
		if (!this.isSolid) {
			this.pH = r.pH;
			this.turbidity = r.turbidity;
			this.refratometer = r.refratometer;
		}
	}

	public void setValues(Compound r){
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

	public Compound Clone() {
		return new Compound (this);
	}

	public void SetName(string _name) { name = _name; }
	public string GetName() { return name; }
	//isSolid
	public void SetSolidFlag(bool _flag){ isSolid = _flag; }
	public bool GetSolidFlag() { return isSolid; }
	//realMass
	public void SetRealMass(float _mass) { realMass = _mass; }
	public float GetRealMass() { return purity * molarMass + (1 - purity)*waterMolarMass; }
	//density
	public void SetDensity(float _density) { density = _density; }
	public float GetDensity() { return density; }
	//solubility
	public void SetSolubilitye(float _solubility) { solubility = _solubility; }
	public float GetSolubility() { return solubility; }
	
	
	//pH
	public void SetPh (float _pH) { pH = _pH; }
	public float GetPh() { return pH;}
	//conductibility
	public void SetConductibility (float _conductibility) { conductibility = _conductibility;}
	public float GetConductibility() { return conductibility; }
	//turbidity
	public void SetTurbidity (float _turbidity) { turbidity = _turbidity;}
	public float GetTurbidity() { return turbidity; }
	//polarizability
	public void SetPolarizability (float _polarizability) { polarizability = _polarizability; }
	public float GetPolarizability() { return polarizability; }
	//refratometer
	public void SetRefratometer (float _refratometer) { refratometer = _refratometer; }
	public float GetRefratometer() { return refratometer; } 
}

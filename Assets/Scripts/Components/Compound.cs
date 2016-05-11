using UnityEngine;
using System.Collections;

//! The basis for all the  chemical compounds.
/*	Compounds include reagents and the results of reactions (solutions).
	The compounds are the reagents as "pure" as they can, having all the reference values. */

public class Compound {

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

	public void CopyCompound(Compound baseCompound) {

		System.Reflection.FieldInfo[] fields = baseCompound.GetType().GetFields(); 
		foreach (System.Reflection.FieldInfo field in fields)
		{
			field.SetValue(this, field.GetValue(baseCompound));
		}
		name = baseCompound.name;
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
		hplc = baseCompound.hplc;
	}

	//Returns the mass based on the purity of the reagent.
	public float GetMass() {


		return molarMass;
		//return purity * molarMass + (1 - purity)*waterMolarMass
	}
}

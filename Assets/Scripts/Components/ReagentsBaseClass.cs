using UnityEngine;
using System.Collections;

//! Base class for the Reagents
/*! Solids and Liquids classes inherits this one */

public abstract class ReagentsBaseClass {

	public string name;
	public int molarMass;
	public float density;
	public float ph;
	public float polarizability;
	public Texture2D uvSpecter;
	public Texture2D irSpecter;
	public Texture2D flameSpecter;
	public float conductibility;
	public float solubility;

	public Texture2D texture;
	public Color color;	
}

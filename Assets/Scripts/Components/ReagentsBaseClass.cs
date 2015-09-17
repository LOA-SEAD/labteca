﻿using UnityEngine;
using System.Collections;

//! Declaration of information for liquid reagents
/*! The liquid class inherits this one, because it has all the same attributes */

public abstract class ReagentsBaseClass {

	public string name;
	public bool isSolid;
	public int molarMass;
	public float density;
	public float polarizability;
	public float conductibility;
	public float solubility;
	public Texture2D irSpecter;

	public Texture2D texture;
	public Color color;
}

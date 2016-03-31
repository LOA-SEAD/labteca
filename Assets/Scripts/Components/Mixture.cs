using UnityEngine;
using System.Collections;

//! This is the result of reaction.
/*! Contains all the properties of the liquid reagent, together with
 *!	a list of the reagents that were reacted, plus the reagents still present here.
 	It also defines if there is any precipitate, and the mesh colors. */
public class Mixture : ReagentsLiquidClass {

	//List of reagents consumed
	//List of reagents still here
	public bool precipitate;		//There is a precipitate


	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}

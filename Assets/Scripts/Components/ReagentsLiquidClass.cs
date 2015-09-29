using UnityEngine;
using System.Collections;

//! Declaration of information for liquid reagents
/*! The liquids have all the attributes from solids, plus a couple more (Reason why it was implemented like this)*/

public class ReagentsLiquidClass : ReagentsBaseClass {

	public float ph;
	public float turbidity;
	public float refratometer;
	public Texture2D hplc;  //High-Performance liquid chromatography
}
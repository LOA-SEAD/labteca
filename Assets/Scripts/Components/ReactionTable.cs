using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ReactionTable {
		
	private Dictionary<string, int> pos;
		
	private string[][] table;
		
	// Singleton
	private static ReactionTable instance;		
	public static ReactionTable getInstance () {
		if (instance == null) {
			instance = new ReactionTable ();
		}
		return instance;
	}

	private ReactionTable () {
		/*pos = new Dictionary<string, int> ();
		pos ["NaOH"] = 0;
		pos ["HCl"] = 1;

		table = new string[2][];
		table [0] = new string[2] { null, "NaCl" }; // linha do NaOH
		table [1] = new string[2] { "NaCl", null }; // linha do HCl*/


	}

	//! Returns the product's name base on the two reagents reacting
	public string getCompoundName (Reagent r1, Reagent r2) {


		return table [pos [r1.Name]] [pos [r2.Name]];
	}

	//Use the component saver for the time being?
}


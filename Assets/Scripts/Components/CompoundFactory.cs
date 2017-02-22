using UnityEngine;
using System.Collections;
using System.Collections.Generic;

//! Receives the dictionary of reagents, and clone instances when needed
public class CompoundFactory {
	
	private Dictionary<string, Compound> cupboardCollection; //Collection of reagents
	public Dictionary<string, Compound> CupboardCollection{ get{ return cupboardCollection; }}
	private Dictionary<string, Compound> collection; //Collection of products
	public Dictionary<string, Compound> Collection { get { return collection; } }
	
	// Singleton
	private static CompoundFactory instance;
	public static CompoundFactory GetInstance () {
		if (instance == null) {
			instance = new CompoundFactory ();
		}
		return instance;
	}

	private CompoundFactory () {
		cupboardCollection = new Dictionary<string, Compound> ();
		cupboardCollection = ComponentsSaver.LoadCupboardCompounds ();

		collection = new Dictionary<string, Compound> ();
		collection = ComponentsSaver.LoadBackgroundCompounds ();
	}

	//! Returns a clone of the given compound
	public Compound GetCupboardCompound (string formula) {
		if (cupboardCollection.ContainsKey (formula))
			return (Compound)cupboardCollection [formula].Clone ();
		else
			return null;
	}														

	public Compound GetCompound (string formula) {
		if (collection.ContainsKey (formula))
			return (Compound)collection [formula].Clone ();
		else
			return null;
	}
}
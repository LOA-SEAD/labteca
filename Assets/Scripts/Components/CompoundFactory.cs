using UnityEngine;
using System.Collections;
using System.Collections.Generic;

//! Receives the dictionary of reagents, and clone instances when needed
public class CompoundFactory {
	
	private Dictionary<string, Compound> collection; //Collection of reagents
	public Dictionary<string, Compound> Collection{ get{ return collection; }}
	private Dictionary<string, Compound> products; //Collection of products
	public Dictionary<string, Compound> Products { get { return products; } }
	
	// Singleton
	private static CompoundFactory instance;
	public static CompoundFactory GetInstance () {
		if (instance == null) {
			instance = new CompoundFactory ();
		}
		return instance;
	}

	private CompoundFactory () {
		collection = new Dictionary<string, Compound> ();
		collection = ComponentsSaver.LoadReagents ();

		products = new Dictionary<string, Compound> ();
		products = ComponentsSaver.LoadProducts ();
	}

	//! Returns a clone of the given compound
	public Compound GetCompound (string formula) {
		if (collection.ContainsKey (formula))
			return (Compound)collection [formula].Clone ();
		else
			return null;
	}														

	public Compound GetProduct (string formula) {
		if (products.ContainsKey (formula))
			return (Compound)products [formula].Clone ();
		else
			return null;
	}
}
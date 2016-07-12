using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MultiDictionary<K1, K2, V> {
	private Dictionary<K1, Dictionary<K2, V>> dict =  new Dictionary<K1, Dictionary<K2, V>>();
	
	public V this[K1 key1, K2 key2] {
		get { return dict[key1][key2]; }
		
		set {
			if (!dict.ContainsKey(key1)) {
				dict[key1] = new Dictionary<K2, V>();
			}
			dict[key1][key2] = value;
		}
	}

	public bool TryGetValue(K1 key1, K2 key2, ref V value) {
		if (!dict.ContainsKey (key1)) { 
			return false;
		} else if (!dict[key1].ContainsKey (key2)) {
			return false;
		} else {
			value = dict[key1][key2];
			return true;
		}
	}
}

public class ReactionTable {

	private MultiDictionary<string, string, string> table; // < r1's Formula, r2's Formula, reaction's name >
	private Dictionary<string, ReactionClass> reactions;   // < reaction's name, reaction >
		
	// Singleton
	private static ReactionTable instance;		
	public static ReactionTable GetInstance () {
		if (instance == null) {
			instance = new ReactionTable ();
		}
		return instance;
	}

	private ReactionTable () {
		table = new MultiDictionary<string, string, string> ();

		//Load the reagents from file into memory
		reactions = ReactionsSaver.LoadReactions ();

		foreach (Compound c in CompoundFactory.GetInstance().Collection.Values) {
			foreach (ReactionClass re in reactions.Values) {
				if(re.reagent1 == c.Formula) {
					table[re.reagent1, re.reagent2] = re.name;
					table[re.reagent2, re.reagent1] = re.name;
				}
			}
		}
	}

	//! Returns the product's name base on the two reagents reacting
	public ReactionClass GetReaction (string r1, string r2) {
		string reactionName = "";
		if (table.TryGetValue(r1, r2, ref reactionName)) {
			return reactions[reactionName].Clone();	
		}
		else {
			return null;
		}
	}
}


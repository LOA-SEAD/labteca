using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SimpleJSON;

/// <summary>
/// Controls and manages all the external resources.
/// </summary>
public class ExternalResourcesController : MonoBehaviour {

	#region Unity methods
	// Use this for initialization
	void Start () {
		
	}

	// Update is called once per frame
	void Update () {

	}
	#endregion


	#region Journal Resources
	private Hashtable journalTable; // Hash of phase -> array of Stages

	/// <summary>
	/// Returns a Stage[] based on the phase
	/// </summary>
	/// <returns>An array of Stage.</returns>
	/// <param name="phase">Number of the phase.</param>
	public ExperimentsState.Stage[] GetSetOfStages (int phase) {
		if (journalTable == null) {
			InitializeJournalDictionary ();
		}
		return (ExperimentsState.Stage[])journalTable [phase];
	}

	/// <summary>
	/// Initializes the journal dictionary.
	/// </summary>
	private void InitializeJournalDictionary () {
		JSONNode json = JSON_IO.ReadJSON ("journalItems");
		journalTable = new Hashtable ();
		for (int i = 0; i < json.Count; i++) { //Iteract through phases in the .json
			ExperimentsState.Stage[] setOfStages = new ExperimentsState.Stage[json[i].Count];
			for (int j = 0; j < json[i].Count; j++) { //Iteract through the stages in the phase i
				setOfStages [j] = new ExperimentsState.Stage ();
				setOfStages [j].name = json [i][j] ["name"];
				setOfStages [j].steps = new ExperimentsState.JournalStep[json [i][j]["steps"].Count];
				for (int k = 0; k < json [i][j] ["steps"].Count; k++) { //Iteract through the steps of stage j
					setOfStages [j].steps [k] = new ExperimentsState.JournalStep ();
					setOfStages [j].steps [k].text   = json [i][j] ["steps"][k]["text"].Value;
					if (json [i] [j] ["steps"] [k] ["isDone"].AsBool != null) {
						setOfStages [j].steps [k].isDone = json [i] [j] ["steps"] [k] ["isDone"].AsBool;
					}
				}
			}
			journalTable [i] = setOfStages;
		}
	}
	#endregion

}

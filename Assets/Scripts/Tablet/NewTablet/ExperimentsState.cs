﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using SimpleJSON;

/// <summary>
/// Experiments state behaviour.
/// </summary>
public class ExperimentsState : MonoBehaviour {

	public GameObject ExperimentTab;
	public RectTransform content;


	// Use this for initialization
	void Start () {
		//Dictionary<string, Dictionary<string, string>> tabsDictionary = HandbookSaver.GetHandbook ();
		Stage[] stages = LoadStages();

		foreach (Stage st in stages) {
			GameObject tab = Instantiate (ExperimentTab.gameObject) as GameObject;
			tab.name = st.name;

			tab.GetComponentInChildren<Text> ().text = st.name;
			tab.GetComponent<Button>().onClick.AddListener (() => OpenJournal(st));
			tab.transform.SetParent (content.transform, false);
		}
	}
		
	/// <summary>
	/// Class to represent the journal steps of a stage.
	/// </summary>
	public class JournalStep {
		public string text;
		public bool isDone;
	}
	/// <summary>
	/// Class to represent the stages of phases.
	/// </summary>
	public class Stage {
		public string name;
		public JournalStep[] steps;
	}

	public static Stage[] LoadStages () {
		System.IO.StreamReader sr = new System.IO.StreamReader ("Assets\\Resources\\glu.json");
		string content = sr.ReadToEnd ();
		sr.Close();

		JSONNode json = JSON.Parse (content);
		Stage[] stages = new Stage[json.Count]; 
		for (int i = 0; i < json.Count; i++) {
			stages [i] = new Stage ();
			stages [i].name = json [i] ["name"];
			stages [i].steps = new JournalStep[json [i] ["steps"].Count];
			for (int j = 0; j < json [i] ["steps"].Count; j++) {
				stages [i].steps [j] = new JournalStep ();
				stages [i].steps [j].text = json [i] ["steps"][j]["text"].Value;
				stages [i].steps [j].isDone = json [i] ["steps"][j]["isDone"].AsBool;
			}
		}
		return stages;
	}

	private void OpenJournal(Stage stage) {
		TabletController tc = GetComponentInParent<TabletController> ();
		tc.JournalState.GetComponent<JournalState> ().SetValues (stage);
		tc.ChangeTabletState((int)TabletSubstate.JournalState);
	}
}

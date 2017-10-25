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
	/// <summary>
	/// Array of Stages
	/// </summary>
	private Stage[] stages;
	/// <summary>
	/// Array of GameObjects that represents Tabs of the Tablet's ExperimentState.
	/// </summary>
	private GameObject[] stageTabs;

	#region Unity Methods
	// Use this for initialization
	void Start () {
		ExternalResourcesController erController = GameObject.Find ("ExternalResourcesController").GetComponent<ExternalResourcesController> ();
		stages = erController.GetSetOfStages(0);
		stageTabs = new GameObject[stages.Length];
		int k = 0;

		//Instantiating Tabs and populating array of StageTabs:GameObjects
		foreach (Stage st in stages) {
			GameObject tab = Instantiate (ExperimentTab.gameObject) as GameObject;
			tab.name = st.name;

			tab.GetComponentInChildren<Text> ().text = st.name;
			tab.GetComponent<Button>().onClick.AddListener (() => OpenJournal(st));
			tab.transform.SetParent (content.transform, false);
			stageTabs [k] = tab;
			k++;
		}

		//Disabling all tabs except the first.
		for (int i = 1; i < stages.Length; i++) {
			stageTabs [i].SetActive (false);
		}
	}
	#endregion

	#region Defition of internal classes
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
	#endregion

	/// <summary>
	/// Loads the stages.
	/// </summary>
	/// <returns>Array of stages loaded.</returns>
	public static Stage[] LoadStages () {
		JSONNode json = JSON_IO.ReadJSON ("glu");
	
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

	/// <summary>
	/// Opens the journal.
	/// </summary>
	/// <param name="stage">Instance of stage.</param>
	private void OpenJournal(Stage stage) {
		TabletController tc = GetComponentInParent<TabletController> ();
		tc.JournalState.GetComponent<JournalState> ().SetValues (stage);
		tc.ChangeTabletState((int)TabletSubstate.JournalState);
	}


	public void CompleteStage(int i) {
		TabletController tc = GetComponentInParent<TabletController> ();
		tc.JournalState.GetComponent<JournalState> ().MarkAsDone (stages[i]);

		//stageTabs [i];
	}

	/// <summary>
	/// Enables the tab.
	/// </summary>
	/// <param name="i">The tab index.</param>
	/// Used externaly by the ProgressController to enable journal tabs.
	public void EnableTab(int i) {
		stageTabs [i].SetActive (true);
	}
}

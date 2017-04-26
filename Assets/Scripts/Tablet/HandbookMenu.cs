using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class HandbookMenu : TabletState {
	public RectTransform content;
	public int lastExperiment;
	public Button prefab;
	public JournalController journalController;
	
	public GameObject[] handbookTabs;
	
	public override TabletStates StateType {
		get {
			return TabletStates.HandbookMenu;
		}
	}
	
	// Use this for initialization
	/*void Start ()
	{
		RefreshScroll ();
	}*/
	
	public void RefreshScroll(){
		//Always clean the previous items
		int child = content.childCount;
		for (int i = 0; i < child; i++) {
			Destroy(content.GetChild(i).gameObject);
		}

		Dictionary<string, Dictionary<string, string>> handbookDictionary = HandbookSaver.GetHandbook ();

		handbookTabs = new GameObject[handbookDictionary.Count];
		int m = 0;
		//Generates new items
		foreach (string formula in handbookDictionary.Keys) {
			GameObject handbookItem = Instantiate (prefab.gameObject) as GameObject;
			handbookItem.name = "Tab"+formula;

			handbookItem.GetComponentInChildren<Text>().text = handbookDictionary[formula]["name"];
			handbookItem.gameObject.GetComponent<Button> ().onClick.AddListener (() => Debug.Log ("lu"));
			handbookItem.transform.SetParent (content.transform, false);
			handbookTabs[m] = handbookItem;
			m++;
		}
	}
	/// <summary>
	/// Goes to "i" experiment.
	/// </summary>
	/// <param name="i">The index.</param>
	public void GoToReagent(int i){
		journalController.changeExperiment (i);
		//GetComponentInParent<TabletStateMachine> ().goToState ((int)TabletStates.Experiments);
	}
	
	/// <summary>
	/// Activates the step tab.
	/// </summary>
	/// <param name="numberOfStep">Number of actual step.</param>
	public void ActivateStepTab(int numberOfStep){
		//Debug.Log (stepTabs.
		handbookTabs[numberOfStep].SetActive (true);
	}
}

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class HandbookMenu : TabletState {
	public RectTransform content;
	public int lastExperiment;
	public Button reagentButtonPrefab;
	public ReagentInfoState reagentState;
	
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
			GameObject handbookItem = Instantiate (reagentButtonPrefab.gameObject) as GameObject;
			handbookItem.name = "Reagent"+formula;

			handbookItem.GetComponentInChildren<Text>().text = handbookDictionary[formula]["formula"];
			handbookItem.gameObject.GetComponent<Button> ().onClick.AddListener (() => GoToReagent(formula));
			handbookItem.transform.SetParent (content.transform, false);
			handbookTabs[m] = handbookItem;
			m++;
		}
	}
	/// <summary>
	/// Go to reagent info of given button "i"
	/// </summary>
	/// <param name="i">The index.</param>
	public void GoToReagent(string formula){
		reagentState.OpenReagentInfo (formula);
		GetComponentInParent<TabletStateMachine> ().goToState ((int)TabletStates.ReagentInfo);
	}
}

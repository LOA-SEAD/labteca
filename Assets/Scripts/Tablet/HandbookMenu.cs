using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class HandbookMenu : TabletState {
	public RectTransform content;
	public Button reagentButtonPrefab;
	public ReagentInfoState reagentState;
	
	public GameObject[] handbookTabs;
	
	public override TabletStates StateType {
		get {
			return TabletStates.HandbookMenu;
		}
	}

	void OnGUI(){
		Event e = Event.current;
		if (this.GetComponent<CanvasGroup> ().alpha == 1f) {
			if (Input.GetKeyDown (KeyCode.Escape)) {
				GetComponentInParent<TabletStateMachine>().resetState();
			}
		}
	}

	/// <summary>
	/// Refreshes the content canvas.
	/// </summary>
	/// The method is used to initilize all the buttons.
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
			handbookItem.name = formula;

			handbookItem.GetComponentInChildren<Text>().text = handbookDictionary[formula]["formula"];
			handbookItem.gameObject.GetComponent<Button> ().onClick.AddListener (() => GoToReagent(handbookItem.name));
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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HandbookState : MonoBehaviour {
	public RectTransform content;
	public Button reagentButtonPrefab;

	// Use this for initialization
	public void Start() {
		Dictionary<string, Dictionary<string, string>> handbookDictionary = HandbookSaver.GetHandbook ();

		foreach (string formula in handbookDictionary.Keys) {
			GameObject handbookItem = Instantiate (reagentButtonPrefab.gameObject) as GameObject;
			handbookItem.name = formula;

			handbookItem.GetComponentInChildren<Text>().text = handbookDictionary[formula]["formula"];
			handbookItem.gameObject.GetComponent<Button> ().onClick.AddListener (() => OpenReagent(handbookItem.name));
			handbookItem.transform.SetParent (content.transform, false);
		}
	}

	/// <summary>
	/// Go to reagent info according to info in the button
	/// </summary>
	/// <param name="formula">The reagent formula.</param>
	public void OpenReagent(string formula){
		TabletController tc = GetComponentInParent<TabletController> ();
		tc.ReagentInfoState.GetComponent<NewReagentInfoState> ().SetValues (formula);
		tc.ChangeTabletState((int)TabletSubstate.ReagentInfoState);
	}
}
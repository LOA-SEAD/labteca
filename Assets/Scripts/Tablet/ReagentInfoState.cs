using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class ReagentInfoState: TabletState {
	
	public Text reagentDescription;
	public Text reagentFormula;
	public Image handle;
	private bool changed,backspace;
	
	public override TabletStates StateType {
		get {
			return TabletStates.ReagentInfo;
		}
	}
	
	// Use this for initialization
	void Start () {

	}
	
	void OnGUI(){
		Event e = Event.current;
		if (this.GetComponent<CanvasGroup> ().alpha == 1f) {
			if (Input.GetKeyDown (KeyCode.Escape)) {
				GetComponentInParent<TabletStateMachine>().goToState((int)TabletStates.HandbookMenu);
			}
		}
	}
	
	public void GoToBottom(){
		if (changed) {
			this.GetComponentInChildren<Scrollbar>().value = 0f;
			this.GetComponentInChildren<ScrollRect> ().normalizedPosition = new Vector2 (0, 0);
			changed = false;
		}
	}
	
	/// <summary>
	/// Sets the values of the state according to the reagent selected
	/// </summary>
	public void OpenReagentInfo(string formula) {
		Dictionary<string, Dictionary<string, string>> handbookDictionary = HandbookSaver.GetHandbook ();


		Debug.Log ("Opening info of " + formula);
		if (formula != "") {
			reagentFormula.text = formula;
			reagentDescription.text = handbookDictionary [formula] ["description"];
		}
	}
}

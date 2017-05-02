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
			/*if (e.isKey && (int)e.character != 0) {
				notesText.text += e.character.ToString ();
				changed = true;
			}
			if (Input.GetKeyDown (KeyCode.Backspace) && notesText.text.Length > 0 && backspace) {
				notesText.text = notesText.text.Remove (notesText.text.Length - 1);
				backspace = false;
			}
			if (Input.GetKeyUp (KeyCode.Backspace))
				backspace = true;
			*/
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

		if (formula != "") {
			reagentFormula.text = formula;
			reagentDescription.text = handbookDictionary [formula] ["description"];
		}
	}
}

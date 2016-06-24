using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class NotesState : TabletState {

	public Text notesText;
	public Image handle;
	private bool changed,backspace;
	// Use this for initialization
	void Start () {
		backspace = true;
		changed = false;
	}

	void OnGUI(){
		Event e = Event.current;
		if (this.GetComponent<CanvasGroup> ().alpha == 1f) {
			if (e.isKey && (int)e.character != 0) {
				notesText.text += e.character.ToString ();
				changed = true;
			}
			if (Input.GetKeyDown (KeyCode.Backspace) && notesText.text.Length > 0 && backspace) {
				notesText.text = notesText.text.Remove (notesText.text.Length - 1);
				backspace = false;
			}
			if (Input.GetKeyUp (KeyCode.Backspace))
				backspace = true;
		}
	}

	public void GoToBottom(){
		if (changed) {
			this.GetComponentInChildren<Scrollbar>().value = 0f;
			this.GetComponentInChildren<ScrollRect> ().normalizedPosition = new Vector2 (0, 0);
			changed = false;
		}
	}
}

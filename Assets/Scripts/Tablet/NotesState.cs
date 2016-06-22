using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class NotesState : TabletState {

	public InputField input;
	// Use this for initialization
	void Start () {
		input.caretPosition = 0;
	}

}

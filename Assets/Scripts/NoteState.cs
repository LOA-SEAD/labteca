using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NoteState : MonoBehaviour {

	// Use this for initialization
	void Start () {
		LoadNotes();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	private void LoadNotes() {
		string loadedNotes = NotesSaver.LoadNotes ();
		if (loadedNotes != "") {
			string previousText = this.GetComponentInChildren<InputField> ().text;
			string loadedText = loadedNotes;

			loadedText += "\n\n";
			loadedText += previousText;
			this.GetComponentInChildren<InputField> ().text = loadedText;
		}
	}
}

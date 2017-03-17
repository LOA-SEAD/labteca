using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// I/O class for the Tablet notes
/// </summary>
public class NotesSaver : MonoBehaviour {

	public static string LoadNotes() {
		string loadedNotes;

		JSONEdit jsonEditor = new JSONEdit("Assets/Resources/tabletNotes.json");
		loadedNotes = jsonEditor.GetMainValue ("notes");

		if (loadedNotes == null) {
			loadedNotes = "";
		}
		return loadedNotes;
	}
}

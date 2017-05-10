using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// I/O class for the Tablet notes
/// </summary>
public class NotesSaver : MonoBehaviour {

	public static string LoadNotes() {
		string loadedNotes = "";

		JSONEditor jsonEditor = new JSONEditor("anotacoes");
		int numberOfNotes = int.Parse(jsonEditor.GetMainValue ("quantidadeAnotacoes"));
		for (int i = 1; i <= numberOfNotes; i++) {
			loadedNotes += jsonEditor.GetMainValue (i.ToString());
			loadedNotes += "\n";
		}

		return loadedNotes;
	}
}

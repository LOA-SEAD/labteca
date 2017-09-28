using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NewReagentInfoState : MonoBehaviour {
	public Text reagentDescription;
	public Text reagentFormula;

	public void SetValues(string formula) {
		Dictionary<string, Dictionary<string, string>> handbookDictionary = HandbookSaver.GetHandbook ();

		if (formula != "") {
			reagentFormula.text = formula;
			reagentDescription.text = handbookDictionary [formula] ["description"];
		}
	}
}

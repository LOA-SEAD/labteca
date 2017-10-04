using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Journal state behaviour.
/// </summary>
public class JournalState : MonoBehaviour {

	public GameObject stepItemPrefab;
	public GameObject journalContentPrefab;
	public Text title;

	private Hashtable journals = new Hashtable();

	// Use this for initialization
	void Start () {
		
	}

	public void SetValues(ExperimentsState.Stage stage) {
		if (!journals.Contains (stage)) {
			GameObject journal = Instantiate (journalContentPrefab.gameObject) as GameObject;
			journal.name = stage.name;

			journal.transform.SetParent (this.transform, false);

			GameObject content = journal.GetComponentInChildren<ContentSizeFitter> ().gameObject;
			for (int i = 0; i < stage.steps.Length; i++) {
				GameObject stepItem = Instantiate (stepItemPrefab.gameObject) as GameObject;
				stepItem.transform.SetParent (content.transform, false);

				stepItem.GetComponentInChildren<Text> ().text = stage.steps [i].text;
			}
			journals.Add (stage, journal);
		} else {
			(journals [stage] as GameObject).SetActive (true);
		}
		title.text = stage.name;
	}
		
	void OnDisable() {
		foreach (GameObject gObject in journals.Values) {
			gObject.SetActive (false);
		}
	}
}

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

	/// <summary>
	/// Hashtable for the journals
	/// </summary>
	/// Hash: instance of Stage -> instance of Journal Content
	private Hashtable journals = new Hashtable();

	// Use this for initialization
	void Start () {
		
	}
		
	/// <summary>
	/// Sets the values.
	/// </summary>
	/// <param name="stage">Stage.</param>
	public void SetValues(ExperimentsState.Stage stage) {
		if (!journals.Contains (stage)) {
			/*CloseAll ();
			GameObject journal = Instantiate (journalContentPrefab.gameObject) as GameObject;
			journal.name = stage.name;

			journal.transform.SetParent (this.transform, false);

			GameObject content = journal.GetComponentInChildren<ContentSizeFitter> ().gameObject;
			for (int i = 0; i < stage.steps.Length; i++) {
				GameObject stepItem = Instantiate (stepItemPrefab.gameObject) as GameObject;
				stepItem.transform.SetParent (content.transform, false);

				stepItem.GetComponentInChildren<Text> ().text = stage.steps [i].text;
			}
			journals.Add (stage, journal);*/
			LoadValues (stage);
			SetValues (stage);
		} else {
			CloseAll ();
			(journals [stage] as GameObject).SetActive (true);
			title.text = stage.name;
		}
	}

	public void LoadValues(ExperimentsState.Stage stage) {
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
		(journals [stage] as GameObject).SetActive (false);
	}

	/// <summary>
	/// Closes all journalItems.
	/// </summary>
	private void CloseAll() {
		foreach (GameObject gObject in journals.Values) {
			gObject.SetActive (false);
		}
	}

	/// <summary>
	/// Marks the step as done.
	/// </summary>
	/// <param name="stage">Stage.</param>
	public void MarkAsDone(ExperimentsState.Stage stage) {
		if (journals.Contains (stage)) {
			(journals [stage] as GameObject).GetComponentInChildren<JournalUIItem> ().MarkAsDone ();
		}
	}

	/// <summary>
	/// Contains the specified stage.
	/// </summary>
	/// <param name="stage">Stage.</param>
	public bool Contains(ExperimentsState.Stage stage) {
		if (journals.Contains (stage)) {
			return true;
		} else {
			return false;
		}
	}
}

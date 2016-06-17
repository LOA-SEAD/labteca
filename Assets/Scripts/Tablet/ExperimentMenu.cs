using UnityEngine;
using System.Collections;
using UnityEngine.UI;


public class ExperimentMenu : TabletState
{
	public RectTransform content;
	public int lastExperiment;
	public Button prefab;
	public JournalController journalController;
	// Use this for initialization
	void Start ()
	{
		RefreshScroll ();
	}

	public void RefreshScroll(){
		int child = content.childCount;
		for (int i = 0; i < child; i++) {
			Destroy(content.GetChild(i).gameObject);
		}

		for (int i = 0; i <= lastExperiment; i++) {
			GameObject tempItem = Instantiate (prefab.gameObject) as GameObject;
			tempItem.name = "MenuButton"+i;
			tempItem.GetComponentInChildren<Text> ().text = tempItem.GetComponentInChildren<Text> ().text + (i+1);
			tempItem.gameObject.GetComponent<Button> ().onClick.AddListener (() => GoToExperiment(int.Parse(tempItem.name.Substring(10))));
			tempItem.transform.SetParent (content.transform, false);
		}
	}

	public void GoToExperiment(int i){
		journalController.changeExperiment (i);
		GetComponentInParent<TabletStateMachine> ().goToState (2);
	}
}


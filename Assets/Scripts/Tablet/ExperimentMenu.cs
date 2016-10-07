using UnityEngine;
using System.Collections;
using UnityEngine.UI;


public class ExperimentMenu : TabletState
{
	public RectTransform content;
	public int lastExperiment;
	public Button prefab;
	public JournalController journalController;

	public override TabletStates StateType {
		get {
			return TabletStates.ExperimentsMenu;
		}
	}

	// Use this for initialization
	void Start ()
	{
		RefreshScroll ();
	}

	public void RefreshScroll(){
		//Always clean the previous items
		int child = content.childCount;
		for (int i = 0; i < child; i++) {
			Destroy(content.GetChild(i).gameObject);
		}
		//Generates new items
		for (int i = 0; i <= lastExperiment; i++) {
			GameObject tempItem = Instantiate (prefab.gameObject) as GameObject;
			tempItem.name = "MenuButton"+i;
			//tempItem.GetComponentInChildren<Text> ().text = tempItem.GetComponentInChildren<Text> ().text + (i+1); //The name is based on the prefab's text
			string name = "";
			if(i == 0) {
				name = "Preparo de solução - NaCl";
			} else {
				tempItem.GetComponentInChildren<Text> ().text = tempItem.GetComponentInChildren<Text> ().text + (i+1); //The name is based on the prefab's text
			}
			tempItem.GetComponentInChildren<Text> ().text = name;
			tempItem.gameObject.GetComponent<Button> ().onClick.AddListener (() => GoToExperiment(int.Parse(tempItem.name.Substring(10))));
			tempItem.transform.SetParent (content.transform, false);
		}
	}
	/// <summary>
	/// Goes to "i" experiment.
	/// </summary>
	/// <param name="i">The index.</param>
	public void GoToExperiment(int i){
		journalController.changeExperiment (i);
		GetComponentInParent<TabletStateMachine> ().goToState ((int)TabletStates.Experiments);
	}
}
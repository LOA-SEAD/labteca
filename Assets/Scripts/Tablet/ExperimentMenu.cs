using UnityEngine;
using System.Collections;
using UnityEngine.UI;


public class ExperimentMenu : TabletState
{
	public RectTransform content;
	public int lastExperiment;
	public Button prefab;
	public JournalController journalController;

	public GameObject[] stepTabs;

	public override TabletStates StateType {
		get {
			return TabletStates.ExperimentsMenu;
		}
	}

	// Use this for initialization
	/*void Start ()
	{
		RefreshScroll ();
	}*/

	void OnGUI(){
		Event e = Event.current;
		if (this.GetComponent<CanvasGroup> ().alpha == 1f) {
			if (Input.GetKeyDown (KeyCode.Escape)) {
				GetComponentInParent<TabletStateMachine>().resetState();
			}
		}
	}

	public void RefreshScroll(int numberOfSteps){
		//Always clean the previous items
		int child = content.childCount;
		for (int i = 0; i < child; i++) {
			Destroy(content.GetChild(i).gameObject);
		}

		stepTabs = new GameObject[numberOfSteps];
		//Generates new items
		for (int i = 0; i < numberOfSteps; i++) {
			GameObject tempItem = Instantiate (prefab.gameObject) as GameObject;
			tempItem.name = "MenuButton"+i;
			string name = "";
			/*if(i == 0) {
				name = "Solução de NaCl 1 mol/litro";
			} else {
				tempItem.GetComponentInChildren<Text> ().text = tempItem.GetComponentInChildren<Text> ().text + (i+1); //The name is based on the prefab's text
			}*/
			//name = JournalSaver.GetExperimentName(i);
			name = "Desafio";
			tempItem.GetComponentInChildren<Text> ().text = name;
			tempItem.gameObject.GetComponent<Button> ().onClick.AddListener (() => GoToExperiment(int.Parse(tempItem.name.Substring(10))));
			tempItem.transform.SetParent (content.transform, false);

			stepTabs[i] = tempItem;
			//tempItem.SetActive(false);
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

	/// <summary>
	/// Activates the phase tab.
	/// </summary>
	/// <param name="numberOfStep">Number of actual step.</param>
	public void ActivatePhaseTab(int numberOfStep){
		//Debug.Log (stepTabs.
		stepTabs[numberOfStep].SetActive (true);
	}
}
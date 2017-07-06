using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class JournalController : TabletState {
	public GameObject canvasObject;
	public JournalUIInfo infoPrefab;
	public JournalUIItem journalPrefab;    /*!< Prefab with reagent list layout. */
	public float offSetItens;     /*!< Offset for each reagent on the list. */
	public int experimentNumber=-1;
	public Text experimentText;
	public Dictionary<int,Dictionary<int, JournalUIItem>> experimentDictionary = new Dictionary<int, Dictionary<int, JournalUIItem>>();
	
	public ScrollRect UIScrollList;
	private Vector3 currentPosition;
	private int lastItemPos = 0;
	public RectTransform contentRect;
	private RectTransform prefabRect;
	private List<int> listOfJournalIndexes;

	public override TabletStates StateType {
		get {
			return TabletStates.Experiments;
		}
	}

	// Use this for initialization
	void Start () {
		UIScrollList = canvasObject.GetComponentInChildren<ScrollRect> ();
		contentRect = UIScrollList.content;
	}

	void OnGUI(){
		Event e = Event.current;
		if (this.GetComponent<CanvasGroup> ().alpha == 1f) {
			if (Input.GetKeyDown (KeyCode.Escape)) {
				GetComponentInParent<TabletStateMachine>().goToState((int)TabletStates.ExperimentsMenu);
			}
		}
	}

	public void changeExperiment(int expo){
		experimentText.text = JournalSaver.GetExperimentName (expo);//"Desafio " +(expo+1);
		LoadExperiment (expo);
		SaveExperiment (experimentNumber);
		if (experimentNumber != expo) {
			experimentNumber = expo;
			rewriteContent ();
		}
	}

	public void writeReagentInfo(string name){
		deleteContent ();
		Compound reagent;

		reagent=CompoundFactory.GetInstance ().GetCupboardCompound (name);

		//adds the infoUI to content
		prefabRect = infoPrefab.GetComponent<RectTransform> ();
		// calculate y position
		float y = (prefabRect.rect.height + offSetItens) * lastItemPos;

		// set position
		Vector3 currentPos = new Vector3 (1f, -y);
		//Debug.Log("Current y position: " + y );
			
		// instantiate Item
		GameObject tempItem = Instantiate (prefabRect.gameObject,
		                                   currentPos,
		                                   prefabRect.transform.rotation) as GameObject;
		//add info to prefab
		tempItem.name = "Reagent Info";
		tempItem.GetComponent<JournalUIInfo> ().setReagent (reagent.Name, reagent.MolarMass, reagent.IsSolid, reagent.Density, reagent.Polarizability, reagent.Conductibility, reagent.Solubility, 0, 0, 0);
		// set new item parent to scroll rect content
		tempItem.transform.SetParent (contentRect.transform, false);
	}
	
	public void checkJournalItem(int index){
		GameObject journalUIItem;
		foreach (int i in listOfJournalIndexes) {
			if(i==index){
				journalUIItem = GameObject.Find("JournalUIItem"+i.ToString());
				journalUIItem.GetComponent<JournalUIItem> ().checkItem ();
				break;
			}
		}
	}

	public void deleteContent(){
		GameObject deletableInfo = GameObject.Find ("JournalUIReagent");
		if (deletableInfo != null)
			Destroy (deletableInfo);
		GameObject[] deletable = GameObject.FindGameObjectsWithTag ("JournalUIItem");
		for (int i = 0; i < deletable.Length; i++) 
			DestroyImmediate (deletable [i]);
	}

	public void LoadExperiment(int stepNumber){
		if (!experimentDictionary.ContainsKey (stepNumber)&&stepNumber!=-1) {
			Dictionary<int, JournalUIItem> tempDictionary = new Dictionary<int, JournalUIItem>();
			tempDictionary = JournalSaver.LoadJournalUIItems (stepNumber);
		
			experimentDictionary.Add (stepNumber, tempDictionary);
		}
	}

	public void SaveExperiment(int exp){
		if (exp != -1) {
			Dictionary<int, JournalUIItem> tempDictionary = new Dictionary<int, JournalUIItem>();
			experimentDictionary.Remove(exp);
			GameObject[] journalUIItems = GameObject.FindGameObjectsWithTag ("JournalUIItem");

			for (int i = 0; i < journalUIItems.Length; i++){
				int key = int.Parse(journalUIItems[i].name.Replace("JournalUIItem",""));
				tempDictionary.Add(key,journalUIItems[i].GetComponent<JournalUIItem>());
			}
			
			experimentDictionary.Add(exp,tempDictionary);
		}
	}

	public void rewriteContent(){
		deleteContent ();
		JournalUIItem currentJournalUI;
		Dictionary<int, JournalUIItem> journalUIItem = new Dictionary<int, JournalUIItem>();
		experimentDictionary.TryGetValue (experimentNumber, out journalUIItem);
		
		// Set-up components
		if (canvasObject == null)   
			Debug.LogError ("Canvas not found in GetReagentState");

		if (UIScrollList == null)
			Debug.LogError ("ScrollRect not found in GetReagentState");
		
		prefabRect = journalPrefab.GetComponent<RectTransform> ();
		
		// Store keys in a List
		listOfJournalIndexes = new List<int> (journalUIItem.Keys);
		// Loop through list
			foreach (int k in listOfJournalIndexes) {
				journalUIItem.TryGetValue (k, out currentJournalUI);

				GameObject tempItem = Instantiate (prefabRect.gameObject) as GameObject;
				tempItem.name = "JournalUIItem" + currentJournalUI.index.ToString ();

				tempItem.GetComponent<JournalUIItem> ().setText (currentJournalUI.name);
				tempItem.GetComponent<JournalUIItem> ().index = currentJournalUI.index;
				tempItem.GetComponent<JournalUIItem> ().isDone = currentJournalUI.isDone;
				for (int n = 0; n < currentJournalUI.prerequisites.Count; n++) {
					tempItem.GetComponent<JournalUIItem> ().SetPrerequisites (currentJournalUI.prerequisites [n]);
				}
				tempItem.GetComponent<JournalUIItem> ().checkPrerequisites ();

				// set new item parent to scroll rect content
				tempItem.transform.SetParent (contentRect.transform, false);

				/*if(k > GameObject.Find ("ProgressController").GetComponent<ProgressController> ().ActualStep) {
				tempItem.SetActive(false);
			}*/
			}

	}

	/// <summary>
	/// Activates the step item.
	/// </summary>
	/// <param name="numberOfStep">Number of actual step.</param>
	public void ActivateStepTab(int numberOfPhase, int numberOfStep){
		//Debug.Log (stepTabs.
		//experimentDictionary[numberOfPhase][numberOfStep].gameObject.SetActive (true);
	}
}

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class JournalController : MonoBehaviour {
	public GameObject canvasObject;
	public JournalUIInfo infoPrefab;
	public JournalUIItem journalPrefab;    /*!< Prefab with reagent list layout. */
	public float offSetItens;     /*!< Offset for each reagent on the list. */
	public int experimentNumber;
	
	private ScrollRect UIScrollList;
	private Vector3 currentPosition;
	private int lastItemPos = 0;
	private RectTransform contentRect, prefabRect;
	private List<int> listOfJournalIndexes;
	// Use this for initialization
	void Start () {
		UIScrollList = canvasObject.GetComponentInChildren<ScrollRect> ();
		contentRect = UIScrollList.content;
		rewriteContent ();
	}
	public void changeExperiment(int expo){
		experimentNumber = expo;
		rewriteContent ();
	}

	public void writeReagentInfo(string name){
		deleteContent ();
		Compound reagent;

		reagent=CompoundFactory.GetInstance ().GetCompound (name);

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
			Destroy (deletable [i]);
	}

	public void rewriteContent(){
		deleteContent ();
		JournalUIItem currentJournalUI;
		Dictionary<int, JournalUIItem> journalUIItem = JournalSaver.LoadJournalUIItems (experimentNumber);
		
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

			tempItem.GetComponent<JournalUIItem> ().setText(currentJournalUI.name);

			tempItem.name = "JournalUIItem" + currentJournalUI.index.ToString ();
			tempItem.GetComponent<JournalUIItem> ().index = currentJournalUI.index;
			tempItem.GetComponent<JournalUIItem> ().isDone = currentJournalUI.isDone;
			tempItem.GetComponent<JournalUIItem> ().prerequisites = new JournalUIItem[currentJournalUI.prerequisites.Length];
			for (int n = 0; n < currentJournalUI.prerequisites.Length; n++) {
				tempItem.GetComponent<JournalUIItem> ().prerequisites [n] = GameObject.Find ("JournalUIItem" + currentJournalUI.prerequisites [n].index.ToString ()).GetComponent<JournalUIItem> ();
			}
			tempItem.GetComponent<JournalUIItem> ().checkPrerequisites ();
			
			// set new item parent to scroll rect content
			tempItem.transform.SetParent (contentRect.transform, false);
		}
	}
	
}

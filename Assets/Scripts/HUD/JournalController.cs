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
		UIScrollList = canvasObject.GetComponentInChildren<ScrollRect> ();
		prefabRect = infoPrefab.GetComponent<RectTransform> ();	
		contentRect = UIScrollList.content;
		// calculate y position
		float y = (prefabRect.rect.height + offSetItens) * lastItemPos;

		// set position
		Vector3 currentPos = new Vector3 (1f, -y);
		//Debug.Log("Current y position: " + y );
			
		// resize content rect
		contentRect.sizeDelta = new Vector2 (
											1f, // width doesnt change
											prefabRect.rect.height + (prefabRect.rect.height + offSetItens) * lastItemPos);
			
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

	private void deleteContent(){
		GameObject deletableInfo = GameObject.Find ("JournalUIReagent");
		if (deletableInfo != null)
			Destroy (deletableInfo);
		GameObject[] deletable = GameObject.FindGameObjectsWithTag ("JournalUIItem");
		for (int i = 0; i < deletable.Length; i++) 
			Destroy (deletable [i]);
		lastItemPos = 0;
	}

	public void rewriteContent(){
		deleteContent ();
		JournalUIItem actualJournalUI;
		Dictionary<int, JournalUIItem> journalUIItem = JournalSaver.LoadJournalUIItems (experimentNumber);
		
		// Set-up components
		if (canvasObject == null)   
			Debug.LogError ("Canvas not found in GetReagentState");
		
		UIScrollList = canvasObject.GetComponentInChildren<ScrollRect> ();
		if (UIScrollList == null)
			Debug.LogError ("ScrollRect not found in GetReagentState");
		
		prefabRect = journalPrefab.GetComponent<RectTransform> ();
		
		contentRect = UIScrollList.content;
		
		// Store keys in a List
		listOfJournalIndexes = new List<int> (journalUIItem.Keys);
		// Loop through list
		foreach (int k in listOfJournalIndexes) {
			journalUIItem.TryGetValue (k, out actualJournalUI);
			// calculate y position
			GameObject tempItem = Instantiate (prefabRect.gameObject,
			                                   new Vector3(0,0,0),
			                                   prefabRect.transform.rotation) as GameObject;

			tempItem.GetComponent<JournalUIItem> ().name = actualJournalUI.name;

			tempItem.GetComponent<JournalUIItem>().resize();

			float y = contentRect.sizeDelta.y + offSetItens;
			
			// set position
			Vector3 currentPos = new Vector3 (1f, -y);
			tempItem.transform.localPosition = currentPos;
			//Debug.Log("Current y position: " + y );
			 
			// resize content rect
			contentRect.sizeDelta = new Vector2 (
				1f, // width doesnt change
				tempItem.GetComponent<RectTransform>().sizeDelta.y + contentRect.sizeDelta.y+offSetItens);


			tempItem.name = "JournalUIItem" + actualJournalUI.index.ToString ();
			tempItem.GetComponent<JournalUIItem> ().index = actualJournalUI.index;
			tempItem.GetComponent<JournalUIItem> ().isDone = actualJournalUI.isDone;
			tempItem.GetComponent<JournalUIItem> ().prerequisites = new JournalUIItem[actualJournalUI.prerequisites.Length];
			for (int n = 0; n < actualJournalUI.prerequisites.Length; n++) {
				tempItem.GetComponent<JournalUIItem> ().prerequisites [n] = GameObject.Find ("JournalUIItem" + actualJournalUI.prerequisites [n].index.ToString ()).GetComponent<JournalUIItem> ();
			}
			tempItem.GetComponent<JournalUIItem> ().checkPrerequisites ();


			// next position on inventory grid
			lastItemPos++;
			
			// set new item parent to scroll rect content
			tempItem.transform.SetParent (contentRect.transform, false);
		}
	}
	
}

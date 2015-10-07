using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class JournalController : MonoBehaviour {
	public Canvas canvasUI;
	public JournalUIItem journalPrefab;    /*!< Prefab with reagent list layout. */
	public float offSetItens;     /*!< Offset for each reagent on the list. */


	private ScrollRect UIScrollList;
	private Vector3 currentPosition;
	private int lastItemPos = 0;
	private RectTransform contentRect, prefabRect;
	private List<int> listOfJournalIndexes;
	// Use this for initialization
	void Start () {
		JournalUIItem actualJournalUI;
		Dictionary<int, JournalUIItem> journalUIItem = JournalSaver.LoadJournalUIItems ();
		
		// Set-up components
		if (canvasUI == null)   
			Debug.LogError ("Canvas not found in GetReagentState");
		
		UIScrollList = canvasUI.GetComponentInChildren<ScrollRect> ();
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
			tempItem.name = "JournalUIItem" + actualJournalUI.index.ToString ();
			tempItem.GetComponent<JournalUIItem> ().index = actualJournalUI.index;
			tempItem.GetComponent<JournalUIItem> ().isDone = actualJournalUI.isDone;
			tempItem.GetComponent<JournalUIItem> ().prerequisites = new JournalUIItem[actualJournalUI.prerequisites.Length];
			for (int n = 0; n < actualJournalUI.prerequisites.Length; n++) {
				Debug.Log (actualJournalUI.prerequisites [n].index.ToString ());
				tempItem.GetComponent<JournalUIItem> ().prerequisites [n] = GameObject.Find ("JournalUIItem" + actualJournalUI.prerequisites [n].index.ToString ()).GetComponent<JournalUIItem> ();
			}
			tempItem.GetComponent<JournalUIItem> ().name = actualJournalUI.name;
			tempItem.GetComponent<JournalUIItem> ().checkPrerequisites ();

			// next position on inventory grid
			lastItemPos++;
			
			// set new item parent to scroll rect content
			tempItem.transform.SetParent (contentRect.transform, false);
		}
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


}

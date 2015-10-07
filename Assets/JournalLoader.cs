using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class JournalLoader : MonoBehaviour {
	public Canvas canvasUI;
	public JournalUIItem journalPrefab;    /*!< Prefab with reagent list layout. */
	public float offSetItens;                       /*!< Offset for each reagent on the list. */


	private ScrollRect UIScrollList;
	private Vector3 currentPosition;
	private int lastItemPos = 0;
	private RectTransform contentRect, prefabRect;
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
		List<int> list = new List<int> (journalUIItem.Keys);
		// Loop through list
		foreach (int k in list) {
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
			tempItem.GetComponent<JournalUIItem>().index=actualJournalUI.index;
			tempItem.GetComponent<JournalUIItem>().prerequisites = new JournalUIItem[actualJournalUI.prerequisites.Length];
			tempItem.GetComponent<JournalUIItem>().name = actualJournalUI.name;


			// next position on inventory grid
			lastItemPos++;
			
			// set new item parent to scroll rect content
			tempItem.transform.SetParent (contentRect.transform, false);
		}
	}

}

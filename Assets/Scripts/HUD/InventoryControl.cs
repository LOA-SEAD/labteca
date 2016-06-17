﻿using UnityEngine;
using System.Collections;

public class InventoryControl : MonoBehaviour {

	public GameObject inventory,journal;
	// Use this for initialization
	void Start () {
		setJournalState (false);
		setInventoryState (false);
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void setJournalState(bool state){
		journal.GetComponent<CanvasGroup> ().blocksRaycasts = state;
		journal.GetComponent<CanvasGroup> ().interactable = state;
		journal.GetComponent<CanvasGroup> ().alpha = state ? 1f : 0f;
	}

	public void setInventoryState(bool state){
		if(inventory.activeSelf!=state)
			inventory.SetActive (state);
	}
}

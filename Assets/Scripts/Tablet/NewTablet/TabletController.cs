using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum TabletSubstate {
	HomeState = 0,
	ExperimentState = 1,
	JournalState = 2,
	NotesState = 3,
	HandbookState = 4,
	ReagentInfoState = 5,
	GraphicsState = 6
}

public class TabletController : MonoBehaviour {

	public Hashtable tabletMap = new Hashtable();

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}

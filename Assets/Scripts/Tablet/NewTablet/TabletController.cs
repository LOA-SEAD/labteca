using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum TabletSubstate {
	HomeState = 0,
	ExperimentState = 1,
	NotesState = 2,
	HandbookState = 3,
	GraphicsState = 4,
	JournalState = 5,
	ReagentInfoState = 6
}

public class TabletController : MonoBehaviour {
	
	public GameObject Tablet;
	public GameObject HomeState;
	public GameObject ExperimentState;
	public GameObject JournalState;
	public GameObject NotesState;
	public GameObject HandbookState;
	public GameObject ReagentInfoState;
	public GameObject GraphicsState;

	public void ChangeTabletState(int state) {
		switch ((TabletSubstate)state) {
		case TabletSubstate.HomeState:
			CloseAllState ();
			HomeState.SetActive(true);
			break;
		
		case TabletSubstate.ExperimentState:
			CloseAllState ();
			ExperimentState.SetActive(true);
			break;
		
		case TabletSubstate.JournalState:
			CloseAllState ();
			JournalState.SetActive(true);
			break;
		
		case TabletSubstate.NotesState:
			CloseAllState ();
			NotesState.SetActive(true);
			break;
		
		case TabletSubstate.ReagentInfoState:
			CloseAllState ();
			ReagentInfoState.SetActive(true);
			break;
		
		case TabletSubstate.GraphicsState:
			CloseAllState ();
			GraphicsState.SetActive(true);
			break;
		}
	} 


	protected void OpenTablet() {
		Tablet.SetActive(true);
	}
	protected void CloseTablet() {
		Tablet.SetActive(false);	
	}


	private void CloseAllState() {
		HomeState.SetActive (false);
		ExperimentState.SetActive (false);
		/*JournalState.SetActive (false);
		NotesState.SetActive (false);
		HandbookState.SetActive (false);
		ReagentInfoState.SetActive (false);
		GraphicsState.SetActive (false);*/
	}
}

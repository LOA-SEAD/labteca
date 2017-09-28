using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Substates of the tablet
/// </summary>
public enum TabletSubstate {
	HomeState = 0,
	ExperimentState = 1,
	NotesState = 2,
	HandbookState = 3,
	GraphicsState = 4,
	JournalState = 5,
	ReagentInfoState = 6
}

/// <summary>
/// Controls the tablet state machine.
/// </summary>
public class TabletController : MonoBehaviour {
	
	public GameObject Tablet;
	public GameObject HomeState;
	public GameObject ExperimentState;
	public GameObject JournalState;
	public GameObject NotesState;
	public GameObject HandbookState;
	public GameObject ReagentInfoState;
	public GameObject GraphicsState;

	#region Basic Tablet Operations
	public void ChangeTabletState(int state) {
		switch ((TabletSubstate)state) {
		case TabletSubstate.HomeState:
			CloseAllState ();
			if (HomeState != null) { 
				HomeState.SetActive(true);
			}
			break;

		case TabletSubstate.ExperimentState:
			CloseAllState ();
			if (ExperimentState != null) {
				ExperimentState.SetActive(true);
			}
			break;
		
		case TabletSubstate.JournalState:
			CloseAllState ();
			if (JournalState != null) {
				JournalState.SetActive(true);
			}			
			break;
		
		case TabletSubstate.NotesState:
			CloseAllState ();
			if (NotesState != null) {
				NotesState.SetActive(true);
			}			
			break;
		
		case TabletSubstate.ReagentInfoState:
			CloseAllState ();
			if (ReagentInfoState != null) {
				ReagentInfoState.SetActive(true);
			}
			break;
		
		case TabletSubstate.GraphicsState:
			CloseAllState ();
			if (GraphicsState != null) {
				GraphicsState.SetActive (true);
			}
			break;

		case TabletSubstate.HandbookState:
			CloseAllState ();
			if (HandbookState != null) { 
				HandbookState.SetActive(true);
			}
			break;
		}
	} 

	/// <summary>
	/// Opens the tablet.
	/// </summary>
	protected void OpenTablet() {
		if(Tablet != null) {
			Tablet.SetActive(true);	
		}
		
	}
	/// <summary>
	/// Closes the tablet.
	/// </summary>
	protected void CloseTablet() {
		if(Tablet != null) {
			Tablet.SetActive(false);	
		}
	}
		
	private void CloseAllState() {
		if (HomeState != null) { 
			HomeState.SetActive (false);
		}
		if (ExperimentState != null) {
			ExperimentState.SetActive (false);
		}
		if (NotesState != null) {
			NotesState.SetActive (false);
		}
		if (JournalState != null) { 
			JournalState.SetActive (false);
		}
		if (HandbookState != null) { 
			HandbookState.SetActive (false);
		}
		if (ReagentInfoState != null) { 
			ReagentInfoState.SetActive (false);
		}
		if (GraphicsState != null) { 
			GraphicsState.SetActive (false);
		}
	}
	#endregion
}

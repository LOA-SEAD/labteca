using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
public class TabletController : MonoBehaviour, IInputHandler {
	
	public GameObject Tablet;
	public GameObject HomeState;
	public GameObject ExperimentState;
	public GameObject JournalState;
	public GameObject NotesState;
	public GameObject HandbookState;
	public GameObject ReagentInfoState;
	public GameObject GraphicsState;

	public HUDController hudController;

	#region Unity Methods
	void Update() {
		if (NotesState.GetComponentInChildren<InputField> ().isFocused) {
			hudController.LockKeys (true);
		}
	}
	#endregion

	#region Basic Tablet Operations
	/// <summary>
	/// Changes the state of the tablet.
	/// </summary>
	/// <param name="state">State.</param>
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
	public void OpenTablet() {
		if(Tablet != null) {
			Tablet.SetActive(true);
			this.GetComponent<InputObserver> ().enabled = true;
			hudController.tabletOn = true;
			hudController.changePlayerState (false);
		}
		
	}
	/// <summary>
	/// Closes the tablet.
	/// </summary>
	public void CloseTablet() {
		if(Tablet != null) {
			Tablet.SetActive(false);
			this.GetComponent<InputObserver> ().enabled = false;
			hudController.tabletOn = false;
			hudController.changePlayerState (false);
		}
	}

	/// <summary>
	/// Closes all the tablet states.
	///	To be used internally by the TabletController.
	/// </summary>
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

	#region Input Handler Methods
	public void HandleButtons(string input, bool value) {
		if (!hudController.lockKey) {
			switch (input) {
			case "ReturnInput":
				if (value) {
					if (HomeState.activeSelf) {
						CloseTablet ();	
					} else if (ExperimentState.activeSelf || NotesState.activeSelf || HandbookState.activeSelf || GraphicsState.activeSelf) {
						ChangeTabletState ((int)TabletSubstate.HomeState);
					} else if (JournalState.activeSelf) {
						ChangeTabletState ((int)TabletSubstate.ExperimentState);
					} else if (ReagentInfoState.activeSelf) {
						ChangeTabletState ((int)TabletSubstate.HandbookState);
					}
				}
				break;

			case "TabletInput":
				if (value) {
					if (Tablet.activeSelf) {
						CloseTablet ();
					} else {
						OpenTablet ();
					}
				}
				break;
			case "MapInput":
			case "InventoryInput":
				if (value) {
					if (Tablet.activeSelf) {
						CloseTablet ();
					}
				}
				break;
			}
		}
	}

	public void HandleAxes(string input, float value) {}
	#endregion
}

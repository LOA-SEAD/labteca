using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapController : MonoBehaviour, IInputHandler {

	HUDController hudController;

	#region Basic Map Methods
	/// <summary>
	/// Opens the map.
	/// </summary>
	protected void OpenMap() {
		gameObject.SetActive(true);
		this.GetComponent<InputObserver> ().enabled = true;
		//hudController.changePlayerState (false);
	}
	/// <summary>
	/// Closes the map.
	/// </summary>
	protected void CloseMap() {
		gameObject.SetActive(false);
		this.GetComponent<InputObserver> ().enabled = false;
		//hudController.changePlayerState (true);
	}
	#endregion

	#region Input Handler Methods
	public void HandleButtons(string input, bool value) {
		switch (input) {
		case "ReturnInput":
			if (value) {
				CloseMap();
			}
			break;

		case "MapInput":
			if (value) {
				if (gameObject.activeSelf) {
					CloseMap ();
				} else {
					OpenMap ();
				}
			}
			break;
		case "TabletInput":
		case "InventoryInput":
			if (value) {
				if (gameObject.activeSelf) {
					CloseMap ();
				}
			}
			break;
		}
	}

	public void HandleAxes(string input, float value) {}
	#endregion
}

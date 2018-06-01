using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapController : MonoBehaviour, IInputHandler {

	public HUDController hudController;

	#region Basic Map Methods
	/// <summary>
	/// Opens the map.
	/// </summary>
	public void OpenMap() {
		gameObject.SetActive(true);
		this.GetComponent<InputObserver> ().enabled = true;
		hudController.mapOn = true;
		hudController.changePlayerState (false);
	}
	/// <summary>
	/// Closes the map.
	/// </summary>
	public void CloseMap() {
		gameObject.SetActive(false);
		this.GetComponent<InputObserver> ().enabled = false;
		hudController.mapOn = false;
		hudController.changePlayerState (false);
	}
	#endregion

	#region Input Handler Methods
	public void HandleButtons(string input, bool value) {
		if (!hudController.lockKey) {
			switch (input) {
			case "ReturnInput":
				if (value) {
					CloseMap ();
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
	}

	public void HandleAxes(string input, float value) {}
	#endregion
}

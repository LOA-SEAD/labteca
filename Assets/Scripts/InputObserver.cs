using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Handles the inputs observed by the InputObserver.
/// </summary>
interface IInputHandler {
	void HandleAxes(string input, float value);
	void HandleButtons(string input, bool value);
}


[RequireComponent(typeof(IInputHandler))]
/// <summary>
/// Observer for the GameInputManager that handles the events created by any inputs.
/// </summary>
/// The componenet uses a list of inputs that will be watched, that should be edited via the Unity Inspector for each object with this component.
/// 
public class InputObserver : MonoBehaviour {
	#region Unity magic methods
	void Awake() {
		GameInputManager.Singleton.Register ("MapInput", this.GetComponent<IInputHandler> ().HandleButtons);
		GameInputManager.Singleton.Register ("Horizontal", this.GetComponent<IInputHandler> ().HandleAxes);
	}

	void OnEnable() {
		GameInputManager.Singleton.Register ("MapInput", this.GetComponent<IInputHandler> ().HandleButtons);
		GameInputManager.Singleton.Register ("Horizontal", this.GetComponent<IInputHandler> ().HandleAxes);
		GameInputManager.Singleton.Register ("InteractInput", this.GetComponent<IInputHandler> ().HandleButtons);
	}

	void OnDisable() {
		GameInputManager.Singleton.Unregister ("MapInput", this.GetComponent<IInputHandler> ().HandleButtons);
		GameInputManager.Singleton.Unregister ("Horizontal", this.GetComponent<IInputHandler> ().HandleAxes);
	}
	#endregion

	#region Internals (under the hood)
	public List<string> ButtonsList = new List<string>();
	public List<string> AxesList = new List<string>();
	/*public void HandleButtons(string input, bool value) {
		switch (input) {
		case "MapInput":
			if (value) {
				Debug.Log ("Map Button pressed.");
			}
			break;
		case "InteractInput":
			if (value) {
				Debug.Log ("Interact pressed.");
			}
			break;
		}
	}

	public void HandleAxes(string input, float value) {
		switch (input) {
		case "Horizontal":
			if (value != 0.0f) {
				Debug.Log ("Horizontal = " +value);
			}
			break;
		}
	}*/
	#endregion
}

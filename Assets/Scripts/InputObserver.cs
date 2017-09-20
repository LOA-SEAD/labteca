using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Handles the inputs observed by the InputObserver.
/// </summary>
public interface IInputHandler {
	void HandleAxes(string input, float value);
	void HandleButtons(string input, bool value);
}

/// <summary>
/// Observer for the GameInputManager that handles the events created by any inputs.
/// </summary>
/// The componenet uses a list of inputs to register and unregister accordingly.
/// This list should be edited via the Unity Inspector for each object.
public class InputObserver : MonoBehaviour {
	#region Unity methods
	void OnEnable() {
		EnableAxes ();
		EnableButtons ();
	}

	void OnDisable() {
		DisableAxes ();
		DisableButtons ();
	}
	#endregion

	#region Register control
	protected void EnableAxes () {
		foreach (string axis in AxesToRegister) {
			GameInputManager.Singleton.Register (axis, this.GetComponent<IInputHandler> ().HandleAxes);
		}
	}
	protected void EnableButtons () {
		foreach (string button in ButtonsToRegister) {
			GameInputManager.Singleton.Register (button, this.GetComponent<IInputHandler> ().HandleButtons);	
		}
	}

	protected void DisableAxes () {
		foreach (string axis in AxesToUnregisterOnDisable) {
			GameInputManager.Singleton.Unregister (axis, this.GetComponent<IInputHandler> ().HandleAxes);	
		}
	}
	protected void DisableButtons () {
		foreach (string button in ButtonsToUnregisterOnDisable) {
			GameInputManager.Singleton.Unregister (button, this.GetComponent<IInputHandler> ().HandleButtons);	
		}
	}
	#endregion


	#region Internals
	public List<string> ButtonsToRegister = new List<string>();
	public List<string> ButtonsToUnregisterOnDisable = new List<string>();

	public List<string> AxesToRegister = new List<string>();
	public List<string> AxesToUnregisterOnDisable = new List<string>();
	#endregion
}

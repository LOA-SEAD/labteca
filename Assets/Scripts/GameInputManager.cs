using UnityEngine;
using System.Collections.Generic;
using System.Collections;

/// <summary>
/// Receives from InputController what actions should be done, and generate the events accordingly.
/// </summary>
public class GameInputManager : MonoBehaviour {

	#region Singleton pattern
	protected static GameInputManager singleton;

	public static GameInputManager Singleton { 
		get { 
			if (singleton==null) singleton = FindObjectOfType<GameInputManager>();
			return singleton; 
		} 
	}
	#endregion

	#region Handlers outside methods
	/// <summary>
	/// Register the specified handler in the button input list.
	/// </summary>
	/// <param name="input">Input.</param>
	/// <param name="handler">Handler.</param>
	public void Register(string input, System.Action<string, bool> handler) {
		if(singleton.observedButtons.ContainsKey(input)) {
			(singleton.inputTable [input] as InputNotifier<string, bool>).registerHandler (handler);
		}	
	}
	/// <summary>
	/// Register the specified handler in the axis input list.
	/// </summary>
	/// <param name="input">Input.</param>
	/// <param name="handler">Handler.</param>
	public void Register(string input, System.Action<string, float> handler) {
		if(singleton.observedAxes.ContainsKey(input)) {
			(singleton.inputTable [input] as InputNotifier<string, float>).registerHandler (handler);
		}	
	}
	/// <summary>
	/// Unregister the specified handler from the button input list.
	/// </summary>
	/// <param name="input">Input.</param>
	/// <param name="handler">Handler.</param>
	public void Unregister(string input, System.Action<string, bool> handler) {
		if(singleton.observedButtons.ContainsKey(input)) {
			(singleton.inputTable [input] as InputNotifier<string, bool>).unregisterHandler (handler);
		}	
	}/// <summary>
	/// Unregister the specified handler from the axis input list.
	/// </summary>
	/// <param name="input">Input.</param>
	/// <param name="handler">Handler.</param>
	public void Unregister(string input, System.Action<string, float> handler) {
		if(singleton.observedAxes.ContainsKey(input)) {
			(singleton.inputTable [input] as InputNotifier<string, float>).unregisterHandler (handler);
		}	
	}
	#endregion

	#region Subject Defintion
	/// <summary>
	/// The input notifier for a given input, who holds the list of handlers.
	/// </summary>
	public class InputNotifier<S, T>  {
		/// <summary>
		/// The collection of handlers (methods).
		/// </summary>
		/// For axes 	-> <string, float>
		/// For buttons -> <string, bool>
		public List<System.Action<S, T>> handlerCollection = new List<System.Action<S, T>>();
		/// <summary>
		/// Registers the handler.
		/// </summary>
		/// <param name="inputHandler">Input handler.</param>
		public void registerHandler(System.Action<S, T> inputHandler) {
			if(!handlerCollection.Contains(inputHandler)) {
				handlerCollection.Add (inputHandler);
			}
		}
		/// <summary>
		/// Unregisters the handler.
		/// </summary>
		/// <param name="inputHandler">Input handler.</param>
		public void unregisterHandler(System.Action<S, T> inputHandler) {
			if(handlerCollection.Contains(inputHandler)) {
				handlerCollection.Remove (inputHandler);
			}
		}
		/// <summary>
		/// Notifies the handlers that a given input happened.
		/// </summary>
		/// <param name="input">Input.</param>
		/// <param name="value">Value.</param>
		public void notifyHandlers(S input, T value) {
			foreach (System.Action<S ,T> handler in handlerCollection) {
				handler.Invoke (input, value);
			}
		}
	}
	#endregion

	#region Unity methods
	protected void Awake() {
		singleton = this;

		//---------//
		// Associating each button string with respective InputController event.
		singleton.observedButtons.Add("MapInput", InputController.MapInput);
		singleton.observedButtons.Add("InteractInput", InputController.InteractInput);
		singleton.observedAxes.Add("Horizontal", InputController.Horizontal);
		//---------//

		// Associating each button and axis to a new Notifier instance
		foreach (string k in singleton.observedButtons.Keys) {
			singleton.inputTable.Add (k, new InputNotifier<string, bool>());
		}
		foreach (string k in singleton.observedAxes.Keys) {
			singleton.inputTable.Add (k, new InputNotifier<string, float>());
		}
	}

	protected void Update() {
		// Reading every input present in the game
		foreach (string k in singleton.observedAxes.Keys) {
			(singleton.inputTable [k] as InputNotifier<string, float>).notifyHandlers (k, singleton.observedAxes[k].Invoke());
		}
		foreach (string k in singleton.observedButtons.Keys) {
			(singleton.inputTable [k] as InputNotifier<string, bool>).notifyHandlers (k, singleton.observedButtons[k].Invoke());
		}
	}
	#endregion

	#region Internals
	/// <summary>
	/// List of observed axis.
	/// </summary>
	/// Associating a given string with the function in the InputController for that event.
	protected Dictionary<string, System.Func<float>> observedAxes = new Dictionary<string, System.Func<float>>();
	/// <summary>
	/// List of observed buttons.
	/// </summary>
	/// Associating a given string with the function in the InputController for that event.
	protected Dictionary<string, System.Func<bool>> observedButtons = new Dictionary<string, System.Func<bool>>();
	/// <summary>
	/// Table that associates input names with their notifiers.
	/// </summary>
	protected Hashtable inputTable = new Hashtable (); //Hash of string -> InputNotifier
	#endregion
}
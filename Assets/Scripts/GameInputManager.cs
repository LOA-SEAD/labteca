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
	public void Register(string input, System.Action<string, bool> handler) {
		if(singleton.observedButtons.ContainsKey(input)) {
			(singleton.inputTable [input] as InputNotifier<string, bool>).registerHandler (handler);
		}	
	}
	public void Register(string input, System.Action<string, float> handler) {
		if(singleton.observedAxes.ContainsKey(input)) {
			(singleton.inputTable [input] as InputNotifier<string, float>).registerHandler (handler);
		}	
	}

	public void Unregister(string input, System.Action<string, bool> handler) {
		if(singleton.observedButtons.ContainsKey(input)) {
			(singleton.inputTable [input] as InputNotifier<string, bool>).unregisterHandler (handler);
		}	
	}
	public void Unregister(string input, System.Action<string, float> handler) {
		if(singleton.observedButtons.ContainsKey(input)) {
			(singleton.inputTable [input] as InputNotifier<string, float>).unregisterHandler (handler);
		}	
	}
	#endregion


	#region Subject Defintion
	public class InputNotifier<S, T>  {
		public List<System.Action<S, T>> handlerCollection = new List<System.Action<S, T>>();

		public void registerHandler(System.Action<S, T> inputHandler) {
			if(!handlerCollection.Contains(inputHandler)) {
				handlerCollection.Add (inputHandler);
			}
		}

		public void unregisterHandler(System.Action<S, T> inputHandler) {
			if(handlerCollection.Contains(inputHandler)) {
				handlerCollection.Remove (inputHandler);
			}

		}

		public void notifyHandlers(S input, T value) {
			foreach (System.Action<S ,T> handler in handlerCollection) {
				handler.Invoke (input, value);
			}
		}
	}
	#endregion

	#region Unity methods
	protected void Start() {
		singleton = this;

		//---------//
		#region Defining input strings according to InputController.
		singleton.observedButtons.Add("MapInput", InputController.MapInput);
		singleton.observedButtons.Add("InteractInput", InputController.InteractInput);
		singleton.observedAxes.Add("Horizontal", InputController.Horizontal);
		#endregion
		//---------//

		foreach (string k in singleton.observedButtons.Keys) {
			singleton.inputTable.Add (k, new InputNotifier<string, bool>());
		}
		foreach (string k in singleton.observedAxes.Keys) {
			singleton.inputTable.Add (k, new InputNotifier<string, float>());
		}
	}

	protected void Update() {
		foreach (string k in singleton.observedAxes.Keys) {
			(singleton.inputTable [k] as InputNotifier<string, float>).notifyHandlers (k, singleton.observedAxes[k].Invoke());
		}
		foreach (string k in singleton.observedButtons.Keys) {
			(singleton.inputTable [k] as InputNotifier<string, bool>).notifyHandlers (k, singleton.observedButtons[k].Invoke());
		}
	}
	#endregion

	#region Internals (under the hood)
	protected Dictionary<string, System.Func<float>> observedAxes = new Dictionary<string, System.Func<float>>();
	protected Dictionary<string, System.Func<bool>> observedButtons = new Dictionary<string, System.Func<bool>>();
	protected Hashtable inputTable = new Hashtable (); //Hash of string -> InputNotifier
	#endregion
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestInput : MonoBehaviour, IInputHandler {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void HandleButtons(string input, bool value) {
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
	}
}

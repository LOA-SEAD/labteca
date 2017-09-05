using UnityEngine;
using System.Collections;

/// <summary>
/// Receives all inputs and sends the proper messages to the GameInputManager class.
/// </summary>
public static class InputController {
	
	public static float Horizontal() {
		float r = 0.0f;
		r += Input.GetAxis ("Horizontal");
		//
		return Mathf.Clamp(r, -1.0f, 1.0f);
	}

	public static bool MapInput() {
		return Input.GetButton ("Map_Button");
	}

	public static bool InteractInput() {
		if (Input.GetButton ("Button_A") || Input.GetButton ("Key_E")) {
			return true;
		} else {
			return false;
		}
	}
}
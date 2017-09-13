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
		if (Input.GetButtonDown ("Map_Button") || Input.GetKeyDown (KeyCode.Z)) {
			return true;
		} else {
			return false;
		}
	}

	public static bool InteractInput() {
		if (Input.GetButtonDown ("Joystick_Interact") || Input.GetButtonDown("Keyboard_Interact")) {
			return true;
		} else {
			return false;
		}
	}
}
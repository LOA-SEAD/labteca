using UnityEngine;
using System.Collections;

/// <summary>
/// Receives all inputs and sends the proper messages to the GameInputManager class.
/// </summary>
public static class InputController {

	#region Axes
	public static float Horizontal() {
		float r = 0.0f;
		r += Input.GetAxis ("Horizontal");
		//
		return Mathf.Clamp(r, -1.0f, 1.0f);
	}
	#endregion

	#region Buttons
	public static bool MapInput() {
		if (Input.GetButtonDown ("Keyboard_Map") || Input.GetButtonDown ("Joystick_Map")) {
			return true;
		} else {
			return false;
		}
	}

	public static bool InteractInput() {
		if (Input.GetButtonDown ("Keyboard_Interact") || Input.GetButtonDown("Joystick_Interact")) {
			return true;
		} else {
			return false;
		}
	}

	public static bool TabletInput() {
		if (Input.GetButtonDown ("Keyboard_Tablet") || Input.GetButtonDown ("Joystick_Tablet")) {
			return true;
		} else {
			return false;
		}
	}

	public static bool ReturnInput() {
		if (Input.GetButtonDown ("Keyboard_Return") || Input.GetButtonDown ("Joystick_Return")) {
			return true;
		} else {
			return false;
		}
	}

	public static bool PauseInput() {
		if (Input.GetButtonDown ("Keyboard_Pause") || Input.GetButtonDown ("Joystick_Pause")) {
			return true;
		} else {
			return false;
		}
	}

	#endregion
}
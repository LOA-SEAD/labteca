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
		r += Input.GetAxis ("Joystick_Horizontal");
		return Mathf.Clamp(r, -1.0f, 1.0f);
	}
	public static float Vertical() {
		float r = 0.0f;
		r += Input.GetAxis ("Vertical");
		r += Input.GetAxis ("Joystick_Vertical");
		return Mathf.Clamp(r, -1.0f, 1.0f);
	}
	public static float CameraHorizontal() {
		float r = 0.0f;
		r += Input.GetAxis ("Mouse X");
		r += Input.GetAxis ("Joystick_CameraX");
		return Mathf.Clamp(r, -3.0f, 3.0f);
		//return r;
	}
	public static float CameraVertical() {
		float r = 0.0f;
		r += Input.GetAxis ("Mouse Y");
		r += Input.GetAxis ("Joystick_CameraY");
		return Mathf.Clamp(r, -3.0f, 3.0f);
		//return r;
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

	public static bool InventoryInput() {
		if (Input.GetButtonDown ("Keyboard_Inventory") || Input.GetButtonDown ("Joystick_Inventory")) {
			return true;
		} else {
			return false;
		}
	}

	#endregion
}
using UnityEngine;
using System.Collections;

/// <summary>
/// Receives all inputs and sends the proper messages to the GameInputManager class.
/// </summary>
public static class InputController{
	
	public static float Horizontal() {
		float r = 0.0f;
		r += Input.GetAxis ("Horizontal");
		//
		return Mathf.Clamp(r, -1.0f, 1.0f);
	}

	public static bool MapButton() {
		return Input.GetButton ("Map_Button");
	}
}
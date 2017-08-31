using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Handles the inputs observed by the InputObserver.
/// </summary>
interface IInputHandler {
	void HandlePermanentInput(string input);
	void HandleEnabledInput(string input);
}
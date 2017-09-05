using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(IInputHandler))]

/// <summary>
/// Observer for the GameInputManager that handles the events created by any inputs.
/// </summary>
/// The componenet uses a list of inputs that will be watched, that should be edited via the Unity Inspector for each object with this component.
/// 
public class InputObserver : MonoBehaviour {

	[SerializeField]
	private List<string> PermanentInputList;
	[SerializeField]
	private List<string> OnDisableInputList;

	#region Unity magic methods
	protected void Awake() {
		foreach (string input in PermanentInputList) {
			//GameInputManager.ObserveKeyCode (input);
		}

//		GameInputManager.Register(this.GetComponent<IInputHandler>().HandlePermanentInput);
	}

	protected void OnEnable() {
		/*GameInputManager.ObserveKeyCode(KeyCode.Space);
		GameInputManager.ObserveKeyCode(KeyCode.Escape);
		GameInputManager.ObserveAxis("Horizontal");*/

		//GameInputManager.Register(OnInputEvent);
//		GameInputManager.Register(this.GetComponent<IInputHandler>().HandleEnabledInput);
	}

	protected void OnDisable() {
//		GameInputManager.Unregister(this.GetComponent<IInputHandler>().HandleEnabledInput);
	}
	#endregion

	#region Internals (under the hood)
	/*protected void OnInputEvent(GameInputManager.EventData data) {
		if (data.used) return;

		if (data.keyCode==KeyCode.Space) {
			Debug.Log("Spacebar was pressed");
			data.used = true;
		} else if (data.keyCode==KeyCode.Escape) {
			Debug.Log("Escape was pressed");
			data.used = true;
		} else if (data.axis=="Horizontal") {
			if (data.value!=0f) {
				Debug.Log("Horizontal axis = " + data.value.ToString());
			}
			data.used = true;
		}
	}*/
	#endregion
}

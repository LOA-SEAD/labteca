using UnityEngine;
using System.Collections;

/// <summary>
/// Has the methods that defines the behavious of the cutscenes.
///	Primarly done to end the cutscene when the "Transition" state is achieved.
/// </summary>
public class CutsceneBehaviour : MonoBehaviour {

	public HUDController hudController;

	// Update is called once per frame
	void Update () {
		hudController.LockKeys (true);
		if (this.gameObject.GetComponent<Animator> ().GetCurrentAnimatorStateInfo (0).IsName ("Transition") || Input.GetKeyDown(KeyCode.Escape)) {
			OnEndCutscene();
		}
	}

	/// <summary>
	/// Play the cutscene by activating the gameObject.
	/// </summary>
	public void Play() {
		this.gameObject.SetActive (true);
	}

	/// <summary>
	/// Raises the end cutscene event.
	/// </summary>
	private void OnEndCutscene() {
		GetComponentInParent<CustomModeAnimationsController> ().OnEndAnimation ();
		this.gameObject.SetActive(false);
	}
}

using UnityEngine;
using UnityEngine.UI;
using System.Collections;

/// <summary>
/// Class to control the animations os the custom mode.
/// </summary>
public class CustomModeAnimationsController : MonoBehaviour {

	public ProgressController prgController;
	public FPSInputController player;
	bool ending = false;

	//	Each cutscene is a different cutscene behaviour
	public CutsceneBehaviour compoundClass;
	public CutsceneBehaviour whatCompound;
	public CutsceneBehaviour massConcentration;
	public CutsceneBehaviour dilution;
	public CutsceneBehaviour endingScene;
	public CutsceneBehaviour wrongAnswer;


	void Awake () {
		prgController = GameObject.Find ("ProgressController").GetComponent<ProgressController> ();
		GameObject.Find ("FadeCanvas").GetComponentInChildren<Image>().enabled = true;;
	}

	/// <summary>
	/// Plays the cutscene according to the current step of the ProgressController
	/// </summary>
	public void PlayTransitionCutscene() {
		OnStartAnimation ();

		switch (prgController.StepType) {
		case TypeOfStep.CompoundClass:
			compoundClass.Play ();
			break;
		case TypeOfStep.WhatCompound:
			whatCompound.Play ();
			break;
		case TypeOfStep.MolarityCheck:
			massConcentration.Play ();
			break;
		case TypeOfStep.GlasswareCheck:
			dilution.Play ();
			break;
		}
	}

	/// <summary>
	/// Playss the wrong answer scene.
	/// </summary>
	/// <returns>The wrong answer scene.</returns>
	public void PlayWrongAnswerScene() {
		OnStartAnimation ();
		wrongAnswer.Play ();
	}


	/// <summary>
	/// Plays the ending scene.
	/// </summary>
	/// <returns>The ending scene.</returns>
	public void PlayEndingScene(){
		ending = true;
		OnStartAnimation ();
		endingScene.Play ();
	}

	/// <summary>
	/// Actions to be taken when the cutscene is starting.
	/// </summary>
	public void OnStartAnimation() {
		player.LockKeys();
	}

	/// <summary>
	/// Actions to be taken when the cutscene is over.
	/// </summary>
	public void OnEndAnimation() {
		player.UnlockKeys ();
		if (!ending) {
			// set state back to first state
			GameObject.Find ("GameController").GetComponent<GameController> ().GoToDefaultState ();
		} else {
			GameObject.Find ("FadeCanvas").GetComponentInChildren<Image>().enabled = true;
			GameObject.Find ("FadeCanvas").GetComponentInChildren<Image>().color = new Color(0,0,0,255);
			prgController.PhaseTransition();
		}
	}
}

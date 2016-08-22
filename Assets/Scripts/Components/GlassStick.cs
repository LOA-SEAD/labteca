using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

//! Controls the glass stick
/*! The glass stick is used to active the mix. */

public class GlassStick : MonoBehaviour {

	//For the cursor
	public Texture2D glassStick_CursorTexture;
	public Vector2 hotSpot = Vector2.zero;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	//! Holds the events for when the interactive glass stick on the Workbench is clicked
	void OnClick() {
		MouseState currentState = CursorManager.GetCurrentState ();
		
		switch (currentState) {
		case MouseState.ms_default: 		//Default -> Glass Stick: prepares the glass stick for use
			CursorManager.SetMouseState(MouseState.ms_glassStick);
			CursorManager.SetNewCursor(glassStick_CursorTexture, hotSpot);
			break;
		case MouseState.ms_volPipette: 		//Pipette -> Glass Stick: change to glass stick state
			CursorManager.SetMouseState(MouseState.ms_glassStick);
			CursorManager.SetNewCursor(glassStick_CursorTexture, hotSpot);
			break;
		case MouseState.ms_filledVolPipette: 	// Filled Spatula -> Glass Stick: nothing
			break;
		case MouseState.ms_spatula: 		// Spatula -> Glass Stick: change to wash bottle state
			CursorManager.SetMouseState(MouseState.ms_glassStick);
			CursorManager.SetNewCursor(glassStick_CursorTexture, hotSpot);
			break;
		case MouseState.ms_filledSpatula: 	// Filled Spatula -> Glass Stick: nothing
			break;
		case MouseState.ms_washBottle: 		// Wash Bottle -> Glass Stick: change to glass stick state
			CursorManager.SetMouseState(MouseState.ms_glassStick);
			CursorManager.SetNewCursor(glassStick_CursorTexture, hotSpot);
			break;
		case MouseState.ms_glassStick:		// Glass Stick -> Glass Stick: put back the glass stick
			CursorManager.SetMouseState(MouseState.ms_default);
			CursorManager.SetNewCursor(glassStick_CursorTexture, hotSpot);
			break;
		case MouseState.ms_usingTool:  		// Unable to click somewhere else TODO:is it necessary?
			break;
		}
	}

	public void Mix(Glassware glassware) {
		/*
		 * DO THE MIX, BASED ON THE GLASSWARE
		 * It knows the stoichiometry, so only a certain amount will react
		 * This certain amount will be ~consumed~, and the rest will remain.
		 * 
		 */
	}
}

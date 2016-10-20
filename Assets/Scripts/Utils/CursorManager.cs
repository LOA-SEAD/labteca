using UnityEngine;
using UnityEngine.UI;
using System.Collections;

/*! Manages the cursor texture.
 *  In addition, this classe is used to track the state in the workbench.
 *	The states refers to which object is being used in the moment, and how.
 */


// Enum that defines what mouse states the workbench might be.
public enum MouseState {
	ms_default,			//Holding nothing
	ms_pipette,			//Holding an empty pipette
	ms_filledPipette,	//Holding a filled pipette
	ms_spatula,			//Holding an empty spatula
	ms_filledSpatula,	//Holding a filled spatula
	ms_washBottle,		//Holding the wash bottle
	ms_interacting		/*The player is using a tool.
	               		  interaction box is open, so the player
	               		  is unable to click anywhere else */
};

public class CursorManager : MonoBehaviour {

	private static bool inDefaultCursor = true;
	public static CursorManager instance;
	public static CursorMode cursorMode = CursorMode.Auto;
	private static Texture2D lastCursor;

	private static MouseState currentState; //Current mouse state.


	// Use this for initialization
	/*void Start () {

	
	}
	
	// Update is called once per frame
	void Update () {
	
	}*/

	//! Changes the mouse cursor to the set texture.
	public static void SetNewCursor(Texture2D cursorTexture, Vector2 hotSpot){

		Cursor.SetCursor(cursorTexture, hotSpot, CursorManager.cursorMode);
		inDefaultCursor = false;
		lastCursor = cursorTexture;
	}

	public static void SetToInteractiveCursor(Texture2D cursorTexture, Vector2 hotSpot) {
		Cursor.SetCursor(cursorTexture, hotSpot, CursorManager.cursorMode);
	}

	//! Changes the mouse cursor back to default.
	public static void SetCursorToDefault(){
		Cursor.SetCursor (null, Vector2.zero, CursorManager.cursorMode);
		inDefaultCursor = true;
		lastCursor = null;
	}

	//! Returns true if the current cursor is the default
	public static bool UsingDefaultCursor(){
		return inDefaultCursor;
	}

	//! Returns the texture of the lastCursor
	public static Texture2D GetLastCursor(){
		return lastCursor;
	}

	//! Returns the current mouse state
	public static MouseState GetCurrentState() {
		return currentState;
	}

	//! Setting the new state. This method most likely should be called together with SetNewCursor()
	public static void SetMouseState(MouseState newState) {
		currentState = newState;
	}

	//Return to the previous cursor. Used to return after hoving over interactive objects.
	public static void SetToPreviousCursor() {
		Cursor.SetCursor(lastCursor, Vector2.zero, CursorManager.cursorMode);
	}

}

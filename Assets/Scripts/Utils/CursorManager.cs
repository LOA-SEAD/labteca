using UnityEngine;
using UnityEngine.UI;
using System.Collections;

/*! Has 4 methods:
 * One for setting the cursor to the cursor set in the unity
 * One for setting the cursor back to it's default
 * One to return if the current cursor is the default
 * One to return the lastCursor
 */

public class CursorManager : MonoBehaviour {

	private static bool inDefaultCursor = true;
	public static CursorManager instance;
	public static CursorMode cursorMode = CursorMode.Auto;
	private static Texture2D lastCursor;

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

	//! Changes the mouse cursor back to default.
	public static void SetDefaultCursor(){
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

}

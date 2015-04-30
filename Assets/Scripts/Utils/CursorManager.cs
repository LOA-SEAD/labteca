using UnityEngine;
using UnityEngine.UI;
using System.Collections;


public class CursorManager : MonoBehaviour {

	private static bool inDefaultCursor = true;
	public static CursorManager instance;
	public static CursorMode cursorMode = CursorMode.Auto;
	private static Texture2D lastCursor;

	// Use this for initialization
	void Start () {

	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public static void SetNewCursor(Texture2D cursorTexture, Vector2 hotSpot){

		Cursor.SetCursor(cursorTexture, hotSpot, CursorManager.cursorMode);
		inDefaultCursor = false;
		lastCursor = cursorTexture;
	}

	public static void SetDefaultCursor(){
		Cursor.SetCursor (null, Vector2.zero, CursorManager.cursorMode);
		inDefaultCursor = true;
		lastCursor = null;
	}

	public static bool UsingDefaultCursor(){
		return inDefaultCursor;
	}

	public static Texture2D GetLastCursor(){
		return lastCursor;
	}

}

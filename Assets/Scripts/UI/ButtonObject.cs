using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ButtonObject : MonoBehaviour {
    
    public Texture2D cursorTexture;
    
    public Vector2 hotSpot = Vector2.zero;
    
    private string objectName = "";

	public bool changeIconeOut = false;
	public bool changeIconeEnter = false;
	public bool changeIconIfOnlyDefault = true;

    public void Awake()
    {
        GetComponent<Image>().enabled = false;  // hide the button image while game is running
        objectName = GetComponentInParent<Transform>().parent.transform.parent.name;
    }

    public string getBtnObjName()
    {
        return this.objectName;
    }

    public void cursorEnter()
    {
		if (changeIconeEnter){

			if((changeIconIfOnlyDefault && CursorManager.UsingDefaultCursor()) || !changeIconIfOnlyDefault)
				CursorManager.SetNewCursor(cursorTexture, hotSpot);
		}
    }

    public void cursorExit()
    {
		if (changeIconeOut || CursorManager.GetLastCursor() == cursorTexture) {
			CursorManager.SetDefaultCursor();
		}
    }

}

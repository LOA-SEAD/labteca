using UnityEngine;
using UnityEngine.UI;
using System.Collections;
//! Allows any object inside a 'state' to become interactable.
/*! Contains functions that can be used by objects that are 'buttons'.
 *  To be used, this usually is attached to Event Trigger.
 */
public class ButtonObject : MonoBehaviour {
    
    public Texture2D cursorTexture;         /*!< The cursor that will be used when mouse hover. */

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

    //! Get object's name that this script is linked to.
    public string getBtnObjName()
    {
        return this.objectName;
    }

    //! Set cursor when mouse hover.
    public void cursorEnter()
    {
		if (changeIconeEnter){

			if((changeIconIfOnlyDefault && CursorManager.UsingDefaultCursor()) || !changeIconIfOnlyDefault)
				CursorManager.SetToInteractiveCursor(cursorTexture, hotSpot);
		}
    }

    //! Set cursor when mouse leaves.
    public void cursorExit()
	{
		if (!changeIconIfOnlyDefault) {
			Debug.Log ("Settando cursor anterior 0");
			if (changeIconeOut) {
				Debug.Log ("Settando cursor anterior");
				CursorManager.SetToPreviousCursor ();
			}
		}
    }

}

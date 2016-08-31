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
	public string hoverName;

	public bool changeIconeOut = false;
	public bool changeIconeEnter = false;
	public bool changeIconIfOnlyDefault = true;
	private bool hovering = false;

	private RectTransform hover;

	void Start(){
		hover = GameObject.Find ("GameController").GetComponent<HUDController>().hover;
		if (GetComponentInParent<WorkbenchInteractive> () != null)
			hoverName = GetComponentInParent<WorkbenchInteractive> ().hoverName;
	}

	void Update(){
		if (hoverName.Length == 0 && GetComponentInParent<WorkbenchInteractive> () != null) {
			hoverName = GetComponentInParent<WorkbenchInteractive> ().hoverName;
		}
		if (hovering) {
			if(CursorManager.GetCurrentState()==MouseState.ms_interacting)
				cursorExit();
			else
				hover.position = Input.mousePosition + new Vector3 (7f, 3f);
		}
	}

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
		if (CursorManager.GetCurrentState () != MouseState.ms_interacting&&GameObject.Find("GameController").GetComponent<GameController>().currentStateIndex!=0) {
			if (hoverName.Length != 0) {
				hover.gameObject.SetActive (true);
				hover.GetComponentInChildren<Text> ().text = hoverName;
				float y = Input.mousePosition.x > 0 ? 180f : 0f;
				hover.rotation = Quaternion.Euler (0f, y, 0f);
				hover.GetComponentInChildren<Text> ().rectTransform.localRotation = Quaternion.Euler (0f, y, 0f);
				hovering = true;
			}
			if (changeIconeEnter) {
				if ((changeIconIfOnlyDefault && CursorManager.UsingDefaultCursor ()) || !changeIconIfOnlyDefault)
					CursorManager.SetToInteractiveCursor (cursorTexture, hotSpot);
			}
		}
    }

    //! Set cursor when mouse leaves.
    public void cursorExit()
	{
		if(hovering)
			hover.gameObject.SetActive(hovering=false);
		if (!changeIconIfOnlyDefault) {
			if (changeIconeOut) {
				CursorManager.SetToPreviousCursor ();
			}
		}
    }

}

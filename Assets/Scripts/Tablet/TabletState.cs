using UnityEngine;
using System.Collections;

public class TabletState : MonoBehaviour
{
	public virtual TabletStates StateType {
		get{
			return TabletStates.Main;
		}
	}

	private CanvasGroup canvasGroup;

	public CanvasGroup GetCanvasGroup(){
		return this.GetComponent<CanvasGroup> ();
	}

	/*void OnGUI(){
		Event e = Event.current;
		if (this.GetComponent<CanvasGroup> ().alpha == 1f && StateType == TabletStates.Main) {
			if (Input.GetKeyDown (KeyCode.Escape)) {
				GameObject.Find ("GameController").GetComponent<HUDController>().CallTabletTrigger();
			}
		}
	}*/

	// Use this for initialization
	void Start ()
	{
		canvasGroup = this.GetComponent<CanvasGroup> ();
	}

	/*void OnGUI(){
		Event e = Event.current;
		if (this.GetComponent<CanvasGroup> ().alpha == 1f) {
			if (Input.GetKeyDown (KeyCode.Escape)) {
				GameObject.Find("GameController").GetComponent<HUDController>().CallTablet(false);
			}
		}
	}*/
}


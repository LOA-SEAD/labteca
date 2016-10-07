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
	// Use this for initialization
	void Start ()
	{
		canvasGroup = this.GetComponent<CanvasGroup> ();
	}
}


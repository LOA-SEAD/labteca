using UnityEngine;
using System.Collections;

public class TabletState : MonoBehaviour
{
	public TabletStates stateType;
	public CanvasGroup canvasGroup;
	// Use this for initialization
	void Start ()
	{
		canvasGroup = this.GetComponent<CanvasGroup> ();
	}
	
	// Update is called once per frame
	void Update ()
	{
	
	}
}


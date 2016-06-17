using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TabletStateMachine : MonoBehaviour {

	public List<TabletState> states;
	
	// Use this for initialization
	void Start () {
		resetState ();
	}

	public void resetState(){
		goToState ((int)TabletStates.Main);
	}

	public void goToState(int index){
		Debug.Log (index);

		foreach (TabletState ts in states) {
			if((int)ts.stateType!=index){
				ts.GetCanvasGroup().alpha = 0f;
				ts.GetCanvasGroup().blocksRaycasts = false;
				ts.GetCanvasGroup().interactable = false;
			}else{
				ts.GetCanvasGroup().alpha = 1f;
				ts.GetCanvasGroup().blocksRaycasts = true;
				ts.GetCanvasGroup().interactable = true;
			}
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}

public enum TabletStates{
	Main=0,
	ExperimentsMenu,
	Experiments,
	Notes,
	GraphsMenu,
	Graphs
}
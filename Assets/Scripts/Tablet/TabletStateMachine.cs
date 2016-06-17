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
				ts.canvasGroup.alpha = 0f;
				ts.canvasGroup.blocksRaycasts = false;
				ts.canvasGroup.interactable = false;
			}else{
				ts.canvasGroup.alpha = 1f;
				ts.canvasGroup.blocksRaycasts = true;
				ts.canvasGroup.interactable = true;
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
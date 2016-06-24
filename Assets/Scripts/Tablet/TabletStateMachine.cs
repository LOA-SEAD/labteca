﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;


public class TabletStateMachine : MonoBehaviour {

	public List<TabletState> states;
	public HUDController control;
	public Text time;
	
	// Use this for initialization
	void Start () {
		resetState ();
	}

	public void resetState(){
		goToState ((int)TabletStates.Main);
	}

	public void goToState(int index){
		if ((int)TabletStates.Notes == index)
			control.LockKeys (true);
		else
			control.LockKeys (false);

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
		time.text = (System.DateTime.Now.Hour + ":" + System.DateTime.Now.Minute);
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
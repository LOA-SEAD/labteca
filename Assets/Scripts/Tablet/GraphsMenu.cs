using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class GraphsMenu : TabletState {
	public Transform content;
	public Button prefab;

	public override TabletStates StateType {
		get {
			return TabletStates.GraphsMenu;
		}
	}

	void OnGUI(){
		Event e = Event.current;
		if (this.GetComponent<CanvasGroup> ().alpha == 1f) {
			if (Input.GetKeyDown (KeyCode.Escape)) {
				GetComponentInParent<TabletStateMachine>().resetState();
			}
		}
	}

	public void AddButton(string txt, Sprite image){
		GameObject tempItem = Instantiate (prefab.gameObject) as GameObject;
		tempItem.name = "Graph Button";
		tempItem.GetComponentInChildren<Text> ().text = txt;
		tempItem.GetComponent<GraphButton> ().image = image;
		tempItem.transform.SetParent (content.transform, false);
	}

	public void aux(Sprite image){
		GameObject tempItem = Instantiate (prefab.gameObject) as GameObject;
		tempItem.name = "Graph Button";
		tempItem.GetComponentInChildren<Text> ().text = "teste";
		tempItem.transform.SetParent (content.transform, false);
		tempItem.GetComponent<GraphButton> ().image = image;
	}
}

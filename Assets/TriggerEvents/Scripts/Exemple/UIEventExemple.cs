using UnityEngine;
using System.Collections;

public class UIEventExemple : MonoBehaviour {
	
	public string currentTaskDescription;
	public EventController eventController;
	
	
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		
		if(eventController.currentTask != null && !eventController.currentTask.IsFinished()){
			currentTaskDescription = eventController.currentTask.taskDescription;
		}
		else{
			currentTaskDescription = "Evento Concluido";
		}
		
		if(Input.GetKeyDown(KeyCode.R)){
			eventController.ResetEvent();
		}
		
	
	}
	
	void OnGUI(){
		GUI.Label(new Rect(0,0, 500, 20), currentTaskDescription);
	}
}

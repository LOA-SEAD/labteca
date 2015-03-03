using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EventController : MonoBehaviour {
	
	public List<TaskBehaviour> tasks;
	private bool completed = false;
	public TaskBehaviour currentTask;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		
		if(tasks[tasks.Count-1].IsFinished() && !completed){
			CompleteEvent();
		}
		
		bool findCurrentTask = false;
		foreach(TaskBehaviour t in tasks){
			if(!findCurrentTask){
				if(!t.IsFinished()){
					currentTask = t;
					findCurrentTask = true;
				}
			}
		}
	
	}
	
	private void CompleteEvent(){
		
		completed = true;
		
		Debug.Log("You have complete this event");
	}
	
	public void ResetEvent(){
		foreach(TriggerBase t in tasks){
			t.Reset();
		}
	}
	
	
}

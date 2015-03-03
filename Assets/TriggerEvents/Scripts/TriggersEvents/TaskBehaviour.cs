using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TaskBehaviour : TriggerBase {
	
	public string taskDescription;
	private string tempTaskDescription;
	
	
	public TaskBehaviour concorrentTask;
	
	private TaskBehaviour startConcorrentTask;
	
	public List<TaskBehaviour> ignoreTasks;
	public List<TaskBehaviour> resetTasks;

	
	// Use this for initialization
	void Start () {
		tempTaskDescription = taskDescription;
		startConcorrentTask = concorrentTask;
	}
	
	// Update is called once per frame
	void Update () {
		
		if(concorrentTask != null && !concorrentTask.IsFinished()){
			taskDescription = tempTaskDescription +" ou "+ concorrentTask.taskDescription;
			
		}
		else{
			taskDescription = tempTaskDescription;
		}
		
		if(concorrentTask != null && concorrentTask.IsFinished() && concorrentTask.concorrentTask != null && !concorrentTask.concorrentTask.IsFinished()){
			concorrentTask = concorrentTask.concorrentTask;
		}

	
	}
	
	protected override void OnStart ()
	{
		Debug.Log ("This task start");
	}
	
	protected override void OnReset ()
	{
		concorrentTask = startConcorrentTask;
	}
	protected override void OnCompleted ()
	{
			Debug.Log ("This task is finished");
			foreach(TaskBehaviour t in ignoreTasks){
				t.ForceFinish();
			}
			foreach(TaskBehaviour t in resetTasks){
				t.Reset();
			}
	}
}

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public abstract class TriggerBase : MonoBehaviour {
	
	protected bool starded;
	protected bool finished;
	
	public List<TriggerBase> dependencesTriggers;
	
	

	// Use this for initialization
	void Start () {
		Reset();
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	public void CompleteEvent(){
		if(starded == true && finished == false){
			finished = true;
			OnCompleted();
		}
		else {
			Debug.LogWarning("This event not started");
		}
		
	}
	
	public void ForceFinish(){
		finished = true;
	}
	
	public void StartEvent(){
		if(finished == false && !haveDependences()){
			starded = true;
			OnStart();
		}
		else {
			Debug.LogWarning("This event have been finished or have dependences");
		}
		
	}
	
	public bool haveDependences(){
		bool have = false;
		
		foreach(TriggerBase t in dependencesTriggers){
			if(t.IsFinished() == false)
				have = true;
		}
		
		return have;
		
	}
	
	public bool IsFinished(){
		return finished;
	}
	
	protected abstract void OnCompleted();
	
	protected abstract void OnStart();
	
	protected abstract void OnReset();
	
	public void Reset(){
		starded = false;
		finished = false;
		OnReset();
	}
	
}

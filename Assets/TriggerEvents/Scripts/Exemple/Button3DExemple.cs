using UnityEngine;
using System.Collections;

public class Button3DExemple : MonoBehaviour {
	
	public TaskBehaviour triggerTask;
	
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	public void OnClick(){
		//Debug.Log("Clicked "+gameObject.name);
		triggerTask.StartEvent();
		triggerTask.CompleteEvent();
	}
}

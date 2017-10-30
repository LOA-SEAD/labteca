using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimedInputObject : MonoBehaviour, TimedInputHandler {

	// Use this for initialization
	void Start () {
        GetComponent<Renderer>().material.color = Color.white;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void HandleTimedInput()
    {
        if (GetComponent<Renderer>().material.color == Color.blue)
            GetComponent<Renderer>().material.color = Color.white;
        else
            GetComponent<Renderer>().material.color = Color.blue;
    }
}

using UnityEngine;
using System.Collections;

public class ResetButton : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void MsgMouseDown()
	{
		Application.LoadLevel (Application.loadedLevel);
	}
}

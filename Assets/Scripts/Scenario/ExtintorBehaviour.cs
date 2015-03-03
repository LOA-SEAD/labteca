using UnityEngine;
using System.Collections;

public class ExtintorBehaviour : InteractObjectBase {

	public ParticleSystem smoke;

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public override void Interact ()
	{
		smoke.Play();
	}
}

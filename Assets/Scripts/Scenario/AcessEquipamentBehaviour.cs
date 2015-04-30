using UnityEngine;
using System.Collections;

public class AcessEquipamentBehaviour : InteractObjectBase {

	public GameStateBase targetEquipament;
	private bool callInteract;
	public float delay = 0.5f;
	private float currentDelay;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

		if(callInteract){
			currentDelay += Time.deltaTime;

			if(currentDelay > delay){
				callInteract = false;
				targetEquipament.StartState();
				currentDelay = 0;

			}
		}
	
	}

	public override void Interact ()
	{
		FadeScript.instance.ShowFade();
		callInteract = true;
		HudText.SetText("");
		GetComponent<BoxCollider>().enabled = false;


	}
}

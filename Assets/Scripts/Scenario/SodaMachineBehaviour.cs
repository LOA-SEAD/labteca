using UnityEngine;
using System.Collections;

public class SodaMachineBehaviour : InteractObjectBase {

	public Transform sodaPrefab;
	public Transform pivotMachine;
	public int totalItens;
	private float delayToInteract;
	private bool callInteract;


	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

		if(callInteract){
			delayToInteract += Time.deltaTime;
			if(delayToInteract > 1.5f){
				delayToInteract = 0;
				callInteract = false;
				SpawnItem();
			}
		}
	
	}

	public override void Interact ()
	{
		if(totalItens > 0){
			callInteract = true;
		}
		
	}

	private void SpawnItem(){
		Instantiate(sodaPrefab.gameObject, pivotMachine.position, sodaPrefab.rotation);
		totalItens--;

	}
}

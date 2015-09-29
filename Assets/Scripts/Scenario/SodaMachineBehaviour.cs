using UnityEngine;
using System.Collections;

//! Script that controls the Soda Machine.
/*! Allows the Soda Machine to drop a can. */
public class SodaMachineBehaviour : InteractObjectBase {

	public Transform sodaPrefab;        /*!< Prefab of can. */
    public Transform pivotMachine;      /*!< Transform for pivot. */
	public int totalItens;              /*!< Integer for total items. */
	public AudioSource machineSound; 	/*!< Sound of using the machine */
	private float delayToInteract;
	private bool callInteract;
	
	void Update () {
		if(callInteract){
			delayToInteract += Time.deltaTime;
			if(delayToInteract > 1.5f){
				delayToInteract = 0;
				callInteract = false;
				machineSound.Play();
				SpawnItem();
			}
		}
	}

    //! Interacts if there is still items (soda can) available.
	public override void Interact ()
	{
		if(totalItens > 0){
			callInteract = true;
		}
		
	}

    //! Spawn an item (soda can).
	private void SpawnItem(){
		Instantiate(sodaPrefab.gameObject, pivotMachine.position, sodaPrefab.rotation);
		totalItens--;

	}
}

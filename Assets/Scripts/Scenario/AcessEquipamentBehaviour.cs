using UnityEngine;
using System.Collections;

//! Allows any Equipment to be interactable.
//TODO: refatorar o nome: access e equipment
public class AcessEquipamentBehaviour : InteractObjectBase {

	public GameStateBase targetEquipament;      /*!< GameStateBase for equipment. */
	private bool callInteract;
    public float delay = 0.5f;                  /*!< Float delay time. */
	private float currentDelay;

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

    //! Does the interaction.
    /*! Fades the camera and starts the equipment state. */
	public override void Interact ()
	{
		FadeScript.instance.ShowFade();
		callInteract = true;
		HudText.SetText("");
		GetComponent<BoxCollider>().enabled = false;


	}
}

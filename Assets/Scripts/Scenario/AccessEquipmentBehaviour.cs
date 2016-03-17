using UnityEngine;
using System.Collections;

//! Allows any Equipment to be interactable.
public class AccessEquipmentBehaviour : InteractObjectBase {

	public GameStateBase targetEquipment;      /*!< GameStateBase for equipment. */
	private bool callInteract;
    public float delay = 0.5f;                  /*!< Float delay time. */
	private float currentDelay;
	public string informationText;

	void Update () {
		if(callInteract){
			currentDelay += Time.deltaTime;

			if(currentDelay > delay){
				callInteract = false;
				targetEquipment.StartState();
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

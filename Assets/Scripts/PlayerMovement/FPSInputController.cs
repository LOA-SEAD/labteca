using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

// Require a character controller to be attached to the same game object
[RequireComponent(typeof(CharacterMotor))]
[AddComponentMenu("Character/FPS Input Controller")]

//! First Person Input controller by the Player.
/*! How the Player interact with the environment. */
public class FPSInputController : MonoBehaviour
{
    private CharacterMotor motor;
	public string interactText;			/*!< String value that shows in the screen when an interactable object is near. */
	public float distanceToInteract;    /*!< Float value that defines the interactable player's distance. */
    public float delayInteract;         /*!< Float value to delay the interaction. */
	private float currentDelay;
	private bool inInteraction;
	private Vector3 lastPosition;
	private RaycastHit lastHit;

	private GameController gameController;
	private HUDController hudController;

	public bool keysLocked = false;
	public Camera mainCamera;

	void Start(){
		//if interactText is null, sets the text to a default one
		if (interactText == "")
			interactText = "Pressione \"E\" Para Interagir";

		Ray cameraRay = Camera.main.ScreenPointToRay(new Vector3(Screen.width/2, Screen.height/2,  200));
		Physics.Raycast (cameraRay, out lastHit, Mathf.Infinity);

		gameController = GameObject.Find ("GameController").GetComponent<GameController> ();
		hudController = gameController.GetComponent<HUDController> ();
	}

    void Awake()
    {
        motor = GetComponent<CharacterMotor>();
    }

    void Update()
    {
		if(inInteraction){
			motor.movement.velocity = Vector3.zero;
			transform.position = lastPosition;
			currentDelay += Time.deltaTime;
			if(currentDelay > delayInteract){
				currentDelay = 0;
				inInteraction = false;
			}
			return;
		}

        // Get the input vector from kayboard or analog stick
        Vector3 directionVector = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));

        if (directionVector != Vector3.zero)
        {
            // Get the length of the directon vector and then normalize it
            // Dividing by the length is cheaper than normalizing when we already have the length anyway
            float directionLength = directionVector.magnitude;
            directionVector = directionVector / directionLength;

            // Make sure the length is no bigger than 1
            directionLength = Mathf.Min(1.0f, directionLength);

            // Make the input vector more sensitive towards the extremes and less sensitive in the middle
            // This makes it easier to control slow speeds when using analog sticks
            directionLength = directionLength * directionLength;

            // Multiply the normalized direction vector by the modified length
            directionVector = directionVector * directionLength;
        }

        // Apply the direction to the CharacterMotor
		if (!keysLocked) {
			motor.inputMoveDirection = transform.rotation * directionVector;
			//motor.inputJump = Input.GetButton ("Jump");
		}

        // Implements Raycast to get which object is being Hit and how to interact with it.
		Ray cameraRay = Camera.main.ScreenPointToRay(new Vector3(Screen.width/2, Screen.height/2,  Mathf.Infinity));
		RaycastHit hitInfo;


		if (Physics.Raycast (cameraRay, out hitInfo, Mathf.Infinity)) {
			bool nameReset = false;
			if (hitInfo.collider.GetComponent<AccessEquipmentBehaviour> ()) {
				if(hitInfo.collider.gameObject.Equals(lastHit.collider.gameObject)){
					nameReset = true;
					hitInfo.collider.GetComponent<AccessEquipmentBehaviour> ().SetTrigger(true);
					HudText.SetText (hitInfo.collider.GetComponent<AccessEquipmentBehaviour> ().equipName);
					if(hitInfo.distance>3){
						hitInfo.collider.GetComponent<AccessEquipmentBehaviour> ().setCanvasAlpha(1f);
					}
					else{
						hitInfo.collider.GetComponent<AccessEquipmentBehaviour> ().setCanvasAlpha(0.5f*hitInfo.distance-0.5f);
					}
				}else
					if(lastHit.collider.GetComponent<AccessEquipmentBehaviour> ()!=null)
						lastHit.collider.GetComponent<AccessEquipmentBehaviour> ().SetTrigger(false);
			}else if(lastHit.collider.GetComponent<AccessEquipmentBehaviour> ()!=null)
				lastHit.collider.GetComponent<AccessEquipmentBehaviour> ().SetTrigger(false);
			if (hitInfo.collider.GetComponent<InteractObjectBase> () && hitInfo.distance <= distanceToInteract) {
				if (Input.GetKeyDown (KeyCode.E)) {
					hitInfo.collider.GetComponent<InteractObjectBase> ().Interact ();
					if (!hitInfo.collider.GetComponent<AccessEquipmentBehaviour> ())
						gameObject.GetComponent<PlayerAnimation> ().PlayInteractAnimation ();
					inInteraction = true;
					lastPosition = transform.position;
				}
				HudText.SetText (interactText);
			} else {
				if(!nameReset)
				HudText.EraseText ();
			}
			//show information about the object

			lastHit=hitInfo;
		}


		//State Machine for the different uses of input
		//HUD components:
		/*if (Input.GetKeyDown (KeyCode.Escape)) {
			hudController.menu.Pause ();
		}
		if(Input.GetKeyDown(hudController.journalKey)&&!hudController.lockKey){
			hudController.CallTabletTrigger();
		}
		if ((Input.GetKeyDown (hudController.inventoryKey))&&!hudController.lockKey) {
			hudController.CallInventoryTrigger();
		}
		if((Input.GetKeyDown(hudController.mapKey))&&!hudController.lockKey){
			hudController.CallMapTrigger();
		}

		//End of states
		if (gameController.GetCurrentState () == gameController.gameStates [0]) {
		
		}*/
    }

	public void LockKeys() {
		keysLocked = true;
		this.gameObject.GetComponent<CharacterMotor>().enabled = false;
		this.enabled = false;
		this.gameObject.GetComponent<MouseLook> ().enabled = false;
		mainCamera.gameObject.GetComponent<MouseLook> ().enabled = false;
		GameObject.Find ("GameController").GetComponent<HUDController> ().LockKeys (true);
	}

	public void UnlockKeys() {
		Debug.Log ("Unlocking keys.");
		keysLocked = false;
		this.enabled = true;
		this.gameObject.GetComponent<CharacterMotor>().enabled = true;
		this.gameObject.GetComponent<MouseLook> ().enabled = true;
		mainCamera.gameObject.GetComponent<MouseLook> ().enabled = true;
		GameObject.Find ("GameController").GetComponent<HUDController> ().LockKeys (false);
	}
}
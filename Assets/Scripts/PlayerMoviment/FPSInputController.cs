using UnityEngine;
using System.Collections;
using System.Collections.Generic;

// Require a character controller to be attached to the same game object
[RequireComponent(typeof(CharacterMotor))]
[AddComponentMenu("Character/FPS Input Controller")]



public class FPSInputController : MonoBehaviour
{
    private CharacterMotor motor;
	public float distanceToInteract;
	public float delayInteract;
	private float currentDelay;
	private bool inInteraction;
	private Vector3 lastPosition;

    // Use this for initialization
    void Awake()
    {
        motor = GetComponent<CharacterMotor>();
    }

    // Update is called once per frame
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
        motor.inputMoveDirection = transform.rotation * directionVector;
        motor.inputJump = Input.GetButton("Jump");

		Ray cameraRay = Camera.main.ScreenPointToRay(new Vector3(Screen.width/2, Screen.height/2,  Mathf.Infinity));
		RaycastHit hitInfo;

		if(Physics.Raycast(cameraRay, out hitInfo, distanceToInteract)){
			if(hitInfo.collider.GetComponent<InteractObjectBase>()){
				if(Input.GetKeyDown(KeyCode.E)){
					hitInfo.collider.GetComponent<InteractObjectBase>().Interact();
					gameObject.GetComponent<PlayerAnimation>().PlayInteractAnimation();
					inInteraction = true;
					lastPosition = transform.position;

				}
				HudText.SetText("Pressione \"E\" Para Interagir");

			}
		}
		else{
			HudText.SetText("");
		}
    }
}
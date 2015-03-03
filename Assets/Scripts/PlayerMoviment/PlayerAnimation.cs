using UnityEngine;
using System.Collections;

public class PlayerAnimation : MonoBehaviour {

	public Animator playerAnimator;
	public Animator cameraAnimator;
	public Animator handAnimator;

	private bool inWalkAnim = false;
	private CharacterMotor characterMotor;

	// Use this for initialization
	void Start () {
		characterMotor = GetComponent<CharacterMotor>();

	
	}
	
	// Update is called once per frame
	void Update () {

		if(characterMotor.movement.velocity.x != 0 || characterMotor.movement.velocity.z != 0)
			inWalkAnim = true;
		else
			inWalkAnim = false;

	
		playerAnimator.SetBool("walk", inWalkAnim);
		cameraAnimator.SetBool("walk", inWalkAnim);

	
	}

	public void PlayInteractAnimation(){
		handAnimator.SetTrigger("Interact");
	}
}

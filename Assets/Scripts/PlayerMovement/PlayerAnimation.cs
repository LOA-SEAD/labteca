using UnityEngine;
using System.Collections;

//! Controls all animations involving the Player.
/*! For now there are three animators being used here: Player, Camera and Hand. */
public class PlayerAnimation : MonoBehaviour {

	public Animator playerAnimator;     /*!< Animator component of Player. */
    public Animator cameraAnimator;     /*!< Animator component of Camera. */
    public Animator handAnimator;       /*!< Animator component of Hand. */

	private bool inWalkAnim = false;
	private CharacterMotor characterMotor;

	void Start () {
		characterMotor = GetComponent<CharacterMotor>();
	}
	
	void Update () {

		if(characterMotor.movement.velocity.x != 0 || characterMotor.movement.velocity.z != 0)
			inWalkAnim = true;
		else
			inWalkAnim = false;

		playerAnimator.SetBool("walk", inWalkAnim);
		//cameraAnimator.SetBool("walk", inWalkAnim);	
	}

    //! When Player interacts with something, the Hand animation is played.
	public void PlayInteractAnimation(){
		handAnimator.SetTrigger("Interact");
	}
}

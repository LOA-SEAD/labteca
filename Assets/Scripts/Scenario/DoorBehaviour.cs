using UnityEngine;
using System.Collections;

//! Animation of any door opening and closing.
/*! When the Player interacts with any door, there is an Animation of it opening, it also closes if there is another interaction 
 *  or closes automatically after a few seconds. */
public class DoorBehaviour : InteractObjectBase {

	private bool isClosed = true;           
	private Animator doorAnimator;
	public bool automaticClose;             /*!< Bool to set automatic close. */
    public float timeToClose;               /*!< Float to set time to close. */
	private float currentTimeToClose;
	public DoorBehaviour closeOtherDoor; 

	public AudioSource doorClosingSound;
	public AudioSource doorOpeningSound;

	void Start () {
		base.Start ();
		doorAnimator = GetComponent<Animator> ();
	
	}
	
	void Update () {
		base.Update ();
		if(automaticClose && !isClosed){
			currentTimeToClose += Time.deltaTime;
			if(currentTimeToClose > timeToClose){
				currentTimeToClose = 0;
				Interact();
			}
		}
	}

    //! Actions done when there is interaction with the door.
    /*! If closed, it opens, if opened, it closes. */
	public override void Interact ()
	{
		if(isClosed){
			this.Open();
			if(closeOtherDoor != null && !closeOtherDoor.IsClosed()){
				closeOtherDoor.Interact();
			}
		}
		else {
			this.Close();
		}
	}

    //! Bool to check if the door is closed.
	public bool IsClosed(){
		return isClosed;
	}

    //! Close the door.
	public void Close(){

		if(!isClosed){
			doorAnimator.SetTrigger("Close");
			doorClosingSound.Play();
			isClosed = true;
		}
		
	}

    //! Open the door.
	public void Open(){
		if(isClosed){
			doorAnimator.SetTrigger("Open");
			doorOpeningSound.Play();
			isClosed = false;
		}
	
	}
}

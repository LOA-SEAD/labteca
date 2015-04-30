using UnityEngine;
using System.Collections;

public class DoorBehaviour : InteractObjectBase {

	private bool isClosed = true;
	private Animator doorAnimator;
	public bool automaticClose;
	public float timeToClose;
	private float currentTimeToClose;
	public DoorBehaviour closeOtherDoor;

	// Use this for initialization
	void Start () {
		base.Start ();
		doorAnimator = GetComponent<Animator> ();
	
	}
	
	// Update is called once per frame
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

	public override void Interact ()
	{
		if(isClosed){
			doorAnimator.SetTrigger("Open");
			isClosed = false;
			if(closeOtherDoor != null && !closeOtherDoor.IsClosed()){
				closeOtherDoor.Interact();
			}
		}
		else {
			doorAnimator.SetTrigger("Close");
			isClosed = true;
		}

	}

	public bool IsClosed(){
		return isClosed;
	}

	public void Close(){

		if(!isClosed){
			doorAnimator.SetTrigger("Close");
			isClosed = true;
		}
		
	}

	public void Open(){
		if(isClosed){
			doorAnimator.SetTrigger("Open");
			isClosed = false;
		}
	
	}
}

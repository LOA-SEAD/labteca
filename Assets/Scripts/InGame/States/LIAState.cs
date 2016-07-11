using UnityEngine;
using System.Collections;

public class LIAState : GameStateBase {
	
	// camera and interactive area
	public Camera cameraState;                  /*!< Camera for this state. */
	public GameObject interactBox;              /*!< BoxCollider that allows the Player to enter this state. */
	public Light LIALight;
	private bool up;
	public void Start () {
		cameraState.gameObject.SetActive(false);
	}
	
	protected override void UpdateState ()
	{
		
	}
	
	void Update(){
		base.Update();
		
		if(!canRun)
			return;

		if (up&&LIALight.intensity<0.6f) {
			LIALight.intensity += Time.deltaTime / 4;
		} else {
			up=false;
			if(LIALight.intensity>0.1f)
				LIALight.intensity -= Time.deltaTime / 4;
			else
				up=true;
		}

		//Pressing Esc will exit the state
		if(Input.GetKeyDown(KeyCode.Escape)){
			interactBox.GetComponent<BoxCollider>().enabled = true;
			gameController.ChangeState(0);
			FadeScript.instance.ShowFade();
		}
	}
	
	//! Actions for when the State starts.
	/*! Set the Camera inside the state to be Active, overlaying the Main Camera used at InGameState,
     * close all dialogs that might be enabled. */
	public override void OnStartRun ()
	{
		cameraState.gameObject.SetActive(true);
		cameraState.depth = 2;
		HudText.SetText("");
	}
	
	//! Actions for when the State ends.
	/*! Disable the Camera inside the state, deactivate. */
	public override void OnStopRun ()
	{
		LIALight.intensity = 0f;
		cameraState.depth = -1;
		cameraState.gameObject.SetActive(false);
	}
}

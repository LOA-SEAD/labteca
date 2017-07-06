using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
//! State for Precision Scale.
/*! This state has all the behaviour that controls the precision scale and it's UI.
 */
public  class ScaleState : GameStateBase {

    // camera and interactive area
    //public Camera cameraState;                  /*!< Camera for this state. */

	public EquipmentControllerBase equipmentController;	/*!< Equipment controller for this state */

	public void Start () {
		cameraState.gameObject.SetActive(false);
		equipmentController = this.GetComponentInChildren<EquipmentControllerBase> ();
	}
	
	protected override void UpdateState ()
	{

	}

	void Update(){
		base.Update();

		if(!canRun)
			return;

		//Pressing Esc will exit the state
		if(Input.GetKeyDown(KeyCode.Escape)){
			ExitState();
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
		GetComponentInParent<WorkBench>().OnStartRun ();

	}

    //! Actions for when the State ends.
    /*! Disable the Camera inside the state, deactivate. */
	public override void OnStopRun ()
	{
        cameraState.depth = -1;
        cameraState.gameObject.SetActive(false);
		GetComponentInParent<WorkBench> ().OnStopRun ();

	}

	public override void ExitState(){
		GetComponentInParent<WorkBench>().OnStopRun();
		interactBox.GetComponent<BoxCollider>().enabled = true;
		gameController.ChangeState(0);
	}

	public override EquipmentControllerBase GetEquipmentController () {
		return base.GetEquipmentController ();
	}

}

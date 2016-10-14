using UnityEngine;
using System.Collections;

//! Base Class for any Game State to work as a State inside the State Machine.
/*! This class is inherited by any State and has the basic concepts to become a part of the State Machine,
 *  it has three states: Start Run, Is Running, Stop Run. Also two funcions that can be used to call other
 *  methods when state is starting: 'OnStartRun' and when state is stopping: 'OnStopRun'.
 */
public abstract class GameStateBase : MonoBehaviour {

	public GameObject interactBox;
	protected bool canRun;
	public bool CanRun{set{ canRun = value; }}
	protected int indexState;
	protected GameController gameController;
	public Camera cameraState;

	private Vector3 worldLocation;
	public Vector3 WorldLocation{
		get{
			return this.transform.position;
		}
	}
	
	protected abstract void UpdateState();

	protected void Update(){
		if(!canRun){
			return;
		}
		else{
			UpdateState();
		}
	}

    //! Set GameController for the State.
    /*! Game Controller gets each State on it's State list and add the index number to each one of them. */
    public void SetGameController(GameController gc, int index){
		gameController = gc;
		indexState = index;
	}

    //! Returns the current State condition.
	public bool IsRunning(){
		return canRun;
	}

    //! Start running the State.
	public void StartRun(){
		if (interactBox != null)
			interactBox.SetActive (false);
		if (!(this is InGameState)) {
			gameController.GetComponent<HUDController> ().inventoryLocked = true;
			gameController.GetComponent<HUDController> ().mapLocked = true;
		}
		OnStartRun();
	}

    //! Stop running the State.
	public void StopRun(){
		GameObject.Find ("GameController").GetComponent<HUDController> ().hover.gameObject.SetActive (false);
		if (interactBox != null)
			interactBox.SetActive (true);
		gameController.GetComponent<HUDController> ().inventoryLocked = false;
		gameController.GetComponent<HUDController> ().mapLocked = false;
		gameController.GetComponent<HUDController> ().lockKey = false;
		GameObject.Find ("Tablet").GetComponent<TabletStateMachine> ().resetState ();
		canRun = false;
		OnStopRun();
	}

    //! Interface to run other methods once the State start running.
	public abstract void OnStartRun();

    //! Interface to run other methods once the State is stopping.
	public abstract void OnStopRun();

	public abstract void ExitState ();

    //! Start the State.
	public void StartState(){
		gameController.GetComponent<HUDController> ().CallInventory (true);
		gameController.ChangeState(indexState);
	}

	public virtual EquipmentControllerBase GetEquipmentController () {
		return null;
	}

}

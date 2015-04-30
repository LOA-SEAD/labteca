using UnityEngine;
using System.Collections;

public abstract class GameStateBase : MonoBehaviour {

	protected bool canRun;
	private int indexState;
	protected GameController gameController;

	// Use this for initialization
	public void Start () {
	
	}
	
	// Update is called once per frame
	protected abstract void UpdateState();


	protected void Update(){
		if(!canRun){
			return;
		}
		else{
			UpdateState();
		}
	}

	public void SetGameController(GameController gc, int index){
		gameController = gc;
		indexState = index;
	}

	public bool IsRunning(){
		return canRun;
	}

	public void StartRun(){
		canRun = true;
		OnStartRun();
	}

	public void StopRun(){
		canRun = false;
		OnStopRun();
	
	}

	public abstract void OnStartRun();

	public abstract void OnStopRun();

	public void StartState(){
		gameController.ChangeState(indexState);
	}

}

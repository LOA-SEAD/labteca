using UnityEngine;
using System.Collections;

public class InGameState : GameStateBase {

	public GameObject player;

	// Use this for initialization
	void Start () {
	
	}
	
	void Update(){
		base.Update();
		
	}

	protected override void UpdateState ()
	{

		
	}

	public override void OnStartRun ()
	{
		player.SetActive(true);
	}

	public override void OnStopRun ()
	{
		player.SetActive(false);
	}
}

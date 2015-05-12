using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameController : MonoBehaviour {

	private CharacterController player;

	public List<GameStateBase> gameStates = new List<GameStateBase>(); 
	private GameStateBase currentGameState;

	public static GameController instance;

	//variaveis para suprir a falta de inventario.
	public bool haveReagentNaCl;
	public int totalBackers;
	public GameObject prefabBacker;
	public Glassware selectedGlassWare;

	//variaveis para suprir a falta de inventario.

	// Use this for initialization
	void Start () {

		selectedGlassWare = prefabBacker.GetComponent<Glassware>();

		player = FindObjectOfType(typeof(CharacterController)) as CharacterController;

		Vector3 positionPlayer = new Vector3(PlayerPrefs.GetFloat("PlayerPosX"), 1, PlayerPrefs.GetFloat("PlayerPosZ"));
		player.transform.position = positionPlayer;

		currentGameState = gameStates[0];

		for(int i=0; i< gameStates.Count; i++){
			gameStates[i].SetGameController(this, i);
		}

		if(GameController.instance == null){
			GameController.instance = this;
		}
	}

	public void ChangeState(int indexState){

		GameStateBase selectState = gameStates[indexState];

		currentGameState.StopRun();
		currentGameState = selectState;
		currentGameState.StartRun();
		
	}

	public void GoToDefaultState(){
		ChangeState(0);
	}
}

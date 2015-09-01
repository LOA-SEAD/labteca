using UnityEngine;
using System.Collections;
using System.Collections.Generic;

//! Game Controller
/*! Script that controls the current state of the game - therefore, it is where the 'State Machine' changes. 
 */
public class GameController : MonoBehaviour {

	private CharacterController player;

	public bool lockPlayerStart;

	public List<GameStateBase> gameStates = new List<GameStateBase>();  /*!< List with all game 'states' of the game. */
	private GameStateBase currentGameState;

	public static GameController instance;

    //TODO: Remover variaveis de 'Suporte', renomear Backer para nome correto: Beaker
	//variaveis para suprir a falta de inventario.
	public bool haveReagentNaCl;
	public int totalBackers;
	public GameObject prefabBacker;
	public Glassware selectedGlassWare;

	//variaveis para suprir a falta de inventario.

    //TODO: No Start esta puxando um Glassware do prefab e jogando para selectedGlassware.
    /*Comentario: Esse glassware teoricamente eh apenas para suprir a falta do inventario, nao sei se estar dentro do GameController eh a melhor
     * solucao, talvez uma comunicacao entre o script do 'state' (ex.: Balanca) com o inventario e objeto selecionado atualmente seja mais facil/intuitivo.
     */
	void Start () {

		selectedGlassWare = prefabBacker.GetComponent<Glassware>();

		player = FindObjectOfType(typeof(CharacterController)) as CharacterController;

		if (lockPlayerStart) {
			Vector3 positionPlayer = new Vector3 (PlayerPrefs.GetFloat ("PlayerPosX"), 1, PlayerPrefs.GetFloat ("PlayerPosZ"));
			player.transform.position = positionPlayer;
		}

		currentGameState = gameStates[0];

		for(int i=0; i< gameStates.Count; i++){
			gameStates[i].SetGameController(this, i);
		}

		if(GameController.instance == null){
			GameController.instance = this;
		}
	}

    //! Change current state to another one.
    /*! Using the index from list of game states, changes current state to another one. */
	public void ChangeState(int indexState){

		GameStateBase selectState = gameStates[indexState];

        // TODO: Esse StopRun() e StartRun() eh meio nebuloso, verificar a real necessidade disso.
		currentGameState.StopRun();
		currentGameState = selectState;
		currentGameState.StartRun();
		
	}

    //! Go back to Default State.
    /*! Changes current state to default state: defined as index 0 from list of game states. */
	public void GoToDefaultState(){
		ChangeState(0);
	}
}

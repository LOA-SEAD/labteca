using UnityEngine;
using System.Collections;
using System.Collections.Generic;

//! Game Controller
/*! Script that controls the current state of the game - therefore, it is where the 'State Machine' changes. 
 */
using UnityEngine.UI;


public class GameController : MonoBehaviour {

	private CharacterController player;

	public bool lockPlayerStart;
	public List<InventoryItem> inventoryItems = new List<InventoryItem> ();

	public List<GameStateBase> gameStates = new List<GameStateBase>();  /*!< List with all game 'states' of the game. */
	private GameStateBase currentGameState;
	public int currentStateIndex;
	public static GameController instance;
	
	public AlertDialogBehaviour alertDialog;

	public CanvasGroup exitButton;
	
	void Start () {

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
		//
		GameObject[] invItem = GameObject.FindGameObjectsWithTag ("InventoryItem");
		for (int i=0; i < invItem.Length; i++)
			inventoryItems.Add (invItem [i].GetComponent<InventoryItem>());
	}

	/// <summary>
	/// Gets the current state.
	/// </summary>
	/// <returns>The current state.</returns>
	public GameStateBase GetCurrentState() {
		return currentGameState;
	}

	/// <summary>
	/// Refreshs the inventory.
	/// </summary>
	public void refreshInventory(){
		foreach (InventoryItem inv in inventoryItems) {
			inv.refreshState();
		}
	}

    //! Change current state to another one.
    /*! Using the index from list of game states, changes current state to another one. */
	public void ChangeState(int indexState){

		GameStateBase selectState = gameStates[indexState];
		FadeScript.instance.FadeOut ();

		currentGameState.StopRun();
		currentGameState = selectState;
		currentStateIndex = indexState;
		refreshInventory ();
		//If not on InGameState, shows exit button
		if (currentStateIndex != 0) {
			exitButton.alpha = 1;
			exitButton.interactable = true;
			exitButton.blocksRaycasts = true;
		}else {
			exitButton.alpha = 0;
			exitButton.interactable = false;
			exitButton.blocksRaycasts = false;
		}
		currentGameState.StartRun();
		//Refreshs action button or changes the current list depending on the state
		switch(indexState){
			case 0:
			case 1:
			case 4:
				GameObject.FindObjectOfType<InventoryManager> ().refreshActionButton();
				break;
			case 2:
				GameObject.FindObjectOfType<InventoryManager> ().changeList(1);
				break;
			case 3:
				GameObject.FindObjectOfType<InventoryManager> ().changeList(2);
				break;
		}
		
	}

	/// <summary>
	/// Goes to the default state
	/// </summary>
	public void GoToDefaultState(){
		ChangeState(0);
	}
	/// <summary>
	/// Exits current state.
	/// </summary>
	public void ExitCurrentState(){
		currentGameState.ExitState ();
	}
	/// <summary>
	/// Calls the Journal Saver.
	/// </summary>
	/// <param name="journalUI">Journal user interface.</param>
	/*public void CallJSaver(JournalUIItem journalUI){
		int expo = GameObject.Find ("Journal").GetComponent<JournalController> ().experimentNumber;
		JournalSaver.AddJournalUIItem (journalUI,expo);
	}*/
	/// <summary>
	/// Sends a alert.
	/// </summary>
	/// <param name="text">Text to be shown.</param>
	public void sendAlert(string text){
		alertDialog.ShowAlert (text);
	}

	public void closeAlert() {
		alertDialog.CloseAlert ();
	}
}
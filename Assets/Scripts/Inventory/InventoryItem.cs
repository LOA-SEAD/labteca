using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class InventoryItem : MonoBehaviour {
	public GameController gameController;
	public Image itemIcon;
	public Text nameText,actionText;
	public Button actionButton, infoButton; 

	//Item being held by the button
	public ItemToInventory itemBeingHeld;


	void Start () {
		gameController = GameObject.Find ("GameController").GetComponent<GameController> ();

		//this.GetComponentInChildren<Button> ().onClick.AddListener(() => callGlassToTable());
	}
	//void Update () {}

	void refreshState(){

	}

	public void infoButtonClick(){

	}

	public void actionButtonClick(){
		int currentState = gameController.currentStateIndex;

		switch (currentState) {
			case 0:
				//açao no inGameState(provavelmente nada)
				break;
			case 1:
				//açoes no precisionScale
				break;
			case 2:
				//açoes no getReagents
				break;
			case 3:
				//açoes no getGlassware
				break;
			default:
				break;
		}
	}

	public void disableButton(){
		actionButton.interactable=false;
		actionText.color = new Color (actionText.color.r, actionText.color.g, actionText.color.b, 128/256f);
	}

	public void HoldItem(ItemToInventory item) {
		itemBeingHeld = item;
	}

	//! Calls the method in the workbench that will put the object on the table
	/*! This method should be used as the onClick effect for the button. It calls the method that will put the
	 * 	item on the table */
	public void CallWorkbenchToTable() {
		//GameObject tempItem = Instantiate (itemBeingHeld.gameObject) as GameObject;
		//gameController.GetCurrentState ().GetComponent<WorkBench> ().PutItemFromInventory (tempItem);
		gameController.GetCurrentState ().GetComponent<WorkBench> ().PutItemFromInventory (itemBeingHeld);
	}

}

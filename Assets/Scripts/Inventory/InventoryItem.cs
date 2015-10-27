using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class InventoryItem : MonoBehaviour {
	public GameController gameController;
	public Image itemIcon;
	public Text nameText,actionText;
	public Button actionButton, infoButton; 
	
	void Start () {
		gameController = GameObject.Find ("GameController").GetComponent<GameController> ();
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

}

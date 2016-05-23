using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class InventoryItem : MonoBehaviour {
	public GameController gameController;
	public Image itemIcon;
	public Text nameText,actionText;
	public Button actionButton, infoButton; 

	//Item being held by the button
	public Compound reagent;
	public ItemToInventory itemBeingHeld;
	public GameObject physicalObject; 


	void Start () {
		refreshState ();
		//this.GetComponentInChildren<Button> ().onClick.AddListener(() => callGlassToTable());
	}
	//void Update () {}

	public void refreshState(){
		GameStateBase currentState = gameController.GetCurrentState();
		if (currentState is InGameState)
			disableButton ();
		else {
			if(currentState.gameObject.GetComponent<WorkBench>()!=null){
				enableButton();
				//actionText.text="Utilizar";
			}else if(currentState is GetInterface){
				GetInterface interfaceTest = currentState as GetInterface;
				if(interfaceTest.isGlassware()&&gameObject.GetComponent<AnyObjectInstantiation>().itemType==ItemType.Glassware){
					enableButton();
					//actionText.text="Remover";
				}
				if(!interfaceTest.isGlassware()&&(gameObject.GetComponent<AnyObjectInstantiation>().itemType==ItemType.Liquids||gameObject.GetComponent<AnyObjectInstantiation>().itemType==ItemType.Solids)){
					enableButton();
					//actionText.text="Remover";
				}
			}
		}
	}

	public void infoButtonClick(){

	}

	public void actionButtonClick(){
		GameStateBase currentState = gameController.GetCurrentState();
		if (currentState.GetComponent<WorkBench> () != null)
			CallWorkbenchToTable ();
		else {
			gameObject.GetComponentInParent<InventoryContent>().removeItemUI(gameObject.GetComponent<ItemStackableBehavior>());
		}


	}

	public void disableButton(){
		/*actionButton.interactable=false;
		actionText.color = new Color (actionText.color.r, actionText.color.g, actionText.color.b, 128/256f);
		actionText.text = "Inativo";*/
	}

	public void enableButton(){
		/*actionButton.interactable=true;
		actionText.color = new Color (actionText.color.r, actionText.color.g, actionText.color.b, 1);*/
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
		//gameController.GetCurrentState ().GetComponent<WorkBench> ().PutItemFromInventory (itemBeingHeld,gameObject.GetComponent<ReagentsBaseClass>());
	}

}

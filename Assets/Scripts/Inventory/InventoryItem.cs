using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class InventoryItem : MonoBehaviour {
	public enum itemType
	{
		SOLID,LIQUID,GLASSWARE,OTHERS
	}
	public Image itemIcon;
	public Text nameText,actionText;
	public Button actionButton, infoButton; 
	
	void Start () {

	}
	//void Update () {}

	void refreshState(){

	}

	public void disableButton(){
		actionButton.interactable=false;
		actionText.color = new Color (actionText.color.r, actionText.color.g, actionText.color.b, 128/256f);
	}
}

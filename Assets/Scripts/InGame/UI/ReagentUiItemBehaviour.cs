using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ReagentUiItemBehaviour : MonoBehaviour {

	public Text nameReagent;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}


	public void SetReagent(string name){

		nameReagent.text = name;
	
	}

	public void AddToInventory(){
		//isso e para suprir a falta do inventario entao tem que alterar aqui depois;
		if(nameReagent.text == "NaCl"){
			GameController.instance.haveReagentNaCl = true;
		}
	}

}

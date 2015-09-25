using UnityEngine;
using System.Collections;
using UnityEngine.UI;

//! Set a reagent and add to the inventory.
/*! */

public class ReagentUiItemBehaviour : MonoBehaviour {

	public Text nameReagent;


	// Use this for initialization
	/*void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}*/


	public void SetReagent(string name){

		nameReagent.text = name;
	
	}

	//TODO: Comentario: isso e para suprir a falta do inventario entao tem que alterar aqui depois; 
	/*! Add the reagent to the inventory (NaCL).*/
	public void AddToInventory(){
		//isso e para suprir a falta do inventario entao tem que alterar aqui depois;
		/*if(nameReagent.text == "NaCl"){
			GameController.instance.haveReagentNaCl = true;
		}*/

		Debug.Log ("Add " + nameReagent.text);
	}

}

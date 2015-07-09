using UnityEngine;
using System.Collections;
using UnityEngine.UI;

//! Set a Glassware and add to inventory.

public class GlasswareUiItemBehaviour : MonoBehaviour {

	public Text nameGlass;
	private Glassware prefabGlassware;

	// Use this for initialization
	/*void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}*/

	public void SetGlass(Glassware glass){

		nameGlass.text = glass.name;

		prefabGlassware = glass;
	
	}
	//TODO: Comentario: isso e para suprir a falta do inventario entao tem que alterar aqui depois; 
	/*! Add the glassware to the inventory and increases the variable (bequer).*/
	public void AddToInventory(){
		//isso e para suprir a falta do inventario entao tem que alterar aqui depois;
		if(nameGlass.text == "Bequer"){
			GameController.instance.totalBackers += 1;
		}
	}



}

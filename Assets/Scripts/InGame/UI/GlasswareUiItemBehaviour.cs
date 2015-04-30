using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class GlasswareUiItemBehaviour : MonoBehaviour {

	public Text nameGlass;
	private Glassware prefabGlassware;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}


	public void SetGlass(Glassware glass){

		nameGlass.text = glass.name;

		prefabGlassware = glass;
	
	}

	public void AddToInventory(){
		//isso e para suprir a falta do inventario entao tem que alterar aqui depois;
		if(nameGlass.text == "Bequer"){
			GameController.instance.totalBackers += 1;
		}
	}



}

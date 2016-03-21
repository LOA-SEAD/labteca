using UnityEngine;
using System.Collections;

//! Controls the spatulas
/*! Defines if the spatulas are being used, how much they are holding
 *	and integrates the interaction boxes. */

public class Spatula : MonoBehaviour {

	private float amountHeld; // Mass being held in the spatule [g]
	
	private bool mouse_spatula;			//State for the spatula ready to be used
	private bool mouse_spatulaHolding;	//State for the spatula being used

	//Interaction boxes
	public UI_Manager uiManager;	/*!< The UI Manager Game Object. */

	public GameObject optionDialogPipeta;       /*!< Dialog. */

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}

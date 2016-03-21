using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

//! Controls the pipette
/*! Defines if the pipettes are being used, how much they are holding
 *	and integrates the interaction boxes. */

public class Pipette : MonoBehaviour {

	private float amountHeld; // Mass being held in the spatule [g]

	private bool mouse_pipette;			//State for the spatula ready to be used
	private bool mouse_pipetteHolding;	//State for the spatula being used
	
	//Interaction boxes
	public UI_Manager uiManager;			// The UI Manager Game Object.
	public GameObject optionDialogPipette;  //

	public bool selectPipeta;
	public float amountSelectedPipeta;
	//public CursorMouse pipetaCursor;
	//public ButtonObject pipetaCursor;
	//public CursorMouse pipetaReagentCursor;
	//public ButtonObject pipetaReagentCursor;
	public Slider pipetaValue;
	public Text pipetaValueText;

	//For the cursor
	public Texture2D cursorTexture;
	public Vector2 hotSpot = Vector2.zero;

	public ReagentsLiquidClass heldInPipette; //Reagent being held by the pipette


	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

	}

}

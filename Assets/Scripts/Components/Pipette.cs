using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

//! Controls the pipette
/*! Defines if the pipettes are being used, how much they are holding
 *	and integrates the interaction boxes. */
//TODO: Needs to add the volumetric pipette
public class Pipette : MonoBehaviour {

	private float amountHeld; // Volume being held by the pipette [ml]


	public UI_Manager uiManager;		// The UI Manager Game Object.

	//Interaction boxes to chose between graduated pipettes or volumetric pipettes
	public GameObject interactionBox;	//Interaction box
	//Interaction box for graduated pipette
	public Slider boxSlider;  			//Interaction box's slider
	public Text pipetteValueText; 		//Text showing the slider's value
	public float volumeSelected;		//Amount selected in the slider
	//Interaction box for volumetric pipette


	//For the cursor
	public Texture2D pipette_CursorTexture;
	public Texture2D filledPipette_CursorTexture;
	public Vector2 hotSpot = Vector2.zero;
	
	public ReagentsLiquidClass reagentInPipette; //Reagent being held by the pipette


	//! Use this for initialization
	void Start () {
		interactionBox.SetActive (false);
	}
	
	//! Update is called once per frame
	void Update () {

	}

	//! Holds the events for when the interactive pipette on the Workbench is clicked
	void OnClick() {
		MouseState currentState = CursorManager.GetCurrentState ();

		switch (currentState) {
		case MouseState.ms_default: //Default -> Pipette: prepares the pipette for use
			CursorManager.SetMouseState(MouseState.ms_pipette);
			CursorManager.SetNewCursor(pipette_CursorTexture, hotSpot);
			break;
		case MouseState.ms_pipette: //Pipette -> Pipette: put back the pipette
			CursorManager.SetMouseState(MouseState.ms_default);
			CursorManager.SetDefaultCursor();
			break;
		case MouseState.ms_filledPipette: // Filled Pipette -> Pipette: nothing
			break;
		case MouseState.ms_spatula: // Spatula -> Piepette: nothing
			break;
		case MouseState.ms_filledSpatula: // Filled Spatula -> Pipette: nothing
			break;
		case MouseState.ms_washBottle: // Wash Bottle -> Pipette: nothing
			break;
		case MouseState.ms_glassStick:
			break;
		case MouseState.ms_usingTool:  // Unable to click somewhere else
			break;
		}
	}

	//! Is called in the event of an empty pipette clicking on a liquid reagent
	void FillPipette() {
		/*
		 * CODE FILLING THE PIPETTE
	 	*/

		CursorManager.SetMouseState (MouseState.ms_filledPipette);
		CursorManager.SetNewCursor (filledPipette_CursorTexture, hotSpot);
	}

	//! Close the interaction box
	public void CloseInteractionBox(){
		interactionBox.SetActive(false);
	}

	public void OpenInteractionBox() {
		interactionBox.SetActive (true);
		//CursorManager.SetDefaultCursor ();
		/*
		 * DEFINE HOW TO BLOCK CLICKS OUTSIDE 
		 */
	}

	//! Set value of volume currently set by the slider.
	public void VolumeOnSlider(){
		volumeSelected = boxSlider.value;
		pipetteValueText.text = volumeSelected.ToString ();
	}

	//! Use the pipette to hold the selected volume.
	public void SetVolumeInPipette(){
		CloseInteractionBox ();

		if (volumeSelected > 0.0f) {
			//TODO: Keeping this for checking. Will be removed as soon as the Glassware part is implemented
			/*if (!(lastItemSelected.GetComponent<Glassware> () == null)) //Only removes from the last selected object if it's a glassware
				lastItemSelected.GetComponent<Glassware>().RemoveLiquid (amountSelectedPipeta);*/
			CursorManager.SetMouseState (MouseState.ms_filledPipette);//pipetaReagentCursor.CursorEnter ();
			CursorManager.SetNewCursor (filledPipette_CursorTexture, hotSpot);

		}
	}

}

using UnityEngine;
using System.Collections;

//! Base class for pipettes
//  Holds the basic values for pipettes
public abstract class Pipette : WorkbenchInteractive {

	public float volumeHeld;			//Volume being held by the pipette [ml]
	protected float maxVolume;			//Max volume the pipette can hold [ml]
	protected float pipetteError; 		//Error associated with with pipette [ml]

	public UI_Manager uiManager;		// The UI Manager Game Object.

	public Compound reagentInPipette; //Reagent being held by the pipette
	
	public Glassware interactingGlassware;		 //Glassware which the pipette is interacting with
	public Compound interactingReagent; //Reagent which the pipette is interacting with
	
	//For the cursor
	public Texture2D pipette_CursorTexture;
	public Texture2D filledPipette_CursorTexture;
	public Vector2 hotSpot = Vector2.zero;

	public abstract void OnStartRun();
	public abstract void OnStopRun ();

	//! Close the interaction box
	public abstract void CloseInteractionBox ();

	//! Use the pipette to hold the selected volume.
	public abstract void FillPipette ();
	//! Unloads the pipette into a proper vessel
	public abstract void UnfillPipette ();
}

using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

//! Player interaction with the Glassware.
/*! Contains seven methods that makes the interaction with the glassware
* add and remove liquids and solids.
*/

public class Glassware : ItemToInventory 
{
	public float maxVolume;			//Maximum volume capacity of the glassware [ml or cm]
	public float currentVolume;		//Current volume used
	public float mass;				//Mass of the glassware itself [g]
	public float totalMass;
	//public float uncalibrateVolume; ?
	public bool precisionGlass;		//The glassware is a precision one


	//Mesh of liquids and solids
	public GameObject liquid;
	public GameObject solid;
	private bool hasLiquid;
	private bool hasSolid;

	public GameStateBase stateInUse;

	public GameController gameController;

	public List<ReagentsInGlass> reagents = new List<ReagentsInGlass>();

	private GameObject interactionBoxGlassware; //Interaction box when the object is clicked while on a Workbench
	private bool onScale;	//The glassware is currently on a scale


	[System.Serializable] /*!< Lets you embed a class with sub properties in the inspector. */
	public class ReagentsInGlass{
		public string reagentName;
		public float massReagent;
	}

	//!  Is called when the script instance is being loaded.
	void Awake()
	{
		if(solid!=null)
			solid.SetActive(false);
		if(liquid!=null)
			liquid.SetActive(false);

		gameController = GameObject.Find ("GameController").GetComponent<GameController> ();
		SetStateInUse (gameController.GetCurrentState ());
	}

	// Use this for initialization
	//! Sets a mass to rigidbody
	void Start () 
	{
		this.rigidbody.mass = mass;
		onScale = false;
		hasLiquid = false;
		hasSolid = false;
	}
	
	// Update is called once per frame
	void Update () 
	{
		/*if (volume == 0.0f)
			liquid.SetActive (false);*/
	}

	//! Holds the events for when the interactive spatula on the Workbench is clicked
	void OnClick() {
		MouseState currentState = CursorManager.GetCurrentState ();

		switch (currentState) {
		case MouseState.ms_default: 		//Default -> Glassware: show the interaction options
			this.OpenInteractionBox ();
			break;
		case MouseState.ms_pipette: 		//Pipette -> Glassware: gets the solids, if there's only solid inside. So, opens the pipette's interaction box.
			Pipette pipette = GameObject.Find ("GameController").GetComponent<GameController> ().GetCurrentState ().GetComponent<WorkBench> ().pipette;
			pipette.OpenInteractionBox(maxVolume);
			break;
		case MouseState.ms_filledPipette: 	// Filled Pipette -> Glassware: pours the pipette's contents into the glassware
			Pipette filledPipette = GameObject.Find ("GameController").GetComponent<GameController> ().GetCurrentState ().GetComponent<WorkBench> ().pipette;
			filledPipette.UnfillPipette(this);
			break;
		case MouseState.ms_spatula: 		// Spatula -> Glassware: gets the solids, if there's only solid inside. So, opens the spatula's interaction box
			Spatula spatula = GameObject.Find ("GameController").GetComponent<GameController> ().GetCurrentState ().GetComponent<WorkBench> ().spatula;
			spatula.OpenInteractionBox ();
			break;
		case MouseState.ms_filledSpatula: 	// Filled Spatula -> Glassware: unloads the spatula into the glassare
			Spatula filledSpatula = GameObject.Find ("GameController").GetComponent<GameController> ().GetCurrentState ().GetComponent<WorkBench> ().spatula;
			filledSpatula.UnfillSpatula(onScale, this);
			break;
		case MouseState.ms_washBottle: 		// Washe Bottle -> Glassware: pours water into the glassware
			WashBottle washBottle = GameObject.Find ("GameController").GetComponent<GameController> ().GetCurrentState ().GetComponent<WorkBench> ().washBottle;
			washBottle.OpenInteractionBox(maxVolume);
			break;
		case MouseState.ms_glassStick:		// Glass Stick -> Glassware: mix the contents, if there is any.
			GlassStick glassStick =  GameObject.Find ("GameController").GetComponent<GameController> ().GetCurrentState ().GetComponent<WorkBench> ().glassStick;

			break;
		case MouseState.ms_usingTool:  		// Unable to click somewhere else TODO:is it necessary?
			break;
		}
	}

	//! Refreshes the contents
	/*! The method set the correct values and visual states for the glassware */
	public void RefreshContents() {

	}
	
//	//! Tries to insert new contents into the glassware
//	//! The method returns false in case it wasn't possible to insert it
//	public bool InsertNewContent(Pipette pipette) {
//	/*
//	 *  TRY TO ADD STUFF
//	 */
//		return false;
//	}
//	//! Tries to insert new contents into the glassware
//	//! Spatula overload
//	public bool InsertNewContent(Spatula spatula) {
//	/*
//	 *  TRY TO ADD STUFF
//	 */
//		return false;
//	}
//	//! Tries to insert new contents into the glassware
//	//! Wash bottle overload
//	public bool InsertNewContent(WashBottle washBottle) {
//	/*
//	 *  TRY TO ADD STUFF
//	 */
//		return false;
//	}

	public void InsertReagent(float liquidVolume, ReagentsLiquidClass reagent) {



	}

	//! Close the interaction box
	public void CloseInteractionBox(){
		interactionBoxGlassware.SetActive(false);
	}
	//! Open the interaction box
	public void OpenInteractionBox() {
		interactionBoxGlassware.SetActive (true);
		//CursorManager.SetDefaultCursor ();
		/*
		 * DEFINE HOW TO BLOCK CLICKS OUTSIDE 
		 */
	}

	//! Add the solid
	public void AddSolid(float massSolid, string reagent){
		if(liquid.activeSelf == false)
			solid.SetActive(true);
		GetComponent<Rigidbody>().mass += massSolid;
	}

	//! Remove the solid
	public void RemoveSolid(float massSolid){
		GetComponent<Rigidbody>().mass -= massSolid;
		if(GetComponent<Rigidbody>().mass < mass){
			GetComponent<Rigidbody>().mass = mass;
			solid.SetActive(false);
		}
	}

	//! Add the liquid
	/*! Checks the current volume (higher, lower or equal) and add the liquid. */
	public void AddLiquid(float massLiquid, float volumeLiquid){

		if(currentVolume < maxVolume){

			float lastVolume = currentVolume;

			currentVolume += volumeLiquid;

			Debug.Log (massLiquid);

			if(currentVolume > maxVolume){

				currentVolume = maxVolume;
			}

			Debug.Log (massLiquid);

			liquid.SetActive(true);
			GetComponent<Rigidbody>().mass += massLiquid*(currentVolume-lastVolume);
		}
		else if(currentVolume >= maxVolume ){

            Debug.Log("Recipiente esta cheio");
            //AlertDialogBehaviour.ShowAlert("Recipente esta cheio");
		}
	}

	public void AddLiquid(float volumeLiquid){
		AddLiquid (1, volumeLiquid);
	}

	//! Remove the liquid.
	public void RemoveLiquid(float massLiquid, float volumeLiquid){

		GetComponent<Rigidbody>().mass -= massLiquid*volumeLiquid;
		currentVolume -= volumeLiquid;

		if(currentVolume < 0)
			currentVolume = 0;

		if(GetComponent<Rigidbody>().mass < mass){
			GetComponent<Rigidbody>().mass = mass;
			liquid.SetActive(false);
		}

	}

	public void RemoveLiquid(float volumeLiquid){
		RemoveLiquid (1, volumeLiquid);
	}

	//! Sets the glassware state.
	public void SetStateInUse(GameStateBase state){
		stateInUse = state;
	}

	//!Message when the player clicks in glass.
	public void CLickInGlass(){
		Debug.Log ("CLick");
		gameController.GetCurrentState().GetComponent<WorkBench> ().ClickGlass (this.gameObject);
	}
}

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

	//! The compounds inside
	//Water is a valid reagent, but it is only seem when it's the only thing inside, otherwise it's associated with the reagent's concentration.
	//public Compound[] compounds = new Compound[2];
	public List<Compound> compounds = new List<Compound>();

	//Mesh of liquids and solids
	public GameObject liquid;
	public GameObject solid;
	private bool hasLiquid;
	private bool hasSolid;

	public GameStateBase stateInUse;

	public GameController gameController;

	//public List<ReagentsInGlass> reagents = new List<ReagentsInGlass>();
	private int numberOfReagents = 0;

	private GameObject interactionBoxGlassware; //Interaction box when the object is clicked while on a Workbench
	private bool onScale;	//The glassware is currently on a scale


	[System.Serializable] /*!< Lets you embed a class with sub properties in the inspector. */
	public class ReagentsInGlass{
		public Compound reagent;
		public float howMuch; //[g]

		public ReagentsInGlass(Compound re, float qu) {
			reagent = re;
			howMuch = qu;
		}
	}

	//!  Is called when the script instance is being loaded.
	void Awake()
	{
		if(solid!=null)
			solid.SetActive(false);
		if(liquid!=null)
			liquid.SetActive(false);

		currentVolume = 0.0f;

		gameController = GameObject.Find ("GameController").GetComponent<GameController> ();
		SetStateInUse (gameController.GetCurrentState ());
	}

	// Use this for initialization
	//! Sets a mass to rigidbody
	void Start () 
	{
		this.rigidbody.mass = mass;
		totalMass = mass;
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
	public void OnClick() {
		MouseState currentState = CursorManager.GetCurrentState ();

		switch (currentState) {
		case MouseState.ms_default: 		//Default -> Glassware: show the interaction options
			this.OpenInteractionBox ();
			break;
		case MouseState.ms_pipette: 		//Pipette -> Glassware: gets the liquid, if there's only liquid inside. So, opens the pipette's interaction box.
			Pipette pipette = GameObject.Find ("GameController").GetComponent<GameController> ().GetCurrentState ().GetComponent<WorkBench> ().pipette;
			if(hasLiquid) {
				if(pipette.graduated) {
					pipette.OpenGraduatedFillingBox(currentVolume, this);
					RefreshContents();
				}
				else {
					pipette.FillVolumetricPipette(this);
					RefreshContents();
				}
			}
			break;
		case MouseState.ms_filledPipette: 	// Filled Pipette -> Glassware: pours the pipette's contents into the glassware
			Pipette filledPipette = GameObject.Find ("GameController").GetComponent<GameController> ().GetCurrentState ().GetComponent<WorkBench> ().pipette;
			//filledPipette.UnfillPipette(this);
			if(filledPipette.graduated) {
				filledPipette.OpenGraduatedUnfillingBox(maxVolume - currentVolume, this);
				RefreshContents();
			}
			else {
				filledPipette.UnfillVolumetricPipette(this);
				RefreshContents();
			}
			break;
		case MouseState.ms_spatula: 		// Spatula -> Glassware: gets the solids, if there's only solid inside. So, opens the spatula's interaction box
			Spatula spatula = GameObject.Find ("GameController").GetComponent<GameController> ().GetCurrentState ().GetComponent<WorkBench> ().spatula;
			//TODO:NEEDS TO CHECK THE REAGENT INSIDE, AND IF IT'S THE ONLY ONE
			//!!!spatula.FillSpatula();
			break;
		case MouseState.ms_filledSpatula: 	// Filled Spatula -> Glassware: unloads the spatula into the glassare
			Spatula filledSpatula = GameObject.Find ("GameController").GetComponent<GameController> ().GetCurrentState ().GetComponent<WorkBench> ().spatula;
			//filledSpatula.OpenInteractionBox(maxVolume - currentVolume, this);
			filledSpatula.UnfillSpatula(maxVolume - currentVolume, this);
			break;
		case MouseState.ms_washBottle: 		// Washe Bottle -> Glassware: pours water into the glassware
			WashBottle washBottle = GameObject.Find ("GameController").GetComponent<GameController> ().GetCurrentState ().GetComponent<WorkBench> ().washBottle;
			washBottle.ActivateWashBottle(maxVolume - currentVolume, this);
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
		Debug.Log ("Refreshing contents");
		foreach (Compound re in compounds) {
			if (re != null) {
				if (re.isSolid)
					hasSolid = true;
				else
					hasLiquid = true;
			}
		}

		if (hasLiquid) {
			liquid.SetActive (true);

			/*
			 * CODE SETTING THE COLOUR OF THE LIQUID?
			 */
		} else
			liquid.SetActive (false);

		if (hasSolid) {
			solid.SetActive (true);

			/*
			 * CODE SETTING THE COLOUR OF THE SOLID?
			 */
		} else
			solid.SetActive (false);
			                
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

	//! 
	public void IncomingReagent(Compound incomingCompound) {

	
	}

	//! Pours a liquid into the glassware
	//	The liquid might come from pipettes or wash bottles (H2O)
	/*public void PourLiquid(float volumeFromTool, float liquidMass, Compound reagentFromTool) {
		currentVolume += volumeFromTool;
		totalMass += liquidMass;

		reagents.Add (new ReagentsInGlass(reagentFromTool as Compound, liquidMass));

		RefreshContents ();
	}*/
	public void PourLiquid(float volumeFromTool, float liquidMass, Compound reagentFromTool) {
		currentVolume += volumeFromTool;
		totalMass += liquidMass;

		Reagent liquid = new Reagent(reagentFromTool, volumeFromTool, 1.0f);
	//	Reagent liquid = (Reagent)reagentFromTool.Clone ();
	//	liquid.realMass = liquidMass;
	//	liquid.volume = volumeFromTool;
		//liquid.CopyCompound(liquid);

		compounds.Insert (0, liquid as Compound);
		
		RefreshContents ();
	}

	//! Remove liquids from the glassware
	//  The liquid is removed into a pipette
	public void RemoveLiquid(float volumeChosen) {
		currentVolume -= volumeChosen;
		totalMass -= volumeChosen * compounds[0].density;
		(compounds [0] as Reagent).realMass -= volumeChosen * compounds [0].density;

		if (compounds [0].realMass <= 0.0f) {
			compounds [0] = null;
			hasLiquid = false;
		}


		RefreshContents();
	}

	//!	Inserts a solid into the glassware
	//	The solid only comes from spatulas
	public void InsertSolid(float volumeFromTool, float solidMass, Compound reagentFromTool) {
		currentVolume += volumeFromTool;
		totalMass += solidMass;

		compounds[0]=reagentFromTool;
		/*
		 * ADD THE REAGENT INTO THE REAGENTS LISTS
		 */

		RefreshContents ();
	}

	//! Remove solids from the glassware
	//  The solid is only taken by spatulas
	public void RemoveSolid(float spatulaVolume) {
		currentVolume -= spatulaVolume;
		totalMass -= spatulaVolume * compounds [0].density;
		compounds [0].realMass -= spatulaVolume * compounds [0].density;
		
		if (compounds [0].realMass <= 0.0f) {
			compounds [0] = null;
			hasLiquid = false;
		}

		RefreshContents();
	}

	//! Put the glassware back to the inventory
	public void GlasswareToInventory() {
		/*
		 * GLASS TO INVENTORY();
		 */
	}
	
	//-------------------------------------------------------------------------//

	//! Add the solid
	public void AddSolid(float massSolid, string reagent){
		if(liquid.activeSelf == false)
			solid.SetActive(true);
		GetComponent<Rigidbody>().mass += massSolid;
	}

	//! Remove the solid
	/*public void RemoveSolid(float massSolid){
		GetComponent<Rigidbody>().mass -= massSolid;
		if(GetComponent<Rigidbody>().mass < mass){
			GetComponent<Rigidbody>().mass = mass;
			solid.SetActive(false);
		}
	}*/

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
	/*public void RemoveLiquid(float massLiquid, float volumeLiquid){

		GetComponent<Rigidbody>().mass -= massLiquid*volumeLiquid;
		currentVolume -= volumeLiquid;

		if(currentVolume < 0)
			currentVolume = 0;

		if(GetComponent<Rigidbody>().mass < mass){
			GetComponent<Rigidbody>().mass = mass;
			liquid.SetActive(false);
		}

	}*/

	/*public void RemoveLiquid(float volumeLiquid){
		RemoveLiquid (1, volumeLiquid);
	}*/

	//! Sets the glassware state.
	public void SetStateInUse(GameStateBase state){
		stateInUse = state;
	}

	//!Message when the player clicks in glass.
/*	public void CLickInGlass(){
		Debug.Log ("CLick");
		gameController.GetCurrentState().GetComponent<WorkBench> ().ClickGlass (this.gameObject);
	}*/
}

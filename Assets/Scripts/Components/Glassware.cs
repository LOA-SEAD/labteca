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

	public Material liquidMaterial;
	public Material solidMaterial;

	//public float uncalibrateVolume; ?
	public bool precisionGlass;		//The glassware is a precision one
	public string gl;

	//! The compounds inside
	//Water is a valid reagent, but it is only seem when it's the only thing inside, otherwise it's associated with the reagent's concentration.
	//public Compound[] compounds = new Compound[2];
	//[SerializeField]
	public List<object> compounds = new List<object>();
//	public Mixture mixture = null;

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


	public bool hasReagents(){
		/*if(compounds [0] != null)
			if(compounds [0] is Mixture)
				Debug.Log ((compounds [0] as Mixture).Name);
			if(compounds [0] is Compound)
			Debug.Log ((compounds [0] as Compound));
		if(compounds [1] != null)
			if(compounds [1] is Mixture)
				Debug.Log ((compounds [1] as Mixture).Name);
		if(compounds [1] is Compound)
			Debug.Log ((compounds [1] as Compound).Name);*/
		
		if (compounds [0] == null && compounds [1] == null) 
			return false;
		return true;
	}

	public List<Compound> GetCompounds(){
		List<Compound> comp = new List<Compound>(2);

		if(compounds [0] is Compound)
			comp.Add ((compounds [0] as Compound).Clone() as Compound);
		if(compounds [1] is Compound)
			comp.Add ((compounds [1] as Compound).Clone() as Compound);

		return comp;
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
	}

	// Use this for initialization
	//! Sets a mass to rigidbody
	void Start () 
	{
		
		Color newColor = new Color(1f,1f,1f,0.35f);
		
		MeshRenderer liquidRenderer = liquid.GetComponent<MeshRenderer> ();
		MeshRenderer solidRenderer = solid.GetComponent<MeshRenderer> ();
		
		Material newliquidMaterial = new Material(liquidMaterial.shader);
		Material newsolidMaterial = new Material(solidMaterial.shader);
		
		liquidMaterial.color = newColor;
		liquidRenderer.material = newliquidMaterial;
		
		solidMaterial.color = newColor;
		solidRenderer.material = newsolidMaterial;

		this.rigidbody.mass = mass;
		totalMass = mass;
		onScale = false;
		hasLiquid = false;
		hasSolid = false;

		compounds.Insert (0, null);
		compounds.Insert (1, null);
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
			(GameObject.Find ("GameController").GetComponent<GameController> ().GetCurrentState () as WorkBench).stateUIManager.OpenOptionDialog(this);
			break;
		case MouseState.ms_pipette: 		//Pipette -> Glassware: gets the liquid, if there's only liquid inside. So, opens the pipette's interaction box.
			Pipette pipette = (GameObject.Find ("GameController").GetComponent<GameController> ().GetCurrentState () as WorkBench).pipette;
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
			Pipette filledPipette = (GameObject.Find ("GameController").GetComponent<GameController> ().GetCurrentState () as WorkBench).pipette;
			//filledPipette.UnfillPipette(this);
			if(filledPipette.graduated) {
				filledPipette.OpenGraduatedUnfillingBox(maxVolume - currentVolume, this);
			}
			else {
				filledPipette.UnfillVolumetricPipette(this);
				RefreshContents();
			}
			break;
		case MouseState.ms_spatula: 		// Spatula -> Glassware: gets the solids, if there's only solid inside. So, opens the spatula's interaction box
			Spatula spatula = (GameObject.Find ("GameController").GetComponent<GameController> ().GetCurrentState () as WorkBench).spatula;
			if(compounds[0] != null) {
				if(compounds[0] is Compound && (compounds[0] as Compound).IsSolid) {
					if(compounds[1] == null) {
						spatula.FillSpatula(this);
					}
				}
			}
			//!!!spatula.FillSpatula();
			break;
		case MouseState.ms_filledSpatula: 	// Filled Spatula -> Glassware: unloads the spatula into the glassare
			Spatula filledSpatula = (GameObject.Find ("GameController").GetComponent<GameController> ().GetCurrentState () as WorkBench).spatula;
			//filledSpatula.OpenInteractionBox(maxVolume - currentVolume, this);
			filledSpatula.UnfillSpatula(maxVolume - currentVolume, this);
			break;
		case MouseState.ms_washBottle: 		// Washe Bottle -> Glassware: pours water into the glassware
			if(this.precisionGlass) {

			}
			else {
				WashBottle washBottle = (GameObject.Find ("GameController").GetComponent<GameController> ().GetCurrentState () as WorkBench).washBottle;
				washBottle.ActivateWashBottle(maxVolume - currentVolume, this);
			}
			break;
		case MouseState.ms_glassStick:		// Glass Stick -> Glassware: mix the contents, if there is any.
			//GlassStick glassStick =  GameObject.Find ("GameController").GetComponent<GameController> ().GetCurrentState ().GetComponent<WorkBench> ().glassStick;

			break;
		case MouseState.ms_usingTool:  		// Unable to click somewhere else
			break;
		}
	}

	//! Refreshes the contents
	/*! The method set the correct values and visual states for the glassware */
	public void RefreshContents() {
		foreach (object re in compounds) {
			if (re != null) {
				if(re is IPhysicochemical) {
					if ((re as IPhysicochemical).IsSolid)
						hasSolid = true;
					else
						hasLiquid = true;
				}
			}
		}

		if (hasLiquid) {
			liquid.SetActive (true);

			/*
			 * CODE SETTING THE COLOUR OF THE LIQUID?
			 */
		} else {
			liquid.SetActive (false);
		}
		if (hasSolid) {
			solid.SetActive (true);

			/*
			 * CODE SETTING THE COLOUR OF THE SOLID?
			 */
		} else
			solid.SetActive (false);
	
		currentVolume = this.GetVolume ();
		totalMass = this.GetMass ();
	}

	//! Return the real mass of the glassware
	public float GetMass() {
		float actualMass = this.mass;
		if (compounds [0] != null) {
			if(compounds[0] is Mixture) {
				actualMass += (compounds [0] as Mixture).RealMass;
				}
			else {
				actualMass += (compounds[0] as Compound).RealMass;
			}
			Debug.Log ("GetMass of 0 " + (actualMass - this.mass).ToString ());
		}
		if (compounds [1] != null) {
			if(compounds[1] is Mixture) {
				actualMass += (compounds [1] as Mixture).RealMass;
			}
			else {
				actualMass += (compounds[1] as Compound).RealMass;
			}
		}

		return actualMass;
	}

	public float GetPH() {
	
		float actualPH = 7.0f;

		return actualPH;

	}


	public float GetConductivity() {
	
		float actualConductivity = 0.5f;

		return actualConductivity;

	}

	public float GetPolarity() {
	
		float actualPolarity = 1.0f;

		return actualPolarity;

	}




	public float GetVolume() {
		float actualVolume = 0.0f;
		if (compounds [0] != null) {
			if(compounds[0] is Mixture) {
				actualVolume += (compounds [0] as Mixture).Volume;
			}
			else {
				actualVolume += (compounds[0] as Compound).Volume;
			}
		}
		if (compounds [1] != null) {
			if(compounds[1] is Mixture) {
				actualVolume += (compounds [1] as Mixture).Volume;
			}
			else {
				actualVolume += (compounds[1] as Compound).Volume;
			}
		}
		
		return actualVolume;
	}

	//! Close the interaction box
	public void CloseInteractionBox(){
		interactionBoxGlassware.SetActive(false);
	}
	//! Open the interaction box
	public void OpenInteractionBox() {
		//interactionBoxGlassware.SetActive (true);
		//CursorManager.SetDefaultCursor ();
		/*
		 * DEFINE HOW TO BLOCK CLICKS OUTSIDE 
		 */
	}

	//! 
	public bool IncomingReagent(Compound incomingCompound, float volumeFromTool) {
		Debug.Log (incomingCompound.Formula + " incoming!");
		if (compounds [0] != null) { //Case not empty
			if (compounds [0] is Mixture) { // Case: there's Mixture
				if (incomingCompound.Formula == "H2O") {
					(compounds [0] as Mixture).Dilute (incomingCompound);
				} else {
					//ERROR MESSAGE
					return false;
				}
			} else { // Case: Not Mixture
				if (incomingCompound.Formula == "H2O") { // SubCase: Water is coming
					if ((compounds [0] as Compound).Formula == "H2O") { // There's water already
						AddSameReagent(volumeFromTool, (Compound)incomingCompound.Clone(volumeFromTool));
					} else {	// There's a reagent
						(compounds [0] as Compound).Dilute ((Compound)incomingCompound.Clone(volumeFromTool));
						hasSolid = false;
						hasLiquid = true;
					}
				} else {	// SubCase: A reagent is coming
					if (incomingCompound.Formula == (compounds [0] as Compound).Formula) { // There's the same reagent inside
						AddSameReagent(volumeFromTool, (Compound)incomingCompound.Clone(volumeFromTool));
					} else {
						if((compounds [0] as Compound).Formula == "H2O") { //There's water
							Compound aux = new Compound(incomingCompound);
							aux.Dilute(compounds[0] as Compound);
							compounds[0] = aux;
							hasSolid = false;
							hasLiquid = true;
						}
						//(compounds [0] as Reagent).React (incomingCompound as Reagent);
						/*
						 * SET ALL THE OTHER CONTENTS AND MIXTURE STUFF (wheter here or on another script)
						 */
					}
				}	
			}
			this.RefreshContents();
			return true;
		} else {
			if(!incomingCompound.IsSolid) {
				this.PourLiquid(volumeFromTool, volumeFromTool * incomingCompound.Density, incomingCompound);
			}
			else
				this.InsertSolid(volumeFromTool, volumeFromTool * incomingCompound.Density, incomingCompound);
			return true;
		}
	}

	//! Pours a liquid into the glassware
	//	The liquid might come from pipettes or wash bottles (H2O)
	public void PourLiquid(float volumeFromTool, float liquidMass, Compound reagentFromTool) {
		//currentVolume += volumeFromTool;

		//totalMass += liquidMass;

		Compound liquidCompound = (Compound)reagentFromTool.Clone (volumeFromTool);
		//liquid.realMass = volumeFromTool * liquid.
	//	Reagent liquid = (Reagent)reagentFromTool.Clone ();
	//	liquid.realMass = liquidMass;
	//	liquid.volume = volumeFromTool;
		//liquid.CopyCompound(liquid);

		compounds.Insert (0, liquidCompound as Compound);
		//compounds.Insert (0, (Compound)reagentFromTool.Clone (volumeFromTool));

		RefreshContents ();
	}

	//! Remove liquids from the glassware
	//  The liquid is removed into a pipette
	public void RemoveLiquid(float volumeChosen) {
		//currentVolume -= volumeChosen;
		//totalMass -= volumeChosen * (compounds[0] as IPhysicochemical).Density;
		(compounds [0] as IPhysicochemical).RealMass = (compounds [0] as IPhysicochemical).RealMass - volumeChosen * (compounds [0] as IPhysicochemical).Density;
		(compounds [0] as IPhysicochemical).Volume = (compounds [0] as IPhysicochemical).Volume - volumeChosen;
		if((compounds[0] as IPhysicochemical).Volume <= 0.07f) {
			compounds[0] = null;
			hasLiquid = false;
		}
		/*if ((compounds [0] as IPhysicochemical).RealMass <= 0.0f) {
			compounds [0] = null;
			hasLiquid = false;
		}*/


		RefreshContents();
	}

	//! Pours the same liquid
	public void AddSameReagent(float volumeFromTool, Compound reagentFromTool) { //TODO:Verify all interactions! Volumetric OK / Graduated OK / Spatula OK / WashBottle OK
	
		Debug.Log ("Adding same reagent " + reagentFromTool.Formula + " to " + (compounds[0] as Compound).Formula );
		if ((compounds [0] as Compound).IsSolid == reagentFromTool.IsSolid) { //Case: same physical state
			(compounds [0] as Compound).Molarity = ((compounds [0] as Compound).Volume * (compounds [0] as Compound).Molarity + reagentFromTool.Molarity * volumeFromTool) / ((compounds [0] as Compound).Volume + volumeFromTool);
			(compounds [0] as Compound).Volume = (compounds [0] as Compound).Volume + volumeFromTool;
			(compounds [0] as Compound).RealMass = (compounds [0] as Compound).RealMass + reagentFromTool.Density * volumeFromTool;
			(compounds [0] as Compound).Density = (compounds [0] as Compound).RealMass / (compounds[0] as Compound).Volume;
		} else {
			if(reagentFromTool.IsSolid) {
				(compounds [0] as Compound).Molarity = ((compounds [0] as Compound).Volume * (compounds [0] as Compound).Molarity + reagentFromTool.Molarity * volumeFromTool) / ((compounds [0] as Compound).Volume + volumeFromTool);
				(compounds [0] as Compound).Volume = (compounds [0] as Compound).Volume; //TODO: NEEDS TO CHECK FOR PRECIPITATE
				(compounds [0] as Compound).RealMass = (compounds [0] as Compound).RealMass + reagentFromTool.Density * volumeFromTool;
				(compounds [0] as Compound).Density = (compounds [0] as Compound).RealMass / (compounds[0] as Compound).Volume;
			}
			else {
				(compounds [0] as Compound).Molarity = ((compounds [0] as Compound).Volume * (compounds [0] as Compound).Molarity + reagentFromTool.Molarity * volumeFromTool) / ((compounds [0] as Compound).Volume + volumeFromTool);
				(compounds [0] as Compound).Volume = volumeFromTool; //TODO: NEEDS TO CHECK FOR PRECIPITATE
				(compounds [0] as Compound).RealMass = (compounds [0] as Compound).RealMass + reagentFromTool.Density * volumeFromTool;
				(compounds [0] as Compound).Density = (compounds [0] as Compound).RealMass / (compounds[0] as Compound).Volume;
			}
			(compounds[0] as Compound).IsSolid = false;
			hasSolid = false;
			hasLiquid = true;
		}
		Debug.Log ("Molarity = " + (compounds[0] as Compound).Molarity);
		Debug.Log ("Volume = " + (compounds[0] as Compound).Volume);
		Debug.Log ("RealMass = " + (compounds[0] as Compound).RealMass);

		RefreshContents ();
	}

	//!	Inserts a solid into the glassware
	//	The solid only comes from spatulas
	public void InsertSolid(float volumeFromTool, float solidMass, Compound reagentFromTool) {
		//currentVolume += volumeFromTool;
		//totalMass += solidMass;

		compounds[0]= (Compound)reagentFromTool.Clone(volumeFromTool);

		RefreshContents ();
	}

	//! Remove solids from the glassware
	//  The solid is only taken by spatulas
	public void RemoveSolid(float spatulaVolume) {
		//currentVolume -= spatulaVolume;
		//totalMass -= spatulaVolume * (compounds [0] as IPhysicochemical).Density;

		(compounds [0] as IPhysicochemical).RealMass = (compounds [0] as IPhysicochemical).RealMass - spatulaVolume * (compounds [0] as IPhysicochemical).Density;
		(compounds [0] as IPhysicochemical).Volume = (compounds [0] as IPhysicochemical).Volume - spatulaVolume;
		if((compounds[0] as IPhysicochemical).Volume <= 0.07f) {
			compounds[0] = null;
			hasSolid = false;
		}

		/*if ((compounds [0]  as IPhysicochemical).Volume <= 0.0f) {
			compounds [0] = null;
			hasSolid = false;
		}*/

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

	//!Message when the player clicks in glass.
/*	public void CLickInGlass(){
		Debug.Log ("CLick");
		gameController.GetCurrentState().GetComponent<WorkBench> ().ClickGlass (this.gameObject);
	}*/
}

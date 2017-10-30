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
	
	public bool precisionGlass;		//The glassware is a precision one
	public string gl;
	
	//! The compounds inside
	//Water is a valid reagent, but it is only seem when it's the only thing inside, otherwise it's associated with the reagent's concentration.
	//public Compound[] compounds = new Compound[2];
	//[SerializeField]
	public object content;	//public Mixture mixture = null;

	//Mesh of glassware, liquids and solids
	public GameObject glasswareMesh;
	public GameObject liquid;
	public GameObject originalLiquid;
	public GameObject solid;
	public bool hasLiquid;
	public bool hasSolid;
	
	public GameStateBase stateInUse;

	public GameController gameController;

	private GameObject interactionBoxGlassware; //Interaction box when the object is clicked while on a Workbench
	private bool onScale;	//The glassware is currently on a scale

	private GameObject interactonBoxLabel; //Interaction box for editing the label
	public string label = "";	//Label written by the player

	public RectTransform infoCanvas; //Canvas of information to be shown when mouse is hovering over when glass is on workbench
	public List<Text> tabValues;	 //Values to be shown in the infoCanvas
	public float otherX;
	public float otherZ;
	
	private Color defaultColour;

	public bool hasReagents(){	
		if (content == null) 
			return false;
		return true;
	}

	//!  Is called when the script instance is being loaded.
	void Awake()
	{
		this.GetComponent<Rigidbody>().mass = mass;
		totalMass = mass;
		currentVolume = 0.0f;
		onScale = false;
		hasLiquid = false;
		hasSolid = false;
		
		content = null;

		MeshRenderer liquidRenderer = liquid.GetComponent<MeshRenderer> ();
		MeshRenderer solidRenderer = solid.GetComponent<MeshRenderer> ();
		
		Material newliquidMaterial = new Material(liquidRenderer.material);
		Material newsolidMaterial = new Material(solidRenderer.material);
		
		liquidRenderer.material = newliquidMaterial;
		liquidRenderer.material.SetColor ("_Color", new Color32(0,255,255,130)); 
		solidRenderer.material = newsolidMaterial;
		solidRenderer.material.SetColor ("_Color", Color.red);
		
		originalLiquid = Instantiate (liquid) as GameObject;
		originalLiquid.transform.SetParent (transform, false);
		originalLiquid.transform.position = liquid.transform.position;
		originalLiquid.transform.rotation = liquid.transform.rotation;
		originalLiquid.SetActive (false);

		if(solid!=null)
			solid.SetActive(false);
		if(liquid!=null)
			liquid.SetActive(false);

		//defaultColour = glasswareMesh.GetComponent<MeshRenderer>().materials[0].color;
		defaultColour = glasswareMesh.GetComponent<Renderer>().material.color;


		gameController = GameObject.Find ("GameController").GetComponent<GameController> ();
		if (infoCanvas != null) {
			infoCanvas.gameObject.SetActive (false);
		}
	}

	// Use this for initialization
	//! Sets a mass to rigidbody
	void Start ()
	{
	}
	
	// Update is called once per frame
	void Update () 
	{
		//Debug.Log (CursorManager.GetCurrentState ());
		/*if (volume == 0.0f)
			liquid.SetActive (false);*/
	}

	//! Holds the events for when the interactive spatula on the Workbench is clicked
	public void OnClick() {
		if (gameController.GetCurrentState () != gameController.gameStates [0]) {
			MouseState currentState = CursorManager.GetCurrentState ();

			if (content is Mixture) { //TODO:PROVISORIO!
				if (currentState == MouseState.ms_default) {
					(GameObject.Find ("GameController").GetComponent<GameController> ().GetCurrentState () as WorkBench).stateUIManager.OpenOptionDialog (this);
				} else {
					gameController.sendAlert ("Este tipo de interação não está habilitada após uma possível reação");
				}
			} else {
				switch (currentState) {
				case MouseState.ms_default: 		//Default -> Glassware: show the interaction options
					(GameObject.Find ("GameController").GetComponent<GameController> ().GetCurrentState () as WorkBench).stateUIManager.OpenOptionDialog (this);
					break;
				case MouseState.ms_pipette: 		//Pipette -> Glassware: gets the liquid, if there's only liquid inside. So, opens the pipette's interaction box.
					Pipette pipette = (GameObject.Find ("GameController").GetComponent<GameController> ().GetCurrentState () as WorkBench).pipette;
					if (hasLiquid) {
						if (pipette.graduated) {
							pipette.OpenGraduatedFillingBox (currentVolume, this);
							RefreshContents ();
						} else {
							pipette.FillVolumetricPipette (this);
							RefreshContents ();
						}
					}
					break;
				case MouseState.ms_filledPipette: 	// Filled Pipette -> Glassware: pours the pipette's contents into the glassware
					Pipette filledPipette = (GameObject.Find ("GameController").GetComponent<GameController> ().GetCurrentState () as WorkBench).pipette;
			//filledPipette.UnfillPipette(this);
					if (filledPipette.graduated) {
						filledPipette.OpenGraduatedUnfillingBox (maxVolume - currentVolume, this);
					} else {
						filledPipette.UnfillVolumetricPipette (this);
						RefreshContents ();
					}
					break;
				case MouseState.ms_spatula: 		// Spatula -> Glassware: gets the solids, if there's only solid inside. So, opens the spatula's interaction box
					Spatula spatula = (GameObject.Find ("GameController").GetComponent<GameController> ().GetCurrentState () as WorkBench).spatula;
					Debug.Log ("Spatula clickou no Glassware");
					if (content != null) {
						Debug.Log ("Glassware nao esta vazio");
						if (content is Compound && (content as Compound).IsSolid) {
							Debug.Log ("Enchendo a espatula");
							spatula.FillSpatula (this);
						}
					}
					break;
				case MouseState.ms_filledSpatula: 	// Filled Spatula -> Glassware: unloads the spatula into the glassare
					Spatula filledSpatula = (GameObject.Find ("GameController").GetComponent<GameController> ().GetCurrentState () as WorkBench).spatula;
			//filledSpatula.OpenInteractionBox(maxVolume - currentVolume, this);
					filledSpatula.UnfillSpatula (maxVolume - currentVolume, this);
					break;
				case MouseState.ms_washBottle: 		// Washe Bottle -> Glassware: pours water into the glassware
					WashBottle washBottle = (GameObject.Find ("GameController").GetComponent<GameController> ().GetCurrentState () as WorkBench).washBottle;
					washBottle.ActivateWashBottle (this.maxVolume - this.GetLiquidVolume(), this);
					break;
				case MouseState.ms_interacting:  		// Unable to click somewhere else
					break;
				}
			}

			if(gameController.GetCurrentState () == gameController.gameStates[6]) {
				(gameController.GetCurrentState().GetEquipmentController() as TurbidimeterController).bucketCanvas.SetActive(false);
			}
		}
	}

	//! Handles actions when on hovering the mouse
	//  Shows the infoCanvas (setting its position correctly), and makes the object glow when interaction is possible
	public void OnHoverIn() {
		//Checking where the infoCanvas is, and depending on that, sets it to a pre-defined position to make sure it's not out of the screen
		if (gameController.GetCurrentState () != gameController.gameStates [0]) {
			if (CursorManager.GetCurrentState () != MouseState.ms_interacting) {
				if (content != null && (gameController.GetCurrentState() == gameController.gameStates [1] || gameController.GetCurrentState() == gameController.gameStates [4] ) && infoCanvas != null) {
					if (gameController.GetCurrentState ().cameraState.WorldToScreenPoint (infoCanvas.position).x > (2 * gameController.GetCurrentState ().cameraState.pixelWidth / 3)) {
						infoCanvas.localPosition = new Vector3 (otherX, infoCanvas.localPosition.y, infoCanvas.localPosition.z);
					}
					if (gameController.GetCurrentState ().cameraState.WorldToScreenPoint (infoCanvas.position).y > gameController.GetCurrentState ().cameraState.pixelHeight / 3) {
						infoCanvas.localPosition = new Vector3 (infoCanvas.localPosition.x, infoCanvas.localPosition.y, otherZ);
					}
					if (content is Mixture) {
						tabValues [0].text = (content as Mixture).Leftover1Formula () + " + " + (content as Mixture).Leftover2Formula ();
						tabValues [1].text = "Massa Molar: ??";
					} else if (content is Compound) {
						tabValues [0].text = (content as Compound).Formula;

						tabValues [1].gameObject.SetActive (true);
						tabValues [1].text = "Massa Molar: " + string.Format ("{0:0.00}", (content as Compound).MolarMass);
					}
					//infoCanvas.gameObject.SetActive (true);
				}

				//Glowing state machine
				switch (CursorManager.GetCurrentState ()) {
				/*case MouseState.ms_default: 		//Default -> Glassware
					glasswareMesh.renderer.material.color = Color.white;
					break;*/
				case MouseState.ms_pipette: 		//Pipette -> Glassware
					if (hasLiquid) {
						//glow
						glasswareMesh.GetComponent<Renderer>().material.color = Color.white;
					}
					break;
				case MouseState.ms_filledPipette: 	// Filled Pipette -> Glassware.
					if (this.GetLiquidVolume() < maxVolume) {
						//glow
						glasswareMesh.GetComponent<Renderer>().material.color = Color.white;
					}
					break;
				case MouseState.ms_spatula: 		// Spatula -> Glassware.
					if (hasSolid) {
						glasswareMesh.GetComponent<Renderer>().material.color = Color.white;
					}
					break;
				case MouseState.ms_filledSpatula: 	// Filled Spatula -> Glassware.
					if (currentVolume < maxVolume) {
						//glow
						glasswareMesh.GetComponent<Renderer>().material.color = Color.white;
					}
					break;
				case MouseState.ms_washBottle: 		// Washe Bottle -> Glassware.
					if (this.GetLiquidVolume() < maxVolume) {
						//glow
						glasswareMesh.GetComponent<Renderer>().material.color = Color.white;
					}
					break;
				default:
					break;		
				}
			}
		}
	}
	//! Handles actions when hovering stops
	public void OnHoverOut() {
		if (gameController.GetCurrentState () != gameController.gameStates [0]) {
			if(infoCanvas != null) {
				infoCanvas.gameObject.SetActive (false);
				infoCanvas.GetComponent<RectTransform> ().localPosition = Vector3.zero;
			}

			glasswareMesh.GetComponent<Renderer>().material.color = defaultColour;
		}
	}

	public void RefreshLiquid(){
		bool active = liquid.activeSelf;
		Destroy (liquid);
		liquid = Instantiate (originalLiquid) as GameObject;
		liquid.transform.SetParent (transform, false);
		liquid.transform.position = originalLiquid.transform.position;
		liquid.transform.rotation = originalLiquid.transform.rotation;
		liquid.SetActive (active);

		float prop = (originalLiquid.GetComponent<MeshRenderer> ().bounds.max.y
		            - originalLiquid.GetComponent<MeshRenderer> ().bounds.min.y)
					* (currentVolume / maxVolume);

		GameObject[] aux = BLINDED_AM_ME.MeshCut.Cut (liquid.gameObject, transform.position + new Vector3(0,prop,0), new Vector3 (0, 1, 0), liquid.GetComponent<MeshRenderer> ().material);
		Destroy (aux [1]);
		aux [0].name = "liquid";
		liquid = aux [0];
	}

	public void RefreshSolid(){


		if ((content as Compound) != null && (content as Compound).IsSolid) {
			float scaleValue = Mathf.Log10 (((content as Compound).Volume * 0.9f / maxVolume) * 8 + 1f) + (1 - Mathf.Log10 (0.9f * 8 + 1f));
			solid.transform.localScale = new Vector3 (scaleValue * 0.8f, 
			                                          scaleValue * 0.8f,
			                                          scaleValue * 0.9f);
		}
		/*if((content as Compound)!=null&&(content as Compound).IsSolid)
			solid.transform.localScale = new Vector3 ((content as Compound).Volume*0.7f / maxVolume, 
			                                          (content as Compound).Volume*0.7f / maxVolume, 
			                                          (content as Compound).Volume*0.7f / maxVolume); */
	}


	//! Refreshes the contents
	/*! The method set the correct values and visual states for the glassware */
	public void RefreshContents() {
		Color32 liquidColor = new Color32(), solidColor = new Color32();

		if (content != null) {
			if(content is IPhysicochemical) {
				Color32 thisColor;
				if(!(content is Mixture)) {
//					Debug.Log ("Molarity of " +(content as Compound).Formula + " = " + (content as Compound).Molarity);
//					Debug.Log ("Volume of " +(content as Compound).Formula + " = " + (content as Compound).Volume);
					if(!(content as Compound).Formula.Contains("H2O"))
						thisColor = (content as Compound).compoundColor;
					else
						thisColor = new Color32(255,255,255,40);
				}
				else {
					thisColor = new Color32(255,255,255,40);
					(content as Mixture).ShowMolarity ();
				}

				if ((content as IPhysicochemical).IsSolid){
					hasSolid = true;
					solidColor = thisColor;
				}else{
					hasLiquid = true;
					liquidColor = thisColor;
				}
			}
		}
		
		currentVolume = this.GetVolume ();
		totalMass = this.GetMass ();
		
		if (hasLiquid) {
			liquid.SetActive (true);
			originalLiquid.GetComponent<MeshRenderer>().material.SetColor("_Color",liquidColor);
			RefreshLiquid ();
		} else {
			liquid.SetActive (false);
		}
		if (hasSolid) {
			solid.SetActive (true);
			solid.GetComponent<MeshRenderer>().material.SetColor("_Color",solidColor);
			RefreshSolid ();
		} else
			solid.SetActive (false);
	}

	//! Return the real mass of the glassware
	public float GetMass() {
		float actualMass = this.mass;

		if (content != null) {
			if(content is Mixture) {
				actualMass += (content as Mixture).RealMass;
				}
			else {
				actualMass += (content as Compound).RealMass;
			}
		}
		return actualMass;
	}

	public float GetPH() {
		float actualPH = 7.0f;

		if (content != null) {
			if(content is Mixture) {
				actualPH = (content as Mixture).PH;
			}
			else {
				actualPH = (content as Compound).PH;
			}
		}
	
		return actualPH;
	}

	public float GetMolarity() {
		float actualMolarity = 0.0f;

		if (content != null) {
			actualMolarity = (content as Compound).Molarity;
		}

		return actualMolarity;
	}


	public float GetConductivity() {
		float actualConductivity = 0.0f;

		if (content != null) {
			if(content is Mixture) {
				actualConductivity = (content as Mixture).Conductibility;
			}
			else {
				actualConductivity = (content as Compound).Conductibility;
			}
		}

		return actualConductivity;
	}

	public float GetPolarity() {
		float actualPolarity = 1.0f;

		if (content != null) {
			if(content is Mixture) {
				actualPolarity = (content as Mixture).Polarizability;
			}
			else {
				actualPolarity = (content as Compound).Polarizability;
			}
		}

		return actualPolarity;
	}

	public float GetTurbidity() {
		float actualTurbidity = 0.0f;

		if (content != null) {
			if(content is Mixture) {
				actualTurbidity = (content as Mixture).Turbidity;
			}
			else {
				actualTurbidity = (content as Compound).Turbidity;
			}
		}

		return actualTurbidity;
	}
	
	public float GetVolume() {
		float actualVolume = 0.0f;
		if (content != null) {
			if(content is Mixture) {
				actualVolume += (content as Mixture).Volume;
			}
			else {
				actualVolume += (content as Compound).Volume;
			}
		}
		return actualVolume;
	}

	//! Get method to be used by tools to decide the maximum amount of liquid that can be poured
	public float GetLiquidVolume() {
		float actualVolume = 0.0f;
		if (content != null) {
			if(content is Mixture) {
				actualVolume += (content as Mixture).Volume;
			}
			else {
				if(!(content as Compound).IsSolid) {
					actualVolume += (content as Compound).Volume;
				}
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

	//! Treatment of cases for when something is being put into the glassware
	public bool IncomingReagent(Compound incomingCompound, float volumeFromTool) {
//		Debug.Log (incomingCompound.Formula + " incoming!");
		if (content != null) { //Case not empty
			if (content is Mixture) { // Case: there's Mixture
				if (incomingCompound.Formula == "H2O") {
					(content as Mixture).Dilute (incomingCompound);
				} else {
					//ERROR MESSAGE
					return false;
				}
			} else { // Case: Not Mixture
				if (incomingCompound.Formula == "H2O") { // SubCase: Water is coming
					if ((content as Compound).Formula == "H2O") { // There's water already
						AddSameReagent(volumeFromTool, (Compound)incomingCompound.Clone(volumeFromTool));
					} else {	// There's a reagent
						(content as Compound).Dilute ((Compound)incomingCompound.Clone(volumeFromTool));
						hasSolid = false;
						hasLiquid = true;
					}
				} else {	// SubCase: A reagent is coming
					if (incomingCompound.Formula == (content as Compound).Formula) { // There's the same reagent inside
						AddSameReagent(volumeFromTool, (Compound)incomingCompound.Clone(volumeFromTool));
					} else {
						if((content as Compound).Formula == "H2O") { //There's water
							Compound aux = new Compound(incomingCompound);
							aux.Dilute(content as Compound);
							content = aux;
							hasSolid = false;
							hasLiquid = true;
						}
						else { //A mixure has to be created
							Debug.Log ("r1 = " + (content as Compound).Formula + "   r2 = " + incomingCompound.Formula);
							Mixture mix = new Mixture(content as Compound, incomingCompound);
							content = mix;

						}
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
		Compound liquidCompound = (Compound)reagentFromTool.Clone (volumeFromTool);

		content = liquidCompound;
		hasLiquid = true;
		hasSolid = false;

		RefreshContents ();
	}

	//! Remove liquids from the glassware
	//  The liquid is removed into a pipette
	public void RemoveLiquid(float volumeChosen) {
		(content as IPhysicochemical).RealMass = (content as IPhysicochemical).RealMass - volumeChosen * (content as IPhysicochemical).Density;
		(content as IPhysicochemical).Volume = (content as IPhysicochemical).Volume - volumeChosen;
		if((content as IPhysicochemical).Volume <= 0.07f) {
			content = null;
			hasLiquid = false;
			if(infoCanvas != null) {
				infoCanvas.gameObject.SetActive(false);
			}
		}
		/*if ((compounds as IPhysicochemical).RealMass <= 0.0f) {
			compounds = null;
			hasLiquid = false;
		}*/

		RefreshContents();
	}

	//! Pours the same liquid
	public void AddSameReagent(float volumeFromTool, Compound reagentFromTool) { //TODO:Verify all interactions! Volumetric OK / Graduated OK / Spatula OK / WashBottle OK
	
		Debug.Log ("Adding same reagent " + reagentFromTool.Formula + " to " + (content as Compound).Formula );
		if ((content as Compound).IsSolid == reagentFromTool.IsSolid) { //Case: same physical state
			(content as Compound).Molarity = ((content as Compound).Volume * (content as Compound).Molarity + reagentFromTool.Molarity * volumeFromTool) / ((content as Compound).Volume + volumeFromTool);
			(content as Compound).Volume = (content as Compound).Volume + volumeFromTool;
			(content as Compound).RealMass = (content as Compound).RealMass + reagentFromTool.Density * volumeFromTool;
			(content as Compound).Density = (content as Compound).RealMass / (content as Compound).Volume;
		} else {
			if(reagentFromTool.IsSolid) {
				(content as Compound).Molarity = ((content as Compound).Volume * (content as Compound).Molarity + reagentFromTool.Molarity * volumeFromTool) / ((content as Compound).Volume + volumeFromTool);
				(content as Compound).Volume = (content as Compound).Volume; //TODO: NEEDS TO CHECK FOR PRECIPITATE
				(content as Compound).RealMass = (content as Compound).RealMass + reagentFromTool.Density * volumeFromTool;
				(content as Compound).Density = (content as Compound).RealMass / (content as Compound).Volume;
			}
			else {
				(content as Compound).Molarity = ((content as Compound).Volume * (content as Compound).Molarity + reagentFromTool.Molarity * volumeFromTool) / ((content as Compound).Volume + volumeFromTool);
				(content as Compound).Volume = volumeFromTool; //TODO: NEEDS TO CHECK FOR PRECIPITATE
				(content as Compound).RealMass = (content as Compound).RealMass + reagentFromTool.Density * volumeFromTool;
				(content as Compound).Density = (content as Compound).RealMass / (content as Compound).Volume;
			}
			(content as Compound).IsSolid = false;
			hasSolid = false;
			hasLiquid = true;
		}
		Debug.Log ("Molarity = " + (content as Compound).Molarity);
		Debug.Log ("Volume = " + (content as Compound).Volume);
		Debug.Log ("RealMass = " + (content as Compound).RealMass);

		RefreshContents ();
	}

	//!	Inserts a solid into the glassware
	//	The solid only comes from spatulas
	public void InsertSolid(float volumeFromTool, float solidMass, Compound reagentFromTool) {
		//currentVolume += volumeFromTool;
		//totalMass += solidMass;

		content= (Compound)reagentFromTool.Clone(volumeFromTool);
		hasSolid = true;
		RefreshContents ();
	}

	//! Remove solids from the glassware
	//  The solid is only taken by spatulas
	public void RemoveSolid(float spatulaVolume) {
		//currentVolume -= spatulaVolume;
		//totalMass -= spatulaVolume * (compounds as IPhysicochemical).Density;

		(content as IPhysicochemical).RealMass = (content as IPhysicochemical).RealMass - spatulaVolume * (content as IPhysicochemical).Density;
		(content as IPhysicochemical).Volume = (content as IPhysicochemical).Volume - spatulaVolume;
		if((content as IPhysicochemical).Volume <= 0.07f) {
			content = null;
			hasSolid = false;
			if(infoCanvas != null) {
				infoCanvas.gameObject.SetActive(false);
			}
		}

		/*if ((compounds  as IPhysicochemical).Volume <= 0.0f) {
			compounds = null;
			hasSolid = false;
		}*/

		RefreshContents();
	}
}

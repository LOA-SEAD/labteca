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
	public float volume;
	public float uncalibrateVolume;
	public float mass;
	public bool calibrate;
	
	public GameObject liquid;
	public GameObject solid;

	public GameStateBase stateInUse;

	public float currentVolumeUsed;

	public List<ReagentsInGlass> reagents = new List<ReagentsInGlass>();

	[System.Serializable] /*!< Lets you embed a class with sub properties in the inspector. */
	public class ReagentsInGlass{
		public string reagentName;
		public float massReagent;
	}

	//!  Is called when the script instance is being loaded.
	void Awake()
	{
		solid.SetActive(false);
		liquid.SetActive(false);
	}

	// Use this for initialization
	//! Sets a mass to rigidbody
	void Start () 
	{
		this.rigidbody.mass = mass;
	}
	
	// Update is called once per frame
	/*void Update () 
	{
	
	}*/
	
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

		if(currentVolumeUsed < volume){

			float lastVolume = currentVolumeUsed;

			currentVolumeUsed += volumeLiquid;

			Debug.Log (massLiquid);

			if(currentVolumeUsed > volume){

				currentVolumeUsed = volume;
			}

			Debug.Log (massLiquid);

			liquid.SetActive(true);
			GetComponent<Rigidbody>().mass += massLiquid*(currentVolumeUsed-lastVolume);
		}
		else if(currentVolumeUsed >= volume ){

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
		currentVolumeUsed -= volumeLiquid;

		if(currentVolumeUsed < 0)
			currentVolumeUsed = 0;

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
		stateInUse.SendMessage ("ClickGlass", gameObject, SendMessageOptions.DontRequireReceiver);
	}
}

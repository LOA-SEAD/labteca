using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class Glassware : MonoBehaviour 
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

	[System.Serializable]
	public class ReagentsInGlass{
		public string reagentName;
		public float massReagent;
	}


	void Awake()
	{
		solid.SetActive(false);
		liquid.SetActive(false);
	}

	// Use this for initialization
	void Start () 
	{
		this.rigidbody.mass = mass;
	}
	
	// Update is called once per frame
	void Update () 
	{
	
	}
	

	public void AddSolid(float massSolid, string reagent){
		if(liquid.activeSelf == false)
			solid.SetActive(true);
		GetComponent<Rigidbody>().mass += massSolid;
	}

	public void RemoveSolid(float massSolid){
		GetComponent<Rigidbody>().mass -= massSolid;
		if(GetComponent<Rigidbody>().mass < mass){
			GetComponent<Rigidbody>().mass = mass;
			solid.SetActive(false);
		}
	}

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
			AlertDialogBehaviour.ShowAlert("Recipente esta cheio");

		}


	}

	public void AddLiquid(float volumeLiquid){
		AddLiquid (1, volumeLiquid);
	}
	
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

	public void SetStateInUse(GameStateBase state){
		stateInUse = state;

	}

	public void CLickInGlass(){
		stateInUse.SendMessage ("ClickGlass", gameObject, SendMessageOptions.DontRequireReceiver);
	}


}

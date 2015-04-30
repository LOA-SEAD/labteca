using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BalanceController : MonoBehaviour 
{
	private InventoryController inventory;

	public float realMass;

	public TextMesh balanceText;

	public float timeToCheckBalanceValue;
	private float timeToCheckBalanceValueAcc;

	public float errorAmplitude;
	public int errorPrecision;

	private List<GameObject> activeMass = new List<GameObject>();

	public float addRemoveValue;
	public float addRemoveError;


	

	public BalanceState balanceState;

	public ReagentsSolid solidSelected;
	

	void Awake()
	{
		inventory = FindObjectOfType (typeof(InventoryController)) as InventoryController;

		PlayerPrefs.SetFloat ("setupBalance", 0);
//		buttonAdd.SetActive(false);
//		buttonRemove.SetActive(false);
//		buttonFinishBalance.SetActive(false);
	}

	// Use this for initialization
	void Start () 
	{
		balanceState = GetComponent<BalanceState>();
	}
	
	// Update is called once per frame
	void Update () 
	{
		if (Time.time - timeToCheckBalanceValueAcc > timeToCheckBalanceValue && activeMass.Count > 0) 
		{
			balanceText.text = applyErrorInFloat(realMass).ToString();
			timeToCheckBalanceValueAcc = Time.time;
			RefreshEquipament();

		}


	}

	private float applyErrorInFloat(float realValue)
	{
		float value = Mathf.Round (realValue * Mathf.Pow (10f, errorPrecision) + (Random.Range(-1f,1f) * errorAmplitude));
		return value / Mathf.Pow (10f, errorPrecision);
	}

	public void AddMoreMassButton(float min, float max)
	{
		realMass += Random.Range(min , max);
	}

	public void RemoveMassButton()
	{
		realMass -= (addRemoveValue + Random.Range (-1f, 1f) * addRemoveError);
		if(realMass < PlayerPrefs.GetFloat ("setupBalance"))
		{
			realMass = PlayerPrefs.GetFloat ("setupBalance");
		}
	}



	public void SetupBalance()
	{
		PlayerPrefs.SetFloat ("setupBalance", realMass);
	}

	public void ResetBalance()
	{
		PlayerPrefs.SetFloat ("setupBalance", 0);
		RefreshEquipament ();
	}

	public Glassware GetGlassInEquipament(){
		Glassware glassToReturn = null;

		foreach(GameObject g in activeMass){
			if(g.GetComponent<Glassware>() != null){
				glassToReturn = g.GetComponent<Glassware>();
			}
		}
		return glassToReturn;
	}

	public void AddObjectInEquipament(GameObject objectToAdd){
		activeMass.Add(objectToAdd);
		RefreshEquipament();
	}

	public void RemoveObjectInEquipament(GameObject objectToRemove){
		activeMass.Remove(objectToRemove);
		RefreshEquipament();
		balanceText.text = applyErrorInFloat(realMass).ToString();
	}

	private void RefreshEquipament(){
		if(balanceState.IsRunning()){
			
			float tempMass = 0;
			
			foreach(GameObject g in activeMass){
				tempMass += g.GetComponent<Rigidbody>().mass;
				
			}
			
			realMass = tempMass - PlayerPrefs.GetFloat ("setupBalance");

			if(realMass < 0)
				realMass = 0;
		}
	}


}

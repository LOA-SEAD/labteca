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

	public GameObject buttonAdd;
	public GameObject buttonRemove;
	public GameObject buttonFinishBalance;

	public ReagentsSolid solidSelected;

	void Awake()
	{
		inventory = FindObjectOfType (typeof(InventoryController)) as InventoryController;
		realMass = PlayerPrefs.GetFloat ("setupBalance");

		buttonAdd.SetActive(false);
		buttonRemove.SetActive(false);
		buttonFinishBalance.SetActive(false);
	}

	// Use this for initialization
	void Start () 
	{
		inventory.UpdateInventory ();

		inventory.DisactiveAllReagentsLiquid ();
		inventory.DisactiveAllReagentsSolid ();
		inventory.ActiveAllGlassware ();
	}
	
	// Update is called once per frame
	void Update () 
	{
		if (Time.time - timeToCheckBalanceValueAcc > timeToCheckBalanceValue) 
		{
			balanceText.text = applyErrorInFloat(realMass).ToString();
			timeToCheckBalanceValueAcc = Time.time;

			//realMass = 0f;
			//activeMass.Clear();
		}
	}

	private float applyErrorInFloat(float realValue)
	{
		float value = Mathf.Round (realValue * Mathf.Pow (10f, errorPrecision) + (Random.Range(-1f,1f) * errorAmplitude));
		return value / Mathf.Pow (10f, errorPrecision);
	}

	public void AddMoreMassButton()
	{
		realMass += (addRemoveValue + Random.Range (-1f, 1f) * addRemoveError);
	}

	public void RemoveMassButton()
	{
		realMass -= (addRemoveValue + Random.Range (-1f, 1f) * addRemoveError);
		if(realMass < PlayerPrefs.GetFloat ("setupBalance"))
		{
			realMass = PlayerPrefs.GetFloat ("setupBalance");
		}
	}

	public void SelectedSolid()
	{
		buttonAdd.SetActive(true);
		buttonRemove.SetActive(true);
		buttonFinishBalance.SetActive(true);
	}


	public void FinishButton()
	{
		GameObject newReagent = Instantiate (solidSelected.gameObject) as GameObject ;
		newReagent.GetComponent<ReagentsSolid>().solidSource = false;
		newReagent.GetComponent<ReagentsSolid>().totalMass = realMass;
		DontDestroyOnLoad (newReagent);
		inventory.AddReagentSolid (newReagent.GetComponent<ReagentsSolid>());

		ResetBalance ();
	}

	void OnTriggerEnter(Collider other)
	{
		realMass += other.gameObject.rigidbody.mass;
		activeMass.Add (other.gameObject);
	}

	void OnTriggerStay(Collider other)
	{
		if (!activeMass.Contains (other.gameObject)) 
		{
			realMass += other.gameObject.rigidbody.mass;
			activeMass.Add (other.gameObject);	
		}
	}

	void OnTriggerExit(Collider other)
	{
		realMass -= other.gameObject.rigidbody.mass;
		activeMass.Remove (other.gameObject);
	}

	public void SetupBalance()
	{
		PlayerPrefs.SetFloat ("setupBalance", -realMass);
		realMass -= realMass;
	}

	public void ResetBalance()
	{
		Application.LoadLevel (Application.loadedLevelName);
	}
}

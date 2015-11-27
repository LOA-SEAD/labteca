using UnityEngine;
using System.Collections;
using System.Collections.Generic;

//! Scale Controller
/*! This is the controller for the Scale, it has all the methods that allows the Scale to work with 
 * the Glassware and Reagents, calculating their mass together and displaying it.
 */
public class ScaleController : EquipmentControllerBase 
{
	private InventoryController inventory;  // TODO: linkar c/ Inventory, se necessario aqui nesse script.

	public float previousMass;
    public float realMass;  /*!< Real mass that is on the scale. */
	public bool changed;
    // TODO: quando isso foi implementado a versao do Unity era anterior a 4.6, da pra usar Text da nova UI do Unity.
	public TextMesh balanceText;    /*!< Text display of Scale. */

	public float timeToCheckBalanceValue;
	private float timeToCheckBalanceValueAcc;

    // TODO: verificar com a profa. Teca a respeito dessa variacao em casas decimais, ela disse que n fica 'variando' muito.
	public float errorAmplitude;    /*!< Float number representing the 'error' amplitude. */
    public int errorPrecision;      /*!< Int number that defines the scale's precision. */

	public List<GameObject> activeMass = new List<GameObject>();   /*!< List of GameObjects that composes the mass. */

    // TODO: Add Remove tah bem, mas bem, confuso.
	public float addRemoveValue;    /*!< Float number to add remove value? */
    public float addRemoveError;    /*!< Float number to add remove error? */

    public ScaleState balanceState;       /*!< BalanceState component. */
	public ReagentsSolid solidSelected;     /*!< ReagentsSolid component. */
	
    // Awake happens before all Start()
	void Awake()
	{
	    // TODO: quando implemeentar o inventario, talvez precise alterar isso.
        inventory = FindObjectOfType (typeof(InventoryController)) as InventoryController;
		PlayerPrefs.SetFloat ("setupBalance", 0);
//		buttonAdd.SetActive(false);
//		buttonRemove.SetActive(false);
//		buttonFinishBalance.SetActive(false);
	}

	void Start () 
	{
		previousMass = 0;
        // TODO: se pega o componente BalanceState dentro do proprio GameObject pq variavel publica entao?
		balanceState = GetComponent<ScaleState>();
		timeToCheckBalanceValueAcc = 0;
		changed = false;
	}

	void Update () 
	{
		RefreshEquipament ();
		if (previousMass != realMass) {
			timeToCheckBalanceValueAcc=0;
			previousMass=realMass;
		}
        // Creates the 'fluctuation' effect on Scale display.
		if (timeToCheckBalanceValueAcc < timeToCheckBalanceValue && activeMass.Count > 0) {
			balanceText.text = applyErrorInFloat (realMass).ToString ();
			timeToCheckBalanceValueAcc += Time.fixedDeltaTime;
			Debug.Log (timeToCheckBalanceValueAcc);
			RefreshEquipament ();
		} 
	}

    //! Apply the "error" on Scale value - the value is rounded and sometimes the display keeps changing the value.
	private float applyErrorInFloat(float realValue)
	{
		float value = Mathf.Round ((realValue + (Random.Range(-1f,1f) * errorAmplitude)*realValue) * Mathf.Pow (10f, errorPrecision));
		return value/ Mathf.Pow (10f, errorPrecision);
	}

    //! Add more mass to Real Mass.
    /*! Receives a float 'min' and float 'max' numbers and Real Mass receives a float value in Range(min, max). */
	public void AddMoreMassButton(float min, float max)
	{
		realMass += Random.Range(min , max);
		changed = true;
	}

    //! Remove mass from Real Mass.
    /*! Removes mass within a Range(-1f, 1f) * addRemoveError from Real Mass. */
    public void RemoveMassButton()
	{
		realMass -= (addRemoveValue + Random.Range (-1f, 1f) * addRemoveError);
		if(realMass < PlayerPrefs.GetFloat ("setupBalance"))
		{
			realMass = PlayerPrefs.GetFloat ("setupBalance");
		}
		changed = true;
	}
    
    //! Set PlayerPrefs "setupBalance" to realMass.
	public void SetupBalance()
	{

		PlayerPrefs.SetFloat ("setupBalance", realMass);
		changed = true;
	}

    //! Set PlayerPrefs "setupBalance" to zero.
	public void ResetBalance()
	{
		PlayerPrefs.SetFloat ("setupBalance", 0);
		RefreshEquipament ();
		changed = true;
	}

    //! Get Glassware that is on Scale.
	public Glassware GetGlassInEquipament(){
		Glassware glassToReturn = null;

		foreach(GameObject g in activeMass){
			if(g.GetComponent<Glassware>() != null){
				glassToReturn = g.GetComponent<Glassware>();
			}
		}
		return glassToReturn;
	}

    //! Add a GameObject to be measured on Scale.
	public override void AddObjectInEquipament(GameObject objectToAdd){
		activeMass.Add(objectToAdd);
		RefreshEquipament();
	}

    //! Remove a GameObject from being measured on Scale.
	public override void RemoveObjectInEquipament(GameObject objectToRemove){
		activeMass.Remove(objectToRemove);
		RefreshEquipament();
		balanceText.text = applyErrorInFloat(realMass).ToString();
	}

    //! Update Real Mass to the mass of all GameObjects on ActiveMass.
    // TODO: refatorar nome em ingles incorreto - Equipment
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

using UnityEngine;
using System.Collections;
using UnityEngine.UI;

//! Alert Dialog
/*! */
//TODO: Para testes?
public class AlertDialogBehaviour : MonoBehaviour {

	//public static AlertDialogBehaviour instance;
	public Text textAlert;

	// Use this for initialization
	void Start () 
    {
	    if(this.enabled)
        {
            this.gameObject.SetActive(false);
        }
    }

    public void ShowAlert(string text)
    {
        textAlert.text = text;
        this.gameObject.SetActive(true);

	}

	public void CloseAlert()
    {
        textAlert.text = "";
        this.gameObject.SetActive(false);	
	}

	public bool IsShowing()
    {
		if(this.gameObject.activeSelf)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}

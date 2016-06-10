using UnityEngine;
using System.Collections;
using UnityEngine.UI;

//! Alert Dialog
/*! Class to create alert dialogs in the screen
 * Is used in /States/PrecisionScale/UI/AlertDialog
 */
public class AlertDialogBehaviour : MonoBehaviour {

	//public static AlertDialogBehaviour instance;
	public Text textAlert;


    public void ShowAlert(string text)
    {
        textAlert.text = text;
		this.gameObject.SetActive (true);

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

using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class AlertDialogBehaviour : MonoBehaviour {

	public static AlertDialogBehaviour instance;
	public Text textAlert;
	public GameObject boxAlert;
	private bool show;

	// Use this for initialization
	void Start () {

		instance = this;
		instance.boxAlert.SetActive(false);
	
	}
	
	// Update is called once per frame
	void Update () {

		if (Input.GetKeyDown (KeyCode.O)) {
			Debug.Log(show);
		}
	
	}

	public static void ShowAlert(string text){
		instance.textAlert.text = text;

		instance.boxAlert.SetActive(true);
		instance.show = true;

	
	}

	public void CloseAlert(){
		
		instance.boxAlert.SetActive(false);
		instance.show = false;
		
		
	}

	public static bool IsShowing(){
		return instance.show;
	}
}

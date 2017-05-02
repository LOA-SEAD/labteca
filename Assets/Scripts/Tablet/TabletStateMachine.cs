using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;


public class TabletStateMachine : MonoBehaviour {

	public List<TabletState> states;
	public HUDController control;
	public Text time;
	public List<GameObject> localNotification;
	public static List<GameObject> notification = new List<GameObject> (3);
	
	// Use this for initialization
	void Start () {
		foreach (GameObject notificationObject in localNotification)
			notification.Add (notificationObject);
		SendNotification ((int)TabletStates.ExperimentsMenu);
		resetState ();
	}

	public static void SendNotification(int i){
		notification[i].SetActive (true);
		notification[(int)TabletStates.Main].SetActive (true);
	}

	public static void CloseNotification(int i){
		Debug.Log ("closing");
		if (notification [i].activeSelf)
			notification [i].SetActive (false);
		if (!notification [(int)TabletStates.ExperimentsMenu].activeSelf &&
			!notification [(int)TabletStates.GraphsMenu].activeSelf)
			notification [(int)TabletStates.Main].SetActive (false);
	}

	public void resetState(){
		goToState ((int)TabletStates.Main);
	}

	public void goToState(int index){
		if (index == (int)TabletStates.ExperimentsMenu || index == (int)TabletStates.GraphsMenu)
			CloseNotification (index);

		if ((int)TabletStates.Notes == index)
			control.LockKeys (true);
		else
			control.LockKeys (false);

		foreach (TabletState ts in states) {
			if((int)ts.StateType!=index){
				ts.GetCanvasGroup().alpha = 0f;
				ts.GetCanvasGroup().blocksRaycasts = false;
				ts.GetCanvasGroup().interactable = false;
			}else{
				ts.GetCanvasGroup().alpha = 1f;
				ts.GetCanvasGroup().blocksRaycasts = true;
				ts.GetCanvasGroup().interactable = true;
			}
		}
	}
	
	// Update is called once per frame
	void Update () {
		time.text = GetTime();
	}

	private string GetTime(){
		string str = "";
		str += System.DateTime.Now.Hour >= 10 ? System.DateTime.Now.Hour.ToString () :
											   "0" + System.DateTime.Now.Hour.ToString ();
		str += ":";
		str += System.DateTime.Now.Minute >= 10 ? System.DateTime.Now.Minute.ToString () :
											   "0" + System.DateTime.Now.Minute.ToString ();

		return str;
	}
}

public enum TabletStates{
	Main=0,
	ExperimentsMenu=1,
	GraphsMenu=2,
	Experiments=3,
	Notes=4,
	HandbookMenu=5,
	Graphs=6,
	ReagentInfo=7
}
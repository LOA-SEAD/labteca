using UnityEngine;
using System.Collections;

public class InventoryControl : MonoBehaviour {

	public GameObject inventory,tablet;
	// Use this for initialization
	void Start () {
		setTabletState (false);
		setInventoryState (false);
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void setTabletState(bool state){
		tablet.GetComponent<CanvasGroup> ().blocksRaycasts = state;
		tablet.GetComponent<CanvasGroup> ().interactable = state;
		tablet.GetComponent<CanvasGroup> ().alpha = state ? 1f : 0f;
	}

	public void setInventoryState(bool state){
		if(inventory.activeSelf!=state)
			inventory.SetActive (state);
		if(state==false)
			GameObject.Find("InventoryManager").GetComponent<InventoryManager>().actionTab.SetActive (false);
	}
}

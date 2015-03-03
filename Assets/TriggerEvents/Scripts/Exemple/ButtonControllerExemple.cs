using UnityEngine;
using System.Collections;

public class ButtonControllerExemple : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		
		Ray rayMouse = Camera.main.ScreenPointToRay(new Vector3(Input.mousePosition.x, Input.mousePosition.y, Input.mousePosition.z));
		
		RaycastHit hit;
		
		if(Physics.Raycast(rayMouse, out hit)){
			if(hit.collider.GetComponent<Button3DExemple>() != null && Input.GetMouseButtonDown(0)){
				hit.collider.gameObject.SendMessage("OnClick", SendMessageOptions.DontRequireReceiver);
			}
		}
	
	}
}

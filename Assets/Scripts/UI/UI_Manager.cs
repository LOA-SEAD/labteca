using UnityEngine;
using System.Collections;

public class UI_Manager : MonoBehaviour {

    public AlertDialogBehaviour alertDialog;
    private GameObject currentDialog;

	void Start () {
        currentDialog = null;
	}

    void Update()
    {
        // TODO: Raycast para click do mouse em objetos
        /*  
        if (Input.GetMouseButtonDown(0))
        {
            Debug.Log("Mouse Left Button clicked!");
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, 1))
            {
                Debug.DrawLine(ray.origin, hit.point);
                Debug.Log("Collider: " + hit.collider.name);
            }
        }
         */
    }

    public GameObject getCurrentDialog()
    {
        return this.currentDialog;
    }

    public void setCurrentDialog(GameObject dialog)
    {
        if (this.currentDialog == null)
            this.currentDialog = dialog;
        else
            Debug.Log("There is a dialog already set to currentDialog");
    }

    public void setDialogActive(bool condition)
    {
        if (this.currentDialog != null)
            this.currentDialog.SetActive(condition);
        else
            Debug.Log("No currentDialog set, cant set condition for null");
    }

}

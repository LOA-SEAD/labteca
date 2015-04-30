using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

public class UI_ObjectManager : MonoBehaviour {

    // GameObject with all objects and their button canvas
    public GameObject interactiveObjects;
    
    public Canvas baseCanvas;   // prefab canvas_base
    public Canvas[] objCanvas;  // canvas that will be used

    private Button[] intObjButtons; // objects' button that are inside interactiveObjects
    private Canvas activeCanvas = null;
    private bool objLock = false;

    void Start()
    {
        // start variables
        this.baseCanvas.enabled = false;
        this.activeCanvas = null;
        this.objLock = false;

        // disable all canvas when loaded
        foreach (Canvas c in objCanvas)
        {
            //print(c.name);
            c.enabled = false;
        }
        
        // get all buttons inside gameObject interactiveObjects
        this.intObjButtons = interactiveObjects.GetComponentsInChildren<Button>();
        
        // get all interactive buttons' name and print on Debug.Log
        // ---- for debug only ---
        foreach(Button b in intObjButtons)
        {
            string btn_obj_name = b.GetComponent<ButtonObject>().getBtnObjName();
            Debug.Log("Button: " + btn_obj_name + " loaded.");
        }
        // ---- ---- ---- ---- ----
    }

    public bool openCanvas(Canvas c)
    {
        if(objLock == false)    // no canvas already opened
        {
            this.activeCanvas = c;  // activeCanvas now is c
            this.baseCanvas.enabled = true; // enable base canvas
            c.enabled = true;   // enable canvas
            disableIntObjectsButtons(); // disable all interactive buttons
            Debug.Log("Open Canvas: " + c.name);
            return true;
        }
        else
        {
            Debug.Log("ActiveCanvas already set: " + c.name);
            return false;
        }
    }

    public Canvas getActiveCanvas()
    {
        if (activeCanvas != null)   // there is an active canvas
            Debug.Log("Active Canvas: " + activeCanvas.name);
        else
            Debug.Log("No Active Canvas found");
        return this.activeCanvas;
    }

    public bool closeActiveCanvas()
    {
        if (this.activeCanvas != null)  // there is an active canvas
        {
            Debug.Log("Closing Canvas: " + this.activeCanvas.name);
            enableIntObjectsButtons();  // enable all interactive objects
            this.activeCanvas.enabled = false;  // disable active canvas
            this.baseCanvas.enabled = false;    // disable base canvas
            this.activeCanvas = null;
            return true;
        }
        else
        {
            Debug.Log("No Active Canvas to be closed.");
            return false;
        }
    }

    private void disableIntObjectsButtons()
    {
        // Disable all interactive objects
        this.objLock = true;
        foreach(Button b in intObjButtons)
            b.GetComponentInParent<Canvas>().enabled = false;
    }

    private void enableIntObjectsButtons()
    {
        // Enable all interactive objects
        this.objLock = false;
        foreach (Button b in intObjButtons)
            b.GetComponentInParent<Canvas>().enabled = true;
    }

    /*
     * The following methods can be used in any button inside the scene.
     * They use the methods above: openCanvas, getActiveCanvas and closeActiveCanvas
     * but they return void to allow buttons to use it.
     */
    public void btnOpenCanvas(Canvas c)
    {
        if (openCanvas(c))
            Debug.Log("Canvas loaded: " + c.name);
        else
            Debug.Log("Cannot load " + c.name + ".\n"
                + getActiveCanvas().name + " already active");
    }

    public void btnGetActiveCanvas()
    {
        if (getActiveCanvas() != null)
            Debug.Log("Active Canvas: " + getActiveCanvas().name);
        else
            Debug.Log("No Active Canvas found");
    }

    public void btnCloseActiveCanvas()
    {
        if (closeActiveCanvas())
            Debug.Log("Close active canvas");
        else
            Debug.Log("No Active Canvas found");
    }
}

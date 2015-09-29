using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

// exTODO: Alterar controle por Canvas para Raycast vindo do UI_Manager
/* Explicacao: Essa classe controla Canvas individuais dentro de cada objeto interagivel. Ou seja, na Balanca, a pisseta,
 * a espatula, etc, sao interagiveis pois neles ha um Canvas posicionado World Space e nesse canvas ha um botao que ocupa todo 
 * o espaco. Atualmente esta classe nao esta implementada no jogo, mas a maneira como foi pensada pode ser encontrada em:
 * 'States\PrecisionScale\InteractiveObjects\' sendo que esta classe deveria estar em InteractiveObjects.
 * 
 * Essa foi uma solucao imediatada mas nao eh a mais inteligente, ha um codigo comentado para utilizacao de Raycast 
 * dentro de UI_Manager.
 */

/* Nota do Leo²:
 * Assim como falei na anterior, atualmente nao da pra alterar isso,
 * caso tenhamos tempo no futuro, podemos refatorar isso :\
 */

//! Class that works with 3D items that has mouse interaction. 
/*! This class is used inside a GameObject that has all the items as children and respective Canvas World Positioned,
 *  it get's all canvas and check which one is enabled at time and manages all of them.
 */
public class UI_ObjectManager : MonoBehaviour {

    // GameObject with all objects and their button canvas
    public GameObject interactiveObjects;
    
    //public Canvas baseCanvas;   // prefab canvas_base
    public AlertDialogBehaviour alertDialog;
    public Canvas[] objCanvas;  // canvas that will be used

    private Button[] intObjButtons; // objects' button that are inside interactiveObjects
    private Canvas activeCanvas = null;
    private bool objLock = false;

    void Start()
    {
        // start variables
        //this.baseCanvas.enabled = false;
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

    //! Open Canvas showing it at current camera.
    /*! Checks if any other canvas is already being shown and decides if the selected Canvas can be shown. */
    public bool openCanvas(Canvas c)
    {
        if(objLock == false)    // no canvas already opened
        {
            this.activeCanvas = c;  // activeCanvas now is c
            //this.baseCanvas.enabled = true; // enable base canvas
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

    //! Get the current active Canvas at current camera.
    /*! Return null if there isn't any active canvas or the active canvas. */
    public Canvas getActiveCanvas()
    {
        if (activeCanvas != null)   // there is an active canvas
            Debug.Log("Active Canvas: " + activeCanvas.name);
        else
            Debug.Log("No Active Canvas found");
        return this.activeCanvas;
    }

    //! Close any shown canvas at current camera.
    /*! If there is an active canvas, it is disabled. */
    public bool closeActiveCanvas()
    {
        if (this.activeCanvas != null)  // there is an active canvas
        {
            Debug.Log("Closing Canvas: " + this.activeCanvas.name);
            enableIntObjectsButtons();  // enable all interactive objects
            this.activeCanvas.enabled = false;  // disable active canvas
            //this.baseCanvas.enabled = false;    // disable base canvas
            this.activeCanvas = null;
            return true;
        }
        else
        {
            Debug.Log("No Active Canvas to be closed.");
            return false;
        }
    }

    //! Disable interaction with objects.
    /*! All canvas that is being used as 'button' inside objects, are disabled so no interaction can be made. */
    private void disableIntObjectsButtons()
    {
        // Disable all interactive objects
        this.objLock = true;
        foreach(Button b in intObjButtons)
            b.GetComponentInParent<Canvas>().enabled = false;
    }

    //! Enable interaction with objects.
    /*! All canvas that is being used as 'button' inside objects, are enabled so interaction can be made. */
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

    //! Interface for method Open Canvas to be used in a button.
    public void btnOpenCanvas(Canvas c)
    {
        if (openCanvas(c))
            Debug.Log("Canvas loaded: " + c.name);
        else
            Debug.Log("Cannot load " + c.name + ".\n"
                + getActiveCanvas().name + " already active");
    }

    //! Interface for method Get Active Canvas to be used in a button.
    public void btnGetActiveCanvas()
    {
        if (getActiveCanvas() != null)
            Debug.Log("Active Canvas: " + getActiveCanvas().name);
        else
            Debug.Log("No Active Canvas found");
    }

    //! Interface for method Close Active Canvas to be used in a button.
    public void btnCloseActiveCanvas()
    {
        if (closeActiveCanvas())
            Debug.Log("Close active canvas");
        else
            Debug.Log("No Active Canvas found");
    }

}

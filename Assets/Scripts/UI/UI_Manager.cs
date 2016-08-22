using UnityEngine;
using System.Collections;

//! UI Manager for all Dialogs that will be shown to the player inside 'States'.
/*! This UI Manager controls what is being shown and which dialog will be used accordingly to the 
 *  object that was clicked inside the current 'state'.
 */
public class UI_Manager : MonoBehaviour {

    public AlertDialogBehaviour alertDialog;    /*!< Prefab of Alert Dialog. */
    private GameObject currentDialog;           /*!< Current dialog being shown. */

	void Start () {
        currentDialog = null;
	}

    void Update()
    {
        // exTODO: Raycast para click do mouse em objetos
        /* Explicacao: O problema aqui eh o fato de que cada 'State' tem uma camera diferente e na troca de estados ha Disable e Enable 
         * para a camera do Player e camera do 'State'. Para o Raycast funcionar eh preciso usar uma camera, e como nao ha tratamento nenhum de
         * Camera nos estados, apenas o Disable e Enable, o Raycast nao encontra a 'Main Camera' nem 'Current Camera' sendo assim 
         * da crash na hora de tentar fazer o Raycast. 
         */
		/*Nota do Leo:
		 * Conversei com Delano e a Joice, e eles disseram pra usar do jeito que esta,
		 * apesar de o raycast ser a melhor forma de fazer isso, daria algum trabalho refatorar tudo
		 * e estamos correndo contra o tempo, ou seja, quando tivermos mais tempo(se algum dia isso rolar)
		 * podemos tentar refatorar isso :)
		 */
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

	public void OnClick(){
		GetComponentInParent<WorkbenchInteractive> ().OnClick ();
	}

    //! Get the current dialog that is being shown.
    public GameObject getCurrentDialog()
    {
        return this.currentDialog;
    }

    //! Set the dialog that should be shown.
    /*! Check if there is any active Dialog and if isn't set current as the one that was passed as parameter. */
    public void setCurrentDialog(GameObject dialog)
    {
        if (this.currentDialog == null)
            this.currentDialog = dialog;
        else
            Debug.Log("There is a dialog already set to currentDialog");
    }

    //! Set current dialog Active condition.
    /*! If true the current dialog will be exhibited, if false hidden. */
    public void setDialogActive(bool condition)
    {
        if (this.currentDialog != null)
            this.currentDialog.SetActive(condition);
        else
            Debug.Log("No currentDialog set, cant set condition for null");
    }

}

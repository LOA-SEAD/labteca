using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

//! Get Glassware State is when the Player access the cupboard with Glassware.
/*! This state allows the Player to access the cupboard so one can add Glassware to the inventory. */
public  class GetGlasswareState : GameStateBase {

    public Camera cameraState;          /*!< Camera for this State. */
    public GameObject interactBox;      /*!< Box Colider to allow interaction. */
    public GameObject canvasUI;         /*!< Canvas where the UI will be shown. */

    public DoorBehaviour leftDoor;      /*!< GameObject that contains the left door. */
    public DoorBehaviour rightDoor;     /*!< GameObject that contains the right door. */
    public Glassware[] glasswareList;   /*!< List of Glassware that are inside. */

    // TODO: Change ScrollRect variables to match the script GetReagentState.
	public RectTransform UIScrollList;
	public RectTransform startPoint;
	public GlasswareUiItemBehaviour glasswarePrefab;

	private Vector3 currentPosition;
	public float offSetItens;

	public void Start () {
		cameraState.gameObject.SetActive(false);
		canvasUI.GetComponent<Canvas>().enabled = false;
		
        // TODO: hardcode para criar a lista de itens dentro do armario. Alterar para o mesmo que no GetReagentState.
        // Explicacao: basta seguir o exemplo do GetReagentState. Eh bem provavel que alteracoes na Cena da Unity sejam necessarias.

        // ------- daqui \/ ---------------------
        currentPosition = startPoint.localPosition;

		Rect rectScroll = UIScrollList.rect;

		rectScroll.height = (glasswarePrefab.GetComponent<RectTransform>().rect.height * (glasswareList.Length)) + (glasswareList.Length *offSetItens);

		Vector3 newScale = new Vector3(1,1,1);

		newScale.y *= glasswareList.Length;

		RectTransform newUIScrollList = UIScrollList;

		newUIScrollList.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Bottom, 0, rectScroll.height);

		UIScrollList = newUIScrollList;

		for(int i = 0; i<glasswareList.Length; i++)
        {
			GameObject tempGlass = Instantiate(glasswarePrefab.gameObject,  currentPosition, startPoint.rotation) as GameObject;
			tempGlass.transform.SetParent(UIScrollList.transform, false);
			tempGlass.GetComponent<RectTransform>().localPosition = currentPosition;
			tempGlass.GetComponent<GlasswareUiItemBehaviour>().SetGlass(glasswareList[i]);

			currentPosition.y -= glasswarePrefab.GetComponent<RectTransform>().rect.height + offSetItens;
		}
        // ------ ateh aqui /\ -----------------
	}
	
	protected override void UpdateState ()
	{

	}

	void Update(){
		base.Update();

		if(Input.GetKeyDown(KeyCode.Escape) && canRun){
			ExitState();
		}

	}

    //! Actions for when the State starts.
    /*! Set the Camera inside the state to be Active, overlaying the Main Camera used at InGameState,
     * does the animation to open the cupboard left door and enable the UI Canvas. */
	public override void OnStartRun ()
	{
		cameraState.gameObject.SetActive(true);
        //Camera.main.CopyFrom(cameraState);
		HudText.SetText("");

		leftDoor.Open();
        canvasUI.GetComponent<Canvas>().enabled = true;

		UIScrollList.transform.parent.GetComponent<ScrollRect>().verticalScrollbar.value = 1;

	}

    //! Actions for when exits the State.
    /*! Enable the BoxCollider so the cupboard can be accessed again, change the Game State to 'Default' and 
     * plays a fade script. */
	public void ExitState(){
		interactBox.GetComponent<BoxCollider>().enabled = true;
		gameController.ChangeState(0);
		FadeScript.instance.ShowFade();
	}

    //! Actions for when the State stops.
    /*! Disable the Camera inside state, the Canvas for UI and plays animation to close the left door. */
	public override void OnStopRun ()
	{
		cameraState.gameObject.SetActive(false);
		canvasUI.GetComponent<Canvas>().enabled = false;

		leftDoor.Close();


	}


}

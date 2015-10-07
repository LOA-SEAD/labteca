using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

//! Get Glassware State is when the Player access the cupboard with Glassware.
/*! This state allows the Player to access the cupboard so one can add Glassware to the inventory. */
public  class GetGlasswareState : GameStateBase {

    public Camera cameraState;          /*!< Camera for this State. */
    public GameObject interactBox;      /*!< Box Colider to allow interaction. */
    public DoorBehaviour leftDoor;      /*!< GameObject that contains the left door. */
    public DoorBehaviour rightDoor;     /*!< GameObject that contains the right door. */
    public Glassware[] glasswareList;   /*!< List of Glassware that are inside. */

    //UI
	public GlasswareUiItemBehaviour glasswarePrefab;/*!< Prefab with reagent list layout. */
	public Canvas canvasUI;                         /*!< Canvas where the UI will be shown. */
	public float offSetItens;                       /*!< Offset for each reagent on the list. */
	
	// ScrollRect variables
	private ScrollRect UIScrollList;
	private Vector3 currentPosition;
	private int lastItemPos = 0;
	private RectTransform contentRect, prefabRect;

	//Prefabs to instantiate the glasswares
	//! Needs to be attached in the scene
	public AnyObjectInstantiation beaker;
	public AnyObjectInstantiation volumetricFlask0;
	public AnyObjectInstantiation volumetricFlask1;
	public AnyObjectInstantiation volumetricFlask2;
	public AnyObjectInstantiation volumetricFlask3;

	public void Start () {
		cameraState.gameObject.SetActive(false);
		UIScrollList = canvasUI.GetComponentInChildren<ScrollRect>();

		prefabRect = glasswarePrefab.GetComponent<RectTransform>();		
		contentRect = UIScrollList.content;

		for(int i=0; i < glasswareList.Length;i++)
		{
			// calculate y position
			float y = (prefabRect.rect.height + offSetItens) * lastItemPos ;
			
			// set position
			Vector3 currentPos = new Vector3(1f, -y);
			//Debug.Log("Current y position: " + y );
			
			// resize content rect
			contentRect.sizeDelta = new Vector2(
				1f, // width doesnt change
				prefabRect.rect.height + (prefabRect.rect.height + offSetItens) * lastItemPos);
			
			// instantiate Item
			GameObject tempItem = Instantiate(prefabRect.gameObject,
			                                  currentPos,
			                                  prefabRect.transform.rotation) as GameObject;
			
			// set reagent's name
			tempItem.GetComponent<GlasswareUiItemBehaviour>().SetGlass(glasswareList[i]);
			// next position on inventory grid
			lastItemPos++;
			
			// set new item parent to scroll rect content
			tempItem.transform.SetParent(contentRect.transform, false);
			
		}

        // ------- daqui \/ ---------------------
        /*currentPosition = startPoint.localPosition;

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
		}*/
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

		//UIScrollList.transform.parent.GetComponent<ScrollRect>().verticalScrollbar.value = 1;

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

	//! Instantiation of the reagent when clicked
	/*! Uses the name of the reagent clicked to instantiate an game object (AnyObjectInstantiation) that will be
	 *!	added to the inventory*/
	/*public AnyObjectInstantiation GlasswareInstantiation (string glassName) {
		Glassware instantiatingGlass;
	

		return solidReagent;
	}*/




}

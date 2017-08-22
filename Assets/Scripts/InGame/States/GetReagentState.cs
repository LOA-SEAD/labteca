using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

//! Get Reagent State is when the Player access the cupboard with Reagents.
/*! This state allows the Player to access the cupboard so one can add Reagents to the inventory. */
public  class GetReagentState : GameStateBase, GetInterface {

	//public Camera cameraState;          /*!< Camera for this State. */
    public DoorBehaviour leftDoor;      /*!< GameObject that contains the left door. */
    public DoorBehaviour rightDoor;     /*!< GameObject that contains the right door. */
	public Dictionary<string, Compound> reagents = new Dictionary<string, Compound>(); /*!< Dictionary that stores all reagents>!*/

    // UI
    public Canvas canvasUI;                         /*!< Canvas where the UI will be shown. */
    public ReagentUiItemBehaviour reagentPrefab;    /*!< Prefab with reagent list layout. */
    public float offSetItens;                       /*!< Offset for each reagent on the list. */

    // ScrollRect variables
    private ScrollRect UIScrollList;
    private Vector3 currentPosition;
    private int lastItemPos = 0;
    private RectTransform contentRect, prefabRect;

	//Prefabs to instantiate the reagents
	//! Needs to be attached in the scene
	public GameObject solidPrefab;
	public GameObject liquidPrefab;

	public bool isGlassware(){
		return false;
	}

	public void Start () {
        cameraState.gameObject.SetActive(false);
		Compound actualReagent;
		reagents = CompoundFactory.GetInstance().CupboardCollection;

        // Set-up components
        if (canvasUI == null)   
            Debug.LogError("Canvas not found in GetReagentState");
        
        UIScrollList = canvasUI.GetComponentInChildren<ScrollRect>();
        if (UIScrollList == null)
            Debug.LogError("ScrollRect not found in GetReagentState");

        prefabRect = reagentPrefab.GetComponent<RectTransform>();
        
        contentRect = UIScrollList.content;

		// Store keys in a List
		List<string> list = new List<string>(reagents.Keys);
		// Loop through list
		foreach (string k in list)
		{
			reagents.TryGetValue(k,out actualReagent);
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
			if(actualReagent.IsSolid)
				tempItem.GetComponent<ReagentUiItemBehaviour>().SetReagent(actualReagent.Name, solidPrefab);
			else
				tempItem.GetComponent<ReagentUiItemBehaviour>().SetReagent(actualReagent.Name, liquidPrefab);
            // next position on inventory grid
            lastItemPos++;

            // set new item parent to scroll rect content
            tempItem.transform.SetParent(contentRect.transform, false);

        }
        
        //  Old code
        /* 
        currentPosition = startPoint.localPosition;

		Rect rectScroll = UIScrollList.rect;

		rectScroll.height = (reagentPrefab.GetComponent<RectTransform>().rect.height * (reagentList.Length)) + (reagentList.Length *offSetItens);

		RectTransform newUIScrollList = UIScrollList;

		newUIScrollList.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Bottom, 0, rectScroll.height);

		UIScrollList = newUIScrollList;

		for(int i = 0; i<reagentList.Length; i++){

			GameObject tempReagent = Instantiate(reagentPrefab.gameObject,  currentPosition, startPoint.rotation) as GameObject;
			tempReagent.transform.SetParent(UIScrollList.transform, false);
			tempReagent.GetComponent<RectTransform>().localPosition = currentPosition;
			tempReagent.GetComponent<ReagentUiItemBehaviour>().SetReagent(reagentList[i]);

			currentPosition.y -= reagentPrefab.GetComponent<RectTransform>().rect.height + offSetItens;

		}
        */
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
		HudText.SetText("");

		leftDoor.Open();
		canvasUI.GetComponent<Canvas>().enabled = true;

		//UIScrollList.transform.parent.GetComponent<ScrollRect>().verticalScrollbar.value = 1;

	}
    
    //! Actions for when exits the State.
    /*! Enable the BoxCollider so the cupboard can be accessed again, change the Game State to 'Default' and 
     * plays a fade script. */
	public override void ExitState(){
		interactBox.GetComponent<BoxCollider>().enabled = true;
		gameController.ChangeState(0);
	}

    //! Actions for when the State stops.
    /*! Disable the Camera inside state, the Canvas for UI and plays animation to close the left door. */
	public override void OnStopRun ()
	{
		cameraState.gameObject.SetActive(false);
		canvasUI.GetComponent<Canvas>().enabled = false;

        leftDoor.Close();
	}


	//TODO: make a way to set the right texture/models for the reagents
	//! Instantiation of the reagent when clicked
	/*! Uses the name of the reagent clicked to instantiate a game object (AnyObjectInstantiation) that will be
	 *!	added to the inventory */
	public AnyObjectInstantiation ReagentInstantiation (string reagentName) {
		Compound instantiatingReagent;
		reagents.TryGetValue (reagentName, out instantiatingReagent);

		if(instantiatingReagent.IsSolid == true) {
			/*AnyObjectInstantiation solidReagent = Instantiate(solidPrefab) as AnyObjectInstantiation;
			solidReagent.name = reagentName;
			return solidReagent;*/
			GameObject solidReagent = Instantiate(solidPrefab);
			solidReagent.name = reagentName;
			return solidReagent.GetComponent<AnyObjectInstantiation> ();
		} else {
			/*AnyObjectInstantiation liquidReagent = Instantiate (liquidPrefab) as AnyObjectInstantiation;
			liquidReagent.name = reagentName;
			return liquidReagent;*/
			GameObject liquidReagent = Instantiate (liquidPrefab);
			liquidReagent.name = reagentName;
			return liquidReagent.GetComponent<AnyObjectInstantiation>();
		}
	}

}

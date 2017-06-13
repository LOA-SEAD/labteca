using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

//! Get Glassware State is when the Player access the cupboard with Glassware.
/*! This state allows the Player to access the cupboard so one can add Glassware to the inventory. */
public  class GetGlasswareState : GameStateBase, GetInterface {

    //public Camera cameraState;          /*!< Camera for this State. */
    public DoorBehaviour leftDoor;      /*!< GameObject that contains the left door. */
    public DoorBehaviour rightDoor;     /*!< GameObject that contains the right door. */
    public Glassware[] glasswareList;   /*!< List of Glassware that are inside. */

	public AnyObjectInstantiation glasswareToInventory;


    //UI
	public GlasswareUiItemBehaviour glasswarePrefab;/*!< Prefab with reagent list layout. */
	public Canvas canvasUI;                         /*!< Canvas where the UI will be shown. */
	public float offSetItens;                       /*!< Offset for each reagent on the list. */
	
	// ScrollRect variables
	private ScrollRect UIScrollList;
	private Vector3 currentPosition;
	private int lastItemPos = 0;
	private RectTransform contentRect, prefabRect;

	public bool isGlassware(){
		return true;
	}

	public void Start () {
		cameraState.gameObject.SetActive(false);
		UIScrollList = canvasUI.GetComponentInChildren<ScrollRect>();

		prefabRect = glasswarePrefab.GetComponent<RectTransform>();		
		contentRect = UIScrollList.content;

		for(int i=0; i < glasswareList.Length;i++)
		{
			// calculate y position
			float y = (prefabRect.rect.height + offSetItens) * lastItemPos;
			
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
			tempItem.GetComponent<GlasswareUiItemBehaviour>().SetGlass(glasswareList[i], i);
			// next position on inventory grid
			lastItemPos++;
			
			// set new item parent to scroll rect content
			tempItem.transform.SetParent(contentRect.transform, false);
			
		}
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
		//GameObject.Find ("Journal").GetComponent<JournalController> ().checkJournalItem (0);
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

	//TODO: Temporary method. It will change with the new inventory management
	//! Instantiation of the glassware when clicked
	/*! Uses the name of the glassware clicked to instantiate a game object (AnyObjectInstantiation) that will be
	 *!	added to the inventory */
	/*public AnyObjectInstantiation GlasswareInstantiation (int index) {

		AnyObjectInstantiation glasswareObject = Instantiate (glasswareToInventory) as AnyObjectInstantiation;
		glasswareObject.name = glasswareList [index].name;
		return glasswareObject;
	}*/

	public Glassware GlasswareInstantiation (int index) {
		return glasswareList[index];
	}



}

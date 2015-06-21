using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public  class GetReagentState : GameStateBase {

	public Camera cameraState;
    public GameObject interactBox;
	public DoorBehaviour leftDoor;
	public DoorBehaviour rightDoor;
	public string[] reagentList;
	public Dictionary<string, ReagentsLiquidClass> reagents = new Dictionary<string, ReagentsLiquidClass>();

    // UI
    public Canvas canvasUI;
    public ReagentUiItemBehaviour reagentPrefab;
    public float offSetItens;

    private ScrollRect UIScrollList;
    private Vector3 currentPosition;
    private int lastItemPos = 0;
    private RectTransform contentRect, prefabRect;

	// Use this for initialization
	public void Start () {
		
        cameraState.gameObject.SetActive(false);
		//canvasUI.GetComponent<Canvas>().enabled = false;
		reagents = ComponentsSaver.LoadReagents();

        // Set-up components
        if (canvasUI == null)   
            Debug.LogError("Canvas not found in GetReagentState");
        
        UIScrollList = canvasUI.GetComponentInChildren<ScrollRect>();
        if (UIScrollList == null)
            Debug.LogError("ScrollRect not found in GetReagentState");

        prefabRect = reagentPrefab.GetComponent<RectTransform>();
        
        contentRect = UIScrollList.content;

        // Populate reagent list UI
        for (int i = 0; i < reagentList.Length; i++)
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
            tempItem.GetComponent<ReagentUiItemBehaviour>().SetReagent(reagentList[i]);
            
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

	public override void OnStartRun ()
	{
		cameraState.gameObject.SetActive(true);
		HudText.SetText("");

		leftDoor.Open();
		canvasUI.GetComponent<Canvas>().enabled = true;

		//UIScrollList.transform.parent.GetComponent<ScrollRect>().verticalScrollbar.value = 1;

	}

	public void ExitState(){
		interactBox.GetComponent<BoxCollider>().enabled = true;
		gameController.ChangeState(0);
		FadeScript.instance.ShowFade();
	}
	
	public override void OnStopRun ()
	{
		cameraState.gameObject.SetActive(false);
		canvasUI.GetComponent<Canvas>().enabled = false;

		leftDoor.Close();


	}


}

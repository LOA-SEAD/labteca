using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public  class GetReagentState : GameStateBase {

	public Camera cameraState;
	public GameObject interactBox;
	public GameObject canvasUI;

	public DoorBehaviour leftDoor;
	public DoorBehaviour rightDoor;
	public string[] reagentList;


	public Dictionary<string, ReagentsLiquidClass> reagents = new Dictionary<string, ReagentsLiquidClass>();

	public RectTransform UIScrollList;
	public RectTransform startPoint;
	public ReagentUiItemBehaviour reagentPrefab;

	private Vector3 currentPosition;
	public float offSetItens;


	
	// Use this for initialization
	public void Start () {
		cameraState.gameObject.SetActive(false);
		canvasUI.GetComponent<Canvas>().enabled = false;
		reagents = ComponentsSaver.LoadReagents();
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


		UIScrollList.transform.parent.GetComponent<ScrollRect>().verticalScrollbar.value = 1;



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

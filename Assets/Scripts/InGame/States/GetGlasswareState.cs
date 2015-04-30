using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public  class GetGlasswareState : GameStateBase {

	public Camera cameraState;
	public GameObject interactBox;
	public GameObject canvasUI;

	public DoorBehaviour leftDoor;
	public DoorBehaviour rightDoor;
	public Glassware[] glasswareList;


	public RectTransform UIScrollList;
	public RectTransform startPoint;
	public GlasswareUiItemBehaviour glasswarePrefab;

	private Vector3 currentPosition;
	public float offSetItens;


	
	// Use this for initialization
	public void Start () {
		cameraState.gameObject.SetActive(false);
		canvasUI.GetComponent<Canvas>().enabled = false;
		currentPosition = startPoint.localPosition;

		Rect rectScroll = UIScrollList.rect;

		rectScroll.height = (glasswarePrefab.GetComponent<RectTransform>().rect.height * (glasswareList.Length)) + (glasswareList.Length *offSetItens);

		Vector3 newScale = new Vector3(1,1,1);

		newScale.y *= glasswareList.Length;

		RectTransform newUIScrollList = UIScrollList;

		newUIScrollList.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Bottom, 0, rectScroll.height);



		UIScrollList = newUIScrollList;

		for(int i = 0; i<glasswareList.Length; i++){

			GameObject tempGlass = Instantiate(glasswarePrefab.gameObject,  currentPosition, startPoint.rotation) as GameObject;
			tempGlass.transform.SetParent(UIScrollList.transform, false);
			tempGlass.GetComponent<RectTransform>().localPosition = currentPosition;
			tempGlass.GetComponent<GlasswareUiItemBehaviour>().SetGlass(glasswareList[i]);

			currentPosition.y -= glasswarePrefab.GetComponent<RectTransform>().rect.height + offSetItens;

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

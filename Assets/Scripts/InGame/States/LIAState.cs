using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class LIAState : GameStateBase {
	
	// camera and interactive area
	//public Camera cameraState;                  /*!< Camera for this state. */
	public GameObject interactiveCanvas;		//Interactive canvas of the state

	public GameObject correctAnswer;			//Canva with the answer for the correct results
	public GameObject wrongAnswer;				//Canva with the answer for the wrong results   TODO:Take this out. It's only here for SBGames

	public Light LIALight;
	public Image glassware,solid,liquid;

	public string currentIndex;

	private bool up;
	private InventoryManager inventory;
	private Sprite originalSprite;
	public void Start () {
		cameraState.gameObject.SetActive(false);
		interactiveCanvas.SetActive (false);
		inventory = GameObject.Find ("InventoryManager").GetComponent<InventoryManager> ();
		originalSprite = glassware.sprite;

		correctAnswer.SetActive (false);
		wrongAnswer.SetActive (false);

		ResultVerifier.GetInstance ();
	}
	
	protected override void UpdateState ()
	{
		
	}

	public bool ReceiveProduct(ItemInventoryBase item,GameObject obj){
		if (currentIndex.Length == 0) {
			if (obj != null) {
				currentIndex = item.index;
				glassware.sprite = item.icon.sprite;
				if(item.solid.IsActive()){
					solid.sprite = item.solid.sprite;
					solid.color = new Color32(255,255,255,255);
				}
				if(item.liquid.IsActive()){
					liquid.sprite = item.liquid.sprite;
					liquid.color = new Color32(255,255,255,255);
				}
				return true;
			} else {
				Debug.Log ("n recebi");
				return false;
			}
		} else {
			gameController.sendAlert("Já tem um produto");
		}
		return false;
	}

	public void RetrieveProduct(){
		if (currentIndex.Length>0) {
			inventory.AddProductToInventory(GameObject.Find(currentIndex));
			glassware.sprite = originalSprite;
			solid.color = new Color32(255,255,255,0);
			liquid.color = new Color32(255,255,255,0);
			currentIndex = "";
		} else {
			gameController.sendAlert("Não há nenhum produto");
		}
	}
	
	void Update(){
		base.Update();
		
		if(!canRun)
			return;

		if (up&&LIALight.intensity<0.6f) {
			LIALight.intensity += Time.deltaTime / 2;
		} else {
			up=false;
			if(LIALight.intensity>0.1f)
				LIALight.intensity -= Time.deltaTime / 4;
			else
				up=true;
		}

		//Pressing Esc will exit the state
		if(Input.GetKeyDown(KeyCode.E)){
			ExitState();
		}
	}
	
	//! Actions for when the State starts.
	// Set the Camera inside the state to be Active, overlaying the Main Camera used at InGameState,
    // close all dialogs that might be enabled.
	public override void OnStartRun ()
	{
		GameObject.Find ("InventoryManager").GetComponent<InventoryManager> ().changeList (0);
		cameraState.gameObject.SetActive(true);
		cameraState.depth = 2;
		interactiveCanvas.SetActive (true);
		HudText.SetText("");
	}
	
	//! Actions for when the State ends.
	/*! Disable the Camera inside the state, deactivate. */
	public override void OnStopRun ()
	{
		this.RetrieveProduct ();
		interactBox.GetComponent<BoxCollider>().enabled = true;

		gameController.closeAlert ();
		LIALight.intensity = 0f;
		cameraState.depth = -1;
		cameraState.gameObject.SetActive(false);
		interactiveCanvas.SetActive (false);
	}

	public override void ExitState(){
		gameController.ChangeState(0);
	}

	//! Called when the verification action is triggered
	// Calls upon the ResultVerifier class to compare the content of the Glassware with the
	// expected result for the actual phase
	public void VerifyPhase() {
		Debug.Log ("Pressing button");
		if (currentIndex.Length > 0) { //Case there is a Glassware being verified
			if ((GameObject.Find (currentIndex).GetComponent<Glassware> ()).content != null) { //Case there is a content
				//TODO: The parameter is always 0 as it's checking only the first phase. It needs to change for the correct value when phases are changing correctly
				if (ResultVerifier.GetInstance ().VerifyResult (0, (GameObject.Find (currentIndex).GetComponent<Glassware> ()).content)) { //Case Product is correct

					//gameController.sendAlert ("Resultado correto! Parabéns!");
					correctAnswer.SetActive(true);

				} else {	//Case Product is NOT correct
					//gameController.sendAlert ("Resultado incorreto.");
					wrongAnswer.SetActive(true);
				}
			} else {	//Case the Glassware is empty (it won't actually happen)
				gameController.sendAlert ("A vidraria está vazia!");
			}
		} else {	//Case there's no Glassware selected
			gameController.sendAlert("Não há nenhum produto para verificar!");
		}
	}

	//Ending animation TODO:This was created as a finisher for the SBGames Version
	public void EndGame() {
		FadeScript.instance.FadeIn ();

		#if UNITY_EDITOR
		Application.LoadLevel ("Menu");
		#else
		Application.LoadLevel ("Menu");
		#endif
	}
}

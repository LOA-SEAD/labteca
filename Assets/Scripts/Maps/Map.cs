using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Map : MonoBehaviour {

	public Transform pos0,pos1,player;
	public RectTransform playerMarker,map;
	public float XCenter,YCenter;
	public float distanceToCenterY,distanceToCenterX;
	public GameObject markerPrefab;
	// Use this for initialization
	void Start () {
//		Debug.Log("X: "+pos0.position.x+" Y: "+pos0.position.y+" Z: "+pos0.position.z);
//		Debug.Log("X: "+pos1.position.x+" Y: "+pos1.position.y+" Z: "+pos1.position.z);
		YCenter = (pos1.position.z + pos0.position.z) / 2;
		XCenter = (pos1.position.x + pos0.position.x) / 2;

//		Debug.Log("Player X: "+player.position.x+" Y: "+player.position.y+" Z: "+player.position.z);
		GenerateMarkers ();
	}
	
	// Update is called once per frame
	void Update () {
		distanceToCenterY = -(player.position.z-YCenter)/(Mathf.Abs(pos1.position.z - pos0.position.z)/2);
		distanceToCenterX = -(player.position.x-XCenter)/(Mathf.Abs(pos1.position.x - pos0.position.x)/2);

		playerMarker.localPosition = new Vector3 (map.sizeDelta.x * distanceToCenterX/2, map.sizeDelta.y * distanceToCenterY/2,0);
		playerMarker.rotation = Quaternion.Euler (0, 0, 180 - player.rotation.eulerAngles.y);
	}

	public void GenerateMarkers(){
		GameController gc = GameObject.Find ("GameController").GetComponent<GameController> ();
		foreach (GameStateBase gameState in gc.gameStates) {
			if(!(gameState is InGameState)){
				GameObject marker = Instantiate(markerPrefab) as GameObject;
				marker.transform.SetParent(this.transform,false);

				distanceToCenterY = -(gameState.WorldLocation.z-YCenter)/(Mathf.Abs(pos1.position.z - pos0.position.z)/2);
				distanceToCenterX = -(gameState.WorldLocation.x-XCenter)/(Mathf.Abs(pos1.position.x - pos0.position.x)/2);
				marker.transform.localPosition = new Vector3 (map.sizeDelta.x * distanceToCenterX/2, map.sizeDelta.y * distanceToCenterY/2,0);
				marker.GetComponent<Image>().sprite = gameState.interactBox.GetComponentInChildren<Image>().sprite;
			
				float yRotation = distanceToCenterX >0?180f:0f;
				float xRotation = distanceToCenterY >0?180f:0f;

				if(xRotation>0){
					marker.GetComponentsInChildren<Image>()[1].rectTransform.anchorMax= new Vector2(0.5f,0f);
					marker.GetComponentsInChildren<Image>()[1].rectTransform.anchorMin= new Vector2(0.5f,0f);
				}

				marker.GetComponentsInChildren<Image>()[1].rectTransform.rotation = Quaternion.Euler(new Vector3(xRotation,yRotation,0));
				marker.GetComponentsInChildren<Image>()[1].rectTransform.anchoredPosition = Vector2.zero;
				marker.GetComponentInChildren<Text>().rectTransform.localRotation = Quaternion.Euler(new Vector3(xRotation,yRotation,0));

				marker.GetComponentInChildren<Text>().text = gameState.interactBox.GetComponent<AccessEquipmentBehaviour>().equipName;

				EventTrigger trigger = marker.gameObject.GetComponent<EventTrigger> ();
				EventTrigger.Entry enter = new EventTrigger.Entry ();
				enter.eventID = EventTriggerType.PointerEnter;
				enter.callback.AddListener ((eventData) => { 
					marker.GetComponentsInChildren<Image>()[1].enabled=true;
					marker.GetComponentInChildren<Text>().enabled=true;
					marker.transform.SetAsLastSibling();
				});
				trigger.triggers.Add (enter);
				
				EventTrigger.Entry exit = new EventTrigger.Entry ();
				exit.eventID = EventTriggerType.PointerExit;
				exit.callback.AddListener ((eventData) => { 
					marker.GetComponentsInChildren<Image>()[1].enabled=false;
					marker.GetComponentInChildren<Text>().enabled=false;
				});
				trigger.triggers.Add (exit);

				marker.GetComponentsInChildren<Image>()[1].enabled=false;
				marker.GetComponentInChildren<Text>().enabled=false;
			}
		}
	}
}

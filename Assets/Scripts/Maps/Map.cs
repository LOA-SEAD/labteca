using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Map : MonoBehaviour {

	public Transform pos0,pos1,player;
	public RectTransform playerMarker,map;
	public float distanceToCenterY,distanceToCenterX;
	// Use this for initialization
	void Start () {
		Debug.Log("X: "+pos0.position.x+" Y: "+pos0.position.y+" Z: "+pos0.position.z);
		Debug.Log("X: "+pos1.position.x+" Y: "+pos1.position.y+" Z: "+pos1.position.z);

		Debug.Log("Player X: "+player.position.x+" Y: "+player.position.y+" Z: "+player.position.z);
	}
	
	// Update is called once per frame
	void Update () {
		float YCenter = (pos1.position.z + pos0.position.z) / 2;
		distanceToCenterY = -(player.position.z-YCenter)/(Mathf.Abs(pos1.position.z - pos0.position.z)/2);

		float XCenter = (pos1.position.x + pos0.position.x) / 2;
		distanceToCenterX = -(player.position.x-XCenter)/(Mathf.Abs(pos1.position.x - pos0.position.x)/2);

		playerMarker.localPosition = new Vector3 (map.sizeDelta.x * distanceToCenterX/2, map.sizeDelta.y * distanceToCenterY/2,0);
		playerMarker.rotation = Quaternion.Euler (0, 0, 180 - player.rotation.eulerAngles.y);
	}
}

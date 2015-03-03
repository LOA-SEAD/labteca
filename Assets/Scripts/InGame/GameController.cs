using UnityEngine;
using System.Collections;

public class GameController : MonoBehaviour {

	private CharacterController player;

	// Use this for initialization
	void Start () {

		player = FindObjectOfType(typeof(CharacterController)) as CharacterController;

		Vector3 positionPlayer = new Vector3(PlayerPrefs.GetFloat("PlayerPosX"), 1, PlayerPrefs.GetFloat("PlayerPosZ"));
		player.transform.position = positionPlayer;

	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}

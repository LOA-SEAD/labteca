using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SodaBehaviour : MonoBehaviour {

	public List<Texture> textureSoda;
	public float timeToLive;
	private float currentTimeToLive;


	// Use this for initialization
	void Start () {

		int rand  = Random.Range(0, textureSoda.Count);

		GetComponent<Renderer>().material.mainTexture = textureSoda[rand];

	
	}
	
	// Update is called once per frame
	void Update () {
		currentTimeToLive += Time.deltaTime;

		if(currentTimeToLive > timeToLive){
			Destroy(gameObject);
		}
	
	}
}

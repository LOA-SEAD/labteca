using UnityEngine;
using System.Collections;
using System.Collections.Generic;

//! Changes texture soda.
/*! */

//TODO: testar para saber como funciona.
public class SodaBehaviour : MonoBehaviour {

	public List<Texture> textureSoda;
	public float timeToLive;
	private float currentTimeToLive;


	// Use this for initialization
	//! Returns a random texture soda.
	void Start () {

		int rand  = Random.Range(0, textureSoda.Count);

		GetComponent<Renderer>().material.mainTexture = textureSoda[rand];
	}

	// Update is called once per frame
	//! if the currentTimeLive is longer than timeToLive, the object is destroyed.
	void Update () {
		currentTimeToLive += Time.deltaTime;

		if(currentTimeToLive > timeToLive){
			Destroy(gameObject);
		}
	
	}
}

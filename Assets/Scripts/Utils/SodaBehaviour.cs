using UnityEngine;
using System.Collections;
using System.Collections.Generic;

//! 
/*! After the soda has spawned, changes it's texture to a random
 * and sets a clock to destroy the object after timeToLive seconds
 */

public class SodaBehaviour : MonoBehaviour {

	public List<Texture> textureSoda;
	public float timeToLive;
	private float currentTimeToLive;


	//! Changes the texture of the soda to a random
	void Start () {

		int rand  = Random.Range(0, textureSoda.Count);

		GetComponent<Renderer>().material.mainTexture = textureSoda[rand];
	}

	//! if the currentTimeLive is longer than timeToLive, the object is destroyed.
	void Update () {
		currentTimeToLive += Time.deltaTime;

		if(currentTimeToLive > timeToLive){
			Destroy(gameObject);
		}
	
	}
}

using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class LoadingSpriteBehaviour : MonoBehaviour {

	public Sprite[] sprLoading = new Sprite[2];
	public int actualSpr;

	// Use this for initialization
	void Start () {
		actualSpr = 0;
		this.GetComponent<Image> ().sprite = sprLoading [actualSpr];
		InvokeRepeating ("ChangeSprite", 0.0f, 0.5f);
	}
	
	// Update is called once per frame
	void Update () {
	}

	void ChangeSprite() {
		actualSpr++;
		if (actualSpr == sprLoading.Length) {
			actualSpr = 0;
		}
		this.GetComponent<Image> ().sprite = sprLoading [actualSpr];
	}
}

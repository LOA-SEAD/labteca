using UnityEngine;
using System.Collections;

public class FadeScript : MonoBehaviour {

	public static FadeScript instance;
	public Animator fadeAnim;

	// Use this for initialization
	void Awake () {
		instance = this;

		FadeScript[] duplicate  = FindObjectsOfType(typeof(FadeScript)) as FadeScript[];

		if(duplicate.Length > 1)
			Destroy(gameObject);
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void ShowFade(){
		instance.fadeAnim.SetTrigger("fade");
	}
}

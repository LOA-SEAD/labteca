using UnityEngine;
using System.Collections;

//! Makes the fade used in a animation transition.
/*!
 * Contains two methods that makes the fade used in a animation transition (make and show).
 */
using UnityEngine.UI;


public class FadeScript : MonoBehaviour {

	public static FadeScript instance;
	public bool fadeInTrigger,fadeOutTrigger;
	public Image fade;

	//!  The script instance is being loaded (before the game starts).
	/*! Makes the fade*/
	void Awake () {
		instance = this;
     
		FadeScript[] duplicate  = FindObjectsOfType(typeof(FadeScript)) as FadeScript[];

        // if more than 1 is returned - there are two on scene, so delete this one
		if(duplicate.Length > 1)
			Destroy(gameObject);
	}

	void Start(){
		fade = GameObject.Find ("Fade").GetComponent<Image> ();
		FadeOut ();
	}

	void Update(){
		if (fadeInTrigger) {
			fade.color = new Color(0f,0f,0f,fade.color.a+Time.deltaTime);
			if(fade.color.a>=1f){
				fadeInTrigger = false;
				fade.enabled = false;
				GameObject.Find("GameController").GetComponent<GameController>().GetCurrentState().CanRun = true;
			}
		}
		if (fadeOutTrigger) {
			fade.color = new Color (0f, 0f, 0f, fade.color.a - Time.deltaTime);
			if (fade.color.a <= 0f) {
				fadeOutTrigger = false;
				fade.enabled = false;
				GameObject.Find("GameController").GetComponent<GameController>().GetCurrentState().CanRun = true;
			}
		}
	}

	public void FadeIn(){
		fade.enabled = true;
		fade.color = new Color (0f, 0f, 0f, 0f);
		fadeInTrigger = true;
	}

	public void FadeOut(){
		fade.enabled = true;
		fade.color = new Color (0f, 0f, 0f, 1f);
		fadeOutTrigger = true;
	}
}

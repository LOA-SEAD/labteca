using UnityEngine;
using System.Collections;

//! Makes the fade used in a animation transition.
/*!
 * Contains two methods that makes the fade used in a animation transition (make and show).
 */
public class FadeScript : MonoBehaviour {

	public static FadeScript instance;
	public Animator fadeAnim;

	//!  The script instance is being loaded (before the game starts).
	/*! Makes the fade*/
	void Awake () {
		instance = this;
     
		FadeScript[] duplicate  = FindObjectsOfType(typeof(FadeScript)) as FadeScript[];

        // if more than 1 is returned - there are two on scene, so delete this one
		if(duplicate.Length > 1)
			Destroy(gameObject);
	}

	//! Sets a trigger parameter to active the fade.
	public void ShowFade(){
		instance.fadeAnim.SetTrigger("fade");
	}
}

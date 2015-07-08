using UnityEngine;
using System.Collections;

//! Makes the fade used in a animation transition.
/*!
 * Contains two methods that makes the fade used in a animation transition (make and show).
 */
// TODO: Testar para saber exatamente como funciona.
public class FadeScript : MonoBehaviour {

	public static FadeScript instance;
	public Animator fadeAnim;

	//!  The script instance is being loaded (before the game starts).
	/*! Get any object of type FadeScript and if more than 1 is returned - there are two on scene, so delete this one. */
	void Awake () {
		instance = this;
           
        // Get any object of type FadeScript
		FadeScript[] duplicate  = FindObjectsOfType(typeof(FadeScript)) as FadeScript[];

        // if more than 1 is returned - there are two on scene, so delete this one
		if(duplicate.Length > 1)
			Destroy(gameObject);
	}

	//! Sets a trigger parameter to active.
	public void ShowFade(){
		instance.fadeAnim.SetTrigger("fade");
	}
}

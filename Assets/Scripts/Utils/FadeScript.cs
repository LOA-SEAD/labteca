using UnityEngine;
using System.Collections;

public class FadeScript : MonoBehaviour {

	public static FadeScript instance;
	public Animator fadeAnim;

	void Awake () {
		instance = this;
           
        // Get any object of type FadeScript
		FadeScript[] duplicate  = FindObjectsOfType(typeof(FadeScript)) as FadeScript[];

        // if more than 1 is returned - there are two on scene, so delete this one
		if(duplicate.Length > 1)
			Destroy(gameObject);
	
	}

    public void ShowFade(){
		instance.fadeAnim.SetTrigger("fade");
	}
}

using UnityEngine;
using System.Collections;

public class Transition : MonoBehaviour {
	public Animator anim;
	void Update () {
		if (anim.GetCurrentAnimatorStateInfo (0).IsName("Transition"))
			Application.LoadLevel(Application.loadedLevel+1);
	}
}

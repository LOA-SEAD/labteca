using UnityEngine;
using System.Collections;

public class Transition : MonoBehaviour {
	public Animator anim;
	void Update () {
		if (anim.GetCurrentAnimatorStateInfo (0).nameHash == -566307781)
			Application.LoadLevel(Application.loadedLevel+1);
	}
}

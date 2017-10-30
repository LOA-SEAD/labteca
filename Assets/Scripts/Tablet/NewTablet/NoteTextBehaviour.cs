using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NoteTextBehaviour : MonoBehaviour {

	// Use this for initialization
	void Start () {
		this.GetComponent<Text> ().text = "\"Lembre-se, Chris: todas as soluções no laboratório estão padronizadas a 1mol/L!\"\n- LIA";
	}
}

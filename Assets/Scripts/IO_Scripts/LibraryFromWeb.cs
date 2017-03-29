using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LibraryFromWeb : MonoBehaviour {

	private Dictionary<string, string> jsons;

	void Awake() {
		#if UNITY_WEBPLAYER
		DontDestroyOnLoad(this);
		#else
		Destroy (this);
		#endif

	}

	IEnumerator Start() {
		string url = "http://localhost/Assets/Resources/";
		WWW cP = new WWW(url+"customPhase.json");
		WWW jI0 = new WWW(url+"journalItems0.json");
		WWW jI1 = new WWW(url+"journalItems1.json");
		WWW jI2 = new WWW(url+"journalItems2.json");
		WWW jI3 = new WWW(url+"journalItems3.json");
		WWW jI4 = new WWW(url+"journalItems4.json");
		WWW cmp = new WWW(url+"compounds.json");
		WWW prd = new WWW(url+"products.json");
		WWW tN = new WWW(url+"tabletNotes.json");
		yield return cP;
		yield return jI0;
		yield return jI1;
		yield return jI2;
		yield return jI3;
		yield return jI4;
		yield return cmp;
		yield return prd;
		yield return tN;

		jsons = new Dictionary<string, string> ();

		jsons.Add("customPhase", cP.text);
		jsons.Add("journalItems0", jI0.text);
		jsons.Add("journalItems1", jI1.text);
		jsons.Add("journalItems2", jI2.text);
		jsons.Add("journalItems3", jI3.text);
		jsons.Add("journalItems4", jI4.text);
		jsons.Add("compounds", cmp.text);
		jsons.Add("products", prd.text);
		jsons.Add("tabletNotes", tN.text);

		Debug.Log("Pular cena = true");
		FindObjectOfType<SceneManager> ().GetComponent<SceneManager> ().EnableTransition ();
	}

	public string GetData(string fileName) {
		return jsons [fileName];
	}
}
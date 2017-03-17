using UnityEngine;
using System.Collections;
using SimpleJSON;

//! Loads a .json file.
//  The class makes use of SimpleJSON namespace to handle JSON I/O
/*public static class JSONEdit {

	//! Reads the file
	//  If the file doesn't exists, creates the file
	public static JSONNode Read(string path) {
		if(!System.IO.File.Exists(path)) {
			System.IO.File.Create(path);
			return Read(path);
		}
		else {
			System.IO.StreamReader sr = new System.IO.StreamReader (path);
			string content = sr.ReadToEnd ();
			sr.Close();
			
			return JSON.Parse (content);
		}
	}

	public static void Write(string path) {

	}
}*/


public class JSONEditor {
	
	public JSONNode json;
	
	public JSONEditor (string path) {
		
		#if UNITY_WEBPLAYER
		JSONEdit.ReadFromWeb(path, this);
		#endif
		
		/*#if UNITY_EDITOR
		json = JSONEdit.Read (path);
		#endif*/
		
		#if UNITY_STANDALONE_WIN
		json = JSONEdit.Read (path);
		#endif
		
		//Debug.Log (json.Value);
	}

	public void SetJSON(JSONNode jsonRead) {
		json = jsonRead;
	}

	public bool GetBool(int numObject, string data) {
		return json["objects"][numObject][data].AsBool;
	}
	
	public string GetString(int numObject, string data) {
		return json ["objects"] [numObject] [data].Value;
	}
	
	public int GetInt(int numObject, string data) {
		return json ["objects"] [numObject] [data].AsInt;
	}
	
	public float GetFloat(int numObject, string data) {
		return json ["objects"] [numObject] [data].AsFloat;
	}
	
	public int NumberOfObjects() {
		return json ["objects"].Count;
	}
	
	public int NumberOfFields(int numObject) {
		return json["objects"] [numObject].Count;
	}
	//Returns the string value of a sub-item
	public string GetSubValue(int numObject, string data, int position) {
		return json ["objects"] [numObject] [data][position].Value;
	}
	public string GetMainValue(string data) {
		return json [data].Value;
	}
}

public class JSONEdit : MonoBehaviour {
	
	public static JSONEditor NewJSONEditor(string path) {
		return new JSONEditor (path);
	}

	/*IEnumerator StartJSON(string path) {
	//	json = this.Read (path);
	}
*/
	//! Reads the file
	//  If the file doesn't exists, creates the file
	public static JSONNode Read(string path) {
		if(!System.IO.File.Exists(path)) {
			System.IO.File.Create(path);
			return Read(path);
		}
		else {
			System.IO.StreamReader sr = new System.IO.StreamReader (path);
			string content = sr.ReadToEnd ();
			sr.Close();
			
			return JSON.Parse (content);
		}
	}

	public static IEnumerator ReadFromWeb(string path, JSONEditor editor) {
		string url = "http://localhost/LabTecA/"+path;
		Debug.Log (url);
		WWW myWWW = new WWW(url);

		Debug.Log (myWWW.text);
		yield return myWWW;

		if (myWWW.error == null) {
			editor.SetJSON(JSON.Parse (myWWW.text));
			//json = JSON.Parse (myWWW.text);
		} else {
			Debug.Log ("ERROR: " + myWWW.error);
		}
	}

	private void Write() {
		
	}
}

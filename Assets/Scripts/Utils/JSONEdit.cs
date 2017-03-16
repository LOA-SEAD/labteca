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

public class JSONEdit {
	
	private JSONNode json;
	
	public JSONEdit (string path) {

		json = this.Read (path);
		//json = ReadFromWeb(path);
		//Debug.Log (json.Value);
	}
	
	//! Reads the file
	//  If the file doesn't exists, creates the file
	private JSONNode Read(string path) {
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

	private JSONNode ReadFromWeb(string path) {
		string url = "http://200.133.228.213/LabTecA/"+path;
		Debug.Log (url);
		var myWWW = new WWW(url);

		/*while (!myWWW.isDone) {
			Debug.Log (myWWW.error);
			Debug.Log (myWWW.bytesDownloaded);
		}*/
		return JSON.Parse (myWWW.text);
	}
	
	private void Write() {
		
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

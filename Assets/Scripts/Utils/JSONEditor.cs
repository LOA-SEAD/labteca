using UnityEngine;
using System.Collections;
using System.IO;
using SimpleJSON;

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

//! Loads a .json file.
//  The class makes use of SimpleJSON namespace to handle JSON I/O
public class JSONEditor {
	
	private JSONNode json;
	
	public JSONEditor (string file) {
		
		#if UNITY_WEBPLAYER
		json = ReadFromWeb(file);
		#else
		json = Read (file);
		#endif
	}

	public JSONEditor (string file, bool difference) {
		json = ReadFromResources (file);
	}

	//! Reads the file
	//  If the file doesn't exists, creates the file
	private JSONNode Read(string file) {

		string directory = "";

		#if UNITY_STANDALONE_WIN
			directory = "Assets\\Resources\\" + file + ".json";
		#endif
		#if UNITY_STANDALONE_LINUX
			directory = "Assets/Resources/" + file + ".json";
		#endif
		#if UNITY_STANDALONE_OSX
			directory = "LabTecA.app/Contents/Data/Resources/" + file + ".json";
		#endif
		#if UNITY_ANDROID
			char sep = Path.DirectorySeparatorChar;
			directory = "Assets" +sep+ "Resources" +sep+ file + ".json";
		#endif

		if(!System.IO.File.Exists(directory)) {
			System.IO.File.Create(directory);
			return Read(file);
		}
		else {
			System.IO.StreamReader sr = new System.IO.StreamReader (directory);
			string content = sr.ReadToEnd ();
			sr.Close();
			
			return JSON.Parse (content);
		}
	}

	/// <summary>
	/// Reads the files from the LibraryFromWeb class, that previously downloaded the values during game introduction.
	/// </summary>
	/// <returns>Returns .json read in form of JSONNode.</returns>
	/// <param name="file">Name of file to be loaded.</param>
	private JSONNode ReadFromWeb(string file) {
		return JSON.Parse (GameObject.Find ("LibraryFromWeb").GetComponent<LibraryFromWeb>().GetData (file));
	}

	/// <summary>
	/// Reads a file from the resources folder resources, integrated in the .exe.
	/// </summary>
	/// <param name="file">The file to be read.</param>
	private JSONNode ReadFromResources(string file) {
		//string filePath = "SetupData/" + file;
		TextAsset asset = Resources.Load(file) as TextAsset;
		return JSON.Parse (asset.text);
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

	public int GetMainInt(string data) {
		return json[data].AsInt;
	}
}
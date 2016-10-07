using UnityEngine;
using System.Collections;
using SimpleJSON;

//! Loads a .json file.
//  The class makes use of SimpleJSON namespace to handle JSON I/O
public static class JSONEdit {

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
}

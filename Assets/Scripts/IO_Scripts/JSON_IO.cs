﻿using UnityEngine;
using System.Collections;
using System.IO;
using SimpleJSON;

public static class JSON_IO {

	/// <summary>
	/// Reads a JSON and returns its JSONNode
	/// </summary>
	/// <returns>The JSONNode.</returns>
	/// <param name="file">File.</param>
	public static JSONNode ReadJSON(string file) {
		#if UNITY_WEBPLAYER
		json = ReadFromWeb(file);
		#endif

		string directory = "";
		char sep = Path.DirectorySeparatorChar;
		directory = "Assets" +sep+ "Resources" +sep+ file + ".json";

		if(!System.IO.File.Exists(directory)) {
			System.IO.File.Create(directory);
			return ReadJSON(file);
		}
		else {
			System.IO.StreamReader sr = new System.IO.StreamReader (directory);
			string content = sr.ReadToEnd ();
			sr.Close();

			return JSON.Parse (content);
		}
	}

	/// <summary>
	/// Reads a file from the resources folder resources, integrated in the .exe
	/// </summary>
	/// <param name="file">The file to be read.</param>
	public static JSONNode ReadFromResources(string file) {
		//string filePath = "SetupData/" + file;
		TextAsset asset = Resources.Load(file) as TextAsset;
		return JSON.Parse (asset.text);
	}
}
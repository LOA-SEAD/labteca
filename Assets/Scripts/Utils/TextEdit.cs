using System;
using System.Collections.Generic;
using System.Collections;
using System.Text;
using System.IO;
using UnityEngine;

public class TextEdit
{
    private Dictionary<string, string> dictionary = new Dictionary<string, string>();
	
	public TextEdit(string path)
	{
		LoadFile(path);
    }

	public TextEdit(TextAsset LoadFromText)
	{
		LoadFileWithString(LoadFromText.text);
	}

	private string pathAcc;
	private System.IO.StreamWriter WriteFile;
	
    private void LoadFile(string path)
    {
		dictionary.Clear();
		pathAcc = path;
		
		if (File.Exists(path))
		{
	        foreach (string line in File.ReadAllLines(path))
	        {
	            if ((!string.IsNullOrEmpty(line)) &&
	                (!line.StartsWith(";")) &&
	                (!line.StartsWith("#")) &&
				    (!line.StartsWith("//")) &&
	                (!line.StartsWith("'")) &&
	                (line.Contains("=")))
	            {
	                int index = line.IndexOf('=');
	                string key = line.Substring(0, index).Trim();
	                string value = line.Substring(index + 1).Trim();
	
	                if ((value.StartsWith("\"") && value.EndsWith("\"")) ||
	                    (value.StartsWith("'") && value.EndsWith("'")))
	                {
	                    value = value.Substring(1, value.Length - 2);
	                }
	                dictionary.Add(key, value);
	            }
	        }
		} 
		else 
		{
			Debug.LogError("Error [ReadConfig]: specified file [" + path + "] not found.");
		}
    }
	
	private void LoadFileWithString(string text)
	{
		string[] separator = {"/n", "//n", "\n", "\\n" };

		string[] lines = text.Split(separator, StringSplitOptions.None);

		dictionary.Clear();

		for (int i = 0; i < lines.Length; i++) 
		{
			if ((!string.IsNullOrEmpty(lines[i])) &&
			    (!lines[i].StartsWith(";")) &&
			    (!lines[i].StartsWith("#")) &&
			    (!lines[i].StartsWith("//")) &&
			    (!lines[i].StartsWith("'")) &&
			    (lines[i].Contains("=")))
			{
				int index = lines[i].IndexOf('=');
				string key = lines[i].Substring(0, index).Trim();
				string value = lines[i].Substring(index + 1).Trim();
				
				if ((value.StartsWith("\"") && value.EndsWith("\"")) ||
				    (value.StartsWith("'") && value.EndsWith("'")))
				{
					value = value.Substring(1, value.Length - 2);
				}
				dictionary.Add(key, value);
			}
		}
	}


	//Static wrappers...
	
	
	
	#region String
	
	public string GetString(string key)
	{
		return GetString(key, "");
	}
	
	public string GetString(string key, string defaultValue)
	{
		if (dictionary.ContainsKey(key))
		{
			string str = dictionary[key] as string;
			return str.Replace("\\n","\n");
		}
		return defaultValue;
	}
	
	public void SetString(string key,string Value)
	{
		if(!dictionary.ContainsKey(key))
		{
			dictionary.Add(key,Value.ToString());
		}
		
		WriteFile = new System.IO.StreamWriter(pathAcc);
		foreach (string myKey in dictionary.Keys)
		{
			if(myKey == key)
			{
				WriteFile.WriteLine(myKey+"="+Value.ToString());
			}
			else
			{
				WriteFile.WriteLine(myKey+"="+dictionary[myKey].ToString());
			}
		}
		WriteFile.Flush();
		WriteFile.Close();
		LoadFile(pathAcc);
	}
	
	#endregion
	
	
	
	#region Int
	
	public int GetInt(string key)
	{
		return GetInt(key, 0);
	}
	public int GetInt(string key, int defaultValue)
	{
		if (dictionary.ContainsKey(key))
		{
			return int.Parse(GetString(key));
		}
		return defaultValue;
	}
	
	public void SetInt(string key,int Value)
	{
		if(!dictionary.ContainsKey(key))
		{
			dictionary.Add(key,Value.ToString());
		}
		
		WriteFile = new System.IO.StreamWriter(pathAcc);
		foreach (string myKey in dictionary.Keys)
		{
			if(myKey == key)
			{
				WriteFile.WriteLine(myKey+"="+Value.ToString());
			}
			else
			{
				WriteFile.WriteLine(myKey+"="+dictionary[myKey].ToString());
			}
		}
		WriteFile.Flush();
		WriteFile.Close();
		LoadFile(pathAcc);
	}
	
	#endregion
	
	
	
	#region Float
	
	public float GetFloat(string key)
	{
		return GetFloat(key, 0.0f);
	}
	
	public float GetFloat(string key, float defaultValue)
	{
		if (dictionary.ContainsKey(key))
		{
			return float.Parse(GetString(key));	
		}
		return defaultValue;
	}	
	
	public void SetFloat(string key,float Value)
	{
		if(!dictionary.ContainsKey(key))
		{
			dictionary.Add(key,Value.ToString());
		}
		
		WriteFile = new System.IO.StreamWriter(pathAcc);
		foreach (string myKey in dictionary.Keys)
		{
			if(myKey == key)
			{
				WriteFile.WriteLine(myKey+"="+Value.ToString());
			}
			else
			{
				WriteFile.WriteLine(myKey+"="+dictionary[myKey].ToString());
			}
		}
		WriteFile.Flush();
		WriteFile.Close();
		LoadFile(pathAcc);
	}
	#endregion
	
	
	
	#region Bool
	public bool GetBool(string key)
	{
		return GetBool(key, false);
	}
	
	public bool GetBool(string key, bool defaultValue)
	{
		if (dictionary.ContainsKey(key))
		{
			return bool.Parse(GetString(key));	
		}
		return defaultValue;
	}
	
	public void SetBool(string key,bool Value)
	{
		if(!dictionary.ContainsKey(key))
		{
			dictionary.Add(key,Value.ToString());
		}
		
		WriteFile = new System.IO.StreamWriter(pathAcc);
		foreach (string myKey in dictionary.Keys)
		{
			if(myKey == key)
			{
				WriteFile.WriteLine(myKey+"="+Value.ToString());
			}
			else
			{
				WriteFile.WriteLine(myKey+"="+dictionary[myKey].ToString());
			}
		}
		WriteFile.Flush();
		WriteFile.Close();
		LoadFile(pathAcc);
	}
	#endregion

	public void ClearFile()
	{
		WriteFile = new System.IO.StreamWriter(pathAcc);
		WriteFile.Flush();
		WriteFile.Close();
		LoadFile(pathAcc);
	}
}

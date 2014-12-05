using UnityEngine;
using System.Collections;

public class HudText : MonoBehaviour 
{
	private static TextMesh textObject;

	// Use this for initialization
	void Start () 
	{
		textObject = this.gameObject.GetComponent<TextMesh> ();
	}

	public static void SetText(string text)
	{
		textObject.text = text;
	}

	public static void EraseText()
	{
		textObject.text = "";
	}
}

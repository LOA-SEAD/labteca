using UnityEngine;
using System.Collections;
using UnityEngine.UI;


public class HudText : MonoBehaviour 
{
	private static Text textObject;
	public static HudText instance;

	// Use this for initialization
	void Start () 
	{
		instance = this;

		textObject = instance.gameObject.GetComponent<Text> ();
	}

	public static void SetText(string text)
	{
		if(textObject != null)
			textObject.text = text;
	}

	public static void EraseText()
	{

		if(textObject != null)
			textObject.text = "";
	}
}

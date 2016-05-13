using UnityEngine;
using System.Collections;
using UnityEngine.UI;

//! Handles the text.
/*!
 * Contains three methods that handles the text (instance, set and erase). 
 */

public class HudText : MonoBehaviour 
{
	public static Text textObject;
	public static HudText instance;

	// Use this for initialization
	//! Returns the component of type TextObject.
	void Start () 
	{
		instance = this;

		textObject = instance.gameObject.GetComponent<Text> ();
	}

	//! Set the input text.
	public static void SetText(string text)
	{
		if(textObject != null)
			textObject.text = text;
	}

	//! Erase the text.
	public static void EraseText()
	{
		if(textObject != null)
			textObject.text = "";
	}
}

using UnityEngine;
using System.Collections;

public class MachineUse : MonoBehaviour 
{
	public KeyCode keyToUse;
	public string sceneToGo;

	private bool allowChangeScene = false;

	public string machineName;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnTriggerEnter(Collider other)
	{
		if (other.tag == "Player") 
		{
			allowChangeScene = true;
			if(string.IsNullOrEmpty(machineName))
			{
				HudText.SetText("Aperte " + keyToUse.ToString() + " para usar o aparelho.");
			}
			else
			{
				HudText.SetText("Aperte " + keyToUse.ToString() + " para usar " + machineName + ".");
			}
		}
	}

	void OnTriggerStay(Collider other)
	{
		if (other.tag == "Player") 
		{
			if(Input.GetKeyDown(keyToUse))
			{
				HudText.EraseText();
				Application.LoadLevel(sceneToGo);
			}
		}
	}

	void OnTriggerExit(Collider other)
	{
		if (other.tag == "Player") 
		{
			allowChangeScene = false;
			HudText.EraseText();
		}
	}
}

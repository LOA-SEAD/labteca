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

				PlayerPrefs.SetFloat("PlayerPosX", other.transform.position.x);
				PlayerPrefs.SetFloat("PlayerPosy", other.transform.position.y);
				PlayerPrefs.SetFloat("PlayerPosZ", other.transform.position.z);
				PlayerPrefs.SetFloat("RotationCameraX", Camera.main.transform.localEulerAngles.x);
				PlayerPrefs.SetFloat("RotationCameraY", Camera.main.transform.localEulerAngles.y);
				PlayerPrefs.SetFloat("RotationCameraZ", Camera.main.transform.localEulerAngles.z);
				PlayerPrefs.Save();
				Application.LoadLevel(sceneToGo);

				InventoryController inventory = FindObjectOfType(typeof(InventoryController)) as InventoryController;
				FadeScript.instance.ShowFade();

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

using UnityEngine;
using System.Collections;

//! Interaction Player and machine.
/*! Show the messagem and set the position(player) and rotation (camera). */
public class MachineUse : MonoBehaviour 
{
	public KeyCode keyToUse;
	public string sceneToGo;

	private bool allowChangeScene = false;

	public string machineName;

	//! Message for interaction with the machine.
	/*! Is sent to the trigger collider and the rigidbody that the trigger collider belongs to, and to the rigidbody that touches the trigger.*/
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

	//! Set positons (player) and rotation (camera).
	/*! Is sent to the trigger and the collider that touches the trigger.*/
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

			}
		}
	}

	//! Collider other has stopped touching the trigger.
	void OnTriggerExit(Collider other)
	{
		if (other.tag == "Player") 
		{
			allowChangeScene = false;
			HudText.EraseText();
		}
	}
}

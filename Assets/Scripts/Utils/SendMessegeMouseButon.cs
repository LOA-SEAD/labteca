using UnityEngine;
using System.Collections;

//! Sets a message.
/*! */
public class SendMessageMouseButton : MonoBehaviour 
{
	private Camera[] cameras;
	public GameObject target;
	public string message;

	private Ray ray;
	private RaycastHit hit;

	// Use this for initialization
	//! Sets target.
	void Start () 
	{
		if (target == null) 
		{
			target = this.gameObject;
		}
	}

	// Update is called once per frame
	//! Message is sent.
	/* Returns a ray going from camera through a screen point and if collider.Raycast a message is sent. */
	void Update () 
	{
		if (Input.GetMouseButtonDown (0)) 
		{
			cameras = FindObjectsOfType(typeof(Camera)) as Camera[];

			for (int i = 0; i < cameras.Length; i++) 
			{
				ray = cameras[i].ScreenPointToRay(Input.mousePosition);
				if(GetComponent<Collider>().Raycast(ray, out hit, 10000f))
				{
					target.SendMessage(message,SendMessageOptions.DontRequireReceiver);
				}
			}
		}
	}
}

using UnityEngine;
using System.Collections;

public class SendMessegeMouseButonWithParameter : MonoBehaviour 
{
	private Camera[] cameras;
	public GameObject target;
	public string message;

	private Ray ray;
	private RaycastHit hit;

	public int parameter;

	// Use this for initialization
	void Start () 
	{


		if (target == null) 
		{
			target = this.gameObject;
		}
	}
	
	// Update is called once per frame
	void Update () 
	{
		if (Input.GetMouseButtonDown (0)) 
		{
			cameras = FindObjectsOfType(typeof(Camera)) as Camera[];

			for (int i = 0; i < cameras.Length; i++) 
			{
				ray = cameras[i].ScreenPointToRay(Input.mousePosition);
				if(collider.Raycast(ray, out hit, 10000f))
				{
					target.SendMessage(message,parameter,SendMessageOptions.DontRequireReceiver);
				}
			}
		}
	}
}

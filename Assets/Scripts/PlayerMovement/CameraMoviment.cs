using UnityEngine;
using System.Collections;

public class CameraMoviment : MonoBehaviour 
{
	public Camera playerCam;

	public LayerMask layermask;
	public Transform target;
	public float height;
	public float distance;
	public float x = 0;
	public float y = 0;
	public float yMin;
	public float yMax;

	public float movimentSensibility;

	public Vector3 wallPositionCorrection;

	public float playerCamDistanceMultiply;

	private RaycastHit hit;
	void  Start (){
		
	}
	
	void  FixedUpdate ()
	{	
		x += movimentSensibility * Input.GetAxis("Mouse X");
		y += movimentSensibility * Input.GetAxis("Mouse Y");
		if(y > yMax){
			y = yMax;
		}else if(y < yMin){
			y = yMin;
		}
		
		
		
		Quaternion rotation= Quaternion.Euler(y,x,0);
		Vector3 position= rotation * (new Vector3(1.0f, height, -distance)) + target.position;
		RaycastHit hit;
		
		transform.rotation = rotation;
		transform.position = position;
		playerCam.transform.localPosition = Vector3.zero;

		if (Physics.Linecast (target.position,this.transform.position,out hit, layermask))
		{	
			float tempDistance = Vector3.Distance(target.position,hit.point);
			position = rotation *( new Vector3(0.0f,height,-tempDistance) + wallPositionCorrection) + target.position;
			transform.position = position;

			playerCam.transform.localPosition = new Vector3(0f, 0f, (1f/tempDistance)*playerCamDistanceMultiply);
		}	

	}
}
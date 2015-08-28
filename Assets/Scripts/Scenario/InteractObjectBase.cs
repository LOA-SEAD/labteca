using UnityEngine;
using System.Collections;

//! Base Class for Object to become interactable.
/*!Every class that inherits from this class becomes interactable
 * THOU SHALL NOT DELETE!!!!!!!!
 */
public abstract class InteractObjectBase : MonoBehaviour {


	protected void Start () {
	
	}
	
	protected void Update () {
	
	}

	public abstract void Interact();


}

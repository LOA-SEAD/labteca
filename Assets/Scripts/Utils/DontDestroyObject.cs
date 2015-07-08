using UnityEngine;
using System.Collections;

//! Preserve an object during level loading.
/*!
 * Contains one method that makes the object target
 * not be destroyed automatically when loading a new scene.
 */

public class DontDestroyObject : MonoBehaviour 
{
	/*! 
	 * Awake is used to initialize any variables or game state before the game starts.
	 * Awake is called only once during the lifetime of the script instance.*/
	void Awake () {
		DontDestroyOnLoad (this.gameObject);
	}
}

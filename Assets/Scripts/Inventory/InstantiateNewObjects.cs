using UnityEngine;
using System.Collections;

//! Instantiate new objects to be used by the inventory - test only.
/*!
 *  This is a Class used to test the inventory, it instanciates new objects from the public variables
 *  below that are set using prefabs at the Inspector. It get's the prefab and defines the type and 
 *  instanciates inside the scene.
 *  To use: press Q, W, E or R for 'Solid', 'Liquid', 'Glassware' or 'Others' respectively.
 */
public class InstantiateNewObjects : MonoBehaviour {

    public GameObject solid;        /*!< prefab that will be Solid item. */
    public GameObject liquid;       /*!< prefab that will be Liquid item. */
    public GameObject glassware;    /*!< prefab that will be Glassware item. */
    public GameObject others;       /*!< prefab that will be Others item. */

	// Update is called once per frame
	void Update () 
    {
        // if 'Q' pressed - instantiate Solid
        if (Input.GetKeyDown(KeyCode.Q))
        {
            GameObject tempItem = Instantiate(solid.gameObject,
                                          solid.transform.position,
                                          solid.transform.rotation) as GameObject;
            tempItem.transform.SetParent(this.transform, false);
        }
        // if 'W' pressed - instantiate Liquid
        if (Input.GetKeyDown(KeyCode.W))
        {
            GameObject tempItem = Instantiate(liquid.gameObject,
                                          liquid.transform.position,
                                          liquid.transform.rotation) as GameObject;
            tempItem.transform.SetParent(this.transform, false);
        }
        // if 'E' pressed - instantiate Glassware
        if (Input.GetKeyDown(KeyCode.E))
        {
            GameObject tempItem = Instantiate(glassware.gameObject,
                                          glassware.transform.position,
                                          glassware.transform.rotation) as GameObject;
            tempItem.transform.SetParent(this.transform, false);
        }
        // if 'R' pressed - instantiate Others
        if (Input.GetKeyDown(KeyCode.R))
        {
            GameObject tempItem = Instantiate(others.gameObject,
                                          others.transform.position,
                                          others.transform.rotation) as GameObject;
            tempItem.transform.SetParent(this.transform, false);
        }

	}
}

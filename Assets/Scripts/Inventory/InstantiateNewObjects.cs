using UnityEngine;
using System.Collections;

public class InstantiateNewObjects : MonoBehaviour {

    public GameObject solid;
    public GameObject liquid;
    public GameObject glassware;
    public GameObject others;

	// Update is called once per frame
	void Update () 
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            GameObject tempItem = Instantiate(solid.gameObject,
                                          solid.transform.position,
                                          solid.transform.rotation) as GameObject;
            tempItem.transform.SetParent(this.transform, false);
        }
        if (Input.GetKeyDown(KeyCode.W))
        {
            GameObject tempItem = Instantiate(liquid.gameObject,
                                          liquid.transform.position,
                                          liquid.transform.rotation) as GameObject;
            tempItem.transform.SetParent(this.transform, false);
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            GameObject tempItem = Instantiate(glassware.gameObject,
                                          glassware.transform.position,
                                          glassware.transform.rotation) as GameObject;
            tempItem.transform.SetParent(this.transform, false);
        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            GameObject tempItem = Instantiate(others.gameObject,
                                          others.transform.position,
                                          others.transform.rotation) as GameObject;
            tempItem.transform.SetParent(this.transform, false);
        }

	}
}

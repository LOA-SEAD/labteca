using UnityEngine;
using System.Collections;

public class HUDController : MonoBehaviour {

    public void disableHUD()
    {
        this.gameObject.SetActive(false);
    }

    public void enableHUD()
    {
        this.gameObject.SetActive(true);
    }
    public void changeHUDCamera(Camera newCamera)
    {
        GetComponent<Canvas>().worldCamera = newCamera;
    }

}

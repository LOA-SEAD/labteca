using UnityEngine;
using System;
using System.Collections;

public class DownloadManager : MonoBehaviour {

	/*IEnumerator Start() {


	}*/

	/*private WWW wwwData;
	private static DownloadManager dm = null;
	
	// Use this for initialization
	void Awake () {
		if (DownloadManager.dm == null)
			DownloadManager.dm = FindObjectOfType(typeof(DownloadManager)) as DownloadManager; //GameObject.Find ("DownloadManager").GetComponent<DownloadManager> ();
	}
	
	void OnApplicationQuit() {
		DownloadManager.dm = null;
	} */
	
	/*public delegate void DownloadCallback(string data, string sError);
	
	private IEnumerator WaitForDownload(DownloadCallback fn)
	{
		Debug.Log("Yielding");     
		yield return wwwData;
		Debug.Log("Yielded");
		fn(wwwData.text, wwwData.error);  
	}
	
	private void StartDownload(string sURL, DownloadCallback fn)
	{
		try
		{  
			wwwData = new WWW(sURL);           
			Debug.Log("Starting download.");
			StartCoroutine("WaitForDownload", fn);
		}
		catch(Exception e)
		{
			Debug.Log(e.ToString());
		}  
	}
	
	public static void Download(string sURL, DownloadCallback fn)
	{
		dm.StartDownload(sURL, fn);
	}*/
}
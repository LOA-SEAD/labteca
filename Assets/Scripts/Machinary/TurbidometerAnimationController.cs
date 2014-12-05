using UnityEngine;
using System.Collections;

public class TurbidometerAnimationController : MonoBehaviour {

	public Animator jarAnimator;

	private bool clickedOnJar = false;
	private bool clickedOnJarEmpty = false;

	public MachineBehaviour machine;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}


	public void ButtonJar()
	{
		if (!clickedOnJar && clickedOnJarEmpty) 
		{
			clickedOnJar = true;
			jarAnimator.SetTrigger("jar");
		}
	}

	public void ButtonJarEmpty()
	{
		if (!clickedOnJarEmpty) 
		{
			clickedOnJarEmpty = true;
			jarAnimator.SetTrigger("jarEmpty");
		}
	}

	public void ButtonRun()
	{
		if (clickedOnJarEmpty) 
		{
			machine.Setup("water",1f);
			jarAnimator.SetTrigger("jarEmptyBack");
		}

		if (clickedOnJar) //correct to check end animation
		{
			machine.Use("test",1f);
		}
	}
}

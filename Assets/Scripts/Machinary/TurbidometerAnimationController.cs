using UnityEngine;
using System.Collections;

//! Animation of Turbidometer.
/*! Wait to be ready. */
public class TurbidometerAnimationController : MonoBehaviour {

	public Animator jarAnimator;

	private bool clickedOnJar = false;
	private bool clickedOnJarEmpty = false;

	public MachineBehaviour machine;


	//! Active parameter "jar". Used in a transition.
	public void ButtonJar()
	{
		if (!clickedOnJar && clickedOnJarEmpty) 
		{
			clickedOnJar = true;
			jarAnimator.SetTrigger("jar");
		}
	}

	//! Active parameter "jar". Used in a transition when JarEmpty.
	public void ButtonJarEmpty()
	{
		if (!clickedOnJarEmpty) 
		{
			clickedOnJarEmpty = true;
			jarAnimator.SetTrigger("jarEmpty");
		}
	}

	//! Setup reagent and concentration in jar.
	/*! */
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

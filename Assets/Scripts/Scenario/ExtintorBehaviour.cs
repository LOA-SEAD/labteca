using UnityEngine;
using System.Collections;

//! Starts the Particle System that simulates a smoke.
public class ExtintorBehaviour : InteractObjectBase {

	public AudioSource somExtintor;
	public ParticleSystem smoke;    /*! ParticleSystem for smoke. */

    //! When there is interaction, play the ParticleSystem.
	public override void Interact ()
	{
		somExtintor.Play ();
		smoke.Play();
	}
}

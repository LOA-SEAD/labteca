using UnityEngine;
using System.Collections;

//! Starts the Particle System that simulates a smoke.
public class ExtintorBehaviour : InteractObjectBase {

	public ParticleSystem smoke;    /*! ParticleSystem for smoke. */

    //! When there is interaction, play the ParticleSystem.
	public override void Interact ()
	{
		smoke.Play();
	}
}

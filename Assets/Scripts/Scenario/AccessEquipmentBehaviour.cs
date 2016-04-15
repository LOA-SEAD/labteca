using UnityEngine;
using System.Collections;
using UnityEngine.UI;
//! Allows any Equipment to be interactable.



public class AccessEquipmentBehaviour : InteractObjectBase {

	public GameStateBase targetEquipment;      /*!< GameStateBase for equipment. */
	public Canvas descriptionCanvas;
	public float delay = 0.5f;                  /*!< Float delay time. */
	public float fadeTime;
	private float currentDelay;
	private float canvasAlpha=1f;
	public float timeLeft;
	private bool callInteract;
	private bool isFadingOut, isFadingIn;
	
	void Start(){
		if (fadeTime == 0)
			fadeTime = .5f;
		if (descriptionCanvas != null) {
			setCanvasAlphaForce(0f);
		}
	}

	void Update () {
		if(callInteract){
			currentDelay += Time.deltaTime;

			if(currentDelay > delay){
				callInteract = false;
				targetEquipment.StartState();
				currentDelay = 0;

			}
		}
	
	}

	void FixedUpdate(){
		if (isFadingIn) {
			if(timeLeft<=0f){
				setCanvasAlphaForce(canvasAlpha);
			}else{
				float increment = ((canvasAlpha - descriptionCanvas.GetComponentInChildren<Image>().color.a)/timeLeft)*Time.deltaTime;
				setCanvasAlphaForce(descriptionCanvas.GetComponentInChildren<Image>().color.a+increment);
				timeLeft-=Time.deltaTime;
			}
		}
		if(isFadingOut){
			if(timeLeft<=0f){
				setCanvasAlphaForce(canvasAlpha);
			}else{
				float increment = ((- descriptionCanvas.GetComponentInChildren<Image>().color.a)/timeLeft)*Time.deltaTime;
				setCanvasAlphaForce(descriptionCanvas.GetComponentInChildren<Image>().color.a+increment);
				timeLeft-=Time.deltaTime;
			}
		}
	}

    //! Does the interaction.
    /*! Fades the camera and starts the equipment state. */
	public override void Interact ()
	{
		FadeScript.instance.ShowFade();
		callInteract = true;
		HudText.SetText("");
		GetComponent<BoxCollider>().enabled = false;
	}

	public void fadeIn(){
		descriptionCanvas.enabled = true;
		isFadingOut = false;
		if (!isFadingIn)
			timeLeft = fadeTime;
		if (descriptionCanvas.GetComponentInChildren<Image> ().color.a != canvasAlpha)
			isFadingIn = true;
		else
			isFadingIn = false;
	}
	public void fadeOut(){
		descriptionCanvas.enabled = false;
		isFadingIn = false;
		if (!isFadingOut)
			timeLeft = fadeTime;
		if (descriptionCanvas.GetComponentInChildren<Image> ().color.a > 0)
			isFadingOut = true;
		else
			isFadingOut = false;
	}

	public void setCanvasAlpha(float a){
		canvasAlpha = a;
		if (!isFadingIn && !isFadingOut) {
			Color cor = descriptionCanvas.GetComponentInChildren<Image>().color;
			cor.a=a;
			descriptionCanvas.GetComponentInChildren<Image>().color=cor;
		}
	}

	private void setCanvasAlphaForce(float a){
		Color cor = descriptionCanvas.GetComponentInChildren<Image>().color;
		cor.a=a;
		descriptionCanvas.GetComponentInChildren<Image>().color=cor;
	}
}

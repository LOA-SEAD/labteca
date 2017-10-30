using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class SceneManager : MonoBehaviour {
	public Text escape, shadow;
	public float currentTime=5f;
	private bool transitionEnabled = false;

	public GameObject loadingSprite;
	public GameObject skipButton;

	// Use this for initialization
	void Start () {
		Screen.lockCursor = true;
		Cursor.visible = false;

		#if UNITY_STANDALONE
		transitionEnabled = true;
		loadingSprite.SetActive(false);
		skipButton.SetActive(true);
		#endif
		#if UNITY_WEBPLAYER
		transitionEnabled = false;
		loadingSprite.SetActive(true);
		skipButton.SetActive(false);
		#endif
	}
	
	// Update is called once per frame
	void Update () {
		if (transitionEnabled) {
			//If ESC is pressed, goes to next scene
			if (Input.GetKeyDown (KeyCode.Escape))
				Transition();
			else if (Input.anyKeyDown && currentTime >= 5f) {
				currentTime = 0f;
			}
		}
		//If another key is pressed sets trigger for showing message
		if (currentTime < 5) {
			if(currentTime<.5f){
				Color colorEscape = escape.color;
				Color colorShadow = shadow.color;

				colorShadow.a += (1f - colorShadow.a)*Time.fixedDeltaTime/(0.5f-currentTime);
				colorEscape.a += (1f - colorEscape.a)*Time.fixedDeltaTime/(0.5f-currentTime);

				escape.color = colorEscape;
				shadow.color = colorShadow;
			}else if(currentTime>4.5f){
				Color colorEscape = escape.color;
				Color colorShadow = shadow.color;
				
				colorShadow.a += (0f - colorShadow.a)*Time.fixedDeltaTime/(5f-currentTime);
				colorEscape.a += (0f - colorEscape.a)*Time.fixedDeltaTime/(5f-currentTime);
				
				escape.color = colorEscape;
				shadow.color = colorShadow;
			}
			if(currentTime<4.5f&&currentTime>0.5f){
				Color colorEscape = escape.color;
				Color colorShadow = shadow.color;
				
				colorShadow.a =1f;
				colorEscape.a =1f;
				
				escape.color = colorEscape;
				shadow.color = colorShadow;
			}

			currentTime+=Time.deltaTime;

			//Control de loading icon
		}
	}

	public void EnableTransition() {
		transitionEnabled = true;
		/*loadingSprite.SetActive(false);
		skipButton.SetActive(true);*/
	}

	public void Transition() {
		int i = Application.loadedLevel;
		Application.LoadLevel(i + 1);
	}
}

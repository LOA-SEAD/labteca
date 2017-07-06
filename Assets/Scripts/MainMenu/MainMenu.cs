using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class MainMenu : MonoBehaviour {

	public GameObject mainButtonsPanel;


	public GameObject loadPanel;



	public GameObject optionsPanel;
	public Slider musicSlider;
	public Text musicText;
	public Slider effectsSlider;
	public Text effectsText;

	private const string musicVolume = "soundVolume";
	private const string effectsVolume = "effectsVolume";



	// Use this for initialization
	void Start () {
		ReturnToMainMenu ();

		if (!PlayerPrefs.HasKey (musicVolume)) {
			PlayerPrefs.SetFloat (musicVolume, 1.0f);
		}
		if (!PlayerPrefs.HasKey (effectsVolume)) {
			PlayerPrefs.SetFloat (effectsVolume, 1.0f);
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	//Close all panels when called
	public void CloseAllPanels() {
		mainButtonsPanel.SetActive (false);
		optionsPanel.SetActive (false);
		//loadPanel.SetActive (false);
	}

	//Method used to go back to the main menu
	public void ReturnToMainMenu() {
		CloseAllPanels ();
		mainButtonsPanel.SetActive (true);
	}

	//-----------------
	//BUTTONS PANEL

	//Called when the Play Button is pressed
	public void ButtonPlay() {
		int i = Application.loadedLevel;
		Application.LoadLevel(i + 1);
	}

	//Called when the Load button is pressed
	public void ButtonLoad() {

	}
	
	//Called when the Options button is pressed
	public void ButtonOptions() {
		CloseAllPanels ();
		OnStartOptionsPanel ();
	}

	//Called when the Credits button is pressed
	public void ButtonCredits() {
	
	}

	//Called when the Exit button is pressed
	public void ButtonExit() {
		#if UNITY_EDITOR
		Debug.Log ("Quitting Application");	
		#else
		Application.Quit();
		#endif
	
	}

	//-------------------------
	//OPTIONS PANEL
	
	//Starts the OptionsPanel
	public void OnStartOptionsPanel () {
		optionsPanel.SetActive (true);

		musicSlider.value = PlayerPrefs.GetFloat (musicVolume) * 100.0f;
		effectsSlider.value = PlayerPrefs.GetFloat (effectsVolume) * 100.0f;
	}

	//Called while the music volume slider is being dragged
	public void UpdateMusicVolume() {
		musicText.text = musicSlider.value.ToString ();
	}
	//Called while the effects volume slider is being dragged
	public void UpdateEffectsVolume() {
		effectsText.text = effectsSlider.value.ToString ();
	}

	//Called on EndDrag of MusicVolume slider
	public void EndDragMusicVolume() {
		PlayerPrefs.SetFloat (musicVolume, musicSlider.value / 100.0f);
	}
	//Called on EndDrag of EffectsVolume slider
	public void EndDragEffectsVolume() {
		PlayerPrefs.SetFloat (effectsVolume, effectsSlider.value / 100.0f);
	}

	//-------------------------


}

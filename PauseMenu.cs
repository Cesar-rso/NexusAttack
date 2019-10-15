using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

public class PauseMenu : MonoBehaviour {


	public Canvas PauseCanvas;
	public GameObject ResumeButton, RestartButton, QuitButton, MusicButton, SoundButton, pauseText, PausePanel;
	public GameObject datamanager;
	public Sprite SoundOn, SoundOnYellow, SoundOff, SoundOffYellow, MusicOn, MusicOnYellow, MusicOff, MusicOffYellow;
	public int currentPauseMenu = 1;
	public AudioFXController AudioFX;
	public Vector2 PauseTextPos, ResumeBPos, RestartBPos, QuitBPos, SoundBPos, MusicBPos;

	public float timeOfTravel=5; //time after object reach a target place 
	public float currentTime=0; // actual floting time 
	public float normalizedValue;

	private Done_GameController gameController;
	private GameObject gameControllerObject, AudioControllerObject;

	// Use this for initialization
	void Start () {
		//VolumeSlider.GetComponent<AudioManager> ().VolumeSlider.value = datamanager.GetComponent<DataManager> ().volume;
		AudioControllerObject = GameObject.FindGameObjectWithTag ("AudioFX");
		if (AudioControllerObject != null) {

			AudioFX = AudioControllerObject.GetComponent<AudioFXController> ();
		}
		if (AudioFX == null) {

			Debug.Log ("Cannot find 'AudioFXController' script");
		}

		gameControllerObject = GameObject.FindGameObjectWithTag ("GameController");

		if (gameControllerObject != null)
		{
			gameController = gameControllerObject.GetComponent <Done_GameController>();

		}
		if (gameController == null)
		{
			Debug.Log ("Cannot find 'GameController' script");
		}
			

		if(PauseCanvas.enabled){
			Pause ();
		}
			

	}
	
	// Update is called once per frame
	void Update () {

		if(!PauseCanvas.enabled && currentPauseMenu==2){
			currentPauseMenu = 1;

			ResumeButton.SetActive (true);
			RestartButton.SetActive (true);
			//OptionsButton.SetActive (true);
			QuitButton.SetActive (true);
			MusicButton.SetActive (true);
			SoundButton.SetActive (true);
			//VolumeSlider.SetActive (false);
			//LanguageMenu.SetActive (false);
			//BackButton.SetActive (false);
		}

		if (PlayerPrefs.GetInt ("inTutorial") == 1) {
			PausePanel.transform.GetComponent<Animator>().SetBool("Begining", false);
		} else {
			PausePanel.transform.GetComponent<Animator>().SetBool("Begining", true);
		}
	
	}

	public void Pause (){
		//Debug.Log ("Apertou pausa");
		if (PauseCanvas != null) {

			if (PlayerPrefs.GetInt ("inTutorial") == 1) {
				PausePanel.transform.GetComponent<Animator>().SetBool("Begining", false);
			} else {
				PausePanel.transform.GetComponent<Animator>().SetBool("Begining", true);
			}

			if (PauseCanvas.enabled) {
				//Debug.Log (PausePanel.transform.GetComponent<Animator>().GetBool("Closing") + " " + PausePanel.transform.GetComponent<Animator>().GetBool("Opening"));

				if (!PausePanel.transform.GetComponent<Animator> ().GetBool ("Closing")) {
					Time.timeScale = 1f;
					PausePanel.transform.GetComponent<Animator> ().SetBool ("Closing", true);
					PausePanel.transform.GetComponent<Animator> ().SetBool ("Opening", false);
					StartCoroutine (WaitStart ());
					if (PausePanel.transform.localScale.x <= 0.02f) {
						//StartCoroutine (WaitStart ());
						PauseCanvas.enabled = false;
					}
				} else {
					PauseCanvas.enabled = true;
					StartCoroutine (WaitStart ());
					PausePanel.transform.GetComponent<Animator>().SetBool("Closing", false);
					PausePanel.transform.GetComponent<Animator>().SetBool("Opening", true);

					//if (PausePanel.transform.localScale.x >= 1) {
					//StartCoroutine (WaitStart ());
					Time.timeScale = 0f;
					GetComponent<AudioSource> ().Play ();
					//}
				}

			} else
			if(!PauseCanvas.enabled && PlayerPrefs.GetInt ("inTutorial") == 2) {
				//Debug.Log("Abrindo a pausa");
					PauseCanvas.enabled = true;
					StartCoroutine (WaitStart ());
					PausePanel.transform.GetComponent<Animator>().SetBool("Closing", false);
					PausePanel.transform.GetComponent<Animator>().SetBool("Opening", true);

					//if (PausePanel.transform.localScale.x >= 1) {
						//StartCoroutine (WaitStart ());
						Time.timeScale = 0f;
						GetComponent<AudioSource> ().Play ();
					//}
			}
		}
		
	}

	public void RestartLevel(){
		datamanager.GetComponent<DataManager> ().Save ();
		GetComponent<AudioSource> ().Play ();
		PlayerPrefs.SetInt("AlreadyRestarted", 1);
		SceneManager.LoadScene (SceneManager.GetActiveScene().buildIndex);
	}

	public void Exit(){
		datamanager.GetComponent<DataManager> ().Save ();
		GetComponent<AudioSource> ().Play ();
		//SceneManager.LoadScene ("Menu");
		gameController.sceneEnding = true;
		Pause ();
	}

	public void OnOffMusic(){
		bool volume = gameControllerObject.GetComponent<AudioSource> ().mute;

		if (!volume) {
			gameControllerObject.GetComponent<AudioSource> ().mute = true;
			MusicButton.GetComponent<Image> ().sprite = MusicOff;
			SpriteState spritestt = new SpriteState ();
			spritestt = MusicButton.GetComponent<Button> ().spriteState;
			spritestt.highlightedSprite = MusicOffYellow;
			spritestt.pressedSprite = MusicOffYellow;
			MusicButton.GetComponent<Button> ().spriteState = spritestt;
		}else {
			gameControllerObject.GetComponent<AudioSource> ().mute = false;
			MusicButton.GetComponent<Image> ().sprite = MusicOn;
			SpriteState spritestt = new SpriteState ();
			spritestt = MusicButton.GetComponent<Button> ().spriteState;
			spritestt.highlightedSprite = MusicOnYellow;
			spritestt.pressedSprite = MusicOnYellow;
			MusicButton.GetComponent<Button> ().spriteState = spritestt;
		}
	}

	public void OnOffSound(){
		bool volume = AudioControllerObject.GetComponent<AudioSource> ().mute;

		if (!volume) {
			AudioControllerObject.GetComponent<AudioSource> ().mute = true;
			SoundButton.GetComponent<Image> ().sprite = SoundOff;
			SpriteState spritestt = new SpriteState ();
			spritestt = SoundButton.GetComponent<Button> ().spriteState;
			spritestt.highlightedSprite = SoundOffYellow;
			spritestt.pressedSprite = SoundOffYellow;
			SoundButton.GetComponent<Button> ().spriteState = spritestt;
		}else {
			AudioControllerObject.GetComponent<AudioSource> ().mute = false;
			SoundButton.GetComponent<Image> ().sprite = SoundOn;
			SpriteState spritestt = new SpriteState ();
			spritestt = SoundButton.GetComponent<Button> ().spriteState;
			spritestt.highlightedSprite = SoundOnYellow;
			spritestt.pressedSprite = SoundOnYellow;
			SoundButton.GetComponent<Button> ().spriteState = spritestt;
		}
	}

	public void OptionsMenu(){

		if (currentPauseMenu == 1) {
			currentPauseMenu = 2;

			ResumeButton.SetActive (false);
			RestartButton.SetActive (false);
			//OptionsButton.SetActive (false);
			QuitButton.SetActive (false);
			MusicButton.SetActive (false);
			SoundButton.SetActive (false);
			//VolumeSlider.SetActive (true);
			//LanguageMenu.SetActive (true);
			//BackButton.SetActive (true);

		}
		else if(currentPauseMenu == 2){
			currentPauseMenu = 1;

			ResumeButton.SetActive (true);
			RestartButton.SetActive (true);
			//OptionsButton.SetActive (true);
			QuitButton.SetActive (true);
			MusicButton.SetActive (true);
			SoundButton.SetActive (true);
			//VolumeSlider.SetActive (false);
			//LanguageMenu.SetActive (false);
			//BackButton.SetActive (false);
		}

	}

	public void ChangeLanguage(){
	
		//string newLanguage = LanguageMenu.GetComponent<Dropdown> ().captionText.text;
		//Debug.Log (newLanguage);
		//datamanager.GetComponent<DataManager> ().language = newLanguage;  
	}

	IEnumerator WaitStart(){

		yield return new WaitForSeconds (0.45f);

	}
		
}

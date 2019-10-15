using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TutorialPopup : MonoBehaviour {

	public GameObject Popup, gameControllerObject, BackButton, ForwardButton, AsteroidPopUp, EnemiesPopUp, PowerUpPopUp, P1PopUp, P1Attack, P1Special, P1Movement;
	public bool inTutorial = true;
	public int currentPopUp;
	public AudioFXController AudioFX;
	public AudioClip nextTutorial;

	private Done_GameController gameController;

	// Use this for initialization
	void Start () {
		if (PlayerPrefs.GetInt ("AlreadyRestarted") == 1) {
			EndPopup ();
		} else {
			PlayerPrefs.SetInt ("inTutorial", 1); // 1 - yes, 2 - no
			StartCoroutine (WaitStart (1));
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

		GameObject AudioControllerObject = GameObject.FindGameObjectWithTag ("AudioFX");
		if (AudioControllerObject != null) {

			AudioFX = AudioControllerObject.GetComponent<AudioFXController> ();
		}
		if (AudioFX == null) {

			Debug.Log ("Cannot find 'AudioFXController' script");
		}
		currentPopUp = 1;
		BackButton.SetActive (false);
	}
	
	// Update is called once per frame
	void Update () {

		if (currentPopUp == 1) {
			BackButton.SetActive (false);
		} else {
			BackButton.SetActive (true);
		}

		if (currentPopUp == 4) {
			ForwardButton.SetActive (false);
		} else {
			ForwardButton.SetActive (true);
		}
			
		if (Input.GetKeyDown (KeyCode.Space) || Input.GetKeyDown (KeyCode.Return) || Input.GetButtonDown ("1Start") || Input.GetButtonDown ("2Start")) {
			EndPopup ();
		}

		if (inTutorial) {
			StartCoroutine (WaitStart (1));
			PlayerPrefs.SetInt ("inTutorial", 1);
		} else {
			Time.timeScale = 1f;
			PlayerPrefs.SetInt ("inTutorial", 2);
		}

		if (PlayerPrefs.GetInt ("AlreadyRestarted") == 1) {
			EndPopup ();
		}
	}

	public void EndPopup(){
		inTutorial = false;
		Time.timeScale = 1f;
		PlayerPrefs.SetInt ("AlreadyRestarted", 1);
		PlayerPrefs.SetInt ("inTutorial", 2);
		//gameControllerObject.GetComponent <Done_GameController>().tutorialOn = false;
		transform.GetComponent<Animator>().SetBool("Closing", true);
		if(transform.localScale.x <= 0.3f){
			StartCoroutine (WaitStart (2));
			Popup.SetActive (false);
			Destroy (this);
		}
		//Destroy(Popup);
	}

	public void ChangePopUp(int direction){

		if (direction == 1) {
			currentPopUp++;
		}

		if (direction == 2) {
			currentPopUp--;
		}

		switch (currentPopUp) {

		case 1:
			P1PopUp.SetActive (true);
			P1Special.SetActive (true);
			P1Attack.SetActive (true);
			P1Movement.SetActive (true);

			AsteroidPopUp.SetActive (false);
			EnemiesPopUp.SetActive (false);
			PowerUpPopUp.SetActive (false);
			break;

		case 2:
			P1PopUp.SetActive (false);
			P1Special.SetActive (false);
			P1Attack.SetActive (false);
			P1Movement.SetActive (false);

			AsteroidPopUp.SetActive (true);
			EnemiesPopUp.SetActive (false);
			PowerUpPopUp.SetActive (false);
			break;

		case 3:
			P1PopUp.SetActive (false);
			P1Special.SetActive (false);
			P1Attack.SetActive (false);
			P1Movement.SetActive (false);

			AsteroidPopUp.SetActive (false);
			EnemiesPopUp.SetActive (true);
			PowerUpPopUp.SetActive (false);
			break;

		case 4:
			P1PopUp.SetActive (false);
			P1Special.SetActive (false);
			P1Attack.SetActive (false);
			P1Movement.SetActive (false);

			AsteroidPopUp.SetActive (false);
			EnemiesPopUp.SetActive (false);
			PowerUpPopUp.SetActive (true);
			break;
		}

		AudioFX.MainSource.PlayOneShot (nextTutorial);
	}

	IEnumerator WaitStart(int type){

		yield return new WaitForSeconds (0.45f);
		if (type == 1) {
			Time.timeScale = 0f;
		}
	}
		
}

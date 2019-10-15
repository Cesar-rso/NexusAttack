using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerSelection : MonoBehaviour {

	public GameObject ShipImage, Ship2Image, P2Start, P2BackSquare;
	public GameObject[] Power1, Power2, Life1, Life2, FRate1, FRate2, Speed1, Speed2;
	public Sprite[] Ships;
	public Image FadeImage;
	public Text ShipName;
	public int currentShip1, currentShip2;
	public bool P1chosenShip = false, P2chosenShip = false, P2started = false;
	private bool sceneEnding = false;
	public AudioClip Selection, GotShip, SameShipBlock;



	// Use this for initialization
	void Start () {

		#if UNITY_STANDALONE || UNITY_WEBPLAYER
		P2BackSquare.SetActive (false);
		P2Start.SetActive (true);
		#endif

		PlayerPrefs.SetInt ("Player2in", 2);
		sceneEnding = false;
	}
	
	// Update is called once per frame
	void Update () { 
			
		if(sceneEnding) {
			EndScene ();
			//Debug.Log (FadeImage.color.a);
		}

		if(Input.GetKeyDown (KeyCode.Escape) || Input.GetButtonDown("1B")){
			SceneManager.LoadScene ("Menu");
		}

		#if UNITY_STANDALONE || UNITY_WEBPLAYER
		if (!P1chosenShip) {
			if ((Input.GetKeyDown (KeyCode.D) || Input.GetAxis ("1D-Pad Horizontal") > 0.2f) && !P1chosenShip) {
				ChangeShip ();
			}

			if ((Input.GetKeyDown (KeyCode.A) || Input.GetAxis ("1D-Pad Horizontal") < -0.2f) && !P1chosenShip) {
				BackShip ();
			}
		}

		if (!P2chosenShip) {
			if ((Input.GetKeyDown (KeyCode.LeftArrow) || Input.GetAxis ("2D-Pad Horizontal") > 0.2f) && !P2chosenShip) {
				ChangeShip2 ();
			}

			if ((Input.GetKeyDown (KeyCode.RightArrow) || Input.GetAxis ("2D-Pad Horizontal") < -0.2f) && !P2chosenShip) {
				BackShip2 ();
			}
		}
		#endif
		//Debug.Log (Input.GetAxis ("1D-Pad Horizontal"));

		switch (currentShip1) {

		case 0:
			Power1 [0].SetActive (true);
			Power1 [1].SetActive (true);
			Power1 [2].SetActive (false);
			Life1 [0].SetActive (true);
			Life1 [1].SetActive (false);
			Life1 [2].SetActive (false);
			FRate1 [0].SetActive (true);
			FRate1 [1].SetActive (true);
			FRate1 [2].SetActive (false);
			Speed1 [0].SetActive (true);
			Speed1 [1].SetActive (true);
			Speed1 [2].SetActive (false);
			ShipName.text = "Alpha";
			break;
		
		case 1:
			Power1 [0].SetActive (true);
			Power1 [1].SetActive (true);
			Power1 [2].SetActive (true);
			Life1 [0].SetActive (true);
			Life1 [1].SetActive (true);
			Life1 [2].SetActive (false);
			FRate1 [0].SetActive (true);
			FRate1 [1].SetActive (false);
			FRate1 [2].SetActive (false);
			Speed1 [0].SetActive (true);
			Speed1 [1].SetActive (true);
			Speed1 [2].SetActive (true);
			ShipName.text = "Beta";
			break;

		case 2:
			Power1 [0].SetActive (true);
			Power1 [1].SetActive (true);
			Power1 [2].SetActive (true);
			Life1 [0].SetActive (true);
			Life1 [1].SetActive (true);
			Life1 [2].SetActive (true);
			FRate1 [0].SetActive (true);
			FRate1 [1].SetActive (false);
			FRate1 [2].SetActive (false);
			Speed1 [0].SetActive (true);
			Speed1 [1].SetActive (false);
			Speed1 [2].SetActive (false);
			ShipName.text = "Delta";
			break;

		case 3:
			Power1 [0].SetActive (true);
			Power1 [1].SetActive (false);
			Power1 [2].SetActive (false);
			Life1 [0].SetActive (true);
			Life1 [1].SetActive (true);
			Life1 [2].SetActive (false);
			FRate1 [0].SetActive (true);
			FRate1 [1].SetActive (true);
			FRate1 [2].SetActive (true);
			Speed1 [0].SetActive (true);
			Speed1 [1].SetActive (true);
			Speed1 [2].SetActive (false);
			ShipName.text = "Gamma";
			break;

		}

		#if UNITY_STANDALONE || UNITY_WEBPLAYER
		switch (currentShip2) {

		case 0:
			Power2 [0].SetActive (true);
			Power2 [1].SetActive (true);
			Power2 [2].SetActive (true);
			Life2 [0].SetActive (true);
			Life2 [1].SetActive (false);
			Life2 [2].SetActive (false);
			FRate2 [0].SetActive (true);
			FRate2 [1].SetActive (true);
			FRate2 [2].SetActive (false);
			Speed2 [0].SetActive (true);
			Speed2 [1].SetActive (true);
			Speed2 [2].SetActive (false);
			break;

		case 1:
			Power2 [0].SetActive (true);
			Power2 [1].SetActive (true);
			Power2 [2].SetActive (false);
			Life2 [0].SetActive (true);
			Life2 [1].SetActive (true);
			Life2 [2].SetActive (false);
			FRate2 [0].SetActive (true);
			FRate2 [1].SetActive (false);
			FRate2 [2].SetActive (false);
			Speed2 [0].SetActive (true);
			Speed2 [1].SetActive (true);
			Speed2 [2].SetActive (true);
			break;

		case 2:
			Power2 [0].SetActive (true);
			Power2 [1].SetActive (true);
			Power2 [2].SetActive (false);
			Life2 [0].SetActive (true);
			Life2 [1].SetActive (true);
			Life2 [2].SetActive (true);
			FRate2 [0].SetActive (true);
			FRate2 [1].SetActive (true);
			FRate2 [2].SetActive (false);
			Speed2 [0].SetActive (true);
			Speed2 [1].SetActive (false);
			Speed2 [2].SetActive (false);
			break;

		case 3:
			Power2 [0].SetActive (true);
			Power2 [1].SetActive (false);
			Power2 [2].SetActive (false);
			Life2 [0].SetActive (true);
			Life2 [1].SetActive (true);
			Life2 [2].SetActive (false);
			FRate2 [0].SetActive (true);
			FRate2 [1].SetActive (true);
			FRate2 [2].SetActive (true);
			Speed2 [0].SetActive (true);
			Speed2 [1].SetActive (true);
			Speed2 [2].SetActive (false);
			break;

		}
		#endif

		if (!P2started) {
			if (currentShip1 == 3) {
				currentShip2 = 0;
			} else {
				currentShip2 = currentShip1 + 1;
			}

		} 

		if(P1chosenShip){
			if (!P2started) {
				sceneEnding = true;
				//StartCoroutine (StartPlay ());
			} else {
				if(P2chosenShip){
					sceneEnding = true;
					//StartCoroutine (StartPlay ());
				}
			}
		}

		#if UNITY_STANDALONE || UNITY_WEBPLAYER
		if (Input.GetKeyDown (KeyCode.Return) || Input.GetButtonDown("2Start")) {
			if (!P2started) {
				ConfirmPlayer2 ();
			} else {
				ConfirmShip (2);
			}
		}
		#endif

		if(Input.GetKeyDown(KeyCode.Space) || Input.GetButtonDown("1Start")){
			ConfirmShip (1);
		}
	}

	public void ChangeShip(){
		if (!P1chosenShip) {
			if (currentShip1 == 3) {
				currentShip1 = 0;
				ShipImage.GetComponent<Image> ().sprite = Ships [currentShip1];
			} else {
				currentShip1++;
				ShipImage.GetComponent<Image> ().sprite = Ships [currentShip1];
			}

			GetComponent<AudioSource> ().PlayOneShot (Selection);
		}
	}

	#if UNITY_STANDALONE || UNITY_WEBPLAYER
	public void ChangeShip2(){
		if (P2started) {
			if (!P2chosenShip) {
				if (currentShip2 == 3) {
					currentShip2 = 0;
					Ship2Image.GetComponent<Image> ().sprite = Ships [currentShip2];
				} else {
					currentShip2++;
					Ship2Image.GetComponent<Image> ().sprite = Ships [currentShip2];
				}

				GetComponent<AudioSource> ().PlayOneShot (Selection);
			}
		}
	}
	#endif

	public void BackShip(){
		if (!P1chosenShip) {
			if (currentShip1 == 0) {
				currentShip1 = 3;
				ShipImage.GetComponent<Image> ().sprite = Ships [currentShip1];
			} else {
				currentShip1--;
				ShipImage.GetComponent<Image> ().sprite = Ships [currentShip1];
			}

			GetComponent<AudioSource> ().PlayOneShot (Selection);
		}
	}

	#if UNITY_STANDALONE || UNITY_WEBPLAYER
	public void BackShip2(){
		if (P2started) {
			if (!P2chosenShip) {
				if (currentShip2 == 0) {
					currentShip2 = 3;
					Ship2Image.GetComponent<Image> ().sprite = Ships [currentShip2];
				} else {
					currentShip2--;
					Ship2Image.GetComponent<Image> ().sprite = Ships [currentShip2];
				}

				GetComponent<AudioSource> ().PlayOneShot (Selection);
			}
		}
	}
	#endif

	public void ConfirmShip(int player){

		#if UNITY_STANDALONE || UNITY_WEBPLAYER
		if (currentShip1 != currentShip2) {
			if (player == 1) {
				P1chosenShip = true;
				//gameController.P1Ship = currentShip1;
				PlayerPrefs.SetInt ("P1Ship", currentShip1);
			}

			if (player == 2) {
				P2chosenShip = true;
				PlayerPrefs.SetInt ("P2Ship", currentShip2);
			}
		

			GetComponent<AudioSource> ().PlayOneShot (GotShip);
		} else {
			GetComponent<AudioSource> ().PlayOneShot (SameShipBlock);
		}
		#elif UNITY_ANDROID

		P1chosenShip = true;
		PlayerPrefs.SetInt ("P1Ship", currentShip1);
		GetComponent<AudioSource> ().PlayOneShot (GotShip);
		Destroy(GameObject.FindGameObjectWithTag ("AudioFX"));
		#endif
	}

	#if UNITY_STANDALONE || UNITY_WEBPLAYER
	public void ConfirmPlayer2(){
		P2started = true;
		P2BackSquare.SetActive (true);
		P2Start.SetActive (false);
		PlayerPrefs.SetInt ("Player2in", 1);

		GetComponent<AudioSource> ().PlayOneShot(GotShip);
	}
	#endif

	public void EndScene ()
	{
		
		StartCoroutine (StartPlay());
		
	}

	public void ToMenu(){

		StartCoroutine (BackToMenu());
	
	}

	IEnumerator StartPlay(){

		float fadeTime = Camera.main.GetComponent<FadeScene2> ().BeginFade (1);
		yield return new WaitForSeconds (fadeTime);
		SceneManager.LoadScene (SceneManager.GetActiveScene().buildIndex + 1);

	}

	IEnumerator BackToMenu(){

		float fadeTime = Camera.main.GetComponent<FadeScene2> ().BeginFade (1);
		yield return new WaitForSeconds (fadeTime);
		SceneManager.LoadScene (SceneManager.GetActiveScene().buildIndex - 1);

	}
}

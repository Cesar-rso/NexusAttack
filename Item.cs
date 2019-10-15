using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour {

	public int ItemType = 0;
	public AudioFXController AudioFX;
	private Done_GameController gameController;


	void Start ()
	{
		GameObject gameControllerObject = GameObject.FindGameObjectWithTag ("GameController");
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
	}

	void OnTriggerEnter2D (Collider2D other)
	{
		if (other.tag == "Player") {

			//AudioFX.GotItem ();

			if (transform.name == "DoubleShot" || ItemType == 1) {
				AudioFX.GotItem ();
				if (other.GetComponent<Done_PlayerController> ().ShotType == 1) {
					other.GetComponent<Done_PlayerController> ().ShotType = 2;
				} else {
					other.GetComponent<Done_PlayerController> ().ShotType = 4;
				}
				Destroy (transform.gameObject);
			}

			/*if (transform.name == "TripleShot" || ItemType == 2) {
				AudioFX.GotItem ();
				other.GetComponent<Done_PlayerController> ().ShotType = 3;
				Destroy (transform.gameObject);
			}*/
			if ((transform.name == "SpecialRecharge" || ItemType == 3)&& other.GetComponent<Done_PlayerController> ().SpFire < 4) {
				AudioFX.GotItem ();
				other.GetComponent<Done_PlayerController> ().SpFire = other.GetComponent<Done_PlayerController> ().SpFire + 1;
				Destroy (transform.gameObject);
			}
			if (transform.name == "LifeRecharge"  || ItemType == 4) {
				AudioFX.GotItem ();
				other.GetComponent<Done_PlayerController> ().GotHit(-3);
				Destroy (transform.gameObject);
			}
			if (transform.name == "Shield"  || ItemType == 5) {
				AudioFX.ShieldsUp ();
				other.GetComponent<Done_PlayerController> ().ShieldCharge += 3;
				Destroy (transform.gameObject);
			}
			if (transform.name == "Bonus"  || ItemType == 6) {
				AudioFX.GotItem ();
				gameController.scoreBonus = 2;
				Destroy (transform.gameObject);
			}
			if (transform.name == "PowerUpShot" || ItemType == 7) {
				AudioFX.GotItem ();
				if (other.GetComponent<Done_PlayerController> ().currentPlayer == 1) {
					gameController.P1PowerUpShot++;
				}
				if (other.GetComponent<Done_PlayerController> ().currentPlayer == 2) {
					gameController.P2PowerUpShot++;
				}
				Destroy (transform.gameObject);
			}
				
		}
	}
}

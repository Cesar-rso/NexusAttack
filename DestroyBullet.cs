using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyBullet : MonoBehaviour {

	public int scoreValue, bulletOrigin;
	public GameObject explosion, playerExplosion, rocketExplosion;
	private Done_GameController gameController;
	public AudioFXController AudioFX;

	// Use this for initialization
	void Start () {
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

	void Update(){
	
		if (transform.position.y > 9 || transform.position.y < -7) {
			Destroy (transform.gameObject);
		}
	
	}

	void OnTriggerEnter2D(Collider2D other){

		if (other.tag.Contains ("Pathing")) {
			return;
		}
		
		if((other.tag.Contains("Enemy") && transform.name.Contains("Player")) || (other.tag.Contains("Enemy") && transform.name.Contains("Special"))){  //Player bullets hitting enemies

			Instantiate(explosion, transform.position, transform.rotation);
			if (transform.name.Contains ("Rocket")) {
				GameObject RocketBoom = Instantiate(rocketExplosion, transform.position, transform.rotation);
				RocketBoom.GetComponent<Done_DestroyByTime> ().Player = bulletOrigin;
				AudioFX.GotShot ();
			}
			AudioFX.DestroyedEnemySound ();
			Destroy (transform.gameObject);
		}

		if ((other.tag.Contains ("Boss") && transform.name.Contains("Player")) || (other.tag.Contains("Boss") && transform.name.Contains("Special"))) { //Player bullets hitting bosses

			Instantiate(explosion, transform.position, transform.rotation);
			if (transform.name.Contains ("Rocket")) {
				GameObject RocketBoom = Instantiate(rocketExplosion, transform.position, transform.rotation);
				RocketBoom.GetComponent<Done_DestroyByTime> ().Player = bulletOrigin;
				AudioFX.GotShot ();
			}
			//AudioFX.GotShot ();
			Destroy (transform.gameObject);
		
		}

		if (other.tag.Equals ("Boundary")) { //Player bullets going through the boundaries
			Destroy (transform.gameObject);
		}

		if (other.tag.Equals ("Player") && transform.tag.Contains ("Enemy")) { // enemy bullets hitting the player
			
			Instantiate (playerExplosion, other.transform.position, other.transform.rotation);
			if (transform.name.Contains ("Rocket")) {
				GameObject RocketBoom = Instantiate(rocketExplosion, transform.position, transform.rotation);
				other.GetComponent<Done_PlayerController> ().GotHit (1);
				RocketBoom.GetComponent<Done_DestroyByTime> ().Player = bulletOrigin;
				AudioFX.GotShot ();
			}
			other.GetComponent<Done_PlayerController> ().GotHit (2);
			if (other.GetComponent<Done_PlayerController> ().playerLife <= 0) {
				gameController.PlayerDestroyed(other.GetComponent<Done_PlayerController> ().currentPlayer);
				Destroy (other.gameObject);
			}
		}

		if(other.tag.Contains("Attack") && transform.tag.Contains ("Enemy")){ //enemy bulletd hitting player bullets
			Instantiate (playerExplosion, other.transform.position, other.transform.rotation);
			if (transform.name.Contains ("Rocket")) {
				GameObject RocketBoom = Instantiate(rocketExplosion, transform.position, transform.rotation);
				RocketBoom.GetComponent<Done_DestroyByTime> ().Player = bulletOrigin;
				AudioFX.GotShot ();
			}
			Destroy (other.gameObject);
			Destroy (transform.gameObject);
		}
	
	}
}

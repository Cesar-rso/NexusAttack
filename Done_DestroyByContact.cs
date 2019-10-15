using UnityEngine;
using System.Collections;

public class Done_DestroyByContact : MonoBehaviour
{
	public GameObject explosion, nextColor;
	public GameObject playerExplosion, miniAsteroid, smokeFX;
	public AudioFXController AudioFX;
	public GameObject DoubleShot, TripleShot, SpecialRecharge, LifeRecharge, Shield, Bonus, PowerUp;
	public int scoreValue, lastHitByPlayer, UpgradeShot;
	private Done_GameController gameController;
	private int EnemyLife = 4;
	public Sprite[] Colors;
	private int lastColor = 0;
	public float timer = 0f;
	public bool smoking = false;

	void Start ()
	{
		GameObject gameControllerObject = GameObject.FindGameObjectWithTag ("GameController");
		if (gameControllerObject != null)
		{
			gameController = gameControllerObject.GetComponent <Done_GameController>();
			if (gameController.WaveCount > 1 && !transform.tag.Contains ("Asteroid")) {
				EnemyLife = 4 + (gameController.WaveCount/4);
			}
			//GetComponent<SpriteRenderer> ().color = new Vector4 (gameController.EnemyColorR/255, gameController.EnemyColorG/255, gameController.EnemyColorB/255, 255f);
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

		if (!transform.tag.Contains ("Asteroid")) {
			nextColor.AddComponent<SpriteRenderer> ();
			nextColor.GetComponent<SpriteRenderer> ().color = new Vector4 (255f, 255f, 255f, 0f);

			StartCoroutine (PhasingColors ());
		} else {
			EnemyLife = 4;
		}
	}

	void Update(){
		if (AudioFX == null) {
			GameObject AudioControllerObject = GameObject.FindGameObjectWithTag ("AudioFX");
			if (AudioControllerObject != null) {

				AudioFX = AudioControllerObject.GetComponent<AudioFXController> ();
			}

		}

		if (transform.position.y > 9 || transform.position.y < -7) {
			Destroy (transform.gameObject);
		}
	}

	void OnTriggerEnter2D (Collider2D other)
	{
		if (other.tag == "Boundary" || other.tag.Contains("Enemy") || other.tag == "Item" || other.tag == "Pathing" || other.tag == "BossPathing")
		{
			return;
		}

		if (other.tag == "Boss" && transform.tag == "Asteroid") {
			return;
		}

		if (explosion != null)
		{
			Instantiate(explosion, transform.position, transform.rotation);
			//AudioFX.GotShot ();
		}

		if (other.tag == "Player") {
			if (!smoking) {
				Instantiate (playerExplosion, other.transform.position, other.transform.rotation);
				other.GetComponent<Done_PlayerController> ().GotHit (2);
				AudioFX.GotShot ();
				if (other.GetComponent<Done_PlayerController> ().playerLife <= 0) {
					gameController.PlayerDestroyed (other.GetComponent<Done_PlayerController> ().currentPlayer);
					Destroy (other.gameObject);
				}
			} else {
				Instantiate (playerExplosion, other.transform.position, other.transform.rotation);
				other.GetComponent<Done_PlayerController> ().ShotUpgrade = UpgradeShot;
			}
		} 

		if (other.tag.Contains ("Attack")) {
			lastHitByPlayer = other.GetComponent<DestroyBullet> ().bulletOrigin;
		}

		if (other.tag == "Special" && other.name.Contains ("Rocket")) {
			lastHitByPlayer = other.GetComponent<Done_DestroyByTime> ().Player;
		}
		int damage = 2;

		if (lastHitByPlayer == 1) {
			damage = 2 + gameController.P1PowerUpShot;
		}

		if (lastHitByPlayer == 2) {
			damage = 2 + gameController.P2PowerUpShot;
		}

		EnemyLife = EnemyLife - damage;
		if (transform.tag == "Enemy" && !transform.name.Contains("Attack") && EnemyLife <= 0) {
				if (Random.Range (1, 100) <= 25) {
					int itemchance = Random.Range (1, 14);
					if (itemchance <= 2) {
						Instantiate (DoubleShot, other.transform.position, new Quaternion(0f, 0f, 0f, 0f));
					} else if (itemchance <= 4 && itemchance > 2) {
						//Instantiate (TripleShot, other.transform.position, other.transform.rotation);
					} else if (itemchance <= 6 && itemchance > 4) {
						Instantiate (LifeRecharge, other.transform.position, new Quaternion(0f, 0f, 0f, 0f));
					} else if (itemchance <= 8 && itemchance > 6) {
						Instantiate (SpecialRecharge, other.transform.position, new Quaternion(0f, 0f, 0f, 0f));
					} else if (itemchance < 10 && itemchance > 8) {
						Instantiate (Shield, other.transform.position, new Quaternion(0f, 0f, 0f, 0f));
					} else if (itemchance > 10 && itemchance <= 12) {
						Instantiate (Bonus, other.transform.position, new Quaternion(0f, 0f, 0f, 0f));
					} else if (itemchance >= 13) {
						Instantiate (PowerUp, other.transform.position, new Quaternion(0f, 0f, 0f, 0f));
					}
				}
			//}
			if (EnemyLife <= 0) {
				//AudioFX.DestroyedEnemySound ();
				if (lastHitByPlayer == 1) {
					gameController.AddScore (scoreValue);
				}
				#if UNITY_STANDALONE || UNITY_WEBPLAYER
				if (lastHitByPlayer == 2) {
					gameController.AddScoreP2 (scoreValue);
				}
				#endif

				gameController.DeadEnemy ();
				Destroy (gameObject);
			}


		}

		if(transform.tag == "Enemy" && !transform.name.Contains("Attack") && EnemyLife < 4 /*&& gameController.WaveCount >= 7*/){
			GameObject lowSmoke = Instantiate (smokeFX, transform.position, transform.rotation);
			lowSmoke.transform.SetParent (this.transform);
			smoking = true;
		}

		if (transform.tag.Contains ("Asteroid") && !other.tag.Contains ("Item")) {
			if (EnemyLife <= 0) {
				if (lastHitByPlayer == 1) {
					gameController.AddScore (scoreValue);
				}
				#if UNITY_STANDALONE || UNITY_WEBPLAYER
				if (lastHitByPlayer == 2) {
					gameController.AddScoreP2 (scoreValue);
				}
				#endif

				if (!transform.name.Contains ("mini")) {
					AudioFX.GotShot ();
					Instantiate (miniAsteroid, transform.position, new Quaternion (transform.rotation.x, transform.rotation.y, -25f, transform.rotation.w));
					Instantiate (miniAsteroid, transform.position, new Quaternion (transform.rotation.x, transform.rotation.y, 10f, transform.rotation.w));
					Instantiate (miniAsteroid, transform.position, new Quaternion (transform.rotation.x, transform.rotation.y, 40f, transform.rotation.w));
					Instantiate (miniAsteroid, transform.position, transform.rotation);
				}

				Destroy (gameObject);
			}
		}
	}

	void OnTriggerStay2D(Collider2D other){

		if (other.tag == "Special") {
			if (timer > 0.3f) {
				timer = 0f;
				if (EnemyLife <= 3) {
					AudioFX.DestroyedEnemySound ();
				}
				if (explosion != null) {
					Instantiate (explosion, transform.position, transform.rotation);
					//AudioFX.GotShot ();
				}
				EnemyLife = EnemyLife - 7;
			}
			timer = timer + Time.deltaTime;
		}

		if (other.tag.Contains ("Attack")) {
			lastHitByPlayer = other.GetComponent<DestroyBullet> ().bulletOrigin;
			if (timer > 0.3f) {
				timer = 0f;
				if (EnemyLife <= 3) {
					AudioFX.DestroyedEnemySound ();
				}
				if (explosion != null) {
					Instantiate (explosion, transform.position, transform.rotation);
					AudioFX.GotShot ();
				}

				int damage = 2;

				if (lastHitByPlayer == 1) {
					damage = 2 + gameController.P1PowerUpShot;
				}

				if (lastHitByPlayer == 2) {
					damage = 2 + gameController.P2PowerUpShot;
				}

				EnemyLife = EnemyLife - (damage/2);
			}
			timer = timer + Time.deltaTime;
		}

	}

	IEnumerator PhasingColors(){

		while (true) {
			int i = lastColor;
			while (i == lastColor) {
				i = Random.Range (0,7);
			}

			nextColor.GetComponent<SpriteRenderer> ().sprite = Colors [i];
			lastColor = i;

			float timer = 0f;

			while (timer < 1f) {

				yield return new WaitForSeconds (0.1f);

				GetComponent<SpriteRenderer> ().color = new Vector4 (255, 255, 255, GetComponent<SpriteRenderer> ().color.a - 25.5f);

				nextColor.GetComponent<SpriteRenderer> ().color = new Vector4 (255, 255, 255, nextColor.GetComponent<SpriteRenderer> ().color.a + 25.5f);

				timer = timer + 0.1f;
			
			}

			GetComponent<SpriteRenderer> ().sprite = Colors [i];
			GetComponent<SpriteRenderer> ().color = new Vector4 (255, 255, 255, 255);

			nextColor.GetComponent<SpriteRenderer> ().color = new Vector4 (255, 255, 255, 0f);
		}
	
	}
}
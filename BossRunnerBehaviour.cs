using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossRunnerBehaviour : MonoBehaviour {

	private int BossLife = 190, maxBossLife;
	public int lastHitByPlayer;
	public bool startposition = false, Shooting = false, shootingCoolDown = true;
	public GameObject explosion, bullet;
	//public AudioClip normalexplosion, deathexplosion;
	public Done_GameController gameController;
	public float speed, timer = 0f;
	public GameObject[] players;
	public GameObject[] runningPositions;
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

		//explosion.GetComponent<AudioSource> ().clip = normalexplosion;

		players = GameObject.FindGameObjectsWithTag ("Player");
		maxBossLife = BossLife;
	}

	// Update is called once per frame
	void Update () {

		if (BossLife <= 0) {
			if (lastHitByPlayer == 1) {
				gameController.AddScore (2500);
			}
			#if UNITY_STANDALONE || UNITY_WEBPLAYER
			if (lastHitByPlayer == 2) {
				gameController.AddScoreP2 (2500);
			}
			#endif
			gameController.DeadEnemy ();
			gameController.GetComponent<AudioSource> ().clip = gameController.MainTheme;
			gameController.GetComponent<AudioSource> ().loop = false;
			gameController.GetComponent<AudioSource> ().Play ();
			Destroy (transform.gameObject);
		}

		if(transform.position.y <= 4f && !startposition){
			startposition = true;
		}

		//Specific boss behaviour
		if (startposition) {

			if (transform.position.x <= 0f) {
				if (transform.position.y >= 2.5f) {

					if (!Shooting && shootingCoolDown) {

						StartCoroutine (ShotgunAttack ());
					} else {

						transform.position = Vector3.MoveTowards (transform.position, new Vector3(10f, 4.5f, 0f), (speed / 2f) * Time.deltaTime);
						//transform.position = Vector3.MoveTowards (transform.position, runningPositions [1].transform.position, (speed / 2f) * Time.deltaTime);
					}
				}

				if (transform.position.y >= 0.5f && transform.position.y <= 2f) {
					transform.position = Vector3.MoveTowards (transform.position, runningPositions [3].transform.position, (speed / 2f) * Time.deltaTime);
				}

				if (transform.position.y <= -2.5f && transform.position.y >= -3.5f) {
					transform.position = Vector3.MoveTowards (transform.position, runningPositions [5].transform.position, (speed / 2f) * Time.deltaTime);
				}
			}

			if (transform.position.x > 0f) {
				
				if (transform.position.y >= 2.5f) {
					
					if (transform.position.x >= 9.5f) {
						transform.position = new Vector3 (runningPositions [2].transform.position.x, runningPositions [2].transform.position.y, runningPositions [2].transform.position.z);
					} else {
						transform.position = Vector3.MoveTowards (transform.position, new Vector3(10f, 4.5f, 0f), (speed / 2f) * Time.deltaTime);
					}
				}

				if (transform.position.y >= 0.5f && transform.position.y <= 2f) {
					if (transform.position.x >= 9.5f) {
						transform.position = new Vector3 (runningPositions [4].transform.position.x, runningPositions [4].transform.position.y, runningPositions [4].transform.position.z);
					} else {
						transform.position = Vector3.MoveTowards (transform.position, runningPositions [3].transform.position, (speed / 2f) * Time.deltaTime);
					}
				}

				if (transform.position.y <= -2.5f && transform.position.y >= -3.5f) {
					if (transform.position.x >= 9.5f) {
						transform.position = new Vector3 (runningPositions [0].transform.position.x, runningPositions [0].transform.position.y, runningPositions [0].transform.position.z);
					} else {
						transform.position = Vector3.MoveTowards (transform.position, runningPositions [5].transform.position, (speed / 2f) * Time.deltaTime);
					}
				}
			}
				
		} else {
			transform.position = Vector3.MoveTowards (transform.position, new Vector3 (0f, 3f, 0f), (speed / 2f) * Time.deltaTime);
		}
	}

	public void DamageBoss(int damage){

		BossLife = BossLife - damage;

	}

	void OnTriggerEnter2D (Collider2D other)
	{
		
		if (other.tag == "Boundary" || other.tag == "Enemy" || other.tag == "Item") {
			return;
		}
			

		if (other.tag == "Player") {

			other.GetComponent<Done_PlayerController> ().GotHit (1);
		}

		if (other.tag == "Attack") {
			lastHitByPlayer = other.GetComponent<DestroyBullet> ().bulletOrigin;
			if (BossLife <= 2) {
				//explosion.GetComponent<AudioSource> ().clip = deathexplosion;
				Debug.Log("Boss dead by " + other.name);
				AudioFX.DeadBoss ();
			} else {
				AudioFX.DestroyedEnemySound ();
			}
			if (explosion != null)
			{
				Instantiate(explosion, transform.position, transform.rotation);
			}
			DamageBoss (2);
			StartCoroutine (BlinkingAfterHit());
		}

		if (other.tag == "Special") {
			if (BossLife <= 3) {
				//explosion.GetComponent<AudioSource> ().clip = deathexplosion;
				Debug.Log("Boss dead by " + other.name);
				AudioFX.DeadBoss ();
			} else {
				AudioFX.DestroyedEnemySound ();
			}
			if (explosion != null)
			{
				Instantiate(explosion, transform.position, transform.rotation);
			}
			DamageBoss (3);
			StartCoroutine (BlinkingAfterHit());
		}
			

	}

	IEnumerator BlinkingAfterHit(){

		for (i = 0; i < 2; i++){
			GetComponent<SpriteRenderer> ().enabled = false;

			yield return new WaitForSeconds (0.1f);

			GetComponent<SpriteRenderer> ().enabled = true;

			yield return new WaitForSeconds (0.1f);
		}

	}

	IEnumerator ShotgunAttack(){
		if (transform.position.x < 0f) {
			transform.position = Vector3.MoveTowards (transform.position, new Vector3 (0f, transform.position.y, transform.position.z), (speed / 2f) * Time.deltaTime);
			Debug.Log ("Shotgun Attack: moving towards x=0");
		} else {
			shootingCoolDown = false;
			Shooting = true;
			float shotAngle = 0.7f;
			for (int c = 0; c <= 3; c++) {
				for (int i = 0; i <= 15; i++) {
					Instantiate (bullet, transform.position, new Quaternion (transform.rotation.x, transform.rotation.y - 1f, transform.rotation.z + shotAngle, transform.rotation.w));
					shotAngle = shotAngle - 0.1f;
				}
				yield return new WaitForSeconds (0.5f);
			}
			yield return new WaitForSeconds (0.5f);
			Debug.Log ("Shotgun Attack: moving towards position 1");
			transform.position = Vector3.MoveTowards (transform.position, runningPositions [1].transform.position, (speed / 2f) * Time.deltaTime);
			shootingCoolDown = true;
			Shooting = false;
		}
	}

	void OnTriggerStay2D(Collider2D other){

		if (other.tag == "Special") {
			if (timer > 0.3f) {
				timer = 0f;
				if (BossLife <= 3) {
					//explosion.GetComponent<AudioSource> ().clip = deathexplosion;
					//AudioFX.DeadBoss ();
				} else {
					//AudioFX.GotShot ();
				}
				if (explosion != null) {
					Instantiate (explosion, transform.position, transform.rotation);
				}
				DamageBoss (3);
				StartCoroutine (BlinkingAfterHit ());
			}
			timer = timer + Time.deltaTime;
		}

	}
}

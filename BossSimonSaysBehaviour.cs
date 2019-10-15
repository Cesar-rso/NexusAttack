using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossSimonSaysBehaviour : MonoBehaviour {


	private int BossLife = 420, maxBossLife;
	public int lastHitByPlayer, currentAttack = 0, pathing = 0;
	public Vector3 spawnValues;
	public bool startposition = false, blinking = false, attacking = false, shooting = false;
	public GameObject explosion, bullet, rocket, asteroid, light1, light2, light3, light4, Target1, Target2, Target3;
	public Done_GameController gameController;
	public float speed, timer = 0f;
	public AudioFXController AudioFX;
	int Qattack = 0;

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

		maxBossLife = BossLife;
		light1.SetActive (false);
		light2.SetActive (false);
		light3.SetActive (false);
		light4.SetActive (false);
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

		//Boss specific behaviour
		if (startposition) {
			if (currentAttack == 0) {
				StartCoroutine (Lights ());
				Qattack = 0;
			} else {
				shooting = true;
				StartCoroutine (AttackList (currentAttack));
				if (currentAttack == 3) {
					transform.position = Vector3.MoveTowards (transform.position, new Vector3 (0f, -12f, 0f), (speed / 1.5f) * Time.deltaTime);
					if (transform.position.y <= -12f) {
						transform.position = new Vector3 (2f, 10f, 0f);
						startposition = false;
						currentAttack = 0;
						attacking = false;
					}
				}

				if (currentAttack == 4) {
					if (pathing == 0/*transform.position.y > 2.5f*/) {
						transform.position = Vector3.MoveTowards (transform.position, Target1.transform.position/*new Vector3 (-5f, 2.5f, 0f)*/, (speed / 2f) * Time.deltaTime);
						if (transform.position.y > 2.5f && transform.position.x > -5f) {
							transform.position = Vector3.MoveTowards (transform.position, Target1.transform.position/*new Vector3 (-5f, 2.5f, 0f)*/, (speed / 2f) * Time.deltaTime);
							//Debug.Log ("Moving left");
						}
						if (transform.position.y > 2.5f && transform.position.x <= -5f) {
							pathing = 1;
						}
					} else {
						//shooting = true;
						//StartCoroutine (AttackList (currentAttack));
						//Debug.Log ("Boss x position: " + transform.position.x);
						if (pathing == 1/*transform.position.x < 5f*/) {
							//Debug.Log ("Moving right= " + transform.position.x + " " + Target2.transform.position);
							transform.position = Vector3.MoveTowards (transform.position, /*Target2.transform.position*/new Vector3 (7f, 2.5f, 0f), (speed / 2f) * Time.deltaTime);
							if (transform.position.y > 2.5f && transform.position.x < 5f) {
								transform.position = Vector3.MoveTowards (transform.position, /*Target2.transform.position*/new Vector3 (7f, 2.5f, 0f), (speed / 2f) * Time.deltaTime);
								//Debug.Log ("Still moving right = " + transform.position.x + " " + Target2.transform.position);
							}
							if (transform.position.x >= 6.5f) {
							//	Debug.Log ("Changing to 2");
								pathing = 2;
							}
						}
						if(pathing == 2/*transform.position.x >= 4f*/){
							//Debug.Log ("Moving upward");
							transform.position = Vector3.MoveTowards (transform.position, Target3.transform.position/*new Vector3 (0f, 6f, 0f)*/, (speed / 2f) * Time.deltaTime);
							shooting = false;
							if (transform.position.y >= 5.5f) {
							//	Debug.Log ("Changing to 3");
								pathing = 3;
							}

						}
						if (pathing == 3) {
						//	Debug.Log ("End of attack 4");
							startposition = false;
							attacking = false;
							currentAttack = 0;
							pathing = 0;
						}
					}
				}
			}
		}else {
			transform.position = Vector3.MoveTowards (transform.position, new Vector3 (0f, 3f, 0f), (speed / 2f) * Time.deltaTime);
		}
	}

	public void DamageBoss(int damage){

		BossLife = BossLife - damage;

	}

	void OnTriggerEnter2D (Collider2D other)
	{

		if (other.tag == "Boundary" || other.tag == "Enemy" || other.tag == "Item" || other.tag == "Asteroid") {
			return;
		}


		if (other.tag == "Player") {

			other.GetComponent<Done_PlayerController> ().GotHit (1);
		}

		if (other.tag == "Attack") {
			lastHitByPlayer = other.GetComponent<DestroyBullet> ().bulletOrigin;
			if (BossLife <= 2) {
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

		if(other.tag == "BossPathing"){
			if (other.name == "OutOfBounds1") {
				pathing = 1;
			}

			if (other.name == "OutOfBounds2") {
				pathing = 2;
			}

			/*if(other.name == "TopPathPoint"){
				pathing = 3;
			}*/
			
		}


	}

	IEnumerator BlinkingAfterHit(){

		GetComponent<SpriteRenderer> ().enabled = false;

		yield return new WaitForSeconds (0.1f);

		GetComponent<SpriteRenderer> ().enabled = true;

		yield return new WaitForSeconds (0.1f);

		GetComponent<SpriteRenderer> ().enabled = false;

		yield return new WaitForSeconds (0.1f);

		GetComponent<SpriteRenderer> ().enabled = true;

	}

	IEnumerator Lights(){

		if (!blinking) {
			blinking = true;
			for (int i = 0; i <= 6; i++) {
				int randLight = Random.Range (1, 5);
				switch (randLight) {

				case 1:
					light1.SetActive (true);
					light2.SetActive (false);
					light3.SetActive (false);
					light4.SetActive (false);
					break;

				case 2:
					light1.SetActive (false);
					light2.SetActive (true);
					light3.SetActive (false);
					light4.SetActive (false);
					break;

				case 3:
					light1.SetActive (false);
					light2.SetActive (false);
					light3.SetActive (true);
					light4.SetActive (false);
					break;

				case 4:
					light1.SetActive (false);
					light2.SetActive (false);
					light3.SetActive (false);
					light4.SetActive (true);
					break;
				}

				yield return new WaitForSeconds (0.5f);

				if (i == 6) {
					currentAttack = randLight;
					blinking = false;
				}
			}
		}
	}

	IEnumerator AttackList(int whichAttack){
	
		if (!attacking) {
			attacking = true;
			switch (whichAttack) {

			case 1:
				yield return new WaitForSeconds (0.5f);
				for (int i = 0; i < 4; i++) {
					Instantiate (rocket, transform.position, transform.rotation);
					yield return new WaitForSeconds (0.75f);
				}
				currentAttack = 0;
				attacking = false;
				break;

			case 2:
				yield return new WaitForSeconds (0.5f);
				for (int i = 0; i < 13; i++) {
					Vector3 spawnPosition = new Vector3 (Random.Range (-spawnValues.x, spawnValues.x), spawnValues.y, spawnValues.z);
					Quaternion spawnRotation = Quaternion.identity;
					Instantiate (asteroid, spawnPosition, spawnRotation);
					yield return new WaitForSeconds (0.75f);
				}
				yield return new WaitForSeconds (1.1f);
				currentAttack = 0;
				attacking = false;
				break;

			case 4:
				while(shooting && currentAttack == 4){
					Instantiate (bullet, transform.position, transform.rotation);
					Qattack++;
					if(Qattack >= 30){
						shooting = false;
						pathing = 3;
						//transform.position = Vector3.MoveTowards (transform.position, new Vector3 (0f, 6f, 0f), (speed / 2f) * Time.deltaTime);
						break;
					}
					yield return new WaitForSeconds (0.2f);
				}
				break;
			}
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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossRusherBehaviour : MonoBehaviour {


	private int BossLife = 160;
	public int LastPathPoint = 0, nextDirection = 0, RushChance, lastHitByPlayer;
	public float speed, timer = 0f;
	public GameObject explosion;
	//public AudioClip normalexplosion, deathexplosion;
	public bool moving = true, ReachedPathPoint = true, RushAttackCooldown = true;
	public GameObject[] enemyPath;
	public GameObject Bullet, RushPoint;
	public Done_GameController gameController;
	public AudioFXController AudioFX;
	public Sprite[] BossFace;

	private float StopBug = 0f;

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

		if (gameController.WaveCount <= 30 && gameController.WaveCount > 10) {
			GetComponent<SpriteRenderer> ().sprite = BossFace [0];
		}

		if (gameController.WaveCount <= 50 && gameController.WaveCount > 30) {
			GetComponent<SpriteRenderer> ().sprite = BossFace [1];
		}

		if (gameController.WaveCount <= 70 && gameController.WaveCount > 50) {
			GetComponent<SpriteRenderer> ().sprite = BossFace [2];
		}

		if (gameController.WaveCount <= 90 && gameController.WaveCount > 70) {
			GetComponent<SpriteRenderer> ().sprite = BossFace [3];
		}
		//explosion.GetComponent<AudioSource> ().clip = normalexplosion;
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

		if (moving) {
			if (ReachedPathPoint) {
				ReachedPathPoint = false;
				nextDirection = Random.Range (0, 9);
				if (nextDirection == LastPathPoint) {
					nextDirection = Random.Range (0, 9);
				}
			} else {
				StopBug = StopBug + Time.deltaTime;
				if (StopBug > 1.0f) {
					StopBug = 0f;
					ReachedPathPoint = true;
				}
			
			}

			if (nextDirection >= 10) {
				if(RushAttackCooldown){
					RushAttackCooldown = false;
					moving = false;
					transform.position = Vector3.MoveTowards (transform.position, RushPoint.transform.position, speed * 3 * Time.deltaTime);
				}
			} else if (nextDirection <= 9){
				transform.position = Vector3.MoveTowards (transform.position, enemyPath [nextDirection].transform.position, speed * Time.deltaTime);
			}
				
		} else {
				transform.position = Vector3.MoveTowards (transform.position, RushPoint.transform.position, speed * 3 * Time.deltaTime);
				if (transform.position.y <= -4) {
					moving = true;
					StartCoroutine (CooldownAttack ());
				}
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
			if (moving) {
				other.GetComponent<Done_PlayerController> ().GotHit (2);
			} else {
				other.GetComponent<Done_PlayerController> ().GotHit (6);
			}
		}

		if (other.tag == "Attack") {
			lastHitByPlayer = other.GetComponent<DestroyBullet> ().bulletOrigin;
			if (BossLife <= 2) {
				//explosion.GetComponent<AudioSource> ().clip = deathexplosion;
				AudioFX.DeadBoss ();
			} else {
				AudioFX.DestroyedEnemySound();
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

		if(other.name == "BossRusherPoint"){
			moving = true;
			LastPathPoint = nextDirection;
			ReachedPathPoint = true;
		}

		if (other.tag == "Pathing") {
			if (other.name == enemyPath [nextDirection].transform.name && nextDirection <= 9) {
				LastPathPoint = nextDirection;
				ReachedPathPoint = true;
			}
		}
	}

	IEnumerator CooldownAttack(){
		yield return new WaitForSeconds (5.0f);
		RushAttackCooldown = true;
	}

	IEnumerator BlinkingAfterHit(){

		for (i = 0; i < 2; i++){
			GetComponent<SpriteRenderer> ().enabled = false;

			yield return new WaitForSeconds (0.1f);

			GetComponent<SpriteRenderer> ().enabled = true;

			yield return new WaitForSeconds (0.1f);
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

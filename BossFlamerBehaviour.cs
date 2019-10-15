using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossFlamerBehaviour : MonoBehaviour {

	private int BossLife = 190, chasedplayer, maxBossLife;
	public int lastHitByPlayer;
	public bool startposition = false, secondphase = false;
	public GameObject explosion;
	//public AudioClip normalexplosion, deathexplosion;
	public Done_GameController gameController;
	public float speed, timer = 0f, timer2 = 0f;
	public GameObject[] players;
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
		chasedplayer = Random.Range (0, players.Length - 1);
		maxBossLife = BossLife;
	}
	
	// Update is called once per frame
	void Update () {
		timer2 = timer2 + Time.deltaTime;

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

		if(transform.position.y <= 3f && !startposition){
			startposition = true;
		}

		if (BossLife > (maxBossLife / 2)) {
			if (startposition) {
				if (timer2 > 7f && Random.Range (1, 10) > 5) {
					transform.RotateAround (new Vector3 (0f, 0f, 0f), Vector3.forward, speed * Time.deltaTime);
					transform.Rotate (Vector3.back * speed * Time.deltaTime);
					timer2 = 0f;
				} else {
					transform.RotateAround (new Vector3 (0f, 0f, 0f), Vector3.forward, -speed * Time.deltaTime);
					transform.Rotate (Vector3.back * -speed * Time.deltaTime);
				}

			} else {
				transform.position = Vector3.MoveTowards (transform.position, new Vector3 (0f, 3f, 0f), (speed / 2f) * Time.deltaTime);
			}
				
		} else {
			if (!secondphase) {
				//Debug.Log ("Chamando second Phase!!!");
				StartCoroutine(SecondPhaseAlert ());
			} else {
				try {
					if (players [chasedplayer] != null) {
						transform.position = Vector3.MoveTowards (transform.position, players [chasedplayer].transform.position, (speed / 10f) * Time.deltaTime);
					} else {
						players = GameObject.FindGameObjectsWithTag ("Player");
						chasedplayer = Random.Range (0, players.Length - 1);
						secondphase = false;
						StartCoroutine(SecondPhaseAlert ());
						transform.position = Vector3.MoveTowards (transform.position, players [chasedplayer].transform.position, (speed / 10f) * Time.deltaTime);
					}
				} catch {
					players = GameObject.FindGameObjectsWithTag ("Player");
					chasedplayer = Random.Range (0, players.Length - 1);
					secondphase = false;
					StartCoroutine(SecondPhaseAlert ());
					transform.position = Vector3.MoveTowards (transform.position, players [chasedplayer].transform.position, (speed / 8f) * Time.deltaTime);
				}
			}
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

		GetComponent<SpriteRenderer> ().enabled = false;

		yield return new WaitForSeconds (0.1f);

		GetComponent<SpriteRenderer> ().enabled = true;

		yield return new WaitForSeconds (0.1f);

		GetComponent<SpriteRenderer> ().enabled = false;

		yield return new WaitForSeconds (0.1f);

		GetComponent<SpriteRenderer> ().enabled = true;

	}

	IEnumerator SecondPhaseAlert (){
		
		GetComponent<SpriteRenderer> ().color = new Vector4(0f,255f,255f,255f);

		yield return new WaitForSeconds (0.2f);

		GetComponent<SpriteRenderer> ().color = new Vector4(255f,255f,255f,255f);

		yield return new WaitForSeconds (0.2f);

		GetComponent<SpriteRenderer> ().color = new Vector4(0f,255f,255f,255f);

		yield return new WaitForSeconds (0.2f);

		GetComponent<SpriteRenderer> ().color = new Vector4(255f,255f,255f,255f);

		yield return new WaitForSeconds (0.2f);

		GetComponent<SpriteRenderer> ().color = new Vector4(0f,255f,255f,255f);

		yield return new WaitForSeconds (0.2f);

		GetComponent<SpriteRenderer> ().color = new Vector4(255f,255f,255f,255f);

		yield return new WaitForSeconds (0.2f);

		GetComponent<SpriteRenderer> ().color = new Vector4(0f,255f,255f,255f);

		yield return new WaitForSeconds (0.2f);

		GetComponent<SpriteRenderer> ().color = new Vector4(255f,255f,255f,255f);

		yield return new WaitForSeconds (0.2f);

		GetComponent<SpriteRenderer> ().color = new Vector4(0f,255f,255f,255f);

		yield return new WaitForSeconds (0.2f);

		GetComponent<SpriteRenderer> ().color = new Vector4(255f,255f,255f,255f);

		secondphase = true;
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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossTornadoBehaviour : MonoBehaviour {

	private int BossLife = 190, maxBossLife;
	public int lastHitByPlayer;
	public bool startposition = false, attacking = false, slamming = false;
	public GameObject explosion, bullet, straightbullet;
	public Transform shotSpawn, shotSpawn2, shotSpawn3, shotSpawn4;
	//public AudioClip normalexplosion, deathexplosion;
	public Done_GameController gameController;
	public float speed, timer = 0f;
	public AudioFXController AudioFX;
	public Sprite[] BossFace;

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

		if((transform.position.y <= 4f && transform.position.y >= 3.5f) && !startposition){
			startposition = true;
		}

		if (startposition) {
			if (slamming) {
				StartCoroutine (SlamAttack ());
			} else {
				int rand = Random.Range (1, 20);
				Debug.Log (rand + "" + transform.position.y);
				if (rand <= 10) {
					StartCoroutine (TornadoAttack ());
				} else {
					//StartCoroutine (SlamWarning());
					StartCoroutine (SlamAttack ());
				}
			}

		} else {
			transform.position = Vector3.MoveTowards (transform.position, new Vector3 (0f, 4f, 0f), (speed / 2f) * Time.deltaTime);
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

	IEnumerator TornadoAttack(){

		if (!attacking) {
			attacking = true;
			yield return new WaitForSeconds (3f);
			int dir = Random.Range (0, 10);
			if (dir <= 5) {
				dir = -1;
			} else {
				dir = 1;
			}
			for (int i = 0; i < 5; i++) {
				GameObject bullet1 = Instantiate (bullet, shotSpawn.position, shotSpawn.rotation);
				bullet1.GetComponent<SpinShot> ().startPoint = transform.gameObject;
				bullet1.GetComponent<SpinShot> ().direction = dir;
				GameObject bullet2 = Instantiate (bullet, shotSpawn2.position, shotSpawn2.rotation);
				bullet2.GetComponent<SpinShot> ().startPoint = transform.gameObject;
				bullet2.GetComponent<SpinShot> ().direction = dir;
				GameObject bullet3 = Instantiate (bullet, shotSpawn3.position, shotSpawn3.rotation);
				bullet3.GetComponent<SpinShot> ().startPoint = transform.gameObject;
				bullet3.GetComponent<SpinShot> ().direction = dir;
				GameObject bullet4 = Instantiate (bullet, shotSpawn4.position, shotSpawn4.rotation);
				bullet4.GetComponent<SpinShot> ().startPoint = transform.gameObject;
				bullet4.GetComponent<SpinShot> ().direction = dir;
				//GameObject bullet5 = Instantiate (straightbullet, shotSpawn2.position, shotSpawn2.rotation);
				yield return new WaitForSeconds (0.3f);
			}

			attacking = false;
		}
	}

	IEnumerator SlamAttack(){
		if (!attacking) {
			attacking = true;
			slamming = true;
			//StartCoroutine (SlamWarning());
			transform.position = Vector3.MoveTowards (transform.position, new Vector3 (0f, -3.5f, 0f), speed * Time.deltaTime);

			attacking = false;
		}

		if(transform.position.y <= -3.5f){
			startposition = false;
			slamming = false;
			//transform.position = Vector3.MoveTowards (transform.position, new Vector3 (0f, 4f, 0f), (speed / 2f) * Time.deltaTime);
			yield return new WaitForSeconds (0.2f);
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

	IEnumerator SlamWarning (){

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

	}
}

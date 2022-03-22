using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossTheShieldBehaviour : MonoBehaviour {

	private int BossLife = 190, maxBossLife;
	public int lastHitByPlayer;
	public bool startposition = false, ShieldOn = true, attacking = false;
	public GameObject explosion, bullet, shield;
	public Transform shotSpawn;
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

		if(transform.position.y <= 4f && !startposition){
			startposition = true;
		}

		if (startposition) {
			if (ShieldOn) {
				shield.SetActive (true);
				StartCoroutine (ShieldOnWait ());
			} else {
				shield.SetActive (false);
				if (!attacking) {
					StartCoroutine (MachineGunAttack ());
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

		for (i = 0; i < 2; i++){
			GetComponent<SpriteRenderer> ().enabled = false;

			yield return new WaitForSeconds (0.1f);

			GetComponent<SpriteRenderer> ().enabled = true;

			yield return new WaitForSeconds (0.1f);
		}

	}

	IEnumerator ShieldOnWait(){

		yield return new WaitForSeconds (4f);
		ShieldOn = false;
	
	}

	IEnumerator MachineGunAttack(){

		attacking = true;
		float shotAngle = 0.7f;
		for (int i = 0; i <= 15; i++) {
			Instantiate (bullet, shotSpawn.position, new Quaternion(shotSpawn.rotation.x, shotSpawn.rotation.y, shotSpawn.rotation.z + shotAngle, shotSpawn.rotation.w));
			Instantiate (bullet, new Vector3(shotSpawn.position.x + 1.5f, shotSpawn.position.y, shotSpawn.position.z), new Quaternion(shotSpawn.rotation.x, shotSpawn.rotation.y, shotSpawn.rotation.z + shotAngle, shotSpawn.rotation.w));
			Instantiate (bullet, new Vector3(shotSpawn.position.x - 1.5f, shotSpawn.position.y, shotSpawn.position.z), new Quaternion(shotSpawn.rotation.x, shotSpawn.rotation.y, shotSpawn.rotation.z + shotAngle, shotSpawn.rotation.w));

			yield return new WaitForSeconds (0.2f);
			shotAngle = shotAngle - 0.1f;
		}
		for (int i = 0; i <= 15; i++) {
			Instantiate (bullet, shotSpawn.position, new Quaternion(shotSpawn.rotation.x, shotSpawn.rotation.y, shotSpawn.rotation.z + shotAngle, shotSpawn.rotation.w));
			Instantiate (bullet, new Vector3(shotSpawn.position.x + 1.5f, shotSpawn.position.y, shotSpawn.position.z), new Quaternion(shotSpawn.rotation.x, shotSpawn.rotation.y, shotSpawn.rotation.z + shotAngle, shotSpawn.rotation.w));
			Instantiate (bullet, new Vector3(shotSpawn.position.x - 1.5f, shotSpawn.position.y, shotSpawn.position.z), new Quaternion(shotSpawn.rotation.x, shotSpawn.rotation.y, shotSpawn.rotation.z + shotAngle, shotSpawn.rotation.w));

			yield return new WaitForSeconds (0.2f);
			shotAngle = shotAngle + 0.1f;
		} 

		yield return new WaitForSeconds (0.5f);
		ShieldOn = true;
		attacking = false;
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

using UnityEngine;
using System.Collections;
using UnityStandardAssets.CrossPlatformInput;

[System.Serializable]
public class Done_Boundary 
{
	public float xMin, xMax, zMin, zMax;
}

public class Done_PlayerController : MonoBehaviour
{
	public float speed;
	public Done_Boundary boundary;

	public GameObject shot, DoubleShot, SpecialShot, ShieldObject, slowShot, BigSpecial, Rocket, SpinShot;
	public Transform shotSpawn, shotSpawn2, shotSpawn3, shotSpawn4, shotSpawn5;
	public float fireRate, initialFireRate;

	public int playerLife = 10;
	public int maxLife = 10;
	public int currentPlayer = 1;
	public int SpFire = 4;
	public bool SpCoolDown = true;
	public int ShotType = 1;
	public int ShotUpgrade = 0;
	public int ShieldCharge = 0;

	public AudioClip OneShot, DoubleShotAudio, TripleShotAudio;
	public AudioFXController AudioFX;
	private Done_GameController gameController;
	private GameObject[] Shotgun;

	float moveHorizontal;
	float moveVertical;

	float moveHorizontalP2;
	float moveVerticalP2;
	 
	float moveHorizontalGamepad;
	float moveVerticalGamepad;

	private float nextFire;

	void Start(){
		ShieldObject.SetActive (false);
		SpecialShot.SetActive (false);

		GameObject AudioControllerObject = GameObject.FindGameObjectWithTag ("AudioFX");
		if (AudioControllerObject != null) {

			AudioFX = AudioControllerObject.GetComponent<AudioFXController> ();
		}
		if (AudioFX == null) {

			Debug.Log ("Cannot find 'AudioFXController' script");
		}

		GameObject gameControllerObject = GameObject.FindGameObjectWithTag ("GameController");

		if (gameControllerObject != null)
		{
			gameController = gameControllerObject.GetComponent <Done_GameController>();
			if (currentPlayer == 1) {
				gameControllerObject.GetComponent<PlayerUI> ().Player = transform.gameObject;
			}
			if (currentPlayer == 2) {
				gameControllerObject.GetComponent<PlayerUI> ().Player2 = transform.gameObject;
			}
		}
		if (gameController == null)
		{
			Debug.Log ("Cannot find 'GameController' script");
		}

		if(currentPlayer == 1){
			int currentShip = PlayerPrefs.GetInt ("P1Ship");
			GetComponent<SpriteRenderer> ().sprite = gameController.Ships[currentShip];
			SpCoolDown = true;
			SpFire = 4;

			switch (currentShip) {

			case 0:
				playerLife = 10;
				maxLife = 10;
				speed = 15;
				fireRate = 0.5f;
				gameController.P1PowerUpShot = 2;
				ShotUpgrade = 4;
				break;

			case 1:
				playerLife = 15;
				maxLife = 15;
				speed = 20;
				fireRate = 0.75f;
				gameController.P1PowerUpShot = 3;
				ShotUpgrade = 3;
				break;

			case 2:
				playerLife = 20;
				maxLife = 20;
				speed = 10;
				fireRate = 0.5f;
				gameController.P1PowerUpShot = 3;
				ShotUpgrade = 2;
				break;

			case 3:
				playerLife = 15;
				maxLife = 15;
				speed = 15;
				fireRate = 0.25f;
				gameController.P1PowerUpShot = 1;
				break;

			}
		}
		#if UNITY_STANDALONE || UNITY_WEBPLAYER
		if(currentPlayer == 2){
			int currentShip2 = PlayerPrefs.GetInt ("P2Ship");
			GetComponent<SpriteRenderer> ().sprite = gameController.Ships[currentShip2];

			switch (currentShip2) {

			case 0:
				playerLife = 10;
				maxLife = 10;
				speed = 15;
				fireRate = 0.5f;
				gameController.P2PowerUpShot = 3;
				break;

			case 1:
				playerLife = 15;
				maxLife = 15;
				speed = 20;
				fireRate = 0.75f;
				gameController.P2PowerUpShot = 2;
				ShotUpgrade = 3;
				break;

			case 2:
				playerLife = 20;
				maxLife = 20;
				speed = 10;
				fireRate = 0.5f;
				gameController.P2PowerUpShot = 2;
				ShotUpgrade = 2;
				break;

			case 3:
				playerLife = 15;
				maxLife = 15;
				speed = 15;
				fireRate = 0.25f;
				gameController.P2PowerUpShot = 1;
				break;

			}
		}
		#endif

		initialFireRate = fireRate;
	}

	void Update ()
	{
		if (AudioFX == null) {
			GameObject AudioControllerObject = GameObject.FindGameObjectWithTag ("AudioFX");
			if (AudioControllerObject != null) {

				AudioFX = AudioControllerObject.GetComponent<AudioFXController> ();
			}

		}

		if (playerLife > maxLife) {
			playerLife = maxLife;
		}

		if (ShieldCharge > 0) {
			ShieldObject.SetActive (true);
		} else {
			ShieldObject.SetActive (false);
		}

		if(ShotType==2){
			GetComponent<AudioSource> ().clip = DoubleShotAudio;
		}

		if(ShotType==3){
			GetComponent<AudioSource> ().clip = TripleShotAudio;
		}

		//player1 controls on keyboard
		if (currentPlayer == 1) {
			if (Input.GetKey (KeyCode.LeftControl) && Time.time > nextFire && SpCoolDown) {
				nextFire = Time.time + fireRate;
				if (ShotUpgrade == 0) {
					fireRate = initialFireRate;
					if (ShotType == 1) {
						GameObject bullet1 = Instantiate (shot, shotSpawn.position, shotSpawn.rotation);

						bullet1.GetComponent<DestroyBullet> ().bulletOrigin = currentPlayer;
						try {
							if (!AudioFX.currentVolume) {
								GetComponent<AudioSource> ().PlayOneShot (OneShot);
							}
						} catch {
							if (AudioFX == null) {
								Debug.Log ("Audio nulo! Não carregou Obj");
							}
						}
					}
					if (ShotType == 2) {
						GameObject bullet1 = Instantiate (shot, shotSpawn2.position, shotSpawn2.rotation);
						GameObject bullet2 = Instantiate (shot, shotSpawn3.position, shotSpawn3.rotation);

						bullet1.GetComponent<DestroyBullet> ().bulletOrigin = currentPlayer;
						bullet2.GetComponent<DestroyBullet> ().bulletOrigin = currentPlayer;


						if (!AudioFX.currentVolume) {
							GetComponent<AudioSource> ().PlayOneShot (DoubleShotAudio);
						}
					}
					if (ShotType == 3) {
						GameObject bullet1 = Instantiate (shot, shotSpawn.position, shotSpawn.rotation);
						GameObject bullet2 = Instantiate (shot, shotSpawn2.position, shotSpawn2.rotation);
						GameObject bullet3 = Instantiate (shot, shotSpawn3.position, shotSpawn3.rotation);

						bullet1.GetComponent<DestroyBullet> ().bulletOrigin = currentPlayer;
						bullet2.GetComponent<DestroyBullet> ().bulletOrigin = currentPlayer;
						bullet3.GetComponent<DestroyBullet> ().bulletOrigin = currentPlayer;

						if (!AudioFX.currentVolume) {
							GetComponent<AudioSource> ().PlayOneShot (TripleShotAudio);
						}
					}

					if (ShotType == 4) {
						GameObject bullet2 = Instantiate (DoubleShot, shotSpawn2.position, new Quaternion (shotSpawn2.rotation.x, 180f, shotSpawn2.rotation.z, shotSpawn2.rotation.w));
						GameObject bullet3 = Instantiate (DoubleShot, shotSpawn3.position, shotSpawn3.rotation);

						bullet2.GetComponent<DestroyBullet> ().bulletOrigin = currentPlayer;
						bullet3.GetComponent<DestroyBullet> ().bulletOrigin = currentPlayer;


						if (!AudioFX.currentVolume) {
							GetComponent<AudioSource> ().PlayOneShot (TripleShotAudio);
						}
					}
				}

				if (ShotUpgrade == 1) {
					SpecialShot.SetActive (true);
					SpecialShot.transform.localScale = new Vector3 (1.0f, SpecialShot.transform.localScale.y, SpecialShot.transform.localScale.z);
					if (!AudioFX.currentVolume) {
						GetComponent<AudioSource> ().PlayOneShot (OneShot);
					}
				}

				if (ShotUpgrade == 2) {
					float shotAngle = 0.6f;
					fireRate = 1.0f;
					for (int i = 0; i <= 13; i++) {
						Shotgun[i] = Instantiate (shot, shotSpawn.position, new Quaternion(shotSpawn.rotation.x, shotSpawn.rotation.y, shotSpawn.rotation.z + shotAngle, shotSpawn.rotation.w));
						Shotgun [i].GetComponent<DestroyBullet> ().bulletOrigin = currentPlayer;
						shotAngle = shotAngle - 0.1f;
						Debug.Log ("Origin: PC, i = " + i + ", shotAngle = " + shotAngle);
					}
					if (!AudioFX.currentVolume) {
						GetComponent<AudioSource> ().PlayOneShot (OneShot);
					}
				}

				if (ShotUpgrade == 3) {
					fireRate = 1.0f;
					GameObject RocketShot = Instantiate (Rocket, shotSpawn.position, shotSpawn.rotation);
					RocketShot.GetComponent<DestroyBullet> ().bulletOrigin = currentPlayer;
					if (!AudioFX.currentVolume) {
						GetComponent<AudioSource> ().PlayOneShot (OneShot);
					}
				}

				if(ShotUpgrade == 4){
					fireRate = 1.0f;
					GameObject Spinner = Instantiate (SpinShot, shotSpawn.position, shotSpawn.rotation);
					Spinner.GetComponent<DestroyBullet> ().bulletOrigin = currentPlayer;
					if (!AudioFX.currentVolume) {
						GetComponent<AudioSource> ().PlayOneShot (OneShot);
					}
					StartCoroutine (CustomWaitTime(0.15f));
					GameObject Spinner2 = Instantiate (SpinShot, shotSpawn.position, shotSpawn.rotation);
					Spinner2.GetComponent<DestroyBullet> ().bulletOrigin = currentPlayer;
					if (!AudioFX.currentVolume) {
						GetComponent<AudioSource> ().PlayOneShot (OneShot);
					}
					StartCoroutine (CustomWaitTime(0.15f));
					GameObject Spinner3 = Instantiate (SpinShot, shotSpawn.position, shotSpawn.rotation);
					Spinner3.GetComponent<DestroyBullet> ().bulletOrigin = currentPlayer;
					if (!AudioFX.currentVolume) {
						GetComponent<AudioSource> ().PlayOneShot (OneShot);
					}
					StartCoroutine (CustomWaitTime(0.15f));

					
				}

			}

			if (Input.GetKeyDown (KeyCode.LeftShift) && SpFire > 0 && Time.deltaTime > 0f) {
				if (SpCoolDown) {
					SpCoolDown = false;

					StartCoroutine(BigSpecialAttack());

					SpFire = SpFire - 1;
				}
			}

			/*if (Input.GetKeyDown (KeyCode.Q)) {
				//transform.rotation.Set (transform.rotation.x, transform.rotation.y, transform.rotation.z + 90f, transform.rotation.w);
				transform.Rotate (new Vector3 (0f, 0f, 90f), Space.World);
			}*/

			/*if (Input.GetKeyDown (KeyCode.E)) {
				//transform.rotation.Set (transform.rotation.x, transform.rotation.y, transform.rotation.z + 90f, transform.rotation.w);
				transform.Rotate (new Vector3 (0f, 0f, -90f), Space.World);
			}*/
		}

		if (currentPlayer == 2) {
			//player2 controls on keyboard
			if (Input.GetKey (KeyCode.RightControl) && Time.time > nextFire && SpCoolDown) {
				nextFire = Time.time + fireRate;
				if (ShotUpgrade == 0) {
					fireRate = initialFireRate;
					if (ShotType == 1) {
						GameObject bullet1 = Instantiate (shot, shotSpawn.position, shotSpawn.rotation);

						bullet1.GetComponent<DestroyBullet> ().bulletOrigin = currentPlayer;

						if (!AudioFX.currentVolume) {
							GetComponent<AudioSource> ().PlayOneShot (OneShot);
						}
					}
					if (ShotType == 2) {
						GameObject bullet1 = Instantiate (shot, shotSpawn2.position, shotSpawn2.rotation);
						GameObject bullet2 = Instantiate (shot, shotSpawn3.position, shotSpawn3.rotation);

						bullet1.GetComponent<DestroyBullet> ().bulletOrigin = currentPlayer;
						bullet2.GetComponent<DestroyBullet> ().bulletOrigin = currentPlayer;


						if (!AudioFX.currentVolume) {
							GetComponent<AudioSource> ().PlayOneShot (DoubleShotAudio);
						}
					}
					if (ShotType == 3) {
						GameObject bullet1 = Instantiate (shot, shotSpawn.position, shotSpawn.rotation);
						GameObject bullet2 = Instantiate (shot, shotSpawn2.position, shotSpawn2.rotation);
						GameObject bullet3 = Instantiate (shot, shotSpawn3.position, shotSpawn3.rotation);

						bullet1.GetComponent<DestroyBullet> ().bulletOrigin = currentPlayer;
						bullet2.GetComponent<DestroyBullet> ().bulletOrigin = currentPlayer;
						bullet3.GetComponent<DestroyBullet> ().bulletOrigin = currentPlayer;
					
						if (!AudioFX.currentVolume) {
							GetComponent<AudioSource> ().PlayOneShot (TripleShotAudio);
						}
					}
					if (ShotType == 4) {
						
						GameObject bullet2 = Instantiate (DoubleShot, shotSpawn2.position, new Quaternion (shotSpawn2.rotation.x, 180f, shotSpawn2.rotation.z, shotSpawn2.rotation.w));
						GameObject bullet3 = Instantiate (DoubleShot, shotSpawn3.position, shotSpawn3.rotation);
						
						bullet2.GetComponent<DestroyBullet> ().bulletOrigin = currentPlayer;
						bullet3.GetComponent<DestroyBullet> ().bulletOrigin = currentPlayer;
						

						if (!AudioFX.currentVolume) {
							GetComponent<AudioSource> ().PlayOneShot (TripleShotAudio);
						}
					}
				}
				if (ShotUpgrade == 1) {
					SpecialShot.SetActive (true);
					SpecialShot.transform.localScale = new Vector3 (1.0f, SpecialShot.transform.localScale.y, SpecialShot.transform.localScale.z);
					if (!AudioFX.currentVolume) {
						GetComponent<AudioSource> ().PlayOneShot (OneShot);
					}
				}

				if (ShotUpgrade == 2) {
					float shotAngle = 0.6f;
					fireRate = 1.0f;
					for (int i = 0; i <= 13; i++) {
						Shotgun[i] = Instantiate (shot, shotSpawn.position, new Quaternion(shotSpawn.rotation.x, shotSpawn.rotation.y, shotSpawn.rotation.z + shotAngle, shotSpawn.rotation.w));
						Shotgun [i].GetComponent<DestroyBullet> ().bulletOrigin = currentPlayer;
						shotAngle = shotAngle - 0.1f;
					Debug.Log ("Origin: PC2, i = " + i + ", shotAngle = " + shotAngle);
					}
					if (!AudioFX.currentVolume) {
						GetComponent<AudioSource> ().PlayOneShot (OneShot);
					}
				}

				if (ShotUpgrade == 3) {
					fireRate = 1.0f;
					GameObject RocketShot = Instantiate (Rocket, shotSpawn.position, shotSpawn.rotation);
					RocketShot.GetComponent<DestroyBullet> ().bulletOrigin = currentPlayer;
					if (!AudioFX.currentVolume) {
						GetComponent<AudioSource> ().PlayOneShot (OneShot);
					}
				}

				if(ShotUpgrade == 4){
					fireRate = 1.0f;
					GameObject Spinner = Instantiate (SpinShot, shotSpawn.position, shotSpawn.rotation);
					Spinner.GetComponent<DestroyBullet> ().bulletOrigin = currentPlayer;
					if (!AudioFX.currentVolume) {
						GetComponent<AudioSource> ().PlayOneShot (OneShot);
					}
					StartCoroutine (CustomWaitTime(0.15f));
					GameObject Spinner2 = Instantiate (SpinShot, shotSpawn.position, shotSpawn.rotation);
					Spinner2.GetComponent<DestroyBullet> ().bulletOrigin = currentPlayer;
					if (!AudioFX.currentVolume) {
						GetComponent<AudioSource> ().PlayOneShot (OneShot);
					}
					StartCoroutine (CustomWaitTime(0.15f));
					GameObject Spinner3 = Instantiate (SpinShot, shotSpawn.position, shotSpawn.rotation);
					Spinner3.GetComponent<DestroyBullet> ().bulletOrigin = currentPlayer;
					if (!AudioFX.currentVolume) {
						GetComponent<AudioSource> ().PlayOneShot (OneShot);
					}
					StartCoroutine (CustomWaitTime(0.15f));


				}
				
			}

			if (Input.GetKeyDown (KeyCode.RightShift) && SpFire > 0 && Time.deltaTime > 0f) {
				if (SpCoolDown) {
				
					SpCoolDown = false;
					StartCoroutine(BigSpecialAttack());

					SpFire = SpFire - 1;
				}
			}

			/*if (Input.GetKeyDown (KeyCode.Delete)) {
				//transform.rotation.Set (transform.rotation.x, transform.rotation.y, transform.rotation.z + 90f, transform.rotation.w);
				transform.Rotate (new Vector3 (0f, 0f, 90f), Space.World);
			}*/

			/*if (Input.GetKeyDown (KeyCode.PageDown)) {
				//transform.rotation.Set (transform.rotation.x, transform.rotation.y, transform.rotation.z + 90f, transform.rotation.w);
				transform.Rotate (new Vector3 (0f, 0f, -90f), Space.World);
			}*/
		}

		//Xbox360 controller support for both player 1 and 2
		if ( Input.GetButton(currentPlayer + "A") && Time.time > nextFire && SpCoolDown) 
		{
			nextFire = Time.time + fireRate;
			if (ShotUpgrade == 0) {
				fireRate = initialFireRate;
				if (ShotType == 1) {
					GameObject bullet1 = Instantiate (shot, shotSpawn.position, shotSpawn.rotation);

					bullet1.GetComponent<DestroyBullet> ().bulletOrigin = currentPlayer;

					if (!AudioFX.currentVolume) {
						GetComponent<AudioSource> ().PlayOneShot (OneShot);
					}
				}
				if (ShotType == 2) {
					GameObject bullet1 = Instantiate (shot, shotSpawn2.position, shotSpawn2.rotation);
					GameObject bullet2 = Instantiate (shot, shotSpawn3.position, shotSpawn3.rotation);

					bullet1.GetComponent<DestroyBullet> ().bulletOrigin = currentPlayer;
					bullet2.GetComponent<DestroyBullet> ().bulletOrigin = currentPlayer;

					if (!AudioFX.currentVolume) {
						GetComponent<AudioSource> ().PlayOneShot (DoubleShotAudio);
					}
				}
				if (ShotType == 3) {
					GameObject bullet1 = Instantiate (shot, shotSpawn.position, shotSpawn.rotation);
					GameObject bullet2 = Instantiate (shot, shotSpawn2.position, shotSpawn2.rotation);
					GameObject bullet3 = Instantiate (shot, shotSpawn3.position, shotSpawn3.rotation);

					bullet1.GetComponent<DestroyBullet> ().bulletOrigin = currentPlayer;
					bullet2.GetComponent<DestroyBullet> ().bulletOrigin = currentPlayer;
					bullet3.GetComponent<DestroyBullet> ().bulletOrigin = currentPlayer;


					if (!AudioFX.currentVolume) {
						GetComponent<AudioSource> ().PlayOneShot (TripleShotAudio);
					}
				}
				if (ShotType == 4) {
					//GameObject bullet1 = Instantiate (shot, shotSpawn4.position, shotSpawn.rotation);
					GameObject bullet2 = Instantiate (DoubleShot, shotSpawn2.position, new Quaternion (shotSpawn2.rotation.x, 180f, shotSpawn2.rotation.z, shotSpawn2.rotation.w));
					GameObject bullet3 = Instantiate (DoubleShot, shotSpawn3.position, shotSpawn3.rotation);
					//GameObject bullet4 = Instantiate (shot, shotSpawn5.position, shotSpawn3.rotation);

					//bullet1.GetComponent<DestroyBullet> ().bulletOrigin = currentPlayer;
					bullet2.GetComponent<DestroyBullet> ().bulletOrigin = currentPlayer;
					bullet3.GetComponent<DestroyBullet> ().bulletOrigin = currentPlayer;
					//bullet4.GetComponent<DestroyBullet> ().bulletOrigin = currentPlayer;

					if (!AudioFX.currentVolume) {
						GetComponent<AudioSource> ().PlayOneShot (TripleShotAudio);
					}
				}
			}
			if (ShotUpgrade == 1) {
				SpecialShot.SetActive (true);
				SpecialShot.transform.localScale = new Vector3 (1.0f, SpecialShot.transform.localScale.y, SpecialShot.transform.localScale.z);
				if (!AudioFX.currentVolume) {
					GetComponent<AudioSource> ().PlayOneShot (OneShot);
				}
			}
			
			if (ShotUpgrade == 2) {
				float shotAngle = 0.6f;
				fireRate = 1.0f;
				for (int i = 0; i <= 13; i++) {
					Shotgun[i] = Instantiate (shot, shotSpawn.position, new Quaternion(shotSpawn.rotation.x, shotSpawn.rotation.y, shotSpawn.rotation.z + shotAngle, shotSpawn.rotation.w));
					Shotgun [i].GetComponent<DestroyBullet> ().bulletOrigin = currentPlayer;
					shotAngle = shotAngle - 0.1f;
					Debug.Log ("Origin: XBox, i = " + i + ", shotAngle = " + shotAngle);
				}
				if (!AudioFX.currentVolume) {
					GetComponent<AudioSource> ().PlayOneShot (OneShot);
				}
			}

			if (ShotUpgrade == 3) {
				fireRate = 1.0f;
				GameObject RocketShot = Instantiate (Rocket, shotSpawn.position, shotSpawn.rotation);
				RocketShot.GetComponent<DestroyBullet> ().bulletOrigin = currentPlayer;
				if (!AudioFX.currentVolume) {
					GetComponent<AudioSource> ().PlayOneShot (OneShot);
				}
			}

			if(ShotUpgrade == 4){
				fireRate = 0.5f;
				GameObject Spinner = Instantiate (SpinShot, shotSpawn.position, shotSpawn.rotation);
				Spinner.GetComponent<DestroyBullet> ().bulletOrigin = currentPlayer;
				if (!AudioFX.currentVolume) {
					GetComponent<AudioSource> ().PlayOneShot (OneShot);
				}
				StartCoroutine (CustomWaitTime(0.15f));
				GameObject Spinner2 = Instantiate (SpinShot, shotSpawn.position, shotSpawn.rotation);
				Spinner2.GetComponent<DestroyBullet> ().bulletOrigin = currentPlayer;
				if (!AudioFX.currentVolume) {
					GetComponent<AudioSource> ().PlayOneShot (OneShot);
				}
				StartCoroutine (CustomWaitTime(0.15f));
				GameObject Spinner3 = Instantiate (SpinShot, shotSpawn.position, shotSpawn.rotation);
				Spinner3.GetComponent<DestroyBullet> ().bulletOrigin = currentPlayer;
				if (!AudioFX.currentVolume) {
					GetComponent<AudioSource> ().PlayOneShot (OneShot);
				}
				StartCoroutine (CustomWaitTime(0.15f));


			}
		
		}

		if (Input.GetButtonDown(currentPlayer + "X") && SpFire > 0 && Time.deltaTime > 0f) {
			if (SpCoolDown) {
				
				SpCoolDown = false;
				StartCoroutine(BigSpecialAttack());
				SpFire = SpFire - 1;
			}
		}

		/*if (Input.GetButtonDown(currentPlayer + "LB")) {
			//transform.rotation.Set (transform.rotation.x, transform.rotation.y, transform.rotation.z + 90f, transform.rotation.w);
			transform.Rotate (new Vector3(0f, 0f, 90f), Space.World);
		}*/

		/*if (Input.GetButtonDown(currentPlayer + "RB")) {
			//transform.rotation.Set (transform.rotation.x, transform.rotation.y, transform.rotation.z + 90f, transform.rotation.w);
			transform.Rotate (new Vector3(0f, 0f, -90f), Space.World);
		}*/


	}

	public void AndroidAttack(bool attacking){
		if (attacking) {
			if (Time.time > nextFire && SpCoolDown) {
				nextFire = Time.time + fireRate;
				if (ShotUpgrade == 0 || ShotUpgrade == 1) {
					fireRate = initialFireRate;
					if (ShotType == 1) {
						GameObject bullet1 = Instantiate (shot, shotSpawn.position, shotSpawn.rotation);

						bullet1.GetComponent<DestroyBullet> ().bulletOrigin = currentPlayer;

						if (!AudioFX.currentVolume) {
							GetComponent<AudioSource> ().PlayOneShot (OneShot);
						}
					}
					if (ShotType == 2) {
						GameObject bullet1 = Instantiate (shot, shotSpawn2.position, shotSpawn2.rotation);
						GameObject bullet2 = Instantiate (shot, shotSpawn3.position, shotSpawn3.rotation);

						bullet1.GetComponent<DestroyBullet> ().bulletOrigin = currentPlayer;
						bullet2.GetComponent<DestroyBullet> ().bulletOrigin = currentPlayer;


						if (!AudioFX.currentVolume) {
							GetComponent<AudioSource> ().PlayOneShot (DoubleShotAudio);
						}
					}
					if (ShotType == 3) {
						GameObject bullet1 = Instantiate (shot, shotSpawn.position, shotSpawn.rotation);
						GameObject bullet2 = Instantiate (shot, shotSpawn2.position, shotSpawn2.rotation);
						GameObject bullet3 = Instantiate (shot, shotSpawn3.position, shotSpawn3.rotation);

						bullet1.GetComponent<DestroyBullet> ().bulletOrigin = currentPlayer;
						bullet2.GetComponent<DestroyBullet> ().bulletOrigin = currentPlayer;
						bullet3.GetComponent<DestroyBullet> ().bulletOrigin = currentPlayer;

						if (!AudioFX.currentVolume) {
							GetComponent<AudioSource> ().PlayOneShot (TripleShotAudio);
						}
					}
					if (ShotType == 4) {

						GameObject bullet2 = Instantiate (DoubleShot, shotSpawn2.position, new Quaternion (shotSpawn2.rotation.x, 180f, shotSpawn2.rotation.z, shotSpawn2.rotation.w));
						GameObject bullet3 = Instantiate (DoubleShot, shotSpawn3.position, shotSpawn3.rotation);

						bullet2.GetComponent<DestroyBullet> ().bulletOrigin = currentPlayer;
						bullet3.GetComponent<DestroyBullet> ().bulletOrigin = currentPlayer;


						if (!AudioFX.currentVolume) {
							GetComponent<AudioSource> ().PlayOneShot (TripleShotAudio);
						}
					}
				}
				//if (ShotUpgrade == 1) {
					/*SpecialShot.SetActive (true);
					SpecialShot.transform.localScale = new Vector3 (1.0f, SpecialShot.transform.localScale.y, SpecialShot.transform.localScale.z);
					if (!AudioFX.currentVolume) {
						GetComponent<AudioSource> ().PlayOneShot (OneShot);
					}*/
				//}

				if (ShotUpgrade == 2) {
					float shotAngle = 0.6f;
					fireRate = 1.0f;
					for (int i = 0; i <= 13; i++) {
						/*Shotgun [i] = Instantiate (shot, shotSpawn.position, new Quaternion (shotSpawn.rotation.x, shotSpawn.rotation.y, shotSpawn.rotation.z + shotAngle, shotSpawn.rotation.w));
						Shotgun [i].GetComponent<DestroyBullet> ().bulletOrigin = currentPlayer;*/
						GameObject SpreadShot = Instantiate (shot, shotSpawn.position, new Quaternion (shotSpawn.rotation.x, shotSpawn.rotation.y, shotSpawn.rotation.z + shotAngle, shotSpawn.rotation.w));
						SpreadShot.GetComponent<DestroyBullet> ().bulletOrigin = currentPlayer;
						shotAngle = shotAngle - 0.1f;
						//Debug.Log ("i = " + i + ", shotAngle = " + shotAngle);
					}
					if (!AudioFX.currentVolume) {
						GetComponent<AudioSource> ().PlayOneShot (OneShot);
					}
				}

				if (ShotUpgrade == 3) {
					fireRate = 1.0f;
					GameObject RocketShot = Instantiate (Rocket, shotSpawn.position, shotSpawn.rotation);
					RocketShot.GetComponent<DestroyBullet> ().bulletOrigin = currentPlayer;
					if (!AudioFX.currentVolume) {
						GetComponent<AudioSource> ().PlayOneShot (OneShot);
					}
				}

				if(ShotUpgrade == 4){
					fireRate = 0.5f;
					GameObject Spinner = Instantiate (SpinShot, shotSpawn.position, shotSpawn.rotation);
					Spinner.GetComponent<DestroyBullet> ().bulletOrigin = currentPlayer;
					if (!AudioFX.currentVolume) {
						GetComponent<AudioSource> ().PlayOneShot (OneShot);
					}
					StartCoroutine (CustomWaitTime(0.15f));
					/*GameObject Spinner2 = Instantiate (SpinShot, shotSpawn.position, shotSpawn.rotation);
					Spinner2.GetComponent<DestroyBullet> ().bulletOrigin = currentPlayer;
					if (!AudioFX.currentVolume) {
						GetComponent<AudioSource> ().PlayOneShot (OneShot);
					}
					StartCoroutine (CustomWaitTime(0.15f));
					GameObject Spinner3 = Instantiate (SpinShot, shotSpawn.position, shotSpawn.rotation);
					Spinner3.GetComponent<DestroyBullet> ().bulletOrigin = currentPlayer;
					if (!AudioFX.currentVolume) {
						GetComponent<AudioSource> ().PlayOneShot (OneShot);
					}
					StartCoroutine (CustomWaitTime(0.15f));*/

				}

			}
		}
	}

	public void AndroidSpecialAttack(){
	
		if (SpFire > 0 && Time.deltaTime > 0f && SpCoolDown) {
			SpCoolDown = false;
			//StartCoroutine(BigSpecialAttack());

			SpecialShot.SetActive (true);
			SpecialShot.transform.localScale = new Vector3 (1.0f, SpecialShot.transform.localScale.y, SpecialShot.transform.localScale.z);
			if (!AudioFX.currentVolume) {
				GetComponent<AudioSource> ().PlayOneShot (OneShot);
			}

			SpFire = SpFire - 1;
		}	
	}

	void FixedUpdate ()
	{
		
	#if UNITY_STANDALONE || UNITY_WEBPLAYER
		moveHorizontal = Input.GetAxis ("Horizontal");
		moveVertical = Input.GetAxis ("Vertical");

		moveHorizontalP2 = Input.GetAxis ("HorizontalP2");
		moveVerticalP2 = Input.GetAxis ("VerticalP2");

		moveHorizontalGamepad = Input.GetAxis (currentPlayer + "D-Pad Horizontal");
		moveVerticalGamepad = Input.GetAxis (currentPlayer + "D-Pad Vertical");
	#elif UNITY_ANDROID
		moveHorizontal = CrossPlatformInputManager.GetAxis("Horizontal");
		moveVertical = CrossPlatformInputManager.GetAxis("Vertical");

		AndroidAttack(CrossPlatformInputManager.GetButton("Attack"));
	#endif

		Vector3 movement = new Vector3(0f, 0f, 0f);

		if (moveHorizontalGamepad != 0 || moveVerticalGamepad != 0) {
			if (moveHorizontalGamepad < -1) {
				moveHorizontalGamepad = -1;
			} else if (moveHorizontalGamepad > 1){
				moveHorizontalGamepad = 1;
			}
			if (moveVerticalGamepad < -1) {
				moveVerticalGamepad = -1;
			} else if (moveVerticalGamepad > 1){
				moveVerticalGamepad = 1;
			}
			movement = new Vector3 (moveHorizontalGamepad, moveVerticalGamepad, 0.0f);
		} else {
			if (currentPlayer == 1) {
				movement = new Vector3 (moveHorizontal, moveVertical, 0.0f);
			}
			if (currentPlayer == 2) {
				movement = new Vector3 (moveHorizontalP2, moveVerticalP2, 0.0f);
			}
		}
	#if UNITY_STANDALONE || UNITY_WEBPLAYER
		GetComponent<Rigidbody2D>().velocity = movement * speed;
	#elif UNITY_ANDROID
		GetComponent<Rigidbody2D>().velocity = movement * (speed/2);
	#endif

		if (Camera.main.aspect == 0.625f) {
			GetComponent<Rigidbody2D> ().position = new Vector2 (
			Mathf.Clamp (GetComponent<Rigidbody2D> ().position.x, boundary.xMin * 0.9375f, boundary.xMax * 0.9375f), 
			Mathf.Clamp (GetComponent<Rigidbody2D> ().position.y, boundary.zMin * 0.9375f, boundary.zMax * 0.9375f)
		);
		} else {
			GetComponent<Rigidbody2D> ().position = new Vector2 (
				Mathf.Clamp (GetComponent<Rigidbody2D> ().position.x, boundary.xMin, boundary.xMax), 
				Mathf.Clamp (GetComponent<Rigidbody2D> ().position.y, boundary.zMin, boundary.zMax)
			);
		}

	}
	

	public void GotHit (int damage){

		if (damage > 0 && ShieldCharge > 0) {
			ShieldCharge = ShieldCharge - damage;
			AudioFX.GotShot ();
			if (ShieldCharge < 0) {
				playerLife = playerLife + ShieldCharge;
				ShieldCharge = 0;
				AudioFX.ShieldsDown ();
			}
			return;
		}

		playerLife = playerLife - damage;
		if (damage > 0) {
			AudioFX.GotShot ();
			StartCoroutine (BlinkingAfterHit ());
		}
	}

	IEnumerator BlinkingAfterHit(){

		//GetComponent<SpriteRenderer> ().enabled = false;
		GetComponent<SpriteRenderer> ().color = new Vector4(GetComponent<SpriteRenderer> ().color.r, GetComponent<SpriteRenderer> ().color.g, GetComponent<SpriteRenderer> ().color.b, 0f);

		yield return new WaitForSeconds (0.1f);

		//GetComponent<SpriteRenderer> ().enabled = true;
		GetComponent<SpriteRenderer> ().color = new Vector4(GetComponent<SpriteRenderer> ().color.r, GetComponent<SpriteRenderer> ().color.g, GetComponent<SpriteRenderer> ().color.b, 1f);

		yield return new WaitForSeconds (0.1f);

		//GetComponent<SpriteRenderer> ().enabled = false;
		GetComponent<SpriteRenderer> ().color = new Vector4(GetComponent<SpriteRenderer> ().color.r, GetComponent<SpriteRenderer> ().color.g, GetComponent<SpriteRenderer> ().color.b, 0f);

		yield return new WaitForSeconds (0.1f);

		//GetComponent<SpriteRenderer> ().enabled = true;
		GetComponent<SpriteRenderer> ().color = new Vector4(GetComponent<SpriteRenderer> ().color.r, GetComponent<SpriteRenderer> ().color.g, GetComponent<SpriteRenderer> ().color.b, 1f);

	}

	IEnumerator MachineGunSpecial(){//not being used

		float shotAngle = 0.7f;
		for (int i = 0; i <= 15; i++) {
			Instantiate (slowShot, shotSpawn.position, new Quaternion(shotSpawn.rotation.x, shotSpawn.rotation.y, shotSpawn.rotation.z + shotAngle, shotSpawn.rotation.w));
			if (!AudioFX.currentVolume) {
				GetComponent<AudioSource> ().PlayOneShot (OneShot);
			}
			yield return new WaitForSeconds (0.2f);
			shotAngle = shotAngle - 0.1f;
		}
		for (int i = 0; i <= 15; i++) {
			Instantiate (slowShot, shotSpawn.position, new Quaternion(shotSpawn.rotation.x, shotSpawn.rotation.y, shotSpawn.rotation.z + shotAngle, shotSpawn.rotation.w));
			if (!AudioFX.currentVolume) {
				GetComponent<AudioSource> ().PlayOneShot (OneShot);
			}
			yield return new WaitForSeconds (0.2f);
			shotAngle = shotAngle + 0.1f;
		} 

		yield return new WaitForSeconds (1f);
		SpCoolDown = true;
	}

	IEnumerator BigSpecialAttack(){

		for (int i = 0; i <= 15; i++) {
		
			Instantiate (BigSpecial, shotSpawn.position, shotSpawn.rotation);
			Instantiate (BigSpecial, shotSpawn2.position, shotSpawn.rotation);
			Instantiate (BigSpecial, shotSpawn3.position, shotSpawn.rotation);
			if (!AudioFX.currentVolume) {
				GetComponent<AudioSource> ().PlayOneShot (OneShot);
			}
			yield return new WaitForSeconds (0.2f);
		}
		yield return new WaitForSeconds (1f);
		SpCoolDown = true;
	}

	IEnumerator CustomWaitTime(float howMuch){

		yield return new WaitForSeconds (howMuch);
	}
}

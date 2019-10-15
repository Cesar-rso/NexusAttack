using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Advertisements;
using GooglePlayGames;
using UnityEngine.SocialPlatforms;

public class Done_GameController : MonoBehaviour
{
	public GameObject[] hazards;
	public GameObject[] bosses;
	public GameObject[] fogs;

	public GameObject asteroidObj;
	public Vector3 spawnValues;
	public int hazardCount, liveEnemyCount;
	public int WaveCount = 0, lastBossFought = -1, nextBoss = -1;
	public float spawnWait;
	public float startWait;
	public float waveWait;
	public float EnemyColorG = 1.0f, EnemyColorB = 1.0f, EnemyColorR = 1.0f;

	public AudioClip MainTheme, BossTheme, GameOverTheme, MainThemeB, BossThemeB;
	public AudioFXController AudioFX;
	
	public Text scoreText, P2scoreText, BestScores;
	public Text restartText;
	public Text gameOverText;
	public Text ContinuesText, P2ContinuesText;
	public int scoreBonus = 1;
	public int scoreBonusP2 = 1;
	public int P1PowerUpShot = 0, P2PowerUpShot = 0;

	public GameObject player1, player2, playerSpawnPoint;
	public int P1Ship = 0, P2Ship = 0;
	public Sprite[] Ships;
	public Image FadeImage;

	private bool gameOver;
	private bool restart;
	private bool AsteroidBelt;
	public bool tutorialOn;
	private bool p1destroyed;
	private bool p2destroyed;
	private bool endgame = false;
	private int score, scoreP2;
	private int continues, extracontinue = 0;
	private int continuesP2, extracontinueP2 = 0;
	private float fogSpawn;
	private DataManager data;
	public bool player2start = false;
	public bool sceneEnding = false;
	public InputField P1name, P2name;

	public Canvas PauseCanvas, RestartCanvas;
	public GameObject PauseObject;

	public string ExceptionCondition;
	public int conditionType;
	public GameObject gamecontrollerObj;

	#if UNITY_ANDROID
	private string gameId = "1646359";
	#endif

	
	void Start ()
	{
		gameOver = false;
		restart = false;
		player2start = false;
		p1destroyed = false;
		p2destroyed = true;
		tutorialOn = true;
		sceneEnding = false;
		AsteroidBelt = false;
		restartText.text = "";
		gameOverText.text = "";
		score = 0;
		scoreP2 = 0;
		continues = 4;
		continuesP2 = 4;
		UpdateContinues ();

		#if UNITY_STANDALONE || UNITY_WEBPLAYER
		UpdateContinuesP2 ();
		UpdateScoreP2 ();
		#endif

		#if UNITY_ADS
		if (Advertisement.isSupported) {
			Advertisement.Initialize (gameId, true);
		}
		#endif

		UpdateScore ();
		StartCoroutine (SpawnWaves ());
		GetComponent<AudioSource> ().clip = MainTheme;
		GetComponent<AudioSource> ().loop = false;
		GetComponent<AudioSource> ().Play ();
		fogSpawn = Time.time;
		Instantiate (fogs [Random.Range (0, fogs.Length - 1)], new Vector3 (0f, 12f, 3f), Quaternion.identity);

		GameObject AudioControllerObject = GameObject.FindGameObjectWithTag ("AudioFX");
		if (AudioControllerObject != null) {

			AudioFX = AudioControllerObject.GetComponent<AudioFXController> ();
		}
		if (AudioFX == null) {

			Debug.Log ("Cannot find 'AudioFXController' script");
		}

		GameObject DataObject = GameObject.FindGameObjectWithTag ("Datamanager");
		if (DataObject != null) {

			data = DataObject.GetComponent<DataManager> ();
			data.Load ();
		}
		if (data == null) {

			Debug.Log ("Cannot find 'DataManager' script");
		}

		#if UNITY_STANDALONE || UNITY_WEBPLAYER
		if (PlayerPrefs.GetInt ("Player2in") == 1) {
			Instantiate (player2, playerSpawnPoint.transform.position, playerSpawnPoint.transform.rotation);
			player2start = true;
		} else {
			P2name.image.color = new Vector4 (0, 0, 0, 0);
			P2name.text = "";
			P2name.interactable = false;
		}
		#endif

		if (PlayerPrefs.GetInt ("AlreadyRestarted") == 1) {
			tutorialOn = false;
		}

		//Instantiate (gamecontrollerObj, transform.position, transform.rotation);
		//P1Ship = PlayerPrefs.GetInt ("P1Ship");
	}
	
	void Update ()
	{ 

		if(sceneEnding) {
			EndScene ();
		}
			

		if (AudioFX == null) {
			GameObject AudioControllerObject = GameObject.FindGameObjectWithTag ("AudioFX");
			if (AudioControllerObject != null) {

				AudioFX = AudioControllerObject.GetComponent<AudioFXController> ();
			}

		}

		if (restart)
		{
			RestartCanvas.enabled = true;
			if (Input.GetKeyDown (KeyCode.R))
			{
				PlayerPrefs.SetInt("AlreadyRestarted", 1);
				SceneManager.LoadScene (SceneManager.GetActiveScene().buildIndex);
			}
				
		} else{
			RestartCanvas.enabled = false;

			if ((Time.time - fogSpawn) > 90f) {
				Instantiate (fogs [Random.Range (0, fogs.Length - 1)], new Vector3 (0f, 15f, 3f), Quaternion.identity);
				fogSpawn = Time.time;
			}

			if (GetComponent<AudioSource> ().clip.name.Equals ("Main Theme A") && !GetComponent<AudioSource> ().isPlaying) {
				GetComponent<AudioSource> ().clip = MainThemeB;
				GetComponent<AudioSource> ().loop = true;
				GetComponent<AudioSource> ().Play ();
			}

			if (GetComponent<AudioSource> ().clip.name.Equals ("Boss Theme A") && !GetComponent<AudioSource> ().isPlaying) {

				GetComponent<AudioSource> ().clip = BossThemeB;
				GetComponent<AudioSource> ().loop = true;
				GetComponent<AudioSource> ().Play ();
			}
				
		}

		if((continues <= 0 && continuesP2 <= 0 && (p1destroyed && p2destroyed)) || (continues <= 0 && p1destroyed && !player2start)){
			GameOver ();
		}

		if (Input.GetKeyDown (KeyCode.Escape) && !gameOver) {
			PauseCanvas.GetComponent<PauseMenu> ().Pause ();
		}
	}
	
	IEnumerator SpawnWaves ()
	{
		yield return new WaitForSeconds (startWait);
		while (true)
		{
			//Debug.Log (liveEnemyCount + " inimigos antes da verificacao");
			try{
				GameObject[] enemies = GameObject.FindGameObjectsWithTag ("Enemy"); 
				GameObject[] bossesleft = GameObject.FindGameObjectsWithTag ("Boss");
				GameObject[] asteroidsLeft = GameObject.FindGameObjectsWithTag("Asteroid");
				//Debug.Log("Verificacao dentro do try 1 " + enemies.Length + " " + bossesleft.Length);
				if (enemies.Length <= 0 && bossesleft.Length <= 0) {
					liveEnemyCount = 0;
				}
				if(enemies == null && (WaveCount%5) != 0){
					liveEnemyCount = 0;
				}
				if((WaveCount%5) == 0 && bossesleft == null){
					liveEnemyCount = 0;
				}
				if(enemies.Length > (hazardCount + (WaveCount / 2)) && (WaveCount%5) != 0){
					liveEnemyCount = 0;
				} else if(enemies.Length > 0){
					liveEnemyCount = enemies.Length;
				}
				if(bossesleft.Length > 2 && (WaveCount%5) == 0){
					liveEnemyCount = 0;
				}else if(bossesleft.Length > 0){
					liveEnemyCount = bossesleft.Length;
				}
				if((asteroidsLeft.Length <=0 || asteroidsLeft == null) && AsteroidBelt){
					AsteroidBelt = false;
					liveEnemyCount = 0;
				}
			}catch{
				liveEnemyCount = 0;
			}
			if (liveEnemyCount <= 0) {
				WaveCount++;
				scoreBonus = 1;
				int Bosswave = WaveCount % 5;
				if (Bosswave == 0) {

					AudioFX.BossIncoming ();
					GetComponent<AudioSource> ().clip = BossTheme;
					GetComponent<AudioSource> ().loop = false;
					GetComponent<AudioSource> ().Play ();
					gameOverText.text = "Boss Battle";
					yield return new WaitForSeconds (8f);
					while (nextBoss == lastBossFought) {
						nextBoss = Random.Range (0, bosses.Length);
					}
					GameObject Boss = bosses [nextBoss];
					lastBossFought = nextBoss;
					Vector3 spawnPosition = new Vector3 (Random.Range (-spawnValues.x, spawnValues.x), spawnValues.y, spawnValues.z);
					Quaternion spawnRotation = Quaternion.identity;
					Instantiate (Boss, spawnPosition, spawnRotation);
					yield return new WaitForSeconds (spawnWait);
					gameOverText.text = "";
					liveEnemyCount = liveEnemyCount + 1;
				} else {
					gameOverText.text = "Wave " + WaveCount.ToString ();
					EnemyColorR = (float)Random.Range (175, 255);
					EnemyColorB = (float)Random.Range (175, 255);
					EnemyColorG = (float)Random.Range (175, 255);

					if (WaveCount >= 6 && (!WaveCount.ToString().Contains("4"))) {
						int ABelt = Random.Range (1, 20);
						if (ABelt == 4 && !AsteroidBelt) {
							liveEnemyCount = 1;
							AsteroidBelt = true;
							gameOverText.text = "Asteroid Belt";
							for (int i = 0; i < 31; i++) {
								Vector3 spawnPosition = new Vector3 (Random.Range (-spawnValues.x, spawnValues.x), spawnValues.y, spawnValues.z);
								Quaternion spawnRotation = Quaternion.identity;
								Instantiate (asteroidObj, spawnPosition, spawnRotation);
								yield return new WaitForSeconds (spawnWait);
								if (i > 5) {
									gameOverText.text = "";
								}
							}
						} else {
							for (int i = 0; i < (hazardCount + (WaveCount / 2)); i++) {
								GameObject hazard = hazards [Random.Range (0, hazards.Length)];
								Vector3 spawnPosition = new Vector3 (Random.Range (-spawnValues.x, spawnValues.x), spawnValues.y, spawnValues.z);
								Quaternion spawnRotation = Quaternion.identity;
								Instantiate (hazard, spawnPosition, spawnRotation);
								if (Random.Range (0, 10) > 8) {
									Instantiate (asteroidObj, spawnPosition, spawnRotation);
								}
								yield return new WaitForSeconds (spawnWait);
								gameOverText.text = "";
								liveEnemyCount = liveEnemyCount + 1;
							}
						}
					} else {
						int Initialenemytype = Random.Range (0, hazards.Length);
						for (int i = 0; i < (hazardCount + (WaveCount / 2)); i++) {
							GameObject hazard = hazards [Initialenemytype];
							Vector3 spawnPosition = new Vector3 (Random.Range (-spawnValues.x, spawnValues.x), spawnValues.y, spawnValues.z);
							Quaternion spawnRotation = Quaternion.identity;
							Instantiate (hazard, spawnPosition, spawnRotation);
							yield return new WaitForSeconds (spawnWait);
							gameOverText.text = "";
							liveEnemyCount = liveEnemyCount + 1;
						}
					}
				}
				//liveEnemyCount++;

			} 
			yield return new WaitForSeconds (waveWait);

			try{
				GameObject[] enemies = GameObject.FindGameObjectsWithTag ("Enemy"); 
				GameObject[] bossesleft = GameObject.FindGameObjectsWithTag ("Boss");

				if (enemies.Length <= 0 && bossesleft.Length <= 0) {
					liveEnemyCount = 0;
				}
				if(enemies == null && (WaveCount%5) != 0){
					liveEnemyCount = 0;
				}
				if((WaveCount%5) == 0 && bossesleft == null){
					liveEnemyCount = 0;
				}
				if(enemies.Length > (hazardCount + (WaveCount / 2)) && (WaveCount%5) != 0){
					liveEnemyCount = 0;
				}
				if(bossesleft.Length > 2 && (WaveCount%5) == 0){
					liveEnemyCount = 0;
				}
			}catch{
				liveEnemyCount = 0;
			}
			if (gameOver)
			{
				restartText.text = "";
				restart = true;
				break;
			}
		}
	}
	
	public void AddScore (int newScoreValue)
	{
		newScoreValue = newScoreValue * scoreBonus;
		score += newScoreValue;
		extracontinue += newScoreValue;
		if (extracontinue >= 5000) {
			continues++;
			UpdateContinues ();
			extracontinue = 0;
		}
		UpdateScore ();
	}

	#if UNITY_STANDALONE || UNITY_WEBPLAYER
	public void AddScoreP2 (int newScoreValue)
	{
		newScoreValue = newScoreValue * scoreBonusP2;
		scoreP2 += newScoreValue;
		extracontinueP2 += newScoreValue;
		if (extracontinueP2 >= 5000) {
			continuesP2++;
			UpdateContinuesP2 ();
			extracontinueP2 = 0;
		}
		UpdateScoreP2 ();
	}

	void UpdateScoreP2(){

		P2scoreText.text = "Score: " + scoreP2;
	}

	void UpdateContinuesP2(){
		P2ContinuesText.text = "Continues: " + continuesP2;
	}
	#endif

	void UpdateScore ()
	{
		scoreText.text = "Score: " + score;
	}


	void UpdateContinues(){
		ContinuesText.text = "Lives: " + continues;
	}

	public void AdContinue(){

		const string RewardedPlacementId = "rewardedVideo";
		#if UNITY_ADS
		if (!Advertisement.IsReady(RewardedPlacementId))
		{
		Debug.Log(string.Format("Ads not ready for placement '{0}'", RewardedPlacementId));
		return;
		}

		var options = new ShowOptions { resultCallback = HandleShowResult };
		Advertisement.Show(RewardedPlacementId, options);
		#endif
	}

	#if UNITY_ADS
	private void HandleShowResult(ShowResult result)
	{
		switch (result)
		{
			case ShowResult.Finished:
				Debug.Log("The ad was successfully shown.");
	//
	// YOUR CODE TO REWARD THE GAMER
	// Give coins etc.
				gameOver = false;
				endgame = false;
				restart = false;
				player2start = false;
				p1destroyed = false;
				p2destroyed = true;
				tutorialOn = true;
				sceneEnding = false;
				AsteroidBelt = false;
				restartText.text = "";
				gameOverText.text = "";
				continues = 5;
				BestScores.GetComponent<CanvasGroup> ().alpha = 0f;

				UpdateScore ();
				StartCoroutine (SpawnWaves ());
				GetComponent<AudioSource> ().clip = MainTheme;
				GetComponent<AudioSource> ().loop = false;
				GetComponent<AudioSource> ().Play ();
				fogSpawn = Time.time;
				UpdateContinues ();
				PlayerDestroyed (1);
				break;

			case ShowResult.Skipped:
				Debug.Log("The ad was skipped before reaching the end.");
				break;

			case ShowResult.Failed:
				Debug.LogError("The ad failed to be shown.");
				break;
		}
	}

	#endif

	
	public void GameOver ()
	{
		gameOverText.text = "Game Over!";
		BestScores.GetComponent<CanvasGroup> ().alpha = 1f;
		gameOver = true;

		if (!endgame) {
			GetComponent<AudioSource> ().Stop ();
			GetComponent<AudioSource> ().PlayOneShot (GameOverTheme);
			AudioFX.GameOverVoice ();
			endgame = true;
		}


		if (score > data.score [0]) {
			data.score [2] = data.score [1];
			data.score [1] = data.score [0];
			data.score [0] = score;

			data.playername [2] = data.playername [1];
			data.playername [1] = data.playername [0];
			data.playername [0] = P1name.text;
		}else if (score > data.score [1] && score < data.score[0]) {
			data.score [2] = data.score [1];
			data.score [1] = score;

			data.playername [2] = data.playername [1];
			data.playername [1] = P1name.text;
		}else if (score > data.score [2] && score < data.score[1]) {
			data.score [2] = score;
			data.playername [2] = P1name.text;
		}

		if (scoreP2 > data.score [0]) {
			data.score [2] = data.score [1];
			data.score [1] = data.score [0];
			data.score [0] = scoreP2;

			data.playername [2] = data.playername [1];
			data.playername [1] = data.playername [0];
			data.playername [0] = P2name.text;
		}else if (scoreP2 > data.score [1] && score < data.score[0]) {
			data.score [2] = data.score [1];
			data.score [1] = scoreP2;

			data.playername [2] = data.playername [1];
			data.playername [1] = P2name.text;
		}else if (scoreP2 > data.score [2] && score < data.score[1]) {
			data.score [2] = scoreP2;
			data.playername [2] = P2name.text;
		}
		data.Save ();
		BestScores.text = "Best Scores (Local)\n1 - " + data.playername [0] + " " + data.score [0] + "\n2 - " + data.playername [1] + " " + data.score [1] + "\n3 - " + data.playername [2] + " " + data.score[2];
		
	}

	public void PlayerDestroyed(int whichPlayer){
		if (whichPlayer == 1) {
			p1destroyed = true;
			if (continues > 0) {
				StartCoroutine (WaitSeconds(1, 1));
			}
		}
		if(whichPlayer == 2){
			p2destroyed = true;
			if (continuesP2 > 0) {
				StartCoroutine (WaitSeconds(1, 2));
			}
		}
	}

	public void RestartGame(){
		if (Social.localUser.authenticated) {
			Social.ReportScore(score, "CgkIk7-ewsAVEAIQAQ", (bool success) => {
				// handle success or failure
			});

			Social.ReportScore(WaveCount, "CgkIk7-ewsAVEAIQAg", (bool success) => {
				// handle success or failure
			});
		}
		PlayerPrefs.SetInt("AlreadyRestarted", 1);
		SceneManager.LoadScene (SceneManager.GetActiveScene().buildIndex);
	}

	public void QuitGame(){
		
		sceneEnding = true;
		if (Social.localUser.authenticated) {
			Social.ReportScore(score, "CgkIk7-ewsAVEAIQAQ", (bool success) => {
				// handle success or failure
			});

			Social.ReportScore(WaveCount, "CgkIk7-ewsAVEAIQAg", (bool success) => {
				// handle success or failure
			});
		}
	}

	public void GoogleLeaderboard(){

		if (Social.localUser.authenticated) {
			Social.ShowLeaderboardUI ();
		}
	}

	public void DeadEnemy(){
		
		liveEnemyCount = liveEnemyCount - 1;

		if (liveEnemyCount < 0) {
			liveEnemyCount = 0;
		}

	}


	public void EndScene ()
	{
			/*DestroyImmediate(BossTheme);
			DestroyImmediate(BossThemeB);
			DestroyImmediate(GameOverTheme);
			DestroyImmediate(MainTheme);
			DestroyImmediate(MainThemeB);
			DestroyImmediate(asteroidObj);*/
		StartCoroutine (StartNewScene(2));	
	}

	IEnumerator WaitSeconds(int seconds, int whichplayer){
	
		yield return new WaitForSeconds (seconds);

		if(whichplayer == 1){
			if (player1 == null) {
				Debug.Log ("Objeto player nulo");
			}
			if(playerSpawnPoint == null){
				Debug.Log ("Spawn point do player nulo");
			}
			Instantiate (player1, playerSpawnPoint.transform.position, playerSpawnPoint.transform.rotation);
			p1destroyed = false;
			continues--;
			UpdateContinues ();
		}

		#if UNITY_STANDALONE || UNITY_WEBPLAYER
		if(whichplayer == 2){
			Instantiate (player2, playerSpawnPoint.transform.position, playerSpawnPoint.transform.rotation);
			player2start = true;
			p2destroyed = false;
			continuesP2--;
			UpdateContinuesP2 ();
		}
		#endif
	}

	IEnumerator StartNewScene(int LoadType){

		float fadeTime = Camera.main.GetComponent<FadeScene2> ().BeginFade (1);
		yield return new WaitForSeconds (fadeTime);
		if (LoadType == 1) {
			SceneManager.LoadScene (SceneManager.GetActiveScene ().buildIndex + 1);
		} else if(LoadType == 2){
			SceneManager.LoadScene ("Menu");
		}
	}
		
}
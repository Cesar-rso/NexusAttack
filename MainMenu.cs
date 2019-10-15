using UnityEngine;
using System.Diagnostics;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using GooglePlayGames;
using UnityEngine.SocialPlatforms;

public class MainMenu : MonoBehaviour {

	public AudioClip StartClip, SelectionClip;
	public GameObject BGM, LeaderboardButton, errorTxt;
	private bool sceneEnding = false;

	void Awake(){

		PlayGamesPlatform.Activate ();
		OnConnectionResponse (PlayGamesPlatform.Instance.localUser.authenticated);
		Social.localUser.Authenticate((bool success) => {
			if(success){
				//((GooglePlayGames.PlayGamesPlatform)Social.Active).LoadScores();
				OnConnectionResponse(success);

			} else {
				UnityEngine.Debug.Log("Failed to log in to Google Services");
			}
		});
	}

	void Start(){
		PlayerPrefs.SetInt ("AlreadyRestarted", 2);
		sceneEnding = false;
		PlayerPrefs.SetInt ("inTutorial", 1);
	}

	void Update ()
	{

		if(sceneEnding) {
			EndScene ();
		}
	}

	public void StartGame(){
		GetComponent<AudioSource> ().PlayOneShot(StartClip);
		//sceneEnding = true;
		EndScene ();
	}

	public void Credits(){
		GetComponent<AudioSource> ().PlayOneShot (SelectionClip);
		StartCoroutine (StartNewScene(2));
	}

	public void QuitGame(){

		try{
			Application.Quit ();
		} catch{
			Process.GetCurrentProcess ().Kill ();
		}
	}

	public void GoogleLeaderboard(){

		if (Social.localUser.authenticated) {
			Social.ShowLeaderboardUI ();
		}
	}
		

	public void EndScene ()
	{
		errorTxt.GetComponent<Text> ().text = "Function EndScene ";
		StartCoroutine (StartNewScene(1));

	}

	IEnumerator StartNewScene(int LoadType){

		errorTxt.GetComponent<Text> ().text = errorTxt.GetComponent<Text> ().text + "+ StartNewScene ";
		float fadeTime = Camera.main.GetComponent<FadeScene2> ().BeginFade (1);
		errorTxt.GetComponent<Text> ().text = errorTxt.GetComponent<Text> ().text + " + " + fadeTime;
		yield return new WaitForSeconds (fadeTime);
		if (LoadType == 1) {
			SceneManager.LoadScene (SceneManager.GetActiveScene ().buildIndex + 1);
		} else if(LoadType == 2){
			SceneManager.LoadScene ("Credits");
		}
	}

	private void OnConnectionResponse (bool authenticated){

		LeaderboardButton.SetActive (authenticated);

	}
}

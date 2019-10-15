using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CreditsScreen : MonoBehaviour {

	public AudioClip SelectionAudio;

	private bool sceneEnding = false;

	// Update is called once per frame
	void Update () {


		if (sceneEnding) {
			EndScene ();
		}

		if(Input.GetKeyDown (KeyCode.Escape) || Input.GetButtonDown("1B")){
			sceneEnding = true;
		}
	}


	public void EndScene ()
	{
		StartCoroutine (WaitNewLevel ());
	}

	public void BackB(){
		GetComponent<AudioSource> ().PlayOneShot (SelectionAudio);
		sceneEnding = true;
	}

	IEnumerator WaitNewLevel(){
		
		float fadeTime = Camera.main.GetComponent<FadeScene2> ().BeginFade (1);
		yield return new WaitForSeconds (fadeTime);
		SceneManager.LoadScene ("Menu");

	}
}

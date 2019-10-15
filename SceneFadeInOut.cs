using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneFadeInOut : MonoBehaviour
{
	public float fadeSpeed = 1.505f;          // Speed that the screen fades to and from black.

	//public Canvas FadeCanvas;
	public Image FadeImage;
	
	private bool sceneStarting = true;      // Whether or not the scene is still fading in.
	private bool sceneEnding = false;
	
	
	void Awake ()
	{
		// Set the texture so that it is the the size of the screen and covers it.
		//FadeImage.minHeight = new Rect(0f, 0f, Screen.width, Screen.height);

	}
	
	
	void Update ()
	{

		//if (sceneEnding)
			//SceneManager.LoadScene (SceneManager.GetActiveScene().buildIndex + 1);
		// If the scene is starting...
		if (sceneStarting) {
			// ... call the StartScene function.
			StartScene ();
		} else {
			EndScene ();
		}
	}
	
	
	void FadeToClear ()
	{
		// Lerp the colour of the texture between itself and transparent.
		FadeImage.color = Color.Lerp(FadeImage.color, Color.clear, fadeSpeed * Time.deltaTime);
	}
	
	
	void FadeToBlack ()
	{
		// Lerp the colour of the texture between itself and black.
		FadeImage.color = Color.Lerp(FadeImage.color, Color.black, fadeSpeed * Time.deltaTime);
	}
	
	
	void StartScene ()
	{
		// Fade the texture to clear.
		FadeToClear();
		
		// If the texture is almost clear...
		if(FadeImage.color.a <= 0.01f)
		{
			// ... set the colour to clear and disable the GUITexture.
			FadeImage.color = Color.clear;
			FadeImage.enabled = false;
			
			// The scene is no longer starting.
			sceneStarting = false;
		}
	}
	
	
	public void EndScene ()
	{
		// Make sure the texture is enabled.
		FadeImage.enabled = true;
		
		// Start fading towards black.
		FadeToBlack();

		// If the screen is almost black...
		if (FadeImage.color.a >= 0.95f){ 
				
				// ... load the next level.
			StartCoroutine (WaitNewLevel (1));
		}

				//SceneManager.LoadScene (SceneManager.GetActiveScene ().buildIndex + 1);

		

	}
	
	public void EndBonusScene ()
	{
		// Make sure the texture is enabled.
		FadeImage.enabled = true;
		
		// Start fading towards black.
		FadeToBlack();
		//Debug.Log (FadeImage.color.a);
		// If the screen is almost black...
		//if(FadeImage.color.a >= 0.95f)
			//Debug.Log (FadeImage.color.a+" inside");
			// ... load the next level.
		StartCoroutine(WaitNewLevel(2));

		//SceneManager.LoadScene("MainMenu");

	}
	
	IEnumerator WaitNewLevel(int LoadType){
		yield return new WaitForSeconds(2);
		if(LoadType == 1){
			SceneManager.LoadScene (SceneManager.GetActiveScene ().buildIndex + 1);
		} else if(LoadType == 2){
			SceneManager.LoadScene ("Menu");
		}
		sceneEnding = true;
		//SceneManager.LoadScene (SceneManager.GetActiveScene ().buildIndex + 1);
	}
}

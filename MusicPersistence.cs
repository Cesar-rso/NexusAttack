using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class MusicPersistence : MonoBehaviour {

	public string ExceptionCondition;
	public int conditionType;

	public static MusicPersistence MenuMusic = null;
	public GameObject music;

	void Awake(){

		if(conditionType==0 || conditionType==null){
			conditionType = 1;
		}

		if(ExceptionCondition=="" || ExceptionCondition==null){
			ExceptionCondition = "Scene";
		}
		
		if (MenuMusic == null) {

			MenuMusic = this;
		
		}else if(MenuMusic != null || (conditionType==1 && SceneManager.GetActiveScene ().name.Contains (ExceptionCondition)) || (conditionType==2 && !SceneManager.GetActiveScene ().name.Contains (ExceptionCondition))){
			Destroy (gameObject);
		}

		DontDestroyOnLoad (gameObject);
	}

	// Use this for initialization
	void Start () {

		Instantiate (music, transform.position, transform.rotation);
		if(!transform.GetComponent<AudioSource>().isPlaying){
			transform.GetComponent<AudioSource> ().Play ();
		}
	
	}
	
	// Update is called once per frame
	void Update () {

		if (conditionType == 1) {
			if (SceneManager.GetActiveScene ().name.Contains (ExceptionCondition)) {
				Destroy (gameObject);
			}
		}
		if (conditionType == 2) {
			if (!SceneManager.GetActiveScene ().name.Contains (ExceptionCondition)) {
				Destroy (gameObject);
			}
		}

	}
}

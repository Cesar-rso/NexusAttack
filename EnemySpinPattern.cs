using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpinPattern : MonoBehaviour {

	public GameObject enemyPath1, enemyPath2, enemyPath3, enemyPath4, enemyPath5, enemyPath6, enemyPath7, enemyPath8, enemyPath9, enemyPath10;
	public float speed, spinspeed;
	int currentPosition = 0, positionCount = 0;
	private int rand = 0;
	private Done_GameController gameController;

	// Use this for initialization
	void Awake () {
		currentPosition = 0;

		GameObject gameControllerObject = GameObject.FindGameObjectWithTag ("GameController");
		if (gameControllerObject != null)
		{
			gameController = gameControllerObject.GetComponent <Done_GameController>();

		}
		if (gameController == null)
		{
			Debug.Log ("Cannot find 'GameController' script");
		}
	}
	
	// Update is called once per frame
	void Update () {
		transform.Rotate (Vector3.forward * spinspeed * Time.deltaTime);
		//GetComponent<SpriteRenderer> ().color = new Color (gameController.EnemyColorR, gameController.EnemyColorG, gameController.EnemyColorB, 255f);

		switch (currentPosition) {
			
		case 0:
			transform.position = Vector3.MoveTowards (transform.position, enemyPath1.transform.position, speed * Time.deltaTime);
			//currentPosition = 1;
			break;

		case 1:
			transform.position = Vector3.MoveTowards (transform.position, enemyPath2.transform.position, speed * Time.deltaTime);
			//currentPosition = 2;
			break;

		case 2: 
			transform.position = Vector3.MoveTowards (transform.position, enemyPath3.transform.position, speed * Time.deltaTime);
			//currentPosition = 3;
			break;

		case 3: 
			transform.position = Vector3.MoveTowards (transform.position, enemyPath4.transform.position, speed * Time.deltaTime);
			//currentPosition = 4;
			break;

		case 4:
			transform.position = Vector3.MoveTowards (transform.position, enemyPath5.transform.position, speed * Time.deltaTime);
			//currentPosition = 5;
			break;

		case 5:
			transform.position = Vector3.MoveTowards (transform.position, enemyPath6.transform.position, speed * Time.deltaTime);
			//currentPosition = 6;
			break;

		case 6:
			transform.position = Vector3.MoveTowards (transform.position, enemyPath7.transform.position, speed * Time.deltaTime);
			//currentPosition = 7;
			break;

		case 7:
			transform.position = Vector3.MoveTowards (transform.position, enemyPath1.transform.position, speed * Time.deltaTime);
			//currentPosition = 0;
			break;


		case 8:
			if (rand <= 33) {
				transform.position = Vector3.MoveTowards (transform.position, enemyPath8.transform.position, speed * Time.deltaTime);
			} else if (rand > 33 && rand <= 66) {
				transform.position = Vector3.MoveTowards (transform.position, enemyPath9.transform.position, speed * Time.deltaTime);
			} else {
				transform.position = Vector3.MoveTowards (transform.position, enemyPath10.transform.position, speed * Time.deltaTime);
			}
			break;
		}

		if (positionCount >= 4) {
			currentPosition = 8;
			positionCount = 0;
		}

		if (transform.position == enemyPath1.transform.position) {
			currentPosition = 1;
		}


		if (transform.position == enemyPath2.transform.position) {
			currentPosition = 2;
		}
			

		if (transform.position == enemyPath3.transform.position) {
			currentPosition = 3;
		}
			

		if (transform.position == enemyPath4.transform.position) {
			currentPosition = 4;
		}
			

		if (transform.position == enemyPath5.transform.position) {
			currentPosition = 5;
		}
			

		if (transform.position == enemyPath6.transform.position) {
			currentPosition = 6;
		}
			

		if (transform.position == enemyPath7.transform.position) {
			currentPosition = 7;
		}

		if (transform.position == enemyPath8.transform.position) {
			currentPosition = 7;
		}

		if (transform.position == enemyPath9.transform.position) {
			currentPosition = 5;
		}

		if (transform.position == enemyPath10.transform.position) {
			currentPosition = 4;
		}
			
			
		
	}

	void OnTriggerEnter2D(Collider2D other){
		if (other.tag == "Pathing" && other.name.Contains ("enemyPath (3)")) {
			positionCount++;
			rand = Random.Range (0, 100);
			//Debug.Log ("Random Range");
		}
	}
}

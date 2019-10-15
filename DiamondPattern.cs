using UnityEngine;
using System.Collections;

public class DiamondPattern : MonoBehaviour {

	public float speed;
	public Done_Boundary boundary;
	public bool UturnBack = false, UturnForward = false;
	float rnd;
	int turnCounter = 0;
	public GameObject EnemyPath, EnemyPath2, EnemyPath3;

	// Use this for initialization
	void Start () {

		rnd = Mathf.Clamp(Random.Range(0.0f,1.0f),0.0f,1.0f);
		if (rnd == 0.0f) {
			rnd = -1.0f;
		}
		GetComponent<Rigidbody2D>().velocity = new Vector2 (1f, 1.0f) * speed;
		GetComponent<Rigidbody2D> ().position = new Vector2 (-7.0f, 4f);

		//EnemyPath = (GameObject)Instantiate(destinyPoint, new Vector3(7.5f, 0f, 0f), Quaternion.identity);
	}
	
	void FixedUpdate (){
		GetComponent<Rigidbody2D>().position = new Vector2
			(
				Mathf.Clamp(GetComponent<Rigidbody2D>().position.x, boundary.xMin, boundary.xMax), 
				Mathf.Clamp(GetComponent<Rigidbody2D>().position.y, boundary.zMin, boundary.zMax)
			);
				

		//teste 2
		if (!UturnBack) {
			transform.position = Vector3.MoveTowards (transform.position, EnemyPath2.transform.position, speed);
		} else {
			if (turnCounter <= 4) {
				transform.position = Vector3.MoveTowards (transform.position, EnemyPath.transform.position, speed);
			} else {
				transform.position = Vector3.MoveTowards (transform.position, EnemyPath3.transform.position, speed);
			}
		}
			
		//fim teste 2

	}

	void OnTriggerEnter2D(Collider2D col){
		if(col.name.Equals(EnemyPath.transform.name)){
			UturnBack = false;
			turnCounter = turnCounter + 1;
		}
		if(col.name.Equals(EnemyPath2.transform.name)){
			UturnBack = true;
		}
		if(col.name.Equals(EnemyPath3.transform.name)){
			UturnBack = false;
			turnCounter = 0;
		}
	}
		
}

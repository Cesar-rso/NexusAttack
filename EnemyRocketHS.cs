using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyRocketHS : MonoBehaviour {


	public GameObject player;
	public float speed;

	// Use this for initialization
	void Start () {
		
		player = GameObject.FindGameObjectWithTag ("Player");

	}
	
	// Update is called once per frame
	void Update () {

		try{
			transform.position = Vector3.MoveTowards (transform.position, player.transform.position, speed * Time.deltaTime);
		}catch{
			Destroy (this);
		}
		if (player == null) {
			Destroy (transform.gameObject);
		}
		
	}
}

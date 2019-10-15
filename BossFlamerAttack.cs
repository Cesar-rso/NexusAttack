using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossFlamerAttack : MonoBehaviour {

	public float speed;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

		transform.RotateAround (new Vector3(0f, 0f, 0f), Vector3.forward, speed * 1.8f * Time.deltaTime);
		transform.Rotate (Vector3.back * speed * Time.deltaTime);
		
	}
}

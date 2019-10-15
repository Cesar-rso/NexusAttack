using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpinShot : MonoBehaviour {

	public float speed, timer, direction;
	public GameObject startPoint;

	// Use this for initialization
	void Start () {
		timer = 0f;
		if (startPoint == null) {
			startPoint = GameObject.FindGameObjectWithTag ("Boss");
		}
		if (direction == 0f) {
			direction = 1f;
		}
	}
	
	// Update is called once per frame
	void Update () {
		
		if (direction == 0f) {
			direction = 1f;
		}

		if (startPoint == null) {
			//Destroy (this);
			transform.RotateAround (new Vector3 ((0f + (direction * timer)), (3f - (1 * timer)), 0f), new Vector3 (0, 0, direction), speed * Time.deltaTime);
		} else {
			transform.RotateAround (new Vector3 ((startPoint.transform.position.x + (direction * timer)), (startPoint.transform.position.y - (1 * timer)), (startPoint.transform.position.z)), new Vector3 (0, 0, direction), speed * Time.deltaTime);
		}
		transform.Rotate (Vector3.back * speed * Time.deltaTime);
		timer = timer + Time.deltaTime;
	}
}

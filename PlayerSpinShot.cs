using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSpinShot : MonoBehaviour {

	public float speed, timer, direction;
	public GameObject startPoint;
	public Vector3 bulletpoint;

	// Use this for initialization
	void Start () {
		timer = 0f;
		if (startPoint == null) {
			startPoint = GameObject.FindGameObjectWithTag ("Player");
			bulletpoint = startPoint.transform.position;
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

		if (bulletpoint == null) {
			//Destroy (this);
			transform.RotateAround (new Vector3 (0f, (3f - (1 * timer)), 0f), new Vector3 (0, 0, direction), speed * Time.deltaTime);
		} else {
			transform.RotateAround (new Vector3 (bulletpoint.x , (bulletpoint.y + (1 * timer)), bulletpoint.z), new Vector3 (0, 0, direction), speed * Time.deltaTime);
		}
		transform.Rotate (Vector3.back * speed * Time.deltaTime);
		timer = timer + Time.deltaTime;
	}
}

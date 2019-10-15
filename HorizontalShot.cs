using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HorizontalShot : MonoBehaviour {

	public float speed;

	// Use this for initialization
	void Start () {
		GetComponent<Rigidbody2D> ().velocity = transform.right * speed;
	}

}

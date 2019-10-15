using UnityEngine;
using System.Collections;

public class NPattern : MonoBehaviour {

	public float speed;
	public Done_Boundary boundary;
	public float tilt;
	public bool UturnBack = false, UturnForward = false;
	float rnd;

	// Use this for initialization
	void Start () {

		rnd = Mathf.Clamp(Random.Range(0.0f,1.0f),0.0f,1.0f);
		if (rnd == 0.0f) {
			rnd = -1.0f;
		}
		GetComponent<Rigidbody>().velocity = new Vector3 (1f, 0.0f, 1f) * speed;
		GetComponent<Rigidbody> ().position = new Vector3 (-1,0,16);
	}

	void FixedUpdate (){
		GetComponent<Rigidbody>().position = new Vector3
			(
				Mathf.Clamp(GetComponent<Rigidbody>().position.x, boundary.xMin, boundary.xMax), 
				0.0f, 
				Mathf.Clamp(GetComponent<Rigidbody>().position.z, boundary.zMin, boundary.zMax)
			);

		//teste 1
		if (transform.position.x <= -5f) {
			GetComponent<Rigidbody> ().velocity = new Vector3(1f, 0, 1f) * speed * Mathf.Sign (transform.position.x);
		}

		if (transform.position.x >= 5f) {
			GetComponent<Rigidbody> ().velocity = new Vector3(-1f, 0, 1f) * speed * Mathf.Sign (transform.position.x);
		}

		if(!UturnForward && UturnBack && GetComponent<Rigidbody>().position.z > 13.0f){
			UturnForward = true;
			//GetComponent<Rigidbody>().velocity = transform.forward * speed;
			GetComponent<Rigidbody> ().velocity = new Vector3 (GetComponent<Rigidbody>().velocity.x, 0.0f, -GetComponent<Rigidbody>().velocity.z);
		}

		if(!UturnForward && !UturnBack && GetComponent<Rigidbody>().position.z < 2.0f){
			UturnBack = true;
			GetComponent<Rigidbody> ().velocity = new Vector3 (GetComponent<Rigidbody>().velocity.x, 0.0f, -GetComponent<Rigidbody>().velocity.z);
		}

		if(GetComponent<Rigidbody>().velocity.z > 0.0f && (!UturnBack || UturnForward)){

			GetComponent<Rigidbody>().velocity = new Vector3 (rnd, 0.0f, 1f) * speed;
		}
		//fim teste 1
		GetComponent<Rigidbody>().rotation = Quaternion.Euler (0, 0, GetComponent<Rigidbody>().velocity.x * -tilt);
		//GetComponent<Rigidbody>().velocity = transform.forward * speed;
	}
}

using UnityEngine;
using System.Collections;

public class ZigZagPattern : MonoBehaviour {


	public float speed;
	public Done_Boundary boundary;
	public float tilt;

	// Use this for initialization
	void Start () {
		transform.position = new Vector3(-5.5f, transform.position.y, transform.position.z);
		GetComponent<Rigidbody>().velocity = transform.right * speed;
	}

	void FixedUpdate (){
		
		StartCoroutine (Zigzag());
		GetComponent<Rigidbody>().position = new Vector3
			(
				Mathf.Clamp(GetComponent<Rigidbody>().position.x, boundary.xMin, boundary.xMax), 
				0.0f, 
				Mathf.Clamp(GetComponent<Rigidbody>().position.z, boundary.zMin, boundary.zMax)
			);
		GetComponent<Rigidbody>().rotation = Quaternion.Euler (0, 0, GetComponent<Rigidbody>().velocity.x * -tilt);
		//GetComponent<Rigidbody>().velocity = transform.forward * speed;
	}

	IEnumerator Zigzag(){

		if (transform.position.x >= 5f || transform.position.x <= -5f) {
			GetComponent<Rigidbody> ().velocity = transform.right * speed * Mathf.Sign (transform.position.x);
		}
		if(GetComponent<Rigidbody> ().velocity.z > speed){
			GetComponent<Rigidbody> ().velocity = new Vector3 (GetComponent<Rigidbody>().velocity.x, GetComponent<Rigidbody>().velocity.y, speed);
		}
		yield return new WaitForSeconds (0.5f);	
	}
}

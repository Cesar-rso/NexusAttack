using UnityEngine;
using System.Collections;

public class Mover : MonoBehaviour
{
	public float speed;

	void Start ()
	{
		if (transform.tag.Equals ("Boss")) {
		
			speed = speed * 2;
		}

		if (transform.name.Contains ("fog")) {
			GetComponent<Rigidbody2D> ().velocity = transform.right * speed;
		} else {
			GetComponent<Rigidbody2D> ().velocity = transform.up * speed;
		}
	}

	void Update (){
	
		if (transform.name.Contains ("fog")) {

			if (transform.position.y <= -9.5f) {
		
				transform.position = new Vector3 (0f, 12f, transform.position.z);

			} else {
				
				transform.position = new Vector3 (0f, transform.position.y, transform.position.z);
			
			}
		}
		if (transform.name.Contains ("Nexus_BG")) {

			if (transform.position.y <= -31.66f) {

				transform.position = new Vector3 (0f, 31.66f, transform.position.z);

			}/* else {

				transform.position = new Vector3 (0f, transform.position.y, transform.position.z);

			}*/
		
		}

		if (transform.tag.Contains ("Asteroid")) {
			transform.Rotate (Vector3.back * speed * Time.deltaTime);
		}
	}
}

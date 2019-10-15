using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSpecialShot : MonoBehaviour {

	bool shotTime = false;
	public GameObject player;


	void Start (){
		//player.GetComponent<Done_PlayerController> ().SpCoolDown = false;

		//transform.GetComponent<DistanceJoint2D> ().connectedBody = player.GetComponent<Rigidbody2D> ();
	}


	// Update is called once per frame
	void Update () {

		/*if (player.transform.rotation.z == 0 || player.transform.rotation.z == 180 || player.transform.rotation.z == 360) {
		
			transform.position = new Vector2 (player.transform.position.x + 0.09f, transform.position.y);
		}

		if (player.transform.rotation.z == 90 || player.transform.rotation.z == 270 || player.transform.rotation.z == 450) {
		
			transform.position = new Vector2 (transform.position.x, player.transform.position.y + 0.09f);
		}*/

		if (!shotTime) {
			StartCoroutine (FadeShot (1.5f, true));
		} else {
			StartCoroutine (FadeShot(0.1f, false));
		}
		if (transform.localScale.x < 0.2f) {
			
			player.GetComponent<Done_PlayerController> ().SpCoolDown = true;
			Debug.Log ("CoolDown depois: " + player.GetComponent<Done_PlayerController> ().SpCoolDown);
			//Destroy (transform.gameObject);
			shotTime = false;
			transform.gameObject.SetActive(false);
		}

		transform.position = new Vector3 (player.transform.position.x + 0.1f, player.transform.position.y + 7f, transform.position.z);
	}

	IEnumerator FadeShot(float seconds, bool justWait){
		
		yield return new WaitForSeconds (seconds);

		if (!justWait) {

			transform.localScale = new Vector3 (transform.localScale.x - 0.2f, transform.localScale.y, transform.localScale.z);
		
		} else {

			shotTime = true;
		
		}
	}
}

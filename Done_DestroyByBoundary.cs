using UnityEngine;
using System.Collections;

public class Done_DestroyByBoundary : MonoBehaviour
{

	private Done_GameController gameController;


	void Start ()
	{
		GameObject gameControllerObject = GameObject.FindGameObjectWithTag ("GameController");
		if (gameControllerObject != null)
		{
			gameController = gameControllerObject.GetComponent <Done_GameController>();
		}
		if (gameController == null)
		{
			Debug.Log ("Cannot find 'GameController' script");
		}
	}
		

	void OnTriggerEnter2D(Collider2D other){

		if (other.tag.Contains ("Pathing") || other.tag.Contains ("Boss")) {
			return;
		}

		if (other.tag.Contains ("Asteroid")) {
			Destroy (other.gameObject);
			return;
		}

		if (other.tag.Equals("Enemy") && !other.name.Contains("Attack")) {
			gameController.DeadEnemy ();
			Debug.Log (other.tag + " " + other.name + " marked as dead enemy");
		}

		if (!other.tag.Equals ("Special") && !other.tag.Equals("Player")) {
			Destroy (other.gameObject);
		}

		if(other.tag.Equals("Attack")){
			Destroy(other.gameObject);
		}

		if (other.tag.Contains("Enemy") && other.name.Contains("Attack")) {
			Destroy(other.gameObject);
		}

	}
}
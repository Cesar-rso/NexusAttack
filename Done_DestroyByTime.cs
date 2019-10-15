using UnityEngine;
using System.Collections;

public class Done_DestroyByTime : MonoBehaviour
{
	public float lifetime;
	public int Player;

	void Start ()
	{
		Destroy (gameObject, lifetime);
	}
}

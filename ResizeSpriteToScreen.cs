using UnityEngine;
using System.Collections;

public class ResizeSpriteToScreen : MonoBehaviour {

	// Use this for initialization
	void Start () {
		SpriteRenderer sr = GetComponent<SpriteRenderer>();
		
		float worldScreenHeight = Camera.main.orthographicSize * 2;
		float worldScreenWidth = worldScreenHeight / Screen.height * Screen.width;

		if (sr != null) {
			transform.localScale = new Vector3 (
				worldScreenWidth / sr.sprite.bounds.size.x,
				worldScreenHeight / sr.sprite.bounds.size.y, 1);
		}
	}

}

using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PlayerUI : MonoBehaviour {

	public GameObject PlayerHPBar;
	public GameObject Player, Player2;
	public GameObject[] SpecialFire = new GameObject[4];
	public GameObject[] SpecialFireP2 = new GameObject[4];
	float HPBar, BarPosition1, BarPositionF;
	public float MaxHP;
	public Slider Player1HP, Player2HP;
	public Button SpecialAttack;
	public Text P2Text, P2Continues, P2Score;
	bool playerdestroyed = false;

	// Use this for initialization
	void Start () {
	
		MaxHP = (float)Player.GetComponent<Done_PlayerController> ().playerLife;
		HPBar = (float)Player.GetComponent<Done_PlayerController> ().playerLife;
		BarPosition1 = PlayerHPBar.transform.position.x;

			
		if(MaxHP==0f){
			MaxHP = 10f;
		}
	}
	
	// Update is called once per frame
	void Update () {
	
		#if UNITY_STANDALONE || UNITY_WEBPLAYER
		if (!GetComponent<Done_GameController> ().player2start) {
			P2Text.text = "";
			Player2HP.GetComponent<CanvasGroup>().alpha = 0f;
			P2Continues.GetComponent<CanvasGroup> ().alpha = 0f;
			P2Score.GetComponent<CanvasGroup> ().alpha = 0f;
			for (int i = 0; i < 4; i++) {
				SpecialFireP2 [i].GetComponentInParent<CanvasGroup>().alpha = 0f;	
			}
		} else {
			P2Text.text = "P2";
			Player2HP.GetComponent<CanvasGroup>().alpha = 1f;
			P2Continues.GetComponent<CanvasGroup> ().alpha = 1f;
			P2Score.GetComponent<CanvasGroup> ().alpha = 1f;
			for (int i = 0; i < 4; i++) {
				SpecialFireP2 [i].GetComponentInParent<CanvasGroup>().alpha = 1f;	
			}
		}
		#endif

		if (Player != null) {
			Player1HP.maxValue = Player.GetComponent<Done_PlayerController> ().maxLife;
			Player1HP.fillRect.localScale = new Vector3 (1f, Player1HP.fillRect.localScale.y, Player1HP.fillRect.localScale.z);

			if (playerdestroyed) {
				SpecialAttack.onClick.AddListener (delegate {Player.GetComponent<Done_PlayerController>().AndroidSpecialAttack();});
				playerdestroyed = false;
			}
			for (int i = 0; i < 4; i++) {
				if ((Player.GetComponent<Done_PlayerController> ().SpFire - 1) < i) {
					SpecialFire [i].SetActive (false);
				} else {
					SpecialFire [i].SetActive (true);
				}
			}
			Player1HP.value = Player.GetComponent<Done_PlayerController> ().playerLife;
			
		} else {
			
			Player1HP.fillRect.localScale = new Vector3 (0.01f, Player1HP.fillRect.localScale.y, Player1HP.fillRect.localScale.z);
			SpecialAttack.onClick.RemoveAllListeners ();
			playerdestroyed = true;
		}

		#if UNITY_STANDALONE || UNITY_WEBPLAYER
		if (Player2 != null) {
			Player2HP.maxValue = Player2.GetComponent<Done_PlayerController> ().maxLife;
			Player2HP.fillRect.localScale = new Vector3 (1f, Player2HP.fillRect.localScale.y, Player2HP.fillRect.localScale.z);
			for (int i = 0; i < 4; i++) {
				if ((Player2.GetComponent<Done_PlayerController> ().SpFire - 1) < i) {
					SpecialFireP2 [i].SetActive (false);
				} else {
					SpecialFireP2 [i].SetActive (true);
				}
			}
			Player2HP.value = Player2.GetComponent<Done_PlayerController> ().playerLife;

		} else {
			
			Player2HP.fillRect.localScale = new Vector3 (0.01f, Player2HP.fillRect.localScale.y, Player2HP.fillRect.localScale.z);
		}
		#endif
	}
}

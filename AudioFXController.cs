using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioFXController : MonoBehaviour {

	public AudioClip ItemPickup, GameStart, EnemyExplosion, BossAlert, ShieldUp, ShieldDown, GameOver, ShotExplosion, BossDying;
	public AudioSource MainSource, extraSource;
	public bool currentVolume;


	// Use this for initialization
	void Awake () {

		MainSource.clip = GameStart;
		MainSource.PlayOneShot(GameStart);
		extraSource.mute = MainSource.mute;
		currentVolume = MainSource.mute;

	}
	
	// Update is called once per frame
	void Update () {

		extraSource.mute = MainSource.mute;

		currentVolume = MainSource.mute;
	}

	public void GotItem(){
	
		if (!MainSource.isPlaying) {
			MainSource.clip = ItemPickup;
			MainSource.enabled = true;
			MainSource.loop = false;
			MainSource.PlayOneShot(ItemPickup);
		} else {
			extraSource.clip = ItemPickup;
			extraSource.enabled = true;
			extraSource.loop = false;
			extraSource.PlayOneShot(ItemPickup);
		}

	}

	public void DestroyedEnemySound(){

		if (!MainSource.isPlaying) {
			MainSource.clip = EnemyExplosion;
			MainSource.enabled = true;
			MainSource.loop = false;
			MainSource.PlayOneShot(EnemyExplosion, 0.7f);
		} else {
				extraSource.clip = EnemyExplosion;
				extraSource.enabled = true;
				extraSource.loop = false;
				extraSource.PlayOneShot (EnemyExplosion, 0.7f);
		}
	}

	public void BossIncoming(){

		if (!MainSource.isPlaying) {
			MainSource.clip = BossAlert;
			MainSource.enabled = true;
			MainSource.loop = false;
			MainSource.PlayOneShot(BossAlert);
		} else {
			extraSource.clip = BossAlert;
			extraSource.enabled = true;
			extraSource.loop = false;
			extraSource.PlayOneShot(BossAlert);
		}
	}

	public void ShieldsUp(){

		if (!MainSource.isPlaying) {
			MainSource.clip = ShieldUp;
			MainSource.enabled = true;
			MainSource.loop = false;
			MainSource.PlayOneShot(ShieldUp);
		} else {
			extraSource.clip = ShieldUp;
			extraSource.enabled = true;
			extraSource.loop = false;
			extraSource.PlayOneShot(ShieldUp);
		}
	}

	public void ShieldsDown(){

		if (!MainSource.isPlaying) {
			MainSource.clip = ShieldDown;
			MainSource.enabled = true;
			MainSource.loop = false;
			MainSource.PlayOneShot(ShieldDown);
		} else {
			extraSource.clip = ShieldDown;
			extraSource.enabled = true;
			extraSource.loop = false;
			extraSource.PlayOneShot(ShieldDown);
		}
	}

	public void GameOverVoice(){

		if (!MainSource.isPlaying) {
			MainSource.clip = GameOver;
			MainSource.enabled = true;
			MainSource.loop = false;
			MainSource.PlayOneShot(GameOver, 0.7f);
		} else {
				extraSource.clip = GameOver;
				extraSource.enabled = true;
				extraSource.loop = false;
				extraSource.PlayOneShot (GameOver, 0.7f);
		}
	}

	public void GotShot(){

		if (!MainSource.isPlaying) {
			MainSource.clip = ShotExplosion;
			MainSource.enabled = true;
			MainSource.loop = false;
			MainSource.PlayOneShot(ShotExplosion);
		} else /*if(!extraSource.isPlaying || (extraSource.isPlaying && !extraSource.clip.name.Contains("Explosion")))*/ {
			extraSource.clip = ShotExplosion;
			extraSource.enabled = true;
			extraSource.loop = false;
			extraSource.PlayOneShot(ShotExplosion);
		}
	
	}  

	public void DeadBoss(){
	
		if (!MainSource.isPlaying) {
			MainSource.clip = BossDying;
			MainSource.enabled = true;
			MainSource.loop = false;
			MainSource.PlayOneShot(BossDying);
		} else {
			extraSource.clip = BossDying;
			extraSource.enabled = true;
			extraSource.loop = false;
			extraSource.PlayOneShot(BossDying);
		}
	}
}

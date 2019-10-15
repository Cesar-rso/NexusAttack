using UnityEngine;
using System.Collections;
using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public class DataManager : MonoBehaviour {

	public string language;
	public float volume;
	public string[] playername = new string[3];
	public int[] score = new int[3];

	// Use this for initialization
	void Awake () {

		Load ();
		if(language.Equals("") || language == null){
			language = "english";
		}
		if (volume == null || volume < 0f) {
			volume = 1f;
		}
		for (int i = 0; i < 3; i++) {
			if (score [i] == null || score[i] <=0) {
				score [i] = 1;
			}
			if (playername [i] == null || playername[i].Equals("")) {
				playername[i] = "---";
			}
		}
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void Load (){
		if(File.Exists(Application.persistentDataPath + "/Nexus.dat")){
			BinaryFormatter format = new BinaryFormatter();
			FileStream file = File.Open(Application.persistentDataPath + "/Nexus.dat",FileMode.Open);

			GameData data = (GameData)format.Deserialize(file);
			file.Close();
			language = data.language;
			volume = data.volume;
			score = data.score;
			playername = data.playername;
		}
	}

	public void Save(){
		BinaryFormatter format = new BinaryFormatter();
		FileStream file = File.Create(Application.persistentDataPath + "/Nexus.dat");

		GameData data = new GameData();
		data.language = language;
		data.volume = volume;
		data.score = score;
		data.playername = playername;

		format.Serialize(file,data);
		file.Close();
	}
}

[Serializable]
public class GameData{
	public string language;
	public float volume;
	public string[] playername;
	public int[] score;
}

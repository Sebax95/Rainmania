using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

[DefaultExecutionOrder(20)]
public static class SaveManager {

	private const string SAVE_NAME = "Save";
	private const string SAVES_FOLDER = "Saves";
	private const string SAVE_EXTENSION = ".dat";

	private static readonly char dash;
	private static readonly string saveDirectory;


	static SaveManager() {
		dash = Path.AltDirectorySeparatorChar;
		saveDirectory = $"{Application.persistentDataPath}{dash}{SAVES_FOLDER}{dash}";
	}

	public static void SaveData(int slot) {
		var data = FetchData();
		string filePath = saveDirectory + SAVE_NAME + slot.ToString() + SAVE_EXTENSION;

		if(!Directory.Exists(saveDirectory))
			Directory.CreateDirectory(saveDirectory);

		Debug.Log("About to write in: " + filePath);

		string serializedData = JsonUtility.ToJson(data);

		using(var writer = new StreamWriter(filePath))
		{
			writer.Write(serializedData);
			writer.Close();
		}
	}

	public static void LoadData(int slot) {
		string filePath = saveDirectory + SAVE_NAME + slot.ToString() + SAVE_EXTENSION;
		string json;
		using (var reader = new StreamReader(filePath))
		{
			json = reader.ReadToEnd();
			reader.Close();
		}
		var saveData = JsonUtility.FromJson<SaveData>(json);
		ApplyData(saveData);

	}

	private static SaveData FetchData() {
		var data = new SaveData();
		data.activeScene = SceneManager.GetActiveScene().name;
		data.checkId = LevelManager.Instance.SpawnId;
		data.upgradesSerialized = UpgradesManager.Instance.Data.GetSerialized();
		data.worldDataSerialized = WorldDataManager.Instance.WorldData.GetSerialized();
		return data;
	}

	private static void ApplyData(SaveData data) {
		UpgradesManager.Instance.ForceSetData(data.upgradesSerialized);
		WorldDataManager.Instance.ForceSetData(data.worldDataSerialized);
		LevelManager.Load(data.activeScene, data.checkId);
	}

}

[Serializable]
public struct SaveData {
	public string activeScene;
	public int checkId;
	public string upgradesSerialized;
	//public PlayerData player;
	public string worldDataSerialized;
}
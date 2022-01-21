using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour {
	public static LevelManager Instance { get; private set; }
	public int SpawnId { get; set; }

	public const string FETCHABLE_CP_PREFIX = "CP";

	private void Awake() {
		if(Instance)
		{
			Destroy(this);
			return;
		}
		Instance = this;
		SceneManager.sceneLoaded += OnLoad;
	}

	private void OnLoad(Scene scene, LoadSceneMode mode) {
		if(mode == LoadSceneMode.Single)
			RelocateTargetOnLoad();
	}

	private void RelocateTargetOnLoad() {
		var targetTrans = Fetchable.Fetch(FETCHABLE_CP_PREFIX + SpawnId)?.transform;
		if(!targetTrans)
			return;
		var player = Fetchable.FetchRaw("player").transform;
		player.position = targetTrans.position;
		player.rotation = targetTrans.rotation;
	}

	//First time using nullables btw
	public static void Load(int id, int? cp = null) {
		if(cp != null)
			Instance.SpawnId = cp.Value;
		SceneManager.LoadScene(id);
	}

	public static void Load(string levelName, int? cp = null) {
		if(cp != null)
			Instance.SpawnId = cp.Value;
		SceneManager.LoadScene(levelName);
	}

}

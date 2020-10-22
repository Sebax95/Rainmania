using UnityEngine;

public class UpgradesManager : MonoBehaviour {
	public static UpgradesManager Instance { get; private set; }
	public UpgradesData data;

	private void Awake() {
		if(Instance)
		{
			Destroy(gameObject);
			return;
		}
		Instance = this;
		DontDestroyOnLoad(this);
	}

	public string SerializeData() => JsonUtility.ToJson(data);
	public void SaveDataFromJson(string data) => JsonUtility.FromJsonOverwrite(data, this.data);
}

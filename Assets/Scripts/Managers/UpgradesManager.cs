using System;
using UnityEngine;

public class UpgradesManager : MonoBehaviour {
	public static UpgradesManager Instance { get; private set; }
	[SerializeField]private UpgradesData data;
	public event Action<UpgradesData> OnUpdateData;
	public UpgradesData Data => data;

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

	public void IncrementIntProperty(string propertyName, int amount) {
		data.IncrementIntProperty(propertyName, amount);
		OnUpdateData?.Invoke(data);
	}
	public void DecrementFloatProperty(string propertyName, float amount) {
		data.DecrementFloatProperty(propertyName, amount);
		OnUpdateData?.Invoke(data);
	}
	public void MultiplyFloatProperty(string propertyName, float mult) {
		data.MultiplyFloatProperty(propertyName, mult);
		OnUpdateData?.Invoke(data);
	}
	public void EnableBoolProperty(string propertyName) {
		data.EnableBoolProperty(propertyName);
		OnUpdateData?.Invoke(data);
	}

	private void OnDestroy() => OnUpdateData = null;
}

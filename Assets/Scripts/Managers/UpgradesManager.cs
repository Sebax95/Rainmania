using System;
using System.Collections.Generic;
using UnityEngine;

public class UpgradesManager : TimedBehaviour {
	public static UpgradesManager Instance { get; private set; }
	[SerializeField]private UpgradeData dataTemplate;
	private UpgradeData data;
	public event Action<UpgradeData> OnUpdateData;
	public UpgradeData Data => data;

	private HashSet<string> acquiredUpgrades = new HashSet<string>();

	private void Awake() {
		if(Instance)
		{
			Destroy(gameObject);
			return;
		}
		Instance = this;
		data = Instantiate(dataTemplate);
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

	public void ConsumeUpgrade(string upgradeId) => acquiredUpgrades.Add(upgradeId);
	public bool HasUpgradeBeenConsumed(string upgradeId) => acquiredUpgrades.Contains(upgradeId);

	public void ForceSetData(UpgradeData data) {
		this.data = data;
		//OnUpdateData?.Invoke(Data);
	}

	public void ForceSetData(string serializedData) => this.data = GenericDataPack.CreateFromJson<UpgradeData>(serializedData);

	private void OnDestroy() => OnUpdateData = null;
}

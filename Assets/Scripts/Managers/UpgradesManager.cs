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

	public void IncrementIntProperty(ref int property, int amount) => data.IncrementIntProperty(ref property, amount);
	public void DecrementFloatProperty(ref float property, float amount) => data.DecrementFloatProperty(ref property, amount);
	public void MultiplyFloatProperty(ref float property, float mult) => data.MultiplyFloatProperty(ref property, mult);
	public void EnableBoolProperty(ref bool property) => data.EnableBoolProperty(ref property);

	public string SerializeData() => JsonUtility.ToJson(data);
	public void SaveDataFromJson(string data) => JsonUtility.FromJsonOverwrite(data, this.data);
}

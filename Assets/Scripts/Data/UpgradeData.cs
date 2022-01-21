using System;
using UnityEngine;

[Serializable]
[CreateAssetMenu(fileName = "Upgrade Data Preset", menuName = "Upgrade Data Preset", order = 4)]
public class UpgradeData : GenericDataPack {
	public void IncrementIntProperty(string propertyName, int amount) => intProperties[GetIntIndex(propertyName)].value += amount;
	public void DecrementFloatProperty(string propertyName, float amount) => floatProperties[GetFloatIndex(propertyName)].value -= amount;
	public void MultiplyFloatProperty(string propertyName, float mult) => floatProperties[GetFloatIndex(propertyName)].value *= mult;
	public void EnableBoolProperty(string propertyName) => boolProperties[GetBoolIndex(propertyName)].value = true;
}


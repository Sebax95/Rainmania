using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
[CreateAssetMenu(fileName = "Upgrade Data Preset", menuName = "Upgrade Data Preset", order = 4)]
public class UpgradesData : ScriptableObject {

	public NameIntPair[] intProperties;
	public NameFloatPair[] floatProperties;
	public NameBoolPair[] boolProperties;
	
	public void IncrementIntProperty(string propertyName, int amount) => intProperties[GetIntIndex(propertyName)].value  += amount;
	public void DecrementFloatProperty(string propertyName, float amount) => floatProperties[GetFloatIndex(propertyName)].value -= amount;
	public void MultiplyFloatProperty(string propertyName, float mult) => floatProperties[GetFloatIndex(propertyName)].value *= mult;
	public void EnableBoolProperty(string propertyName) => boolProperties[GetBoolIndex(propertyName)].value = true;

	public int GetInt(string name) => intProperties[GetIntIndex(name)].value;
	public float GetFloat(string name) => floatProperties[GetFloatIndex(name)].value;
	public bool GetBool(string name) => boolProperties[GetBoolIndex(name)].value;

	#region Seeker Funcs
	private int GetIntIndex(string name) {
		int index = -1;
		int size = intProperties.Length;
		for(int i = 0; i < size; i++)
		{
			if(intProperties[i].name == name)
			{
				index = i;
				return index;
			}
		}
		throw new KeyNotFoundException("Given property name does not exist.");
	}
	private int GetFloatIndex(string name) {
		int index = -1;
		int size = floatProperties.Length;
		for(int i = 0; i < size; i++)
		{
			if(floatProperties[i].name == name)
			{
				index = i;
				return index;
			}
		}
		throw new KeyNotFoundException("Given property name does not exist.");
	}
	private int GetBoolIndex(string name) {
		int index = -1;
		int size = boolProperties.Length;
		for(int i = 0; i < size; i++)
		{
			if(boolProperties[i].name == name)
			{
				index = i;
				return index;
			}
		}
		throw new KeyNotFoundException("Given property name does not exist.");
	}
	#endregion
}

//Por que no tuplas? Porque no se pueden serializar
[Serializable]
public class NameIntPair {
	public string name;
	public int value;
}
[Serializable]
public class NameFloatPair {
	public string name;
	public float value;
}
[Serializable]
public class NameBoolPair {
	public string name;
	public bool value;
}


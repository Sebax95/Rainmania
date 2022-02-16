using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class GenericDataPack : ScriptableObject {

	public List<NameIntPair> intProperties = new List<NameIntPair>();
	public List<NameFloatPair> floatProperties = new List<NameFloatPair>();
	public List<NameBoolPair> boolProperties = new List<NameBoolPair>();

	public int GetInt(string name) => intProperties[GetIntIndex(name)].value;
	public float GetFloat(string name) => floatProperties[GetFloatIndex(name)].value;
	public bool GetBool(string name) => boolProperties[GetBoolIndex(name)].value;

	#region Setters
	public void SetInt(string name, int value) {
		int index = GetIntIndex(name);
		if(index < 0)
			intProperties.Add(new NameIntPair(name, value));
		else
			intProperties[index].value = value;
	}
	public void SetFloat(string name, float value) {
		int index = GetFloatIndex(name);
		if(index < 0)
			floatProperties.Add(new NameFloatPair(name, value));
		else
			floatProperties[index].value = value;
	}
	public void SetBool(string name, bool value) {
		int index = GetFloatIndex(name);
		if(index < 0)
			boolProperties.Add(new NameBoolPair(name, value));
		else
			boolProperties[index].value = value;
	}
#endregion

	#region TryGet Funcs
	public bool TryGetInt(string name, out int value) {
		int index = GetIntIndex(name);
		value = default;

		if(index < 0)
			return false;

		value = intProperties[index].value;
		return true;
	}
	public bool TryGetFloat(string name, out float value) {
		int index = GetFloatIndex(name);
		value = default;

		if(index < 0)
			return false;

		value = floatProperties[index].value;
		return true;
	}
	public bool TryGetBool(string name, out bool value) {
		int index = GetBoolIndex(name);
		value = default;

		if(index < 0)
			return false;

		value = boolProperties[index].value;
		return true;
	}
	#endregion

	#region Seeker Funcs
	protected int GetIntIndex(string name) {
		int index = -1;
		int size = intProperties.Count;
		for(int i = 0; i < size; i++)
		{
			if(intProperties[i].name == name)
			{
				index = i;
				return index;
			}
		}
		//throw new KeyNotFoundException($"Given property name \"{name}\" does not exist.");
		return index;
	}
	protected int GetFloatIndex(string name) {
		int index = -1;
		int size = floatProperties.Count;
		for(int i = 0; i < size; i++)
		{
			if(floatProperties[i].name == name)
			{
				index = i;
				return index;
			}
		}
		return index;
	}
	protected int GetBoolIndex(string name) {
		int index = -1;
		int size = boolProperties.Count;
		for(int i = 0; i < size; i++)
		{
			if(boolProperties[i].name == name)
			{
				index = i;
				return index;
			}
		}
		return index;
	}
	#endregion


	public string GetSerialized() => JsonUtility.ToJson(this);
	public static T CreateFromJson<T>(string json) where T : GenericDataPack {
		var obj = CreateInstance<T>();
		JsonUtility.FromJsonOverwrite(json, obj);
		return obj;
	}
}

//Por que no tuplas? Porque no se pueden serializar
[Serializable]
public class NameIntPair {
	public NameIntPair(string name, int value) { this.name = name; this.value = value; }

	public string name;
	public int value;
}
[Serializable]
public class NameFloatPair {
	public NameFloatPair(string name, float value) { this.name = name; this.value = value; }

	public string name;
	public float value;
}
[Serializable]
public class NameBoolPair {
	public NameBoolPair(string name, bool value) { this.name = name; this.value = value; }

	public string name;
	public bool value;
}

public enum DataType { Bool, Int, Float, String}
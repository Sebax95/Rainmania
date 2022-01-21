using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldDataManager : MonoBehaviour {

	public static WorldDataManager Instance { get; private set; }
	public GenericDataPack WorldData { get; private set; }

	private void Awake() {
		if(Instance)
		{
			Destroy(gameObject);
			return;
		}
		Instance = this;
		DontDestroyOnLoad(this);
		WorldData = new GenericDataPack();
	}


	public bool TryLoadElseWriteDefault(string name, ref int defaultOrRetrunedValue) {
		bool exists = WorldData.TryGetInt(name, out int returnedValue);
		if(exists)
			defaultOrRetrunedValue = returnedValue;
		else
			WorldData.SetInt(name, defaultOrRetrunedValue);

		return exists;
	}

	public bool TryLoadElseWriteDefault(string name, ref float defaultOrRetrunedValue) {
		bool exists = WorldData.TryGetFloat(name, out float returnedValue);
		if(exists)
			defaultOrRetrunedValue = returnedValue;
		else
			WorldData.SetFloat(name, defaultOrRetrunedValue);

		return exists;
	}

	public bool TryLoadElseWriteDefault(string name, ref bool defaultOrRetrunedValue) {
		bool exists = WorldData.TryGetBool(name, out bool returnedValue);
		if(exists)
			defaultOrRetrunedValue = returnedValue;
		else
			WorldData.SetBool(name, defaultOrRetrunedValue);

		return exists;
	}

	public void ForceWrite(string name, int value) => 
		WorldData.SetInt(name, value);

	public void ForceWrite(string name, float value) => 
		WorldData.SetFloat(name, value);

	public void ForceWrite(string name, bool value) => 
		WorldData.SetBool(name, value);

	public void ForceSetData(GenericDataPack data) => WorldData = data;
	public void ForceSetData(string serializedData) => WorldData = GenericDataPack.CreateFromJson<GenericDataPack> (serializedData);

}

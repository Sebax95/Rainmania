using System.Collections.Generic;
using UnityEngine;

public class Fetchable : MonoBehaviour {
	new public string name;
	private static Dictionary<string, Fetchable> dictionary;

	private void OnEnable() {
		if(dictionary == null)
			dictionary = new Dictionary<string, Fetchable>();
		if(dictionary.ContainsKey(name))
			Debug.LogWarning($"There already exists a fetchable item with name {name}. Attempting to fetch this item will yield unreliable results.");

		dictionary[name] = this;
	}

	private void OnDisable() {
		if(dictionary.ContainsKey(name) && dictionary[name] == this)
			dictionary.Remove(name);
	}

	public static GameObject Fetch(string name) {
		if(dictionary.ContainsKey(name))
			return dictionary[name].gameObject;
		return null;
	}

	public static Fetchable FetchRaw(string name) {
		if(dictionary.ContainsKey(name))
			return dictionary[name];
		return null;
	}

	public static T FetchComponent<T>(string name) where T : Component {
		if(dictionary.ContainsKey(name))
			return dictionary[name].GetComponent<T>();
		return null;
	}
}

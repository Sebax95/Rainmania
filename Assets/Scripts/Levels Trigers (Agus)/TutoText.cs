using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutoText : MonoBehaviour {
	public GameObject _text;

	void Start() {

		_text.SetActive(false);

	}

	void Update() {

	}


	private void OnTriggerEnter(Collider other) {
		if(other.CompareTag("Player"))
		{
			_text.SetActive(true);
		}
	}

	private void OnTriggerExit(Collider other) {
		if(other.CompareTag("Player"))
		{
			_text.SetActive(false);
		}
	}
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SavePoint : TimedBehaviour {
	private bool _canUse= false;
	protected override void OnUpdate() {
		base.OnUpdate();
		if(_canUse && Input.GetKeyDown(KeyCode.E))
			SaveScreen();
	}

	private void OnTriggerEnter(Collider other) {
		if(other.gameObject.CompareTag("Player"))
			_canUse = true;
	}

	private void OnTriggerExit(Collider other) {
		if(other.gameObject.CompareTag("Player"))
			_canUse = false;
	}

	private void SaveScreen() {
		SaveMenu.Instance.gameObject.SetActive(true);
	}
}

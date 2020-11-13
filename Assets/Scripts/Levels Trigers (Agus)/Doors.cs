using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Doors : TimedBehaviour {
	public Transform _otherDoor;
	public GameObject door;
	public Transform newPosition;
	Player player;
	Camera _camera;
	bool _canUse;
	public int _lock;
	int _key = 0;

	private void Awake() {
		player = FindObjectOfType<Player>();
		_camera = FindObjectOfType<Camera>();
		EventsManager.SubscribeToEvent(EventsAgus.GP_KeyTipe, KeyType);
	}

	protected override void OnUpdate() {
		if(_canUse)
			if(Input.GetKeyDown(KeyCode.E) && _key >= _lock)
			{
				player.transform.position = _otherDoor.position;
				_camera.transform.position = new Vector3(_otherDoor.transform.position.x, _otherDoor.transform.position.y, _otherDoor.transform.position.z - 6);
			}
	}

	void KeyType(params object[] parameters) {
		_key = (int)parameters[0];
		if(door != null && newPosition != null)
			door.transform.position = newPosition.transform.position;
	}

	private void OnTriggerEnter(Collider other) {
		if(other.gameObject.CompareTag("Player"))
			_canUse = true;
	}

	private void OnTriggerExit(Collider other) {
		if(other.gameObject.CompareTag("Player"))
			_canUse = false;
	}

}

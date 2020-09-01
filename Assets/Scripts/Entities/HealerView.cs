using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealerView : MonoBehaviour {

	public float spinSpeed;

	private Transform child;
	private Vector3 axis;

	public bool Active { get; set; }


	private void Start() {
		child = transform.GetChild(0);
		axis = transform.up;
		Active = true;
	}

	private void Update() {
		if(!Active)
			return;
		child.Rotate(axis, spinSpeed * Time.deltaTime);
	}

	public void Pickup() {
		Active = false;

	}

	public void Respawn() {
		Active = true;

	}
}

﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Swinger))]
public class Test_Swinger : MonoBehaviour {
	public Transform anchor;
	private Swinger swinger;
	private Rigidbody thisRB;
	public Rigidbody throwPrefab;

	// Start is called before the first frame update
	void Start() {
		swinger = GetComponent<Swinger>();
		thisRB = GetComponent<Rigidbody>();
		swinger.SetupSwing(anchor);
	}

	// Update is called once per frame
	void FixedUpdate() {
		thisRB.position = swinger.UpdateSwing();
	}

	private void Update() {
		if(Input.GetKeyDown(KeyCode.Space))
		{
			var rb = Instantiate(throwPrefab, transform.position + Vector3.forward * -2,Quaternion.identity);
			rb.velocity = swinger.GetVelocity();
		}
	}
}

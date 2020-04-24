﻿using UnityEngine;
using CustomMSLibrary.Unity;

public class Swinger : Controllable, IMoveOverride {
	public float gravityMult;

	private float anchorTime;
	public float whipDistance;

	private bool dependOnTransform;
	private Transform anchorTransform;
	private Vector3 anchorPos;

	private float initialState; //Initial angle in rads * Sqrt(gravity / distance from anchor)
	private float initialAngle;

	private bool overriding;
	private IMoveOverrideable swingOverrider;


	private Rigidbody thisRB;
	private MomentumKeeper momentum;
	private bool prevGravitystate;

	float GetTime => Time.time - anchorTime;

	private void Awake() {
		thisRB = GetComponent<Rigidbody>();
		momentum = GetComponent<MomentumKeeper>();
		swingOverrider = GetComponent<IMoveOverrideable>();

		ControllerHandler.Instance.RequestAssignation(Controller.Create<PlayerSwingerController>(), this);
	}

	public void SetupSwing(Transform anchor) {
		anchorTransform = anchor;
		dependOnTransform = true;
		Vector3 relativePos = transform.RelativePosTo(anchor);
		Internal_SetupSwing(relativePos);
	}

	public void SetupSwing(Vector3 anchor) {
		anchorPos = anchor;
		anchorTransform = null;
		dependOnTransform = false;
		Vector3 relativePos = anchor - transform.position;
		Internal_SetupSwing(relativePos);
	}

	private void Internal_SetupSwing(Vector3 relativePos) {
		initialAngle = Vector3.SignedAngle(Vector3.down, -relativePos.ZeroZ(), Vector3.forward) * Mathf.Deg2Rad;
		//distanceFromAnchor = relativePos.magnitude;
		anchorTime = Time.time;
		initialState = Mathf.Sqrt(gravityMult / whipDistance);
	}

	private void FixedUpdate() {
		if(!overriding)
			return;
		thisRB.position = UpdateSwing();
	}

	public Vector3 UpdateSwing() {
		float newAngle = initialAngle * Mathf.Cos(initialState * GetTime);
		var anchorPos = dependOnTransform ? anchorTransform.position : this.anchorPos;
		return new Vector3(
			anchorPos.x + (Mathf.Sin(newAngle) * whipDistance),
			anchorPos.y - (Mathf.Cos(newAngle) * whipDistance),
			transform.position.z);
	}

	public Vector3 GetVelocity() {
		float velocity = initialAngle * Mathf.Sqrt(whipDistance * gravityMult) * Mathf.Sin(initialState * GetTime);
		float newAngle = initialAngle * Mathf.Cos(initialState * GetTime);

		Vector3 addedVelocity = Vector3.zero;
		if(dependOnTransform)
		{
			var rb = anchorTransform?.GetComponent<Rigidbody>();
			if(rb != null)
				addedVelocity = rb.velocity.ZeroZ();
		}
		return new Vector3(
			velocity * -Mathf.Cos(newAngle),
			velocity * -Mathf.Sin(newAngle),
			0f) + addedVelocity;
	}

	public void BreakSwing() {
		Release(swingOverrider);
	}

	public void Attach(IMoveOverrideable user) {
		user.Attach(this);

		var player = user as Controllable;
		if(player!= null)
			ControllerHandler.Instance.OverrideWithUser(this,player);

		overriding = true;
		prevGravitystate = thisRB.useGravity;
		thisRB.useGravity = false;
		thisRB.isKinematic = true;
	}

	public void Release(IMoveOverrideable user) {
		var velocity = GetVelocity();
		user.Release(this);

		var player = user as Controllable;
		if(player != null)
			ControllerHandler.Instance.UndoOverrideWithUser(this, player, true);

		overriding = false;
		thisRB.useGravity = prevGravitystate;
		thisRB.velocity = velocity;
		momentum.velocity = velocity;
		thisRB.isKinematic = false;
	}

	public void StartSwing() {
		Attach(swingOverrider);
	}

	public override void Move(Vector2 direction) {
		if(direction.x == 0)
			return;
		if(direction.x > 0)
			transform.localRotation = Quaternion.Euler(0, 90,0);
		else
			transform.localRotation = Quaternion.Euler(0, -90,0);
	}
}
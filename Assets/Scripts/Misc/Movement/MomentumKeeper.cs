using UnityEngine;
using System;

public class MomentumKeeper : TimedBehaviour {
	public float decayRate;
	[NonSerialized]
	public Vector3 velocity;

	protected override void OnFixedUpdate() {
		if(velocity == Vector3.zero)
			return;
		velocity -= velocity.normalized * decayRate * Time.fixedDeltaTime;
	}
}
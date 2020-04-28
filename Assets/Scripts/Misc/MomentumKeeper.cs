using UnityEngine;
using System;

public class MomentumKeeper : MonoBehaviour {
	public float decayRate;
	[NonSerialized]
	public Vector3 velocity;

	private void FixedUpdate() {
		if(velocity == Vector3.zero)
			return;
		velocity -= velocity.normalized * decayRate * Time.fixedDeltaTime;
	}
}
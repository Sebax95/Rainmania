using UnityEngine;
using CustomMSLibrary.Unity;

public class Swinger : MonoBehaviour {
	public float gravityMult;

	private float anchorTime;
	private float distanceFromAnchor;

	private bool dependOnTransform;
	private Transform anchorTransform;
	private Vector3 anchorPos;

	private float initialState; //Initial angle in rads * Sqrt(gravity / distance from anchor)

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
		float initialAngle = Vector3.SignedAngle(Vector3.down, -relativePos.ZeroZ(), Vector3.forward) * Mathf.Deg2Rad;
		distanceFromAnchor = relativePos.magnitude;
		anchorTime = Time.time;
		initialState = initialAngle * Mathf.Sqrt(gravityMult / distanceFromAnchor);
	}

	public Vector3 UpdateSwing() {
		float newAngle = initialState * Mathf.Cos(Time.time - anchorTime);
		var anchorPos = dependOnTransform ? anchorTransform.position : this.anchorPos;
		return new Vector3(
			anchorPos.x + (Mathf.Sin(newAngle) * distanceFromAnchor),
			anchorPos.y - (Mathf.Cos(newAngle) * distanceFromAnchor),
			transform.position.z);
	}
}

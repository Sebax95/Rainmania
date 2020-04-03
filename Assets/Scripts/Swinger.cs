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
	private float initialAngle;

	float GetTime => Time.time - anchorTime;

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
		distanceFromAnchor = relativePos.magnitude;
		anchorTime = Time.time;
		initialState = Mathf.Sqrt(gravityMult / distanceFromAnchor);
	}

	public Vector3 UpdateSwing() {
		float newAngle = initialAngle * Mathf.Cos(initialState * GetTime);
		var anchorPos = dependOnTransform ? anchorTransform.position : this.anchorPos;
		return new Vector3(
			anchorPos.x + (Mathf.Sin(newAngle) * distanceFromAnchor),
			anchorPos.y - (Mathf.Cos(newAngle) * distanceFromAnchor),
			transform.position.z);
	}

	public Vector3 GetVelocity() {
		float velocity = initialAngle * Mathf.Sqrt(distanceFromAnchor * gravityMult) * Mathf.Sin(initialState * GetTime);
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

	//private void OnDrawGizmos() {
	//	Gizmos.color = Color.green;
	//	Gizmos.DrawLine(transform.position, transform.position + GetVelocity());
	//}
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollapsablePlatform : TimedBehaviour {
	public float stableTime;
	public float respawnDelay;
	public float forgiveTime;
	public float shakeMaxDistance;
	public Vector3 maxFallBlockRotation;
	public Vector3 maxAddedFallVelocity;

	private bool isShaking;
	private bool isCollapsed;
	private Collider actualCollider;
	private (Transform transform, Vector3 originalLocalPos, Quaternion originalRot, Rigidbody rb)[] collapsables;

	private const string PLAYER_TAG = "Player";

	private void Start() {
		actualCollider = GetComponent<Collider>();
		int c = transform.childCount;
		collapsables = new (Transform transform, Vector3 originalLocalPos, Quaternion originalRot, Rigidbody rb)[c];
		for(int i = 0; i < c; i++)
		{
			var child = transform.GetChild(i);
			var current = collapsables[i];
			current.transform = child;
			current.originalLocalPos = child.localPosition;
			current.originalRot = child.localRotation;
			current.rb = child.GetComponent<Rigidbody>();
			collapsables[i] = current;
		}
		ResetState();
	}

	protected override void OnLateUpdate() { 
		if(isCollapsed || !isShaking)
			return;
		int c = collapsables.Length;
		for(int i = 0; i < c; i++)
		{
			var current = collapsables[i];
			current.transform.localPosition = current.originalLocalPos + GetPosOffset(shakeMaxDistance);
		}
	}

	private void OnTriggerEnter(Collider other) {
		if(isShaking || isCollapsed || !other.gameObject.CompareTag(PLAYER_TAG))
			return;
		StartCoroutine(Coroutine_CollapseProcess());
	}

	private IEnumerator Coroutine_CollapseProcess() {
		isShaking = true;
		yield return new WaitForSeconds(stableTime);
		isShaking = false;
		CollapsePlatform();
		yield return new WaitForSeconds(forgiveTime);
		actualCollider.enabled = false;
		isCollapsed = true;
		if(respawnDelay < 0)
		{
			Destroy(gameObject, 0.1f);
			yield break;
		}
		StartCoroutine(Coroutine_Respawn());
	}

	private void CollapsePlatform() {
		int c = collapsables.Length;
		for(int i = 0; i < c; i++)
		{
			var current = collapsables[i];
			current.rb.isKinematic = false;
			current.rb.angularVelocity = GetRotation(maxFallBlockRotation);
			current.rb.velocity = GetAddedVelocity(maxAddedFallVelocity);
		}
	}

	private IEnumerator Coroutine_Respawn() {
		yield return new WaitForSeconds(respawnDelay);
		ResetState();
	}

	private void ResetState() {
		actualCollider.enabled = true;
		isCollapsed = false;
		int c = collapsables.Length;
		for(int i = 0; i < c; i++)
		{
			var current = collapsables[i];
			current.transform.localPosition = current.originalLocalPos;
			current.transform.localRotation = current.originalRot;

			current.rb.isKinematic = true;
			current.rb.velocity = Vector3.zero;
			current.rb.angularVelocity = Vector3.zero;
		}
	}

	private static Vector3 GetPosOffset(float maxDelta) =>
		new Vector3(RandomFloat(maxDelta), RandomFloat(maxDelta), RandomFloat(maxDelta));

	private static Vector3 GetRotation(Vector3 maxAngles) =>
		new Vector3(RandomFloat(maxAngles.x), RandomFloat(maxAngles.y), RandomFloat(maxAngles.z));

	private static Vector3 GetAddedVelocity(Vector3 maxVel) {
		return new Vector3(RandomFloat(maxVel.x), RandValueFloat(-maxVel.y), RandomFloat(maxVel.z));

		float RandValueFloat(float value) => Random.value * value;
	}

	private static float RandomFloat(float max) =>
		Random.Range(-1f, 1f) * max;
}

using System.Collections;
using UnityEngine;
using System;

#if UNITY_EDITOR
using UnityEditor;
#endif

class HurtAndRespawn : MonoBehaviour, IDamager {

	public int damage;
	public float respawnDelay;
	public Vector3 relativeRespawnOffset;
	public Team team;

	private void OnTriggerEnter(Collider other) {
		var dmg = other.GetComponent<IDamageable>();
		if(dmg == null)
			return;

		dmg.Damage(damage, this);
		StartCoroutine(DelayedRelocate(other.gameObject));
	}

	public GameObject SourceObject => gameObject;

	public Team GetTeam => team;

	private IEnumerator DelayedRelocate(GameObject victim) {
		if(!victim)
			yield break;
		victim.SetActive(false);
		yield return new WaitForSeconds(respawnDelay);
		if(!victim)
			yield break;
		victim.transform.position = transform.position + relativeRespawnOffset;
		victim.SetActive(true);
	}

#if UNITY_EDITOR
	private BoxCollider collidr;
	private void OnDrawGizmos() {
		if(!collidr)
			collidr = GetComponent<BoxCollider>();
		Gizmos.color = Color.red;
		Gizmos.DrawWireCube(transform.position + collidr.center, collidr.size);
	}

	private void OnDrawGizmosSelected() {
		Gizmos.color = Color.cyan;
		Vector3 pos = transform.position;
		Vector3 relativePos = pos + relativeRespawnOffset;
		Gizmos.DrawSphere(pos + relativeRespawnOffset, 0.2f);
	}
#endif
}

#if UNITY_EDITOR
[CustomEditor(typeof(HurtAndRespawn))]
public class HurtAndRespawnEditor : Editor {
	HurtAndRespawn trueTarget;
	private int lastUndoGroup;
	private void OnEnable() {
		trueTarget = (HurtAndRespawn)target;
		lastUndoGroup = Undo.GetCurrentGroup();
	}

	private void OnSceneGUI() {
		Vector3 pos = trueTarget.transform.position;
		Vector3 relPos = trueTarget.relativeRespawnOffset;
		var tempPos = Handles.PositionHandle(pos + relPos, Quaternion.identity) - pos;
		if(tempPos != relPos)
		{
			if(Undo.GetCurrentGroup() != lastUndoGroup)
			{
				Undo.IncrementCurrentGroup();
				lastUndoGroup = Undo.GetCurrentGroup();
			}
			Undo.RecordObject(trueTarget, "Change respawner location");
			trueTarget.relativeRespawnOffset = tempPos;
			Undo.CollapseUndoOperations(lastUndoGroup);
		}
	}
}
#endif
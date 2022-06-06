using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using CustomMSLibrary.Core;

public class Whip : Weapon {

	[FormerlySerializedAs("whipHitboxes")]
	public BoxCastParams[] grabHitboxes;
	private Swinger swinger;
	private CrouchStateRouter router;

	public float speed;

	private IWielder wielder;

	public float attackDelay;
	public float attackDuration;
	public float attackCooldown;
	public override float FullAttackDuration => attackDelay + attackDuration + attackCooldown;

	private Collider[] boxcastCache = new Collider[10];
	private bool canAttach = true;

	public override GameObject SourceObject => gameObject;
	public override Team GetTeam => wielder.GetTeam;

	public const string NAME = "Whip";
	public override string Name => NAME;



	private void Awake() {
		swinger = GetComponent<Swinger>();
		wielder = GetComponent<IWielder>();
		router = GetComponent<CrouchStateRouter>();
	}

	private void Start() {
		UpdateStateOnUpgrade(UpgradesManager.Instance.Data);
	}

	/// <summary>
	/// Return true if it should break the attack loop early on grab. Otherwise false.
	/// </summary>
	/// <returns></returns>
	public bool WhipAttack(TargetDirection direction) {

		DetectHits(router.Current.damageHitbox[(int)direction]);
		DamageInColliderBuffer();
		bool grabResult = false;
		if(canAttach)
		{
			DetectHits(router.Current.grabHitbox[(int)direction]);
			grabResult = TryAttachInColliderBuffer();
		}
		return grabResult;
	}

	private void DamageInColliderBuffer() {
		foreach(var item in boxcastCache)
		{
			if(item == null)
				continue;

			if(item.gameObject == gameObject)
				continue;

			var dmg = item.GetComponent<IDamageable>();
			if(dmg != null)
				dmg.Damage(damage, this);
		}
	}

	private bool TryAttachInColliderBuffer() {

		foreach(var col in boxcastCache)
		{
			if(col == null)
				continue;

			var anchor = col.GetComponent<SwingAnchor>();
			if(anchor == null || !anchor.enabled)
				continue;

			anchor.AttachSwinger(swinger);

			swinger.StartSwing();
			return true;
		}
		return false;
	}

	private void DetectHits(BoxCastParams hitbox) {

		//var hitbox = whipHitboxes[(int)direction];

		var pos = transform.position;
		var rot = transform.rotation;

		int c = boxcastCache.Length;
		for(int i = 0; i < c; i++)
			boxcastCache[i] = null;

		c = Physics.OverlapBoxNonAlloc(pos + rot * hitbox.CenterOffset, hitbox.halfExtends,
			boxcastCache, hitbox.GetAdjustedOrientation(rot), Physics.AllLayers, QueryTriggerInteraction.Collide);

	}

	public override void Attack(Vector2 direction) {
		#region UnDeprecated
		bool vertical = direction.y > 0;
		bool horizontal = direction.x != 0;
		TargetDirection directionIndex;

		if(vertical)
			if(horizontal)
				directionIndex = TargetDirection.Diagonal;
			else
				directionIndex = TargetDirection.Vertical;
		else
			directionIndex = TargetDirection.Horizontal;

		StartCoroutine(Coroutine_RepeatAttack(directionIndex));
		#endregion
		//StartCoroutine(Coroutine_RepeatAttack(0));
	}

	private IEnumerator Coroutine_RepeatAttack(TargetDirection direction) {
		float timer = 0;
		while(timer < attackDuration)
		{
			if(!RunUpdates)
				yield return null;

			timer += Time.deltaTime;
			bool breakLoop = WhipAttack(direction);
			if(breakLoop)
				break;
			yield return null;
		}
	}


	private IEnumerator Coroutine_DelayedObjectActiveBlinker(GameObject item, float firstDuration, float secondDuration) {
		yield return new WaitForSeconds(firstDuration);
		item.SetActive(true);
		yield return new WaitForSeconds(secondDuration);
		item.SetActive(false);
	}

	private void UpdateStateOnUpgrade(GenericDataPack data) {
		attackCooldown = data.GetFloat("whipAttackSpeed");
		damage = data.GetInt("whipDamage");
		canAttach = data.GetBool("whipCanGrapple");
	}

	private void OnEnable() => UpgradesManager.Instance.OnUpdateData += UpdateStateOnUpgrade;
	private void OnDisable() => UpgradesManager.Instance.OnUpdateData -= UpdateStateOnUpgrade;

#if UNITY_EDITOR
	Vector3[] points = new Vector3[8];
	private void OnDrawGizmosSelected() {
		Quaternion rotation = transform.rotation;
		Vector3 position = transform.position;
		if(!router)
			router = GetComponent<CrouchStateRouter>();
		Gizmos.color = Color.yellow;
		foreach(var item in router.standing.grabHitbox)
		{
			DrawBox(item);
		}
		Gizmos.color = Color.red;
		foreach(var item in router.standing.damageHitbox)
		{
			DrawBox(item);
		}
		Gizmos.color = Color.white;
		foreach(var item in router.crouched.grabHitbox)
		{
			DrawBox(item);
		}
		Gizmos.color = Color.grey;
		foreach(var item in router.crouched.damageHitbox)
		{
			DrawBox(item);
		}

		void DrawBox(BoxCastParams item) {
			Vector3 centre = position + rotation * item.CenterOffset;
			#region points
			points[0] = centre + item.GetAdjustedOrientation(rotation) * new Vector3(item.halfExtends.x, item.halfExtends.y, item.halfExtends.z);
			points[1] = centre + item.GetAdjustedOrientation(rotation) * new Vector3(item.halfExtends.x, item.halfExtends.y, -item.halfExtends.z);
			points[2] = centre + item.GetAdjustedOrientation(rotation) * new Vector3(item.halfExtends.x, -item.halfExtends.y, item.halfExtends.z);
			points[3] = centre + item.GetAdjustedOrientation(rotation) * new Vector3(item.halfExtends.x, -item.halfExtends.y, -item.halfExtends.z);
			points[4] = centre + item.GetAdjustedOrientation(rotation) * new Vector3(-item.halfExtends.x, item.halfExtends.y, item.halfExtends.z);
			points[5] = centre + item.GetAdjustedOrientation(rotation) * new Vector3(-item.halfExtends.x, item.halfExtends.y, -item.halfExtends.z);
			points[6] = centre + item.GetAdjustedOrientation(rotation) * new Vector3(-item.halfExtends.x, -item.halfExtends.y, item.halfExtends.z);
			points[7] = centre + item.GetAdjustedOrientation(rotation) * new Vector3(-item.halfExtends.x, -item.halfExtends.y, -item.halfExtends.z);
			#endregion

			for(byte i = 0; i < 7; i++)
				for(byte j = 1; j < 8; j++)
					if((i.GetBit(0).XNOR(j.GetBit(0)) ^ i.GetBit(1).XNOR(j.GetBit(1)) ^ !i.GetBit(2).XNOR(j.GetBit(2))) && !(i + j == 7))
						Gizmos.DrawLine(points[i], points[j]);
		}
	}
#endif

}

[Serializable]
public struct BoxCastParams {
	public Vector3 closePoint;
	public Vector3 halfExtends;

	public Vector3 orientation;

	public Vector3 CenterOffset => Quaternion.Euler(orientation) * new Vector3(closePoint.x, closePoint.y, (closePoint.z + halfExtends.z));
	public Quaternion GetAdjustedOrientation(Quaternion parentRotation) => parentRotation * Quaternion.Euler(orientation);

	public int PerformBoxcastNonAlloc(
		Vector3 position,
		Quaternion parentRotation,
		Collider[] colliderCache,
		int mask = -1,
		QueryTriggerInteraction triggerInteratcions = QueryTriggerInteraction.Collide) {

		return Physics.OverlapBoxNonAlloc(position + parentRotation * CenterOffset, halfExtends,
			colliderCache, GetAdjustedOrientation(parentRotation), Physics.AllLayers, QueryTriggerInteraction.Collide);
	}

	public Collider[] PerformBoxcast(
		Vector3 position,
		Quaternion parentRotation,
		int mask = -1,
		QueryTriggerInteraction triggerInteratcions = QueryTriggerInteraction.Collide) {

		return Physics.OverlapBox(position + parentRotation * CenterOffset, halfExtends,
			GetAdjustedOrientation(parentRotation), Physics.AllLayers, QueryTriggerInteraction.Collide);
	}

#if UNITY_EDITOR
	public static Vector3[] points;
	public void DrawGizmo(Vector3 position, Quaternion rotation) {
		if(points == null)
			points = new Vector3[8];

		Vector3 centre = position + rotation * CenterOffset;
		#region points
		points[0] = centre + GetAdjustedOrientation(rotation) * new Vector3(halfExtends.x, halfExtends.y, halfExtends.z);
		points[1] = centre + GetAdjustedOrientation(rotation) * new Vector3(halfExtends.x, halfExtends.y, -halfExtends.z);
		points[2] = centre + GetAdjustedOrientation(rotation) * new Vector3(halfExtends.x, -halfExtends.y, halfExtends.z);
		points[3] = centre + GetAdjustedOrientation(rotation) * new Vector3(halfExtends.x, -halfExtends.y, -halfExtends.z);
		points[4] = centre + GetAdjustedOrientation(rotation) * new Vector3(-halfExtends.x, halfExtends.y, halfExtends.z);
		points[5] = centre + GetAdjustedOrientation(rotation) * new Vector3(-halfExtends.x, halfExtends.y, -halfExtends.z);
		points[6] = centre + GetAdjustedOrientation(rotation) * new Vector3(-halfExtends.x, -halfExtends.y, halfExtends.z);
		points[7] = centre + GetAdjustedOrientation(rotation) * new Vector3(-halfExtends.x, -halfExtends.y, -halfExtends.z);
		#endregion

		for(byte i = 0; i < 7; i++)
			for(byte j = 1; j < 8; j++)
				if((i.GetBit(0).XNOR(j.GetBit(0)) ^ i.GetBit(1).XNOR(j.GetBit(1)) ^ !i.GetBit(2).XNOR(j.GetBit(2))) && (i + j != 7))
					Gizmos.DrawLine(points[i], points[j]);
	}
#endif

}

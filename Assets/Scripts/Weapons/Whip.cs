using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CustomMSLibrary.Core;

public class Whip : Weapon {
	

	public BoxCastParams[] whipHitboxes;
	private Swinger swinger;

	public float speed;
	Vector3 dir;

	private IWielder wielder;

	public float attackDelay;
	public float attackDuration;
	public float attackCooldown;
	public override float FullAttackDuration => attackDelay + attackDuration + attackCooldown;

	private Collider[] boxcastCache = new Collider[1];

	public override GameObject SourceObject => throw new System.NotImplementedException();
	public override Team GetTeam => wielder.GetTeam;

	public const string NAME = "Whip";
	public override string Name => NAME;

	private void Awake() {
		swinger = GetComponent<Swinger>();
		wielder = GetComponent<IWielder>();

	}

	public void WhipAttack(TargetDirection direction) {
		var hitbox = whipHitboxes[(int)direction];

		var pos = transform.position;
		var rot = transform.rotation;

		int c = boxcastCache.Length;
		for(int i = 0; i < c; i++)
			boxcastCache[i] = null;

		c = Physics.OverlapBoxNonAlloc(pos + rot * hitbox.centerOffset, hitbox.halfExtends,
			boxcastCache, hitbox.GetAdjustedOrientation(rot));

		if(c > boxcastCache.Length)
		{
			boxcastCache = new Collider[c];
			Physics.OverlapBoxNonAlloc(pos + rot * hitbox.centerOffset, hitbox.halfExtends,
				boxcastCache, hitbox.GetAdjustedOrientation(rot));
		}

		bool anchorAssigned = false;
		foreach(var item in boxcastCache)
		{
			if(item == null)
				continue;

			if(item.gameObject == gameObject)
				continue;

			var dmg = item.GetComponent<IDamageable>();
			if(dmg != null)
				dmg.Damage(damage, this);


			if(!anchorAssigned)
			{
				var anchor = item.GetComponent<SwingAnchor>();
				if(anchor == null)
					continue;

				if(anchor.transformDependant)
					swinger.SetupSwing(item.transform);
				else
					swinger.SetupSwing(item.transform.position);
				anchorAssigned = true;

				swinger.StartSwing();
			}
		}
	}

	public override void Attack(Vector2 direction) {

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

		WhipAttack(directionIndex);
	}


	private IEnumerator Coroutine_DelayedObjectActiveBlinker(GameObject item, float firstDuration, float secondDuration) {
		yield return new WaitForSeconds(firstDuration);
		item.SetActive(true);
		yield return new WaitForSeconds(secondDuration);
		item.SetActive(false);
	}

#if UNITY_EDITOR
	Vector3[] points = new Vector3[8];
	private void OnDrawGizmosSelected() {
		Gizmos.color = Color.yellow;
		Quaternion rotation = transform.rotation;
		Vector3 position = transform.position;
		foreach(var item in whipHitboxes)
		{
			Vector3 centre = position + rotation * item.centerOffset;
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
	#region Unused
	/*
	public void GrabAndPull(Rigidbody rigid) {
		if(nearGrappleable != null)
		{
			if((nearGrappleable.transform.position - transform.position).sqrMagnitude > 10f.Squared())
			{
				nearGrappleable = null;
			} else
			{
				dir = nearGrappleable.transform.position - transform.position;

				rigid.velocity = new Vector3(dir.x, dir.y, dir.z) * speed;
			}
		}
	}
	*/
	#endregion

}

[Serializable]
public struct BoxCastParams {
	public Vector3 centerOffset;
	public Vector3 halfExtends;

	public Vector3 orientation;

	public Quaternion GetAdjustedOrientation(Quaternion parentRotation) => parentRotation * Quaternion.Euler(orientation);
}

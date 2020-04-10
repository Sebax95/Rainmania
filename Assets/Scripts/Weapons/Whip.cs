using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CustomMSLibrary.Core;

public class Whip : Weapon {

	public BoxCastParams[] whipHitboxes;
	public GameObject nearGrappleable;
	private Swinger swinger;

	public float speed;
	Vector3 dir;

	private IWielder wielder;

	public float whipDelay;
	public float whipDuration;

	private Collider[] boxcastCache = new Collider[1];

	public override GameObject SourceObject => throw new System.NotImplementedException();
	public override Team GetTeam => wielder.GetTeam;

	private void Awake() {
		swinger = GetComponent<Swinger>();
		wielder = GetComponent<IWielder>();

	}

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

	public void WhipAttack(BoxCastParams hitbox) {
		//StartCoroutine(Coroutine_DelayedObjectActiveBlinker(item, firstDuration, secondDuration));
		int c = Physics.OverlapBoxNonAlloc(transform.position + hitbox.centerOffset, hitbox.halfExtends,
			boxcastCache, hitbox.GetAdjustedOrientation(transform.rotation));
		if(c > boxcastCache.Length)

		{
			boxcastCache = new Collider[c];
			Physics.OverlapBoxNonAlloc(transform.position + hitbox.centerOffset, hitbox.halfExtends,
				boxcastCache, hitbox.GetAdjustedOrientation(transform.rotation));
		}
		bool anchorAssigned = false;
		foreach(var item in boxcastCache)
		{
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
				}
		}
	}

	public override void Attack(Vector2 direction) {
		(bool horizontal, bool up) attackDirection = (direction.x != 0, direction.y > 0);

		switch(attackDirection)
		{
			//TODO: Usar componente whip una vez hecho 

			//Order: horizontal, vertical
			case var t when t == (false, false):
				WhipAttack(whipHitboxes[0]);
				break;
			case var t when t == (true, false):
				WhipAttack(whipHitboxes[0]);
				break;
			case var t when t == (false, true):
				WhipAttack(whipHitboxes[2]);
				break;
			case var t when t == (true, true):
				WhipAttack(whipHitboxes[1]);
				break;
		}
		//StartCoroutine(Coroutine_AttackCooldown());
	}

	private IEnumerator Coroutine_DelayedObjectActiveBlinker(GameObject item, float firstDuration, float secondDuration) {
		yield return new WaitForSeconds(firstDuration);
		item.SetActive(true);
		yield return new WaitForSeconds(secondDuration);
		item.SetActive(false);
	}

	private void OnDrawGizmosSelected() {
		Gizmos.color = Color.yellow;
		Quaternion rotation = transform.rotation;
		Vector3 position = transform.position;
		Vector3[] points = new Vector3[8];
		foreach(var item in whipHitboxes)
		{
			Vector3 centre = position + rotation * item.centerOffset;
			points[0] = centre + item.GetAdjustedOrientation(rotation) * new Vector3(item.halfExtends.x, item.halfExtends.y, item.halfExtends.z);
			points[1] = centre + item.GetAdjustedOrientation(rotation) * new Vector3(item.halfExtends.x, item.halfExtends.y, -item.halfExtends.z);
			points[2] = centre + item.GetAdjustedOrientation(rotation) * new Vector3(item.halfExtends.x, -item.halfExtends.y, item.halfExtends.z);
			points[3] = centre + item.GetAdjustedOrientation(rotation) * new Vector3(item.halfExtends.x, -item.halfExtends.y, -item.halfExtends.z);
			points[4] = centre + item.GetAdjustedOrientation(rotation) * new Vector3(-item.halfExtends.x, item.halfExtends.y, item.halfExtends.z);
			points[5] = centre + item.GetAdjustedOrientation(rotation) * new Vector3(-item.halfExtends.x, item.halfExtends.y, -item.halfExtends.z);
			points[6] = centre + item.GetAdjustedOrientation(rotation) * new Vector3(-item.halfExtends.x, -item.halfExtends.y, item.halfExtends.z);
			points[7] = centre + item.GetAdjustedOrientation(rotation) * new Vector3(-item.halfExtends.x, -item.halfExtends.y, -item.halfExtends.z);

			for(int i = 0; i < 7; i++)
				for(int j = 1; j < 8; j++)
					Gizmos.DrawLine(points[i], points[j]);
		}
	}
}

[Serializable]
public struct BoxCastParams {
	public Vector3 centerOffset;
	public Vector3 halfExtends;

	public Vector3 orientation;

	public Quaternion GetAdjustedOrientation(Quaternion parentRotation) => parentRotation * Quaternion.Euler(orientation);
}

using UnityEngine;
using BossMachine;

public class RootAttackState<T> : FiniteState<T> {

	private float timeFromEnterToHit;
	private float clockTimeFromEnter;
	private int attackId;
	private int attackDamage;

	private BoxCastParams hitbox;
	private Vector3 attackPositionOffset;

	private Animator animator;
	private T advanceFeed;
	private LayerMask damageMask;
	private IDamager owner;

	public RootAttackState(Transform transform) : base(transform) { }

	public override void Enter() {
		base.Enter();
		clockTimeFromEnter = Time.time;
		animator.SetInteger("AttackId", attackId);
		animator.SetTrigger("DoAttack");
	}

	public override void Update() {
		base.Update();
		if(!(Time.time > clockTimeFromEnter + timeFromEnterToHit))
			return;
		DoAttack();
		fsm.Feed(advanceFeed);
	}

	private void DoAttack() {
		var rotation = transform.rotation;
		var result = hitbox.PerformBoxcast(transform.position + rotation * attackPositionOffset, rotation, damageMask);

		for(int i = 0; i < result.Length; i++)
		{
			var dmg = result[i] as IDamageable;
			if(dmg == null)
				continue;
			dmg.Damage(attackDamage, owner);
		}
	}

	#region Builder
	public RootAttackState<T> SetAnimator(Animator animator) {
		this.animator = animator;
		return this;
	}
	public RootAttackState<T> SetAdvanceFeed(T feed) {
		advanceFeed = feed;
		return this;
	}
	public RootAttackState<T> SetHitbox(BoxCastParams hitbox) {
		this.hitbox = hitbox;
		return this;
	}
	public RootAttackState<T> SetTimeUntillAttack(float time) {
		timeFromEnterToHit = time;
		return this;
	}
	public RootAttackState<T> SetDamagingLayers(LayerMask mask) {
		damageMask = mask;
		return this;
	}
	public RootAttackState<T> SetAttackId(int id) {
		attackId = id;
		return this;
	}
	public RootAttackState<T> SetAttackPosOffset(Vector3 offset) {
		attackPositionOffset = offset;
		return this;
	}
	public RootAttackState<T> SetAttackDamage(int dmgAmount) {
		attackDamage = dmgAmount;
		return this;
	}
	public RootAttackState<T> SetOwner(IDamager owner) {
		this.owner = owner;
		return this;
	}
	#endregion
}

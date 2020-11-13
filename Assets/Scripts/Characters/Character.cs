using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using CustomMSLibrary.Core;


[DisallowMultipleComponent]
public abstract class Character : Controllable, IDamageable, IHealable, ITeam, IAddableVelocity {
	[SerializeField]
	protected Team myTeam;

	//Health
	public float maxHealth;
	[FormerlySerializedAs("damageTimer")]public float invincibleTime;
	protected bool isInvicible;
	protected bool isDead;
	[SerializeField]
	public float Health { get; protected set; }

	protected Rigidbody rb;
	protected Vector3 addedVelocity;

	public Team GetTeam => myTeam;

	public event Action<IDamager> OnDeath;

	protected virtual void Start() {
		Health = maxHealth;
		rb = GetComponent<Rigidbody>();
	}


	public virtual bool Damage(int amount, IDamager source) {
		if(isInvicible || isDead)
			return false;

		if(!source.GetTeam.CanDamage(myTeam))
			return false;
		Health -= amount;
		if(Health <= 0)
		{
			Die(source);
			return true;
		}

		GameManager.Instance.StartCoroutine(Coroutine_InvinsibleTime());
		return true;
	}
	public virtual bool Heal(int amount, IHealer source) {
		if(Health == maxHealth)
			return false;
		Health = Mathf.Min(Health + amount, maxHealth);
		return true;
	}

	public virtual void Die(IDamager source) {
		isDead = true;
		OnDeath?.Invoke(source);
	}

	protected IEnumerator Coroutine_InvinsibleTime() {
		isInvicible = true;
		yield return new WaitForSeconds(invincibleTime);
		isInvicible = false;
	}

	public void AddVelocity(Vector3 velocity) => addedVelocity = velocity;
	public void ClearAddedVelocity() => addedVelocity = Vector3.zero;

	public GameObject SourceObject => gameObject;
}

public abstract class Controllable : TimedBehaviour {
	public abstract void Move(Vector2 direction);

	protected virtual void OnDestroy() {
		ControllerHandler.Instance.DestroyController(this);
	}
}

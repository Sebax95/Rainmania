using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CustomMSLibrary.Core;


[DisallowMultipleComponent]
public abstract class Character : Controllable, IDamageable, IHealable, ITeam, IAddableVelocity {
	[SerializeField]
	protected Team myTeam;
	public float maxHealth;
	[SerializeField]
	public float Health { get; protected set; }
	protected Rigidbody rb;
	protected Vector3 addedVelocity;

	public Team GetTeam => myTeam;

	protected virtual void Start() {
		Health = maxHealth;
		rb = GetComponent<Rigidbody>();
	}


	public virtual void Damage(int amount, IDamager source) {
		if(!source.GetTeam.CanDamage(myTeam))
			return;
		Health -= amount;
		if(Health <= 0)
			Die(source);
	}
	public virtual void Heal(int amount, IHealer source) {
		Health = Mathf.Min(Health + amount, maxHealth);
	}

	public abstract void Die(IDamager source);

	public void AddVelocity(Vector3 velocity) => addedVelocity = velocity;
	public void ClearAddedVelocity() => addedVelocity = Vector3.zero;
}

public abstract class Controllable : MonoBehaviour {
	public abstract void Move(Vector2 direction);

	protected virtual void OnDestroy() {
		ControllerHandler.Instance.DestroyController(this);
	}
}

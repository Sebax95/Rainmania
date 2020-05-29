﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CustomMSLibrary.Core;


[DisallowMultipleComponent]
public abstract class Character : Controllable, IDamageable, IHealable, ITeam {
	[SerializeField]
	protected Team myTeam;
	public float maxHealth;
	[SerializeField]
	protected float health;
	protected Rigidbody rb;

	public Team GetTeam => myTeam;

	protected virtual void Start() {
		health = maxHealth;
		rb = GetComponent<Rigidbody>();
	}


	public virtual void Damage(int amount, IDamager source) {
		if(!source.GetTeam.CanDamage(myTeam))
			return;
		health -= amount;
		if(health < 0)
			Die(source);
	}
	public virtual void Heal(int amount, IHealer source) {
		health = Mathf.Min(health + amount, maxHealth);
	}

	public abstract void Die(IDamager source);

}

public abstract class Controllable : MonoBehaviour {
	public abstract void Move(Vector2 direction);

	protected virtual void OnDestroy() {
		ControllerHandler.Instance.DestroyController(this);
	}
}

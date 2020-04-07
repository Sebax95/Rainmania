using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CustomMSLibrary.Core;

public abstract class Character : MonoBehaviour, IDamageable, IHealable {
	public float maxHealth;
	protected float health;

	protected virtual void Start() {
		health = maxHealth;
	}

	public abstract void Move(Vector2 direction);
	public abstract void Damage(int amount, IDamager source);
	public abstract void Heal(int amount, IHealer source);
	public abstract void Die(IDamager source);


	public abstract int GetTeam();
}

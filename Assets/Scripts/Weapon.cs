using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Weapon : MonoBehaviour, IWeapon, IDamager {
	public int damage;

	public abstract GameObject SourceObject { get; }
	public abstract Team GetTeam { get; }

	public abstract void Attack(Vector2 direction);

}

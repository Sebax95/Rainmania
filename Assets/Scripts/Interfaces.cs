﻿using UnityEngine;

public interface ITeam {
	Team GetTeam { get; }
}

public interface IDamager : ITeam {
	GameObject SourceObject { get; }
}

public interface IDamageable {
	bool Damage(int amount, IDamager source);
	void Die(IDamager source);
}

public interface IHealer : ITeam {
	GameObject SourceObject { get; }
}

public interface IHealable {
	bool Heal(int amount, IHealer source);
}

public interface IWeapon {
	void Attack(Vector2 direction);
}

public interface IWielder : ITeam{}

public interface IMoveOverride {
	void Attach(IMoveOverrideable user);
	void Release(IMoveOverrideable user);
}

public interface IMoveOverrideable {
	void Attach(IMoveOverride controller);
	void Release(IMoveOverride controller);
}

public interface IAppliableForce
{
    void ApplyForce(Vector3 direction, ForceMode mode);
}

public interface IMainController {
}

public interface IAddableVelocity {
	void AddVelocity(Vector3 velocity);
	void ClearAddedVelocity();
}

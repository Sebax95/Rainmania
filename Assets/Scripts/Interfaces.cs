using UnityEngine;

public interface ITeam {
	int GetTeam();
}

public interface IDamager : ITeam {
	GameObject SourceObject { get; }
}

public interface IDamageable {
	void Damage(int amount, IDamager source);
	void Die(IDamager source);
}

public interface IHealer : ITeam {
	GameObject SourceObject { get; }
}

public interface IHealable {
	void Heal(int amount, IHealer source);
}

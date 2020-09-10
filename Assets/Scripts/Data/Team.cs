using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Team Interaction", menuName = "Team Interaction", order = 3)]
public class Team : ScriptableObject {
	public List<TeamInteraction> interactions;

	public bool CanDamage(Team otherTeam) {
		if(otherTeam == null)
			throw new ArgumentNullException("Opposite team is null");

		var inter = interactions.Find(x => x.otherTeam == otherTeam);
		if(inter.Equals(default))
			throw new KeyNotFoundException("Interaction for this team does not exist");

		return inter.canDamage;
	}
}

[Serializable]
public struct TeamInteraction {
	public Team otherTeam;
	public bool canDamage;
}

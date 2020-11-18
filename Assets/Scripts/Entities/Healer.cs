using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Healer : TimedBehaviour, IHealer {

	public Team targetTeam;
	public int amount;

	[Tooltip("Set to negative for no respawning")]
	public float respawnTime;

	public Team GetTeam => targetTeam;
	public GameObject SourceObject => gameObject;

	public int checkPeriodInFrames =3;
	private int missedCheckCounter = 0;

	private GameObject child;
	new private Collider collider;
	private HealerView view;

	private void Start() {
		child = transform.GetChild(0).gameObject;
		collider = GetComponent<Collider>();
		view = GetComponent<HealerView>();
	}

	private void OnTriggerStay(Collider other) {
		//Para evitar checkear cada frame con algo que no es, puse este limitador de checkeos.
		//Esto cuenta ciclos de fisica. De ser necesario, se puede cambiar a tiempo.
		if(missedCheckCounter > 0)
		{
			missedCheckCounter--;
			return;
		}
		missedCheckCounter = checkPeriodInFrames;

		DoHealing(other);
	}

	private void DoHealing(Collider other) {
		ITeam team = other.GetComponent<ITeam>();
		if(team == null || team.GetTeam != targetTeam) //Si no tiene equipo, skip
			return;

		IHealable healable = other.GetComponent<IHealable>();
		if(healable == null) //Si no puede curar, skip
			return;

		bool succeed = healable.Heal(amount, this);
		if(!succeed) //Si no logra curar (ej: vida llena), skip
			return;

		HandleRespawn();
	}

	private void HandleRespawn() {
		child.SetActive(false);
		collider.enabled = false;

		if(respawnTime < 0) //If set to no respawn, destroy next  frame.
		{
			StartCoroutine(Coroutine_DestroyNextFrame());
			return;
		}
		StartCoroutine(Coroutine_Reenable());
	}

	private IEnumerator Coroutine_DestroyNextFrame() {
		yield return null;
		Destroy(gameObject);
	}

	private IEnumerator Coroutine_Reenable() {
		yield return new WaitForSeconds(respawnTime);
		child.SetActive(true);
		collider.enabled = true;
	}

}
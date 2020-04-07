using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CustomMSLibrary.Core;

public class PlayerController : Controller {

	new private Player pawn;
	private Vector2 direction;

	public KeyCode JumpKey;
	public KeyCode AttackKey;
	public KeyCode SwitchWeaponKey;

	private void Start() {
		
	}

	protected override void DoMovement() {
		direction = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
		pawn.Move(direction);
		if(Input.GetKeyDown(JumpKey))
			pawn.Jump();
	}

	protected override void DoActions() {
		if(Input.GetKeyDown(AttackKey))
			pawn.Attack(direction);

		if(Input.GetKeyDown(SwitchWeaponKey))
			pawn.SwitchWeapons();
	}

}

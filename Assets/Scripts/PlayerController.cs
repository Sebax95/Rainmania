using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CustomMSLibrary.Core;

public class PlayerController : Controller {

	private Player playerPawn;
	private Vector2 direction;

	public KeyCode JumpKey;
	public KeyCode AttackKey;
	public KeyCode SwitchWeaponKey;

	private void Start() {
		
	}

	protected override void DoMovement() {
		direction = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
		playerPawn.Move(direction);
		if(Input.GetKeyDown(JumpKey))
			playerPawn.Jump();
		else if(Input.GetKeyUp(JumpKey))
			playerPawn.ReleaseJump();
	}

	protected override void DoActions() {
		if(Input.GetKeyDown(AttackKey))
			playerPawn.Attack(direction);

		if(Input.GetKeyDown(SwitchWeaponKey))
			playerPawn.SwitchWeapons();
	}

}

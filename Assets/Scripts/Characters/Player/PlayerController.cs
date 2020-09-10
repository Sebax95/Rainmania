﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CustomMSLibrary.Core;
using UnityEngine.Serialization;

public class PlayerController : Controller, IMainController {

	//private Player playerPawn;
	private Vector2 direction;

	public string horizontalAxis = "Horizontal";
	public string verticalAxis = "Vertical";

	public KeyCode JumpKey;
	[FormerlySerializedAs("AttackKey")]
	public KeyCode attackWhipKey;
	[FormerlySerializedAs("SwitchWeaponKey")]
	public KeyCode attackBowKey;
	//public KeyCode SwitchWeaponKey;
	public KeyCode crouchKey;

	private Player User => User<Player>();

	protected override void DoMovement() {
		direction = GetAxises();
		User?.Move(direction);

		if(Input.GetKeyDown(crouchKey))
			User?.ToggleCrouch();

		if(Input.GetKeyDown(JumpKey))
			User?.Jump();
		else if(Input.GetKeyUp(JumpKey))
			User?.ReleaseJump();

	}

	protected override void DoActions() {
		if(Input.GetKeyDown(attackWhipKey))
		{
			User?.SetWeapon(0);
			User?.Attack(direction);
		}
		if(Input.GetKeyDown(attackBowKey))
		{
			User?.SetWeapon(1);
			User?.Attack(direction);
		}

		//if(Input.GetKeyDown(SwitchWeaponKey))
		//	User?.SwitchWeapons();
	}

	public Vector2 GetAxises() => new Vector2(Input.GetAxis(horizontalAxis), Input.GetAxis(verticalAxis));

	//public override void AssignUser(Controllable user) {
	//	base.AssignUser(user);
	//	playerPawn = user as Player;
	//}
}

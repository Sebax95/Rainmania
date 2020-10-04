using System.Collections;
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

	//JOYSTICK TEMPORAL!!!!
	[Header("Joystick")] 
	public KeyCode joystickJump;
	public KeyCode joystickArrow;
	public KeyCode joystickWhip;
	public KeyCode joystickCrouch;
	
	
	private Player User => User<Player>();

	protected override void DoMovement() {
		direction = GetAxises();
		User?.Move(direction);
		
		if(Input.GetKeyDown(crouchKey) || Input.GetKeyDown(KeyCode.Joystick1Button1))//JOYSTICK TEMPORAL!!!!
			User?.ToggleCrouch();

		if(Input.GetKeyDown(JumpKey) || Input.GetKeyDown(KeyCode.Joystick1Button0))//JOYSTICK TEMPORAL!!!!
			User?.Jump();
		else if(Input.GetKeyUp(JumpKey) || Input.GetKeyUp(KeyCode.Joystick1Button0))//JOYSTICK TEMPORAL!!!!
			User?.ReleaseJump();

	}

	protected override void DoActions()
	{
		if (Input.GetKeyDown(attackWhipKey) || Input.GetKeyDown(KeyCode.JoystickButton7))//JOYSTICK TEMPORAL!!!!
		{
			User?.SetWeapon(0);
			User?.Attack(direction);
		}
		if(Input.GetKeyDown(attackBowKey) || Input.GetKeyDown(KeyCode.JoystickButton5))//JOYSTICK TEMPORAL!!!!
		{
			User?.SetWeapon(1);
			User?.Attack(direction);
		}

		//if(Input.GetKeyDown(SwitchWeaponKey))
		//	User?.SwitchWeapons();
	}

	public Vector2 GetAxises() => new Vector2(Input.GetAxisRaw(horizontalAxis), Input.GetAxisRaw(verticalAxis));

	//public override void AssignUser(Controllable user) {
	//	base.AssignUser(user);
	//	playerPawn = user as Player;
	//}
}

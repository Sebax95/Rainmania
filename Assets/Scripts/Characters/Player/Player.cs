﻿using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using CustomMSLibrary.Core;
using CustomMSLibrary.Unity;

public class Player : Character, IWielder, IMoveOverrideable {
	private const float GROUNDED_DISTANCE = 1.1f;

	public float speed;
	public float forceJump;
	public float fallMultiplier = 2.5f;
	public float lowJumpMultiplier = 2f;
	public LayerMask validFloorLayers;
	private bool holdingJump = false;

	private Rigidbody rb;
	private MomentumKeeper momentum;

	private PlayerAnim playerAnimator;

	private bool canAttack = true;


	private IWeapon activeWeapon;
	private LoopingList<Weapon> weapons = new LoopingList<Weapon>();

	public Controller thisControllerPrefab; //temp
	private IMoveOverride overriding;

	protected override void Start() {
		weapons.Add(GetComponent<Whip>());
		weapons.Add(GetComponent<Bow>());
		momentum = GetComponent<MomentumKeeper>();
		activeWeapon = weapons.Current;

		rb = GetComponent<Rigidbody>();
		playerAnimator = GetComponent<PlayerAnim>();
		//Temp, despues ver como SOLIDear asignacion de controller
		ControllerHandler.Instance.RequestAssignation(Instantiate(thisControllerPrefab), this);

		rb = GetComponent<Rigidbody>();
		playerAnimator = GetComponent<PlayerAnim>();
	}

	/*
	private void FixedUpdate() {
		if(overriding != null)
			return;
		if(rb.velocity.y < 0)
			rb.velocity += Vector3.up * Physics.gravity.y * (fallMultiplier - 1) * Time.fixedDeltaTime;
		else if(rb.velocity.y > 0 && !holdingJump)
			rb.velocity += Vector3.up * Physics.gravity.y * (lowJumpMultiplier - 1) * Time.fixedDeltaTime;
	}
	*/

	public void Jump() {
		if(Physics.Raycast(transform.position, Vector3.down, GROUNDED_DISTANCE, validFloorLayers))
		{
			rb.velocity = rb.velocity.ZeroY();
			ForceJump();
		}
		//} else
		//overriding.Release(this);
	}

	public void ForceJump() {
		rb.AddForce(Vector3.up * forceJump,ForceMode.VelocityChange);
        //playerAnimator.TriggerAction(0);
        playerAnimator.thisAnimator.SetBool("inGround", false);
		holdingJump = true;
	}

	public void ReleaseJump() => holdingJump = false;

	public override void Move(Vector2 direction) {
		if(overriding != null)
			return;

		//rb.velocity = new Vector3(direction.x * speed, rb.velocity.y, 0);
		var vel = rb.velocity;
		Vector3 newVel = new Vector3(direction.x * speed, vel.y);

		rb.velocity = newVel + momentum.velocity.ZeroY();

		playerAnimator.SetSpeeds(direction);
	}

	public void Attack(Vector2 direction) {
		if(canAttack)
			activeWeapon.Attack(direction);
	}

	public void SwitchWeapons() =>
		activeWeapon = weapons.Next;


	public override void Die(IDamager source) => throw new System.NotImplementedException();

	public void Attach(IMoveOverride controller) {
		overriding = controller;
	}

	public void Release(IMoveOverride controller) {
		overriding = null;
	}

    public void DetectGround()
    {
        RaycastHit rch;
        if(Physics.Raycast(transform.position, -transform.up * 1, out rch, validFloorLayers))
        {
            if (rch.distance < 2)
                playerAnimator.thisAnimator.SetBool("inGround", true);
            else
                playerAnimator.thisAnimator.SetBool("inGround", false);
        }
    }
    private void OnCollisionEnter(Collision collision)
    {        
        if (collision.gameObject.layer == 9)
        {
            playerAnimator.thisAnimator.SetBool("inGround", true);
        }
    }
}
using System.Collections;
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
	public float groundedTreshold;
	public Vector3 groundCheckOffset;
	public bool Grounded { get; private set; }

	private Rigidbody rb;
	private MomentumKeeper momentum;

	private PlayerAnim playerAnimator;

	private bool canAttack = true;


	private Weapon activeWeapon;
	private LoopingList<Weapon> weapons = new LoopingList<Weapon>();
	public float attackWindupTime;
	public float attackDurationTime;

	public Controller thisControllerPrefab; //temp
	private IMoveOverride overriding;

	private void Awake() {
		ControllerHandler.Instance.RequestAssignation(Instantiate(thisControllerPrefab), this);
	}

	protected override void Start() {
		weapons.Add(GetComponent<Whip>());
		weapons.Add(GetComponent<Bow>());
		momentum = GetComponent<MomentumKeeper>();
		activeWeapon = weapons.Current;

		rb = GetComponent<Rigidbody>();
		playerAnimator = GetComponent<PlayerAnim>();
		//Temp, despues ver como SOLIDear asignacion de controller
		

		rb = GetComponent<Rigidbody>();
		playerAnimator = GetComponent<PlayerAnim>();
	}
	private void Update() 
	{
		DetectGround();
	}

	public void Jump() {
		if(Grounded)
		{
			rb.velocity = rb.velocity.ZeroY();
			playerAnimator.TriggerAction(0);
			ForceJump();
		}
		//} else
		//overriding.Release(this);
	}

	public void ForceJump() {
		rb.AddForce(Vector3.up * forceJump,ForceMode.VelocityChange);
		playerAnimator.Jump();
		//playerAnimator.thisAnimator.SetBool("inGround", false);
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
		if (canAttack)
		{
			canAttack = false;
			activeWeapon.Attack(direction);
			playerAnimator.Attack(direction, activeWeapon.Name);
		}
			
	}

	public void SwitchWeapons() {
		activeWeapon = weapons.Next;
	}

	public override void Die(IDamager source) => throw new System.NotImplementedException();

	public void Attach(IMoveOverride controller) {
		overriding = controller;
	}

	public void Release(IMoveOverride controller) {
		overriding = null;
	}

	public void DetectGround() =>
		Grounded = Physics.Raycast(transform.position + groundCheckOffset, Vector3.down, out _, groundedTreshold, validFloorLayers);

	public string ActiveWeaponName => activeWeapon.Name;

}

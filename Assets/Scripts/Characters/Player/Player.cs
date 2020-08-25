using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using CustomMSLibrary.Core;
using CustomMSLibrary.Unity;

public class Player : Character, IWielder, IMoveOverrideable, IAppliableForce, IAddableVelocity {
	private const float GROUNDED_DISTANCE = 1.1f;

	public float speed;
	public float forceJump;
	public float fallMultiplier = 2.5f;
	public float lowJumpMultiplier = 2f;
	public LayerMask validFloorLayers;
	private bool holdingJump = false;

	public float groundedTreshold;
	public Vector3 groundCheckOffset;
	private const int GROUND_FRAMES_PERIOD = 5;
	private int groundedFramesCounter;
	public bool Grounded { get; private set; }


	private MomentumKeeper momentum;

	public PlayerAnim PlayerAnimator { get; private set; }

	private bool canAttack = true;
	private float currentCooldown;
	private float attackCooldownTimer = 0;
	private bool isDead;
	private Weapon activeWeapon;
	private LoopingList<Weapon> weapons = new LoopingList<Weapon>();

	public Controller thisControllerPrefab; //temp
	private IMoveOverride overriding;

	//agregueyo mati.
	//bool canMove = true;
	////collider salto.
	//public CapsuleCollider legsCollider;
	//public CapsuleCollider bodyUpCollider;

	private void Awake() {
		ControllerHandler.Instance.RequestAssignation(Instantiate(thisControllerPrefab), this);
	}

	protected override void Start() {
		weapons.Add(GetComponent<Whip>());
		weapons.Add(GetComponent<Bow>());
		momentum = GetComponent<MomentumKeeper>();
		activeWeapon = weapons.Current;
		//canMove = true;
		isDead = false;
		PlayerAnimator = GetComponent<PlayerAnim>();
		//Temp, despues ver como SOLIDear asignacion de controller
		Health = maxHealth;


		rb = GetComponent<Rigidbody>();
		PlayerAnimator = GetComponent<PlayerAnim>();
	}
	private void Update() {
		if(isDead)
			return;
		DetectGround();
		AttackTimer();

	}

	public void Jump() {
		if(Grounded)
		{

			rb.velocity = rb.velocity.ZeroY();
			PlayerAnimator.TriggerAction(0);
			ForceJump();
			//colliders legs off

		}

		//} else
		//overriding.Release(this);
	}

	public void ForceJump() {
		//if(canMove)
		//{
			rb.AddForce(Vector3.up * forceJump, ForceMode.VelocityChange);
			PlayerAnimator.Jump();
			//playerAnimator.thisAnimator.SetBool("inGround", false);
			holdingJump = true;
			//bodyUpCollider.enabled = true;
			//legsCollider.enabled = false;


		//}

	}
	public void ReleaseJump() => holdingJump = false;

	public override void Move(Vector2 direction) {

		if(isDead || overriding != null )
			return;

		//if(!canMove)
		//	return;

		//rb.velocity = new Vector3(direction.x * speed, rb.velocity.y, 0);
		var vel = rb.velocity;
		//vel.y -= addedVelocity.y;
		Vector3 newVel = new Vector3(direction.x * speed, vel.y) + addedVelocity.ZeroY();

		rb.velocity = newVel + momentum.velocity.ZeroY();

		PlayerAnimator.SetSpeeds(direction);

	}

	public void Attack(Vector2 direction) {
		if(!canAttack || isDead)
			return;

		//canMove = false;
		//rb.velocity = new Vector3(0, rb.velocity.y, rb.velocity.z);
		SetCooldown();
		activeWeapon.Attack(direction);
		PlayerAnimator.Attack(direction, activeWeapon.Name);
	}

	public void SwitchWeapons() {
		activeWeapon = weapons.Next;
	}

	/// <summary>
	/// Assign Weapon. 0: whip. 1: Bow
	/// </summary>
	/// <param name="id"></param>
	public void SetWeapon(int id) {
		activeWeapon = weapons[id];
	}

	public override void Damage(int amount, IDamager source) {
		base.Damage(amount, source);
		PlayerAnimator.Hurt();
		if(isDead)
			return;
		//PlayerAnimator.TriggerAction(7);
	}

	public override bool Heal(int amount, IHealer source) {
		bool result = base.Heal(amount, source);
		if(!result) //Si curacion falla, saltea
			return result;
		PlayerAnimator.Heal();
		return result;
	}

	public override void Die(IDamager source) {
		isDead = true;
		//PlayerAnimator.ChangeBool(1, true);
		PlayerAnimator.Die();
		//canMove = false;
		GameManager.Instance.PlayerDie();
		Destroy(gameObject, 3);
	}

	public void Attach(IMoveOverride controller) {
		overriding = controller;
	}

	public void Release(IMoveOverride controller) {
		overriding = null;
	}

	public void DetectGround() {
		groundedFramesCounter++;
		if(!(groundedFramesCounter > GROUND_FRAMES_PERIOD))
			return;
		groundedFramesCounter = 0;
		Grounded = Physics.Raycast(transform.position + groundCheckOffset, Vector3.down, out _, groundedTreshold, validFloorLayers);
	}

	public string ActiveWeaponName => activeWeapon.Name;

	private void AttackTimer() {
		if(canAttack)
			return;

		attackCooldownTimer += Time.deltaTime;
		if(attackCooldownTimer > currentCooldown)
		{
			//canMove = true;
			canAttack = true;
		}
	}

	private void SetCooldown() {
		canAttack = false;
		attackCooldownTimer = 0;
		currentCooldown = activeWeapon.FullAttackDuration;

	}

	public void ApplyForce(Vector3 direction, ForceMode mode) {
		rb.velocity = new Vector3(rb.velocity.x, 0);
		rb.AddForce(direction, mode);
		PlayerAnimator.TriggerAction(0);
	}


}

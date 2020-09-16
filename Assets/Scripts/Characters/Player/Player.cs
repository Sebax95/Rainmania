using System;
using UnityEngine;
using CustomMSLibrary.Core;
using CustomMSLibrary.Unity;

public class Player : Character, IWielder, IMoveOverrideable, IAppliableForce, IAddableVelocity {
	#region Variables/Properties
	private const float GROUNDED_DISTANCE = 1.1f;

	//Mobility
	public float speed;
	public float forceJump;
	public float fallMultiplier = 2.5f;
	public float lowJumpMultiplier = 2f;
	public LayerMask validFloorLayers;
	private bool holdingJump = false;

	//Crouched mobility
	private bool crouched = false;
	[Range(0f, 1f)]
	public float crouchSpeedModifier;
	public LayerMask crouchCheckLayerMask;

	//Grounded
	public float groundedTreshold;
	public Vector3 groundCheckOffset;
	private const int GROUND_FRAMES_PERIOD = 5;
	private int groundedFramesCounter;
	public bool Grounded { get; private set; }

	//References
	private MomentumKeeper momentum;
	public PlayerAnim PlayerAnimator { get; private set; }
	private CrouchStateRouter croucher;
	private (CapsuleCollider standing, CapsuleCollider crouching, Bounds standChecker) colliders;

	//Attack
	private bool canAttack = true;
	private float currentCooldown;
	private float attackCooldownTimer = 0;
	private bool isDead;
	private Weapon activeWeapon;
	private LoopingList<Weapon> weapons = new LoopingList<Weapon>();

	//Controller
	public Controller thisControllerPrefab; //temp
	private IMoveOverride overriding;
	#endregion

	#region Monobehaviour
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
		croucher = GetComponent<CrouchStateRouter>();

		var cols = GetComponents<CapsuleCollider>();
		colliders.standing = cols[0];
		colliders.crouching = cols[1];
		InitHeightChecker();
		cols[1].enabled = false;

		//Temp, despues ver como SOLIDear asignacion de controller
		Health = maxHealth;


		rb = GetComponent<Rigidbody>();
		PlayerAnimator = GetComponent<PlayerAnim>();
		InitCollisionMask();
	}
	private void Update() {
		if(isDead)
			return;
		DetectGround();
		AttackTimer();

	}
	#endregion

	#region Movement
	public void Jump() {
		if(Grounded)
		{
			ToggleCrouch(false);

			rb.velocity = rb.velocity.ZeroY();
			PlayerAnimator.TriggerAction(0);
			ForceJump();
			//colliders legs off

		}

		//} else
		//overriding.Release(this);
	}

	public void ForceJump() {
		rb.AddForce(Vector3.up * forceJump, ForceMode.VelocityChange);
		PlayerAnimator.Jump();
		holdingJump = true;


	}
	public void ReleaseJump() => holdingJump = false;

	public override void Move(Vector2 direction) {

		if(isDead || overriding != null)
			return;
		float mult = 1;
		if(crouched)
			mult *= crouchSpeedModifier;

		var vel = rb.velocity;
		Vector3 newVel = new Vector3(direction.x * speed * mult, vel.y) + addedVelocity.ZeroY();

		rb.velocity = newVel + momentum.velocity.ZeroY();

		PlayerAnimator.SetSpeeds(direction);

	}

	public void DetectGround() {
		groundedFramesCounter++;
		if(!(groundedFramesCounter > GROUND_FRAMES_PERIOD))
			return;
		groundedFramesCounter = 0;
		Grounded = Physics.Raycast(transform.position + groundCheckOffset, Vector3.down, out _, groundedTreshold, validFloorLayers);
	}

	public void ToggleCrouch(bool state) {
		if(!state)
		{
			var col = colliders.standing as CapsuleCollider;
			if(!col)
				throw new ArgumentNullException("Oi, shit-ass. You've changed the collider type, didn't you? Ya fucked up. Not really. Just update the code here to compensate.");

			Bounds checkBounds = colliders.standChecker;
			bool solidAboveMe = Physics.CheckBox(transform.position + checkBounds.center, checkBounds.extents, transform.rotation, crouchCheckLayerMask);
			if(solidAboveMe)
				return;
		}
		crouched = state;
		croucher.IsCrouched = state;
		colliders.standing.enabled = !state;
		colliders.crouching.enabled = state;
		PlayerAnimator.SetCrouched(state);
	}

	public void ToggleCrouch() =>
		ToggleCrouch(!crouched);
	#endregion

	#region Attacking
	public void Attack(Vector2 direction) {
		if(!canAttack || isDead)
			return;

		SetCooldown();
		//hack de crouch
		if(crouched)
			direction = transform.forward;

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
	#endregion

	#region Health
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
	#endregion

	#region Misc
	public void Attach(IMoveOverride controller) {
		overriding = controller;
	}

	public void Release(IMoveOverride controller) {
		overriding = null;
	}

	public void ApplyForce(Vector3 direction, ForceMode mode) {
		rb.velocity = new Vector3(rb.velocity.x, 0);
		rb.AddForce(direction, mode);
		PlayerAnimator.TriggerAction(0);
	}

	private void InitHeightChecker() {
		var standCol = colliders.standing;
		var crouchCol = colliders.crouching;

		Vector3 center = standCol.center;
		float standTrueHeight = standCol.height + standCol.radius;
		float crouchTrueHeight = crouchCol.height + crouchCol.radius;

		float deltaHeight = standTrueHeight - crouchTrueHeight;
		float floorHeight = center.y - (standTrueHeight / 2f);
		float newCenterHeight = floorHeight + crouchTrueHeight + (deltaHeight / 2f);

		float radius = colliders.standing.radius;
		Vector3 newCenter = new Vector3(center.x, newCenterHeight, center.z);
		colliders.standChecker = new Bounds() {
			center = newCenter,
			extents = Vector3.one * radius
		};
	}

	private void InitCollisionMask() {
		int myLayer = gameObject.layer;
		int layerMask = 0;

		for(int i = 0; i < 32; i++)
		{
			if(!Physics.GetIgnoreLayerCollision(myLayer, i))
			{
				layerMask = layerMask | (1 << i);
			}
		}
		crouchCheckLayerMask = layerMask;
	}
	#endregion

}

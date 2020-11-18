using System;
using System.Collections;
using UnityEngine;
using CustomMSLibrary.Core;
using CustomMSLibrary.Unity;

public class Player : Character, IWielder, IMoveOverrideable, IAppliableForce, IAddableVelocity {
	#region Variables/Properties
	private const float GROUNDED_DISTANCE = 1.1f;
	public const int PLAYER_LAYER = 8;

	//Mobility
	public float speed;
	public float forceJump;
	public float fallMultiplier = 2.5f;
	public float lowJumpMultiplier = 2f;
	public LayerMask validFloorLayers;
	public bool test_preventJumpWhenCrouched;
	private bool aimMode = false;

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

	public float coyoteDuration;
	public float coyoteSupressionTime;
	private float coyoteTimer;
	private bool CanCoyoteJump => coyoteTimer <= coyoteDuration;
	private bool supressCoyote = false;
	private WaitForSeconds waitSupressCoyote;

	//References
	private MomentumKeeper momentum;
	public PlayerAnim PlayerAnimator { get; private set; }
	private CrouchStateRouter croucher;
	private (CapsuleCollider standing, CapsuleCollider crouching, Bounds standChecker) colliders;

	//Attack
	private bool allowedToAttack = true;
	private float attackCooldownTimer = 0;
	private (Weapon weapon, bool enabled) whip = (null, true);
	private (Weapon weapon, bool enabled) bow = (null, true);
	private bool CanAttack => allowedToAttack && !isDead;

	//Controller
	public Controller thisControllerPrefab; //temp
	private IMoveOverride overriding;
	#endregion

	#region Monobehaviour
	private void Awake() {
		ControllerHandler.Instance.RequestAssignation(Instantiate(thisControllerPrefab), this);
	}

	protected override void Start() {
		whip.weapon = GetComponent<Whip>();
		bow.weapon = GetComponent<Bow>();
		momentum = GetComponent<MomentumKeeper>();
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
		UpdateStateOnUpgrade(UpgradesManager.Instance.Data);

		coyoteTimer = 0;
		waitSupressCoyote = new WaitForSeconds(coyoteSupressionTime);
	}
	private void Update() {
		if(isDead)
			return;
		DetectGround();
		AttackTimer();
		CoyoteTime();
	}


	#endregion

	#region Movement
	public void Jump() {
		if(!(Grounded || (CanCoyoteJump && !supressCoyote)))
			return;

		bool wasCrouched = crouched; //TEST

		ToggleCrouch(false);

		//TEST
		if(wasCrouched && test_preventJumpWhenCrouched)
			return;

		rb.velocity = rb.velocity.ZeroY();
		PlayerAnimator.TriggerAction(0);
		ForceJump();
		Grounded =false;
		coyoteTimer = coyoteDuration + 1;
		groundedFramesCounter = 0;
		StartCoroutine(SupressCoyote());
		//} else
		//overriding.Release(this);
	}

	public void ForceJump() {
		rb.AddForce(Vector3.up * forceJump, ForceMode.VelocityChange);
		PlayerAnimator.Jump();
	}

	public override void Move(Vector2 direction) {

		if(isDead || overriding != null)
			return;

		if(direction.x > 0)
			transform.rotation = Quaternion.Euler(0, 90, 0);
		else if(direction.x < 0)
			transform.rotation = Quaternion.Euler(0, 270, 0);

		PlayerAnimator.SetSpeeds(direction);

		if(aimMode)
			return;

		float mult = 1;
		if(crouched)
			mult *= crouchSpeedModifier;

		var vel = rb.velocity;
		Vector3 newVel = new Vector3(direction.x * speed * mult, vel.y) + addedVelocity.ZeroY();

		rb.velocity = newVel + momentum.velocity.ZeroY();


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
			var col = colliders.standing;
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

	public void SetAimMode(bool state) => aimMode = state;

	private void CoyoteTime() {
		if(Grounded)
		{
			coyoteTimer = 0;
			return;
		}
		coyoteTimer += Time.deltaTime;
	}

	private IEnumerator SupressCoyote() {
		supressCoyote = true;
		yield return waitSupressCoyote;
		supressCoyote = false;
	}

	#endregion

	#region Attacking
	public void Attack(Vector2 direction, Weapon weapon) {
		if(!CanAttack)
			return;

		SetCooldown(weapon.FullAttackDuration);
		//hack de crouch
		if(crouched)
			direction = transform.forward;

		weapon.Attack(direction);
		PlayerAnimator.Attack(direction, weapon.Name);
	}

	public void WhipAttack(Vector2 direction) {
		if(!whip.enabled)
			return;
		Attack(direction, whip.weapon);
	}
	public void BowAttack(Vector2 direction) {
		if(bow.enabled)
			Attack(direction, bow.weapon);
	}

	private void AttackTimer() {
		if(allowedToAttack)
			return;

		attackCooldownTimer -= Time.deltaTime;
		if(attackCooldownTimer < 0)
		{
			//canMove = true;
			allowedToAttack = true;
		}
	}

	private void SetCooldown(float time) {
		allowedToAttack = false;
		attackCooldownTimer = time;

	}
	#endregion

	#region Health
	public override bool Damage(int amount, IDamager source) {
		bool result = base.Damage(amount, source);
		if(result)
			PlayerAnimator.Hurt();

		if(isDead)
			PlayerAnimator.Die();

		return result;
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

	private void UpdateStateOnUpgrade(UpgradesData data) {
		//TODO: ver como manejamos mejoras de vida con vida < 100%. Full restore por ahora
		var newHealth = data.GetInt("playerMaxLife");
		if(newHealth != maxHealth)
		{
			maxHealth = newHealth;
			Health = newHealth;
		}
		whip.enabled = data.GetBool("whipAcquired");
		if (whip.enabled)
			UIManager.Instance.FoundWhip();
		bow.enabled = data.GetBool("bowAcquired");
		if (bow.enabled)
			UIManager.Instance.FoundArcher();
	}

	private void OnEnable() => UpgradesManager.Instance.OnUpdateData += UpdateStateOnUpgrade;
	private void OnDisable() => UpgradesManager.Instance.OnUpdateData -= UpdateStateOnUpgrade;
	#endregion

}

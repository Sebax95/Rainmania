using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using CustomMSLibrary.Core;
using CustomMSLibrary.Unity;

public class Player : Character, IWielder {
	private const float GROUNDED_DISTANCE = 1.1f;

	public float speed;
	public float forceJump;
	public float fallMultiplier = 2.5f;
	public float lowJumpMultiplier = 2f;
	public LayerMask validFloorLayers;
	private bool holdingJump = false;

	private Rigidbody rb;
	private PlayerAnim playerAnimator;
	//private Vector3 movement;
	private bool canMove;

	//Attack
	public GameObject arrow;

	//public float timer;
	public float attackCooldown;
	private WaitForSeconds wait_attackCooldown;
	private bool canAttack = true;


	private IWeapon activeWeapon;
	private bool weaponIsWhip;
	private LoopingList<Weapon> weapons = new LoopingList<Weapon>();

	public Controller thisControllerPrefab; //temp

	protected override void Start() 
	{
		weapons.Add(GetComponent<Whip>());
		weapons.Add(GetComponent<Bow>());
		activeWeapon = weapons.Current;

		rb = GetComponent<Rigidbody>();
		playerAnimator = GetComponent<PlayerAnim>();
		//Temp, despues ver como SOLIDear asignacion de controller
		ControllerHandler.Instance.RequestAssignation(Instantiate(thisControllerPrefab), this);

		canMove = true;
		rb = GetComponent<Rigidbody>();
		playerAnimator = GetComponent<PlayerAnim>();

		wait_attackCooldown = new WaitForSeconds(attackCooldown);
	}

	private void FixedUpdate() {
		if(rb.velocity.y < 0)
			rb.velocity += Vector3.up * Physics.gravity.y * (fallMultiplier - 1) * Time.fixedDeltaTime;
		else if(rb.velocity.y > 0 && !holdingJump)
			rb.velocity += Vector3.up * Physics.gravity.y * (lowJumpMultiplier - 1) * Time.fixedDeltaTime;
	}

	public void Jump() {
		if(Physics.Raycast(transform.position, Vector3.down, GROUNDED_DISTANCE, validFloorLayers))
		{
			rb.velocity = Vector3.up * forceJump;
			playerAnimator.TriggerAction(0);
			holdingJump = true;
		}
	}

	public void ReleaseJump() => holdingJump = false;

	public override void Move(Vector2 direction) {
		rb.velocity = (new Vector3(direction.x * speed, rb.velocity.y, 0));
		playerAnimator.SetSpeeds(direction);
	}

	public void Attack(Vector2 direction) {
		if(canAttack)
			activeWeapon.Attack(direction);
	}

	public void SwitchWeapons() =>
		activeWeapon = weapons.Next;


	private void OnCollisionEnter(Collision collision) {
		if(collision.collider.gameObject.layer == 9)
			canMove = true;
	}

	public override void Die(IDamager source) => throw new System.NotImplementedException();
}

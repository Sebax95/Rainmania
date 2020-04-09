using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using CustomMSLibrary.Core;
using CustomMSLibrary.Unity;

public class Player : Character {
	private const float GROUNDED_DISTANCE = 1.1f;


	public float speed;
	public float forceJump;
	public float fallMultiplier = 2.5f;
	public float lowJumpMultiplier = 2f;
	public LayerMask validFloorLayers;

	private Rigidbody rb;
	private PlayerAnim playerAnimator;
	//private Vector3 movement;
	private bool canMove;

	//Attack
	public GameObject[] attacksWhip;
	public GameObject arrow;
	public float whipDelay;
	public float whipDuration;

	//public float timer;
	public float attackCooldown;
	private WaitForSeconds wait_attackCooldown;
	private bool canAttack = true;

	//public Material weaponColor;
	public MeshRenderer weaponMesh;
	public Material[] weaponMaterials;
	private bool weaponIsWhip;
	Bow _bow;
	Whip _whip;

	/// <summary>
	/// 0: Jump. 1: Attack. 2: Switch weapon
	/// </summary>
	private BoolByte lastFrameInput;

	public Controller thisControllerPrefab; //temp

	void Start() 
	{
		_bow = GetComponent<Bow>();
		_whip = GetComponent<Whip>();
		rb = GetComponent<Rigidbody>();
		anim = GetComponent<Animator>();
		//Temp, despues ver como SOLIDear asignacion de controller
		ControllerHandler.Instance.RequestAssignation(Instantiate(thisControllerPrefab), this);


		// weaponChange.SetColor("_EmissionColor",Color.red );
		attacks[0].SetActive(false);
		attacks[1].SetActive(false);
		attacks[2].SetActive(false);

		canMove = true;
		rb = GetComponent<Rigidbody>();
		playerAnimator = GetComponent<PlayerAnim>();

		wait_attackCooldown = new WaitForSeconds(attackCooldown);
	}

	private void FixedUpdate() {
		if(rb.velocity.y < 0)
			rb.velocity += Vector3.up * Physics.gravity.y * (fallMultiplier - 1) * Time.fixedDeltaTime;
		else if(rb.velocity.y > 0 && !lastFrameInput[0])
			rb.velocity += Vector3.up * Physics.gravity.y * (lowJumpMultiplier - 1) * Time.fixedDeltaTime;
	}

	public void Jump() {
		if(Physics.Raycast(transform.position, Vector3.down, GROUNDED_DISTANCE, validFloorLayers))
		{
			rb.velocity = Vector3.up * forceJump;
			playerAnimator.TriggerAction(0);
		}
	}

	public override void Move(Vector2 direction) {
		rb.velocity = (new Vector3(direction.x * speed, rb.velocity.y, 0));
		playerAnimator.SetSpeeds(direction);
	}

	public void Attack(Vector2 direction) {
		if(weaponIsWhip)
			WhipAttack(direction);
		//TODO: else attacke arco
	}

	public void SwitchWeapons() {
		//TODO: una vez implementado los componentes de armas, cambiar logica
		weaponIsWhip = !weaponIsWhip;
	}

	public void WhipAttack(Vector2 direction) {
		//bool triedAttack = inputs[1] && !lastFrameInput[1];
		(bool horizontal, bool up) attackDirection = (direction.x != 0, direction.y > 0);

		if(canAttack)
		{
			switch(attackDirection)
			{
				//TODO: Usar componente whip una vez hecho 

				//Order: horizontal, vertical
				case var t when t == (false, false):
					DoWhipAttack(attacks[0]);
					break;
				case var t when t == (true, false):
					DoWhipAttack(attacks[0]);
					break;
				case var t when t == (false, true):
					DoWhipAttack(attacks[1]);
					break;
				case var t when t == (true, true):
					DoWhipAttack(attacks[2]);
					break;
			}
			StartCoroutine(Coroutine_AttackCooldown());
		}

	}

	private IEnumerator Coroutine_AttackCooldown() {
		canAttack = false;
		yield return wait_attackCooldown;
		canAttack = true;
	}
	#region depec
	//void WhipNormal() {
	//	timer += 1 * Time.deltaTime;
	//	if(timer >= 0.4f && timer < 0.6f)
	//		attacks[0].SetActive(true);
	//	else if(timer > 0.6f)
	//	{
	//		attacks[0].SetActive(false);
	//	}
	//}

	//void WhipUp() {
	//	timer += 1 * Time.deltaTime;
	//	if(timer >= 0.4f && timer < 0.6f)
	//		attacks[1].SetActive(true);
	//	else if(timer > 0.6f)
	//	{
	//		attacks[1].SetActive(false);
	//	}
	//}
	//void WhipDiag() {
	//	timer += 1 * Time.deltaTime;
	//	if(timer >= 0.4f && timer < 0.6f)
	//		attacks[2].SetActive(true);
	//	else if(timer > 0.6f)
	//	{
	//		attacks[2].SetActive(false);
	//	}
	//}
	#endregion

	private void OnCollisionEnter(Collision collision) {
		if(collision.collider.gameObject.layer == 9)
			canMove = true;
	}

	public override void Damage(int amount, IDamager source) {
		health -= amount;
		if(health < 0)
			Die(source);
	}

	public override void Heal(int amount, IHealer source) {
		health = Mathf.Min(health + amount, maxHealth);
	}

	public override int GetTeam() => GameManager.TEAM_PLAYER;

	public override void Die(IDamager source) => throw new System.NotImplementedException();
}

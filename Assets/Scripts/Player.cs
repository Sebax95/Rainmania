using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using CustomMSLibrary.Core;
using CustomMSLibrary.Unity;

public class Player : Character 
{
	public float speed;
	public float forceJump;
	public float fallMultiplier = 2.5f;
	public float lowJumpMultiplier = 2f;

	Rigidbody rb;
	private Animator anim;
	//private Vector3 movement;
	private bool canMove;

	//Attack
	public GameObject[] attacksWhip;
	public GameObject arrow;
	public float whipDelay;
	public float whipDuration;
	public GameObject bow;
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

	//Jump conditions
	bool _jump;

	/// <summary>
	/// 0: Jump. 1: Attack. 2: Switch weapon
	/// </summary>
	private BoolByte lastFrameInput;

	public Controller thisControllerPrefab; //temp

	void Start() 
	{
		bow.SetActive(false);

		_bow = GetComponent<Bow>();
		_whip = GetComponent<Whip>();
		rb = GetComponent<Rigidbody>();
		anim = GetComponent<Animator>();
		//Temp, despues ver como SOLIDear asignacion de controller
		ControllerHandler.Instance.RequestAssignation(Instantiate(thisControllerPrefab), this);


		//weaponChange.SetColor("_EmissionColor",Color.red );
		attacksWhip[0].SetActive(false);
		attacksWhip[1].SetActive(false);
		attacksWhip[2].SetActive(false);

		canMove = true;
		_jump = true;

		wait_attackCooldown = new WaitForSeconds(attackCooldown);
	}

	private void FixedUpdate() {
		if(rb.velocity.y < 0)
			rb.velocity += Vector3.up * Physics.gravity.y * (fallMultiplier - 1) * Time.fixedDeltaTime;
		else if(rb.velocity.y > 0 && !lastFrameInput[0])
			rb.velocity += Vector3.up * Physics.gravity.y * (lowJumpMultiplier - 1) * Time.fixedDeltaTime;

		if (Input.GetKey(KeyCode.L))
		{
			_whip.Gocha(rb);
			_whip.GetCloseNode();
			_jump = true;
		}

	}

	public override void DoUpdate(Vector3 direction, BoolByte buttons) {
		if(buttons[0] && !lastFrameInput[0])
			Jump();

		//if(canMove)
		Movement(direction);

		if(lastFrameInput[2])
			weaponIsWhip = !weaponIsWhip;

		//if(weaponIsWhip)
		//	weaponMesh.material = weaponMaterials[0];
		//else
		//	weaponMesh.material = weaponMaterials[1];

		WhipInput(direction, buttons);

		lastFrameInput = buttons;

		if (rb.velocity.y >= 0.5f)
			anim.SetBool("IsJump", true);
		anim.SetFloat("Fall", rb.velocity.y);

	}

	void Jump() {
		if(_jump)
		{
			/*canMove = false;
			rb.velocity = new Vector3(rb.velocity.x, 0, 0);*/
			rb.velocity = Vector3.up * forceJump;
			_jump = false;
		}
	}
	
	private void Movement(Vector3 direction) {
		if(direction.x > 0)
			transform.rotation = Quaternion.Euler(0, 90, 0);
		else if(direction.x < 0)
			transform.rotation = Quaternion.Euler(0, 270, 0);
		anim.SetFloat("SpeedX", Mathf.Abs(direction.x));
		rb.velocity = (new Vector3(direction.x * speed, rb.velocity.y, 0));

	}

	private void WhipInput(Vector3 direction, BoolByte inputs) {
		bool triedAttack = inputs[1] && !lastFrameInput[1];
		(bool isWhip, bool horizontal, bool up) attackDirection =
			(weaponIsWhip, direction.x != 0, direction.y > 0);

		#region deprec
		//if(Input.GetKey(KeyCode.W))
		//{
		//	if(Input.GetAxis("Horizontal") != 0)
		//	{
		//		if(triedAttack)
		//		{
		//			isAttackingUp = false;
		//			isAttacking = false;
		//			isAttackingDiag = true;
		//		}
		//	} else if(triedAttack)
		//	{
		//		isAttackingUp = true;
		//		isAttacking = false;
		//		isAttackingDiag = false;
		//	}
		//} else if(triedAttack)
		//{
		//	isAttackingUp = false;
		//	isAttacking = true;
		//	isAttackingDiag = false;
		//}
		#endregion

		if(triedAttack && canAttack)
		{
			bow.SetActive(true);
			switch(attackDirection)
			{
				//Order: isWhip, horizontal, vertical
				//whip
				case var t when t == (true, false, false):
					_whip.WhipAttack(attacksWhip[0], whipDelay, whipDuration);
					break;
				case var t when t == (true, true, false):
					_whip.WhipAttack(attacksWhip[0], whipDelay, whipDuration);
					break;
				case var t when t == (true, false, true):
					_whip.WhipAttack(attacksWhip[1], whipDelay, whipDuration);
					break;
				case var t when t == (true, true, true):
					_whip.WhipAttack(attacksWhip[2], whipDelay, whipDuration);
					break;

				//bow
				case var t when t == (false, false, false):
					_bow.BowNormal(transform);
					anim.SetBool("IsAttack", true);
					anim.SetTrigger("Attack");
					break;
				case var t when t == (false, true, false):
					_bow.BowNormal(transform);
					anim.SetBool("IsAttack", true);
					anim.SetTrigger("Attack");
					break;
				case var t when t == (false, false, true):
					_bow.BowUp(transform);
					anim.SetBool("IsAttack", true);
					anim.SetTrigger("Attack 1");
					break;
				case var t when t == (false, true, true):
					_bow.BowDiag(transform);
					anim.SetBool("IsAttack", true);
					anim.SetTrigger("Attack 0");
					break;
			}
			StartCoroutine(Coroutine_AttackCooldown());
		}
		else
		{
			anim.SetBool("IsAttack", false);
			//bow.SetActive(false);
		}


	}

	private IEnumerator Coroutine_AttackCooldown() {
		canAttack = false;
		yield return wait_attackCooldown;
		bow.SetActive(false);
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

	private void OnCollisionEnter(Collision collision) 
	{
		if(collision.gameObject.layer == 9)
		{
			//canMove = true;
			anim.SetBool("IsJump", false);
			_jump = true;
		}
	}


}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CustomMSLibrary.Core;
using CustomMSLibrary.Unity;

public class Player : MonoBehaviour {
	public float speed;
	public float forceJump;
	public float fallMultiplier = 2.5f;
	public float lowJumpMultiplier = 2f;

	private Rigidbody rb;
	private Animator anim;
	public Vector3 movement;
	bool canMove;

	//Attack
	public GameObject[] attacks;
	public GameObject arrow;

	public float timer;
	bool isAttacking;
	bool isAttackingUp;
	bool isAttackingDiag;

	public Material weaponColor;
	private MeshRenderer weaponMesh;
	public Material[] weaponMaterials;
	bool changeWeapon;

	private bool isTryingJump;
	

	private void Start() {
		// weaponChange.SetColor("_EmissionColor",Color.red );
		attacks[0].SetActive(false);
		attacks[1].SetActive(false);
		attacks[2].SetActive(false);

		canMove = true;
		rb = GetComponent<Rigidbody>();
		anim = GetComponent<Animator>();
	}
	private void FixedUpdate() {
		if(rb.velocity.y < 0)
			rb.velocity += Vector3.up * Physics.gravity.y * (fallMultiplier - 1) * Time.fixedDeltaTime;
		else if(rb.velocity.y > 0 && !isTryingJump)
			rb.velocity += Vector3.up * Physics.gravity.y * (lowJumpMultiplier - 1) * Time.fixedDeltaTime;
	}

	private void Update() {
		if(Input.GetButtonDown("Jump"))
			Jump();

		isTryingJump = Input.GetButton("Jump");

		//if(canMove)
		Movement();

		if(Input.GetKeyDown(KeyCode.K))
			changeWeapon = !changeWeapon;

		if(changeWeapon)
			weaponColor.SetColor("_EmissionColor", Color.blue);
		else
			weaponColor.SetColor("_EmissionColor", Color.red);

		WhipInput();
	}

	void Jump() {
		if(Physics.Raycast(transform.position, Vector3.down, 1.1f, 1 << 9))
		{
			/*canMove = false;
			rb.velocity = new Vector3(rb.velocity.x, 0, 0);*/
			rb.velocity = Vector3.up * forceJump;
		}
	}

	void Movement() {
		anim.SetFloat("SpeedX", movement.x);
		movement = new Vector3(Input.GetAxis("Horizontal"), 0, 0);
		rb.velocity = (new Vector3(movement.x * speed, rb.velocity.y, 0));
	}

	void WhipInput() {
		if(Input.GetKey(KeyCode.W))
		{
			if(Input.GetAxis("Horizontal") != 0)
			{
				if(Input.GetKeyDown(KeyCode.J))
				{
					isAttackingUp = false;
					isAttacking = false;
					isAttackingDiag = true;
				}
			} else if(Input.GetKeyDown(KeyCode.J))
			{
				isAttackingUp = true;
				isAttacking = false;
				isAttackingDiag = false;
			}
		} else if(Input.GetKeyDown(KeyCode.J))
		{
			isAttackingUp = false;
			isAttacking = true;
			isAttackingDiag = false;

		}
		if(changeWeapon)
		{
			if(isAttacking)
				WhipNormal();
			if(isAttackingUp)
				WhipUp();
			if(isAttackingDiag)
				WhipDiag();
		} else
		{
			if(isAttacking)
			{
				BowNormal();
				isAttacking = false;
			}
			if(isAttackingUp)
			{
				BowUp();
				isAttackingUp = false;
			}
			if(isAttackingDiag)
			{
				BowDiag();
				isAttackingDiag = false;
			}
		}


		if(timer > 0.7f)
		{
			isAttacking = false;
			isAttackingUp = false;
			isAttackingDiag = false;

			timer = 0;
		}
	}

	void WhipNormal() {
		timer += 1 * Time.deltaTime;
		if(timer >= 0.4f && timer < 0.6f)
			attacks[0].SetActive(true);
		else if(timer > 0.6f)
		{
			attacks[0].SetActive(false);
		}
	}

	void WhipUp() {
		timer += 1 * Time.deltaTime;
		if(timer >= 0.4f && timer < 0.6f)
			attacks[1].SetActive(true);
		else if(timer > 0.6f)
		{
			attacks[1].SetActive(false);
		}
	}
	void WhipDiag() {
		timer += 1 * Time.deltaTime;
		if(timer >= 0.4f && timer < 0.6f)
			attacks[2].SetActive(true);
		else if(timer > 0.6f)
		{
			attacks[2].SetActive(false);
		}
	}

	void BowNormal() {
		var _arrow = Instantiate(arrow);
		_arrow.transform.position = transform.position + new Vector3(1, 1, 0);
		_arrow.transform.forward = transform.forward;
	}

	void BowUp() {
		var _arrow = Instantiate(arrow);
		_arrow.transform.position = transform.position + new Vector3(0, 2.25f, 0);
		_arrow.transform.forward = transform.up;
	}
	void BowDiag() {
		var _arrow = Instantiate(arrow);
		_arrow.transform.position = transform.position + new Vector3(1.25f, 2, 0);
		_arrow.transform.forward = transform.forward + transform.up;
	}


	private void OnCollisionEnter(Collision collision) {
		if(collision.collider.gameObject.layer == 9)
			canMove = true;
	}


}

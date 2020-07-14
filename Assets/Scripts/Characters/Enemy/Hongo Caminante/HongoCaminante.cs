using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HongoCaminante : Enemy, IDamager {
	public FSM<HongoCaminante> fsm;
	public bool isDeath;
	public bool canJump;
	public float jumpForce;
	public int damage;
	public bool cdDamage = false;
	public Transform groundChecker;
	public float groundCheckDistance = 0.5f;

	public GameObject SourceObject => gameObject;

	protected override void Awake() {
		base.Awake();
		fsm = new FSM<HongoCaminante>(this);
		fsm.AddState(StatesEnemies.Idle, new IdleStateCaminante(this, fsm));
		fsm.AddState(StatesEnemies.Walk, new PatrolState(this, fsm));
		fsm.AddState(StatesEnemies.Attack, new AttackStateCaminante(this, fsm));
	}

	protected override void Start() {
		base.Start();
		fsm.SetState(StatesEnemies.Walk);
		viewEnem.ActivateBool(1, true);
		canJump = true;
	}
	private void Update() {
		if(isDeath)
			return;
		fsm.Update();
	}

	private void FixedUpdate() {
		if(isDeath)
			return;
		fsm.FixedUpdate();
	}

	public override void Damage(int amount, IDamager source) {
		if(!source.GetTeam.CanDamage(myTeam))
			return;
		if(cdDamage)
			return;
		cdDamage = true;
		rb.mass = 100;
		StatesEnemies tempState = fsm.ActualState;
		fsm.SetState(StatesEnemies.Idle);
		StartCoroutine(CdDamage(tempState));
		health -= amount;
		viewEnem.ActivateTriggers(0);
		if(health < 0)
			Die(source);

	}
	IEnumerator CdDamage(StatesEnemies state) {
		yield return new WaitForSeconds(1);
		cdDamage = false;
		rb.mass = 1;
		fsm.SetState(state);

	}

	public override void Die(IDamager source) {
		isDeath = true;
		viewEnem.ActivateBool(2, true);
		Destroy(gameObject, 2);
	}

	private void OnCollisionEnter(Collision collision) {
		var obj = collision.gameObject.GetComponent<Character>();
		if(obj)
			obj.Damage(damage, this);
	}

	public RaycastHit GroundChecker() {
		RaycastHit hit;
		Ray ray = new Ray(groundChecker.position, -groundChecker.up * groundCheckDistance);
		//Debug.DrawRay(ray.origin, ray.direction, Color.red);
		Physics.Raycast(ray, out hit, groundCheckDistance, groundMask);
		return hit;
	}

	public RaycastHit FrontChecker() {
		RaycastHit hit;
		Ray ray = new Ray(groundChecker.position, groundChecker.forward * groundCheckDistance);
		//Debug.DrawRay(groundChecker.position, groundChecker.forward * groundCheckDistance, Color.red);
		Physics.Raycast(ray, out hit, groundCheckDistance, gameAreaMask);
		return hit;
	}

	IEnumerator CdJump() {
		yield return new WaitForSeconds(cdTimer);
		canJump = true;
	}

	public IEnumerator Jump() {
		StartCoroutine(CdJump());
		viewEnem.ActivateBool(1, false);
		viewEnem.ActivateTriggers(1);
		rb.AddForce(Vector3.up * jumpForce + transform.forward * 2, ForceMode.Impulse);
		bool inGround = false;
		yield return new WaitForSeconds(0.1f);
		do
		{
			if(Physics.Raycast(transform.position, -transform.up * 0.2f, groundMask))
			{
				viewEnem.ActivateBool(1, true);
				inGround = true;
			}
			yield return new WaitForEndOfFrame();
		}
		while(!inGround);
	}

	private void OnDrawGizmosSelected() {
		Vector3 pos = groundChecker.position;
		Gizmos.color = Color.red;
		Gizmos.DrawLine(pos, pos + (-groundChecker.up * groundCheckDistance));
		Gizmos.DrawLine(pos, pos + (groundChecker.forward * groundCheckDistance));
		Gizmos.color = Color.cyan;
		Gizmos.DrawSphere(pos, 0.1f);
	}

}

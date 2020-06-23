using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Security.Cryptography;
using UnityEngine;

public class GreenEnemy : Enemy
{

	public bool isInvulnerable;
    [Header("Green Enemy Variables")]
	public FSM<GreenEnemy> fsm;
    public bool isDeath;

    [Header("Jump Variables")]
	public SphereCollider jumpingPad;
	public float forceJump;

    [Header("Shoot Variables")]
    public bool useParabola;
    [Range(-5f, 5f)]
	public float altBullet = 2;
	private float altBulletSave;
	public bool shootWithGravity;
	public PoisonBullet bulletPref;
	public bool canShoot;
	private bool _canJump;
	public float cdJump;

	protected override void Awake() {
		base.Awake();
		fsm = new FSM<GreenEnemy>(this);
		jumpingPad = GetComponent<SphereCollider>();
		fsm.AddState(StatesEnemies.Idle, new IdleState(this, fsm));
		fsm.AddState(StatesEnemies.Shoot, new ShootState(this, fsm));
	}

	protected override void Start() {
		base.Start();
		_canJump = true;
		altBulletSave = altBullet;
		fsm.SetState(StatesEnemies.Idle);
	}

	public void Update() {
        if (isDeath) return;
		fsm.Update();
    }
    public void FixedUpdate() {
        if (isDeath) return;    
        fsm.FixedUpdate();
	}

    public override void Damage(int amount, IDamager source)
    {
        if (!source.GetTeam.CanDamage(myTeam) || isInvulnerable)
            return;
        health -= amount;
        viewEnem.ActivateTriggers(2);
        if (health <= 0)
            Die(source);

    }
    public override void Die(IDamager source)
    {
        isDeath = true;
        viewEnem.ActivateBool(0, true);
        Destroy(gameObject, 2);
    }

    public void Shoot() {
		if(canShoot)
		{
			viewEnem.ActivateTriggers(0);
			canShoot = false;
		}
	}	

	public void ShootBullet() {
        if (!target) return;
		var obj = Instantiate(bulletPref, output.transform.position, Quaternion.identity);
		obj.transform.right = output.transform.right;
		obj.AssignTeam = GetTeam;

		var dist = Vector3.Distance(transform.position, target.transform.position) / 3;
		if (altBullet >= 0)
		{
			if (!shootWithGravity)
				altBullet += dist;
		}
		else
		{
			if (!shootWithGravity)
				altBullet += -dist;
			obj.gravity.y *= -1;
		}

		if (useParabola)
		{
			obj.useGravity = true;
			obj.rb.velocity = ParabolicShot(target.transform, altBullet, obj.gravity);
			altBullet = altBulletSave;
		}
		else
        {

			obj.useGravity = false;
			obj.transform.forward = transform.forward;
		}

		StartCoroutine(CdShoot());
	}

    public Vector3 ParabolicShot(Transform target, float height, Vector3 gravity)
    {
        float displacementY = target.position.y - output.position.y;
        Vector3 displacementXZ = new Vector3(target.position.x - output.position.x, 0, target.transform.position.z - output.position.z);

        float time = Mathf.Sqrt(Mathf.Abs(-2 * height / gravity.y)) + Mathf.Sqrt(Mathf.Abs(2 * (displacementY - height) / gravity.y));

        Vector3 velocityY = Vector3.up * Mathf.Sqrt(Mathf.Abs(2 * gravity.y * height));
        Vector3 velocityXZ = displacementXZ / time;

        return velocityXZ + velocityY * -Mathf.Sign(gravity.y);
    }

    IEnumerator CdShoot() {
		yield return new WaitForSeconds(cdTimer);
		canShoot = true;
	}
	private void OnCollisionEnter(Collision collision) {
		if(collision.GetContact(0).thisCollider == jumpingPad)
		{
			var jump = collision.transform.GetComponent<IAppliableForce>();
			if(jump != null)
            {
				jump.ApplyForce(Vector3.up * forceJump, ForceMode.Impulse);
                viewEnem.ActivateTriggers(1);
            }
		}
	}

	private void OnTriggerEnter(Collider other)
	{
		/*if(other..GetContact(0).thisCollider == jumpingPad)
		{*/
		var jump = other.transform.GetComponent<IAppliableForce>();
		if (jump != null)
		{
			/*if (_canJump)
			{
				_canJump = false;
				StartCoroutine(CdJump());*/
				jump.ApplyForce(Vector3.up * forceJump, ForceMode.Impulse);
				viewEnem.ActivateTriggers(1);
			//}
		}

		//}
	}

	IEnumerator CdJump()
	{
		yield return new  WaitForSeconds(cdJump);
		_canJump = true;
	}
	
}
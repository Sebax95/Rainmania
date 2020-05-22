﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum StatesGreenEnemy {
	Idle,
	Shoot
}

public class GreenEnemy : Enemy {

    [Header("Green Enemy Variables")]
	public FSM<GreenEnemy> fsm;
    public bool isDeath;
	public GreenEnemyView viewEnem;

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


	protected override void Awake() {
		base.Awake();
		viewEnem = GetComponent<GreenEnemyView>();
		fsm = new FSM<GreenEnemy>(this);
		jumpingPad = GetComponent<SphereCollider>();
		fsm.AddState(StatesGreenEnemy.Idle, new IdleState(this, fsm));
		fsm.AddState(StatesGreenEnemy.Shoot, new ShootState(this, fsm));
	}

	protected override void Start() {
		base.Start();
		altBulletSave = altBullet;
		fsm.SetState(StatesGreenEnemy.Idle);
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
        if (!source.GetTeam.CanDamage(myTeam))
            return;
        health -= amount;
        viewEnem.ActivateTriggers(2);
        if (health < 0)
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

	public Vector3 ParabolicShot(Transform target, float height, Vector3 gravity) {
		float displacementY = target.position.y - output.position.y;
		Vector3 displacementXZ = new Vector3(target.position.x - output.position.x, 0, target.transform.position.z - output.position.z);

		float time = Mathf.Sqrt(Mathf.Abs(-2 * height / gravity.y)) + Mathf.Sqrt(Mathf.Abs(2 * (displacementY - height) / gravity.y));

		Vector3 velocityY = Vector3.up * Mathf.Sqrt(Mathf.Abs(2 * gravity.y * height));
		Vector3 velocityXZ = displacementXZ / time;

		return velocityXZ + velocityY * -Mathf.Sign(gravity.y);
	}

	public void ShootBullet() {
		var obj = Instantiate(bulletPref, output.transform.position, Quaternion.identity);
		obj.transform.right = output.transform.right;
		obj.AssignTeam = GetTeam;

		var dist = Vector3.Distance(transform.position, target.transform.position) / 3;
		if(altBullet >= 0)
		{
			if(!shootWithGravity)
				altBullet += dist;
		} else
		{
			if(!shootWithGravity)
				altBullet += -dist;
			obj.gravity.y *= -1;
		}

        if(useParabola)
        {
            obj.useGravity = true;
            obj.rb.velocity = ParabolicShot(target.transform, altBullet, obj.gravity);
		    altBullet = altBulletSave;
        }
        else
        {
            obj.useGravity = false;
            obj.rb.velocity = (target.transform.position - transform.position).normalized * obj.force + (Vector3.up * 0.5f);
        }
		StartCoroutine(CdShoot());
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
}
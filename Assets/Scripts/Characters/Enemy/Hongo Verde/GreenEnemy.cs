using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Security.Cryptography;
using UnityEngine;

public class GreenEnemy : Enemy {

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


	protected override void Awake() {
		base.Awake();
		fsm = new FSM<GreenEnemy>(this);
		jumpingPad = GetComponent<SphereCollider>();
		fsm.AddState(StatesEnemies.Idle, new IdleState(this, fsm));
		fsm.AddState(StatesEnemies.Shoot, new ShootState(this, fsm));
	}

	protected override void Start() {
		base.Start();
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

	public void ShootBullet() {
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
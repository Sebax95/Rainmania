using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Security.Cryptography;
using UnityEngine;

public abstract class MushroomEnemy : Enemy
{

	public bool isInvulnerable;
    [Header("Green Enemy Variables")]
	public FSM<MushroomEnemy> fsm;
    public bool isDeath;

    [Header("Jump Variables")]
	public SphereCollider jumpingPad;
	public float forceJump;

   
	public PoisonBullet bulletPref;
	public bool canShoot;
	public bool cdDamage;
	public float damageTimer;
	
	[Header("Sonidos")]
	public AudioClip shootSound;
	public AudioClip jumpingPadSound;
	public AudioClip hitSound;
	public AudioClip dieSound;

	protected override void Awake() {
		base.Awake();
		fsm = new FSM<MushroomEnemy>(this);
		jumpingPad = GetComponent<SphereCollider>();
		cdDamage = false;
		fsm.AddState(StatesEnemies.Idle, new IdleState(this, fsm));
		fsm.AddState(StatesEnemies.Shoot, new ShootState(this, fsm));
		
	}

	protected override void Start() {
		base.Start();
		fsm.SetState(StatesEnemies.Idle);
		canShoot = true;
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
        if (!source.GetTeam.CanDamage(myTeam) || isInvulnerable || cdDamage || isDeath)
            return;
        Health -= amount;
        cdDamage = true;
        StartCoroutine(CdDamage());
        viewEnem.SetAudioClip(hitSound);
        viewEnem.Au.Play();
        viewEnem.DamageFeedback();
        viewEnem.ActivateTriggers(2);
        if (Health <= 0)
            Die(source);
    }
    public override void Die(IDamager source)
    {
        isDeath = true;
        viewEnem.ActivateBool(0, true);
        rb.isKinematic = true;
        GetComponent<CapsuleCollider>().enabled = false;
        GetComponent<SphereCollider>().enabled = false;
        Destroy(gameObject, 2);
    }

    public abstract void Shoot();

    public abstract void ShootBullet();
    
    public IEnumerator CdShoot() {
		yield return new WaitForSeconds(cdTimer);
		canShoot = true;
	}

    public IEnumerator CdDamage()
    {
	    yield return new WaitForSeconds(damageTimer);
	    cdDamage = false;
    }
    private void OnTriggerEnter(Collider other)
	{
		var jump = other.transform.GetComponent<IAppliableForce>();
		if (jump != null)
		{
			jump.ApplyForce(Vector3.up * forceJump, ForceMode.Impulse);
			viewEnem.ActivateTriggers(1);
			viewEnem.SetAudioClip(jumpingPadSound);
			viewEnem.Au.Play();
		}
	}
}
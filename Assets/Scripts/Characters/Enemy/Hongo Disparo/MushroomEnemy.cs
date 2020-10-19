using System;
using System.Collections;
using UnityEngine;

public abstract class MushroomEnemy : Enemy
{
	public bool isInvulnerable;
    [Header("Green Enemy Variables")]
	public FSM<MushroomEnemy> fsm;
    //public bool isDeath;

    [Header("Jump Variables")]
	public SphereCollider jumpingPad;
	public float forceJump;

   
	public PoisonBullet bulletPref;
	public bool canShoot;

	protected ReusablePool<PoisonBullet> _bulletPool;
	//public bool cdDamage;
	//public float damageTimer;
	
	[Header("Sonidos")]
	public AudioClip shootSound;
	public AudioClip jumpingPadSound;
	public AudioClip hitSound;
	public AudioClip dieSound;

	protected override void Awake() {
		base.Awake();
		fsm = new FSM<MushroomEnemy>(this);
		jumpingPad = GetComponent<SphereCollider>();
		isInvicible = false;
		fsm.AddState(StatesEnemies.Idle, new IdleState(this, fsm));
		fsm.AddState(StatesEnemies.Shoot, new ShootState(this, fsm));
		
	}

	protected override void Start() {
		base.Start();
		fsm.SetState(StatesEnemies.Idle);
		canShoot = true;
		_bulletPool =
			new ReusablePool<PoisonBullet>(bulletPref, 5, true ,10f);
	}

	public void Update() {
        if (isDead) return;
		fsm.Update();
    }
    public void FixedUpdate() {
        if (isDead) return;    
        fsm.FixedUpdate();
	}

    public override bool Damage(int amount, IDamager source)
    {
        if (!source.GetTeam.CanDamage(myTeam) || isInvulnerable || isInvicible || isDead)
            return false;
        Health -= amount;
        StartCoroutine(Coroutine_InvinsibleTime());
        viewEnem.SetAudioClip(hitSound);
        viewEnem.Au.Play();
        viewEnem.DamageFeedback();
        viewEnem.ActivateTriggers(2);
        if (Health <= 0)
            Die(source);
		return true;
    }
    public override void Die(IDamager source)
    {
        isDead = true;
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
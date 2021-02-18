using System;
using System.Collections;
using UnityEngine;

public abstract class MushroomEnemy : Enemy
{
	public bool isInvulnerable;
    [Header("Green Enemy Variables")]
	private FSM<MushroomEnemy> fsm;
    //public bool isDeath;

    [Header("Jump Variables")]
	private SphereCollider jumpingPad;
	public float forceJump;
	
	public bool canShoot;

	public Team team;

	public override void Reset()
	{
		base.Reset();
		canShoot = true;
		isInvicible = false;
		fsm.SetState(StatesEnemies.Idle);
	}

	protected override void Awake() {
		base.Awake();
		myTeam = team;
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
	}

	protected override void OnUpdate(){
        if (isDead) return;
		fsm.Update();
    }
	protected override void OnFixedUpdate(){
        if (isDead) return;    
        fsm.FixedUpdate();
	}

    public override bool Damage(int amount, IDamager source)
    {
        if (!source.GetTeam.CanDamage(myTeam) || isInvulnerable || isInvicible || isDead)
            return false;
        Health -= amount;
        StartCoroutine(Coroutine_InvinsibleTime());
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
        viewEnem.PlaySound(EnemyView.AudioEnemys.Die);
        GetComponent<CapsuleCollider>().enabled = false;
        GetComponent<SphereCollider>().enabled = false;
        TurnOff(this, 2f);
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
			viewEnem.PlaySound(EnemyView.AudioEnemys.JumpingPad);
			viewEnem.Au.Play();
		}
	}
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class HongoCaminante : Enemy
{
    protected FSM<HongoCaminante> fsm;
    public bool isDeath;
    public bool canJump;
    public float jumpForce;
    public int damage;
    public bool cdDamage = false;
    public Transform groundChecker;
    public bool stopCor = false;
    public LayerMask frontCheckerLayer;

    protected override void Awake()
    {
        target = FindObjectOfType<Player>();
        viewEnem = GetComponentInChildren<EnemyView>();
        rb = GetComponent<Rigidbody>();
        fsm = new FSM<HongoCaminante>(this);
        fsm.AddState(StatesEnemies.Idle, new IdleStateCaminante(this, fsm));
        fsm.AddState(StatesEnemies.Walk, new PatrolState(this, fsm));
        fsm.AddState(StatesEnemies.Attack, new AttackStateCaminante(this, fsm));
    }

    protected override void Start()
    {
        base.Start();
        fsm.SetState(StatesEnemies.Walk);
        canJump = true;
    }
    private void Update()
    {
        if (isDeath) return;
        fsm.Update();
    }

    private void FixedUpdate()
    {
        if (isDeath) return;
        fsm.FixedUpdate();
    }
    
    public override void Damage(int amount, IDamager source)
    {
        if (!source.GetTeam.CanDamage(myTeam))
            return;
        if (cdDamage) return;
        cdDamage = true;
        rb.mass = 100;
        StatesEnemies tempState = fsm.ActualState;
        fsm.SetState(StatesEnemies.Idle);
        viewEnem.DamageFeedback();
        StartCoroutine(CdDamage(tempState));
        Health -= amount;
        //viewEnem.ActivateTriggers(0);
        if (Health < 0)
            Die(source);

    }
    IEnumerator CdDamage(StatesEnemies state)
    {
        yield return new WaitForSeconds(1);
        cdDamage = false;
        rb.mass = 1;
        fsm.SetState(state);
    }

    public override void Die(IDamager source)
    {
        isDeath = true;
        viewEnem.ActivateTriggers(1);
        rb.isKinematic = true;
        GetComponent<CapsuleCollider>().enabled = false;
        Destroy(gameObject, 2);
    }

    private void OnCollisionEnter(Collision collision)
    {
        var obj = collision.gameObject.GetComponent<Character>();
        if (obj)
        {
            var jump = obj.gameObject.GetComponent<IAppliableForce>();
            if (jump != null)
            {
                jump.ApplyForce((Vector3.up * 4) + (-transform.forward * 2), ForceMode.Impulse);
                obj.Damage(damage, this);
            }
        }
    }

    public RaycastHit GroundChecker()
    {
        RaycastHit hit;
        Debug.DrawRay(groundChecker.position, -groundChecker.up * 0.5f, Color.red);
        Physics.Raycast(groundChecker.position, -groundChecker.up, out hit, 0.5f, groundMask);
        return hit;
    }
    public RaycastHit FrontChecker()
    {
        RaycastHit hit;
        Debug.DrawRay(groundChecker.position, groundChecker.forward * 0.5f, Color.red);
        Physics.Raycast(groundChecker.position, groundChecker.forward, out hit, 0.5f, frontCheckerLayer);
        return hit;
    }

    protected IEnumerator CdJump()
    {
        yield return new WaitForSeconds(cdTimer);
        canJump = true;
    }

    public void ChangeState(StatesEnemies state) => fsm.SetState(state);
    
    public abstract IEnumerator Attack();
}

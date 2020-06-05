using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HongoCaminante : Enemy, IDamager
{
    public FSM<HongoCaminante> fsm;
    public bool isDeath;
    public bool canJump;
    public float jumpForce;
    public int damage;
    public bool cdDamage = false;
    public Transform groundChecker;

    public GameObject SourceObject => gameObject;

    protected override void Awake()
    {
        base.Awake();
        fsm = new FSM<HongoCaminante>(this);
        fsm.AddState(StatesEnemies.Idle, new IdleStateCaminante(this, fsm));
        fsm.AddState(StatesEnemies.Walk, new PatrolState(this, fsm));
        fsm.AddState(StatesEnemies.Attack, new AttackStateCaminante(this, fsm));
    }

    protected override void Start()
    {
        base.Start();
        fsm.SetState(StatesEnemies.Walk);
        viewEnem.ActivateBool(1, true);
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
        StatesEnemies tempState = fsm.ActualState;
        fsm.SetState(StatesEnemies.Idle);
        StartCoroutine(CdDamage(tempState));
        health -= amount;
        viewEnem.ActivateTriggers(0);
        if (health < 0)
            Die(source);

    }
    IEnumerator CdDamage(StatesEnemies state)
    {
        yield return new WaitForSeconds(1);
        cdDamage = false;
        fsm.SetState(state);

    }

    public override void Die(IDamager source)
    {
        isDeath = true;
        viewEnem.ActivateBool(2, true);
        Destroy(gameObject, 2);
    }

    private void OnCollisionEnter(Collision collision)
    {
        var obj = collision.gameObject.GetComponent<Character>();
        if (obj)
            obj.Damage(damage, this);
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
        Physics.Raycast(groundChecker.position, groundChecker.forward, out hit, 0.5f, groundMask);
        return hit;
    }

    IEnumerator CdJump()
    {
        yield return new WaitForSeconds(cdTimer);
        canJump = true;
    }

    public IEnumerator Jump()
    {
        StartCoroutine(CdJump());
        viewEnem.ActivateBool(1, false);
        viewEnem.ActivateTriggers(1);
        rb.AddForce(Vector3.up * jumpForce + transform.forward * 2, ForceMode.Impulse);
        bool inGround = false;
        RaycastHit hit;
        yield return new WaitForSeconds(0.1f);
        do
        {
            if (Physics.Raycast(transform.position, -transform.up * 0.2f, out hit, groundMask))
            {
                viewEnem.ActivateBool(1, true);
                inGround = true;
            }
            yield return new WaitForEndOfFrame();
        }
        while (!inGround);
    }

}

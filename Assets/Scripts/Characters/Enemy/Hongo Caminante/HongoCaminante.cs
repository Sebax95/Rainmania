using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public abstract class HongoCaminante : Enemy
{
    protected FSM<HongoCaminante> fsm;
    public bool isDeath;
    public bool canJump;
    public float jumpForce;
    public int damage;
    public Transform groundChecker;
    public bool stopCor = true;
    public LayerMask frontCheckerLayer;

    //public bool cdDamage = false;

    public override void Reset()
    {
        base.Reset();
        isDeath = false;
        canJump = true;
        stopCor = true;
        GetComponent<CapsuleCollider>().enabled = true;
        rb.isKinematic = false;
        rb.mass = 1;
        fsm.SetState(StatesEnemies.Walk);
    }

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
        stopCor = true;
    }
	protected override void OnUpdate()
    {
        if (isDeath) return;
        fsm.Update();
    }

	protected override void OnFixedUpdate()
    {
        if (isDeath) return;
        fsm.FixedUpdate();
    }
    
    public override bool Damage(int amount, IDamager source)
    {
        var result = base.Damage(amount, source);

        if(!result) return result;

        rb.mass = 100;
        StatesEnemies tempState = fsm.ActualState;
        fsm.SetState(StatesEnemies.Idle);
        viewEnem.DamageFeedback();
        StartCoroutine(ResetStateOnVulnerable(tempState));

        //viewEnem.ActivateTriggers(0);

        return result;

    }
    IEnumerator ResetStateOnVulnerable(StatesEnemies state)
    {
        yield return new WaitForSeconds(invincibleTime);
        rb.mass = 1;
        fsm.SetState(state);
    }

    public override void Die(IDamager source)
    {
        isDeath = true;
        gameObject.layer = 11;
        viewEnem.ActivateBool(1, false);
        viewEnem.ActivateTriggers(1);
        viewEnem.PlaySound(EnemyView.AudioEnemys.Die);
        //GetComponent<CapsuleCollider>().enabled = false;
        TurnOff(this, 2f);
    }

    private void OnCollisionEnter(Collision collision)
    {
        var obj = collision.gameObject.GetComponent<Character>();
        if (obj)
        {
            var jump = obj.gameObject.GetComponent<IAppliableForce>();
            if (jump != null)
            {
                jump.ApplyForce((Vector3.up * 8) + (-transform.forward * 2), ForceMode.Impulse);
                obj.Damage(damage, this);
            }
        }
    }

    public bool GroundChecker()
    {
        RaycastHit hit;
        Debug.DrawRay(groundChecker.position, -groundChecker.up * 0.5f, Color.red);
        if(Physics.Raycast(groundChecker.position, -groundChecker.up, out hit, 0.5f, groundMask))
        {
            if (hit.collider.gameObject.CompareTag("Stairs"))
                return false;
            return true;
        }
        return false;
    }
    public bool FrontChecker()
    {
        RaycastHit hit;
        Debug.DrawRay(groundChecker.position, groundChecker.forward * 0.5f, Color.red);
        if(Physics.Raycast(groundChecker.position, groundChecker.forward, out hit, 0.5f, frontCheckerLayer))
            return true;
        return false;
    }

    protected IEnumerator CdJump()
    {
        yield return new WaitForSeconds(cdTimer);
        canJump = true;
    }

    public void ChangeState(StatesEnemies state) => fsm.SetState(state);
    
    public abstract IEnumerator Attack();
}

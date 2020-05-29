using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HongoCaminante : Enemy
{
    public FSM<HongoCaminante> fsm;
    public bool isDeath;
    public bool canJump;
    public float jumpForce;

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
        fsm.SetState(StatesEnemies.Idle);
        viewEnem.ActivateBool(1, true);
        canJump = true;
    }
    private void Update()
    {
        if (isDeath) return;
        fsm.Update();
        if (Input.GetKeyDown(KeyCode.P))
            StartCoroutine(Jump());
    }

    private void FixedUpdate()
    {
        if (isDeath) return;
        fsm.FixedUpdate();
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

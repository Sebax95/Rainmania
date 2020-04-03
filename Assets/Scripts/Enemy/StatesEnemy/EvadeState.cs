using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EvadeState : State<Enemy>
{
    public EvadeState(Enemy owner, FSM<Enemy> fsm) : base(owner, fsm)
    {
    }
    bool cdDash;
    public override void Enter()
    {
        cdDash = false;
    }

    public override void Exit()
    {
        
    }

    public override void FixedUpdateState()
    {
        
    }

    public override void UpdateState()
    {
        if (_owner.LineOfSight())
            if (Vector3.Distance(_owner.transform.position, _owner.target.transform.position) <= (_owner.viewDistance / 2))
                Dash();
        else
            _fsm.SetState("SimplePatrol");
    }

    void Dash()
    {
        if(!cdDash)
        {
            cdDash = true;
            _owner.rb.AddForce(-_owner.transform.forward * 4, ForceMode.Impulse);
            _owner.StartCoroutine(CdDash());
        }
    }

    IEnumerator CdDash()
    {
        yield return new WaitForSeconds(1.5f);
        cdDash = false;
        _fsm.SetState("SimplePatrol");
    }
}

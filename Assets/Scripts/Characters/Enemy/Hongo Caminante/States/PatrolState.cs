using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CustomMSLibrary.Unity;

public class PatrolState : State<HongoCaminante>
{
    public PatrolState(HongoCaminante owner, FSM<HongoCaminante> fsm) : base(owner, fsm)
    {
    }
    
    public override void Enter()
    {
        _owner.viewEnem.ActivateBool(0, true);
    }

    public override void Exit()
    {
        
    }

    public override void FixedUpdateState()
    {
        if (_owner.LineOfSight())
            if (Vector3.Distance(_owner.transform.position, _owner.target.transform.position) < _owner.viewDistance /2)
                _owner.Move(_owner.transform.position + -(_owner.transform.forward));
            else
                _fsm.SetState(StatesEnemies.Attack);
        else
            _fsm.SetState(StatesEnemies.Idle);
    }

    public override void UpdateState()
    {

    }

}

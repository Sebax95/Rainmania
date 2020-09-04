using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CustomMSLibrary.Unity;

public class PatrolState : State<HongoCaminante>
{
    public PatrolState(HongoCaminante owner, FSM<HongoCaminante> fsm) : base(owner, fsm) { }

    bool isRight;

    public override void Enter()
    {
        _owner.viewEnem.ActivateBool(0, true);
    }

    public override void Exit() { }

    public override void FixedUpdateState()
    {
        if (!_owner.LineOfSight())
            Moving();
        else
            _fsm.SetState(StatesEnemies.Attack);
    }

    void Moving()
    {
        _owner.Move(_owner.transform.forward);
        if (_owner.GroundChecker().collider == false || _owner.FrontChecker().collider == true)
        {
            if (isRight)
            {
                _owner.transform.rotation = Quaternion.Euler(0, -90, 0);
                isRight = false;
            }
            else
            {
                _owner.transform.rotation = Quaternion.Euler(0, 90, 0);
                isRight = true;
            }
        }
    }

    public override void UpdateState() { }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyState : State<FlyingEnemy>
{
    public FlyState(FlyingEnemy owner, FSM<FlyingEnemy> fsm) : base(owner, fsm) { }

    private Vector3 _move;
    private float rand;
    private float count;

    public override void Enter()
    {
        count = 0;
        if(_owner.minRandomAmplitud != _owner.maxRandomAmplitud)
            rand = Random.Range(_owner.minRandomAmplitud, _owner.maxRandomAmplitud);
    }

    public override void UpdateState()
    {
        count = (_owner.contadorOrTime) ?  count + 1 * Time.deltaTime : Time.time;
    }

    public override void FixedUpdateState()
    {
        _move = (Vector3.up * Mathf.Sin((count) * Mathf.PI * _owner.frecuency) * (_owner.amplitud + rand));
        _move += _owner.transform.forward * 2 * _owner.speed;
        _owner.Move(_move);
    }

    public override void Exit()
    {
        count = 0;
        rand = 0;
        _move = Vector3.zero;
    }
}
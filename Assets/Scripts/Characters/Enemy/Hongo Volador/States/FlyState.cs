using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyState : State<FlyingEnemy>
{
    public FlyState(FlyingEnemy owner, FSM<FlyingEnemy> fsm) : base(owner, fsm) { }

    private Vector3 _move;
    private float rand;

    public override void Enter()
    {
        if(_owner.minRandomAmplitud != _owner.maxRandomAmplitud)
            rand = Random.Range(_owner.minRandomAmplitud, _owner.maxRandomAmplitud);
    }

    public override void UpdateState() { }

    public override void FixedUpdateState()
    {

        _move = (Vector3.up * Mathf.Sin((Time.time) * Mathf.PI * _owner.frecuency) * _owner.amplitud);
        _move = _owner.transform.forward *2 * _owner.speed;

        /* _move = new Vector3(_owner.transform.position.x * 2 * _owner.speed * Time.fixedDeltaTime,
             Mathf.Sin( (Time.time + rand)  * Mathf.PI * _owner.frecuency) * _owner.amplitud, 0);*/
        _owner.Move(_move);
    }

    public override void Exit() { }
}
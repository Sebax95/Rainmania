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
        rand = Random.Range(0f, 2f);
    }

    public override void UpdateState() { }

    public override void FixedUpdateState()
    {
     
        _move = new Vector3(_owner.transform.position.x * 2 * _owner.speed * Time.fixedDeltaTime,
            Mathf.Sin( (Time.time + rand)  * Mathf.PI * _owner.frecuency) * _owner.amplitud, 0);
        _owner.Move(_move);
    }

    public override void Exit() { }
}
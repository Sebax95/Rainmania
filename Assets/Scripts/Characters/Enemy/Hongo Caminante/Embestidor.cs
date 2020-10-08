using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Embestidor : HongoCaminante
{
    public BoxCollider headCollider;

    protected override void Start()
    {
        base.Start();
        headCollider.enabled = false;
    }

    public override IEnumerator Attack()
    {
        headCollider.enabled = true;
        var maxTime = cdTimer;
        var waitTime = 0f;
        var tempSpeed = speed;
        viewEnem.ActivateBool(1, true);
        speed = speed * 3;
        while (waitTime < maxTime || !FrontChecker())
        {
            waitTime += Time.deltaTime * 10;
            Move(transform.forward);
            yield return new WaitForSeconds(0.1f);
        }

        speed = tempSpeed;
        viewEnem.ActivateBool(1, false);
        headCollider.enabled = false;
        canJump = true;
    }
}
﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Embestidor : HongoCaminante
{
    public override IEnumerator Attack()
    {
        stopCor = false;
        var tempSpeed = speed;
        viewEnem.ActivateBool(1, true);
        speed = speed * 2;
        StartCoroutine(StopRun());
        do
        {
            Move(transform.forward);
            yield return new WaitForSeconds(0.1f);
        } while (!stopCor || !FrontChecker());
        
        speed = tempSpeed;
        viewEnem.ActivateBool(1, false);
        canJump = true;
    }

    IEnumerator StopRun()
    {
        yield return new WaitForSeconds(cdTimer);
        stopCor = true;
    }
}
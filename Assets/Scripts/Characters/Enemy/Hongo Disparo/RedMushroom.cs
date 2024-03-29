﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RedMushroom : MushroomEnemy
{
    
    public override void Shoot()
    {
        if(!canShoot) return;
        if(Physics.Raycast(transform.position, transform.forward * viewDistance, viewDistance * viewDistance,  1 >> 15)) return;
        canShoot = false;
        viewEnem.ActivateTriggers(0);
    }
    public override void ShootBullet()
    {
        if(target == null || isDead) return;
        StartCoroutine(CdShoot());
        var obj = bulletPool.GetObject();
        obj.SetSource(this);
        obj.SetValues(output.transform.position, output.transform.forward, false);
        obj.AssignTeam = GetTeam;
        viewEnem.PlaySound(EnemyView.AudioEnemys.Attack);
        viewEnem.Au.Play();
        
        obj.useGravity = false;
        obj.transform.forward = transform.forward;
    }
}

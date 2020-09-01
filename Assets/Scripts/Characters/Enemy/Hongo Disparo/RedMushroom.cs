using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RedMushroom : MushroomEnemy
{
    
    public override void Shoot()
    {
        base.Shoot();
        viewEnem.ActivateTriggers(0);
    }
    public override void ShootBullet()
    {
        if(target == null) return;
        var obj = Instantiate(bulletPref, output.transform.position, Quaternion.identity);
        obj.transform.right = output.transform.right;
        obj.AssignTeam = GetTeam;
        viewEnem.SetAudioClip(shootSound);
        viewEnem.Au.Play();
        
        obj.useGravity = false;
        obj.transform.forward = transform.forward;
        StartCoroutine(CdShoot());
    }
}

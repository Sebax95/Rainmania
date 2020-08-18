using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RedMushroom : MushroomEnemy
{
    public override void ShootBullet()
    {
        if(target == null) return;
        var obj = Instantiate(bulletPref, output.transform.position, Quaternion.identity);
        obj.transform.right = output.transform.right;
        obj.AssignTeam = GetTeam;

        var dist = Vector3.Distance(transform.position, target.transform.position) / 3;
        obj.useGravity = false;
        obj.transform.forward = transform.forward;
        StartCoroutine(CdShoot());
    }
}

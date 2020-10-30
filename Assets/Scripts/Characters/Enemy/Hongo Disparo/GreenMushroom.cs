using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GreenMushroom : MushroomEnemy
{
    [Header("Shoot Variables")]
    [Range(-5f, 5f)]
    public float altBullet = 2;
    private float _altBulletSave;
    public bool shootWithGravity;

    protected override void Start()
    {
        base.Start();
        _altBulletSave = altBullet;
    }

    public override void Shoot()
    {
        if(!canShoot) return;
       
        canShoot = false;
        viewEnem.ActivateTriggers(3);
    }

    public override void ShootBullet()
    {
        if (!target || isDead) return;
        StartCoroutine(CdShoot());
        var obj = bulletPool.GetObject();
        if(!obj) return;
        obj.SetSource(this);
        obj.SetValues(output.transform.position, output.transform.forward, true);
        obj.AssignTeam = GetTeam;
        
        viewEnem.PlaySound(EnemyView.AudioEnemys.Attack);
        viewEnem.Au.Play();
        
        var dist = Vector3.Distance(transform.position, target.transform.position) / 3;
        if (altBullet >= 0)
        {
            if (!shootWithGravity)
                altBullet += dist;
        }
        else
        {
            if (!shootWithGravity)
                altBullet += -dist;
            obj.gravity.y *= -1;
        }
        
        obj.useGravity = true;
        obj.rb.velocity = ParabolicShot(target.transform, altBullet, obj.gravity);
        altBullet = _altBulletSave;
    }
}

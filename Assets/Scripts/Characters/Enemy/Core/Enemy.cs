using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CustomMSLibrary.Unity;

[SelectionBase]
public abstract class Enemy : Character, IDamager
{
    [Header("Enemy Variables", order = 0)] public float speed;
    public EnemyView viewEnem;
    public LayerMask groundMask;
    protected Vector3 startPos;

    [Header("Shooting", order = 2)] public float cdTimer;
    public Transform output;

    [Header("Line Of Sight", order = 1)] public float viewAngle;
    public float viewDistance;
    public Vector3 offsetLOS;
    private Vector3 _posLOS;
    private Vector3 _dirToTarget;
    private float _anglesToAngle;
    private float _distanceToTarget;
    public Vector3 lastPosition;
    public LayerMask gameAreaMask;

    public bool showGizmos;

    public Spawner spawner;
    [HideInInspector] public Player target;
    public ReusablePool<PoisonBullet> bulletPool;
    public PoisonBullet bulletPref;

    public static void TurnOn(Enemy e)
    {
        e.gameObject.SetActive(true);
        e.Reset();
    }
    
    public void ReturnBullet(PoisonBullet p) => bulletPool.DisableObject(p);


    public static void TurnOff(Enemy e) => e.gameObject.SetActive(false);

    public static void TurnOff(Enemy e, float time) => e.StartCoroutine(e.WaitToOff(e, time));

    IEnumerator WaitToOff(Enemy e, float time)
    {
        yield return new WaitForSeconds(time);
        TurnOff(e);
    }

    public virtual void Reset()
    {
        Health = maxHealth;
        transform.position = startPos;
        isDead = false;
    }

    protected override void Start()
    {
        base.Start();
        startPos = transform.position;
    }

    public void SetValues(Vector3 pos, Vector3 forw)
    {
        transform.position = pos;
        transform.forward = forw;
    }

    protected virtual void Awake()
    {
        target = FindObjectOfType<Player>();
        viewEnem = GetComponent<EnemyView>();
        rb = GetComponent<Rigidbody>();
        if(bulletPref!= null)
            bulletPool = new ReusablePool<PoisonBullet>(bulletPref, 5, PoisonBullet.Enable, PoisonBullet.Disable, false);
    }

    public bool LineOfSight()
    {
        if (target == null) return false;
        _posLOS = transform.position + offsetLOS;
        _dirToTarget = target.transform.position - transform.position;
        _anglesToAngle = Vector3.Angle(transform.forward, _dirToTarget);
        _distanceToTarget = Vector3.Distance(_posLOS, target.transform.position);
        if (_anglesToAngle <= viewAngle && _distanceToTarget <= viewDistance)
        {
            RaycastHit rch;
            bool obstacleBetween = false;
            if (Physics.Raycast(_posLOS, _dirToTarget, out rch, _distanceToTarget))
                if (gameAreaMask.ContainsLayer(rch.collider.gameObject.layer))
                    obstacleBetween = true;
            if (!obstacleBetween)
            {
                lastPosition = target.transform.position;
                return true;
            }
            else
                return false;
        }
        else
            return false;
    }

    public override void Move(Vector2 direction)
    {
        var tempVel = rb.velocity;
        Vector3 newVel = new Vector3(direction.x * speed, tempVel.y);
        rb.velocity = newVel;
    }

    public Vector3 ParabolicShot(Transform tar, float height, Vector3 gravity)
    {
        float displacementY = tar.position.y - output.position.y;
        Vector3 displacementXZ = new Vector3(tar.position.x - output.position.x, 0, tar.position.z - output.position.z);

        float time = Mathf.Sqrt(Mathf.Abs(-2 * height / gravity.y)) +
                     Mathf.Sqrt(2 * (displacementY - height) / gravity.y);

        Vector3 velocityY = Vector3.up * Mathf.Sqrt(-2 * gravity.y * height);
        Vector3 velocityXZ = displacementXZ / time;

        return velocityXZ + (velocityY * -Mathf.Sign(gravity.y));

    }

    private void OnDrawGizmosSelected()
    {
        if (!showGizmos) return;
        var posLOS = transform.position + offsetLOS;
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(posLOS, viewDistance);

        Gizmos.color = Color.cyan;
        Gizmos.DrawLine(posLOS, posLOS + (transform.forward * viewDistance));

        Vector3 rightLimit = Quaternion.AngleAxis(viewAngle, transform.up) * transform.forward;
        Gizmos.DrawLine(posLOS, posLOS + (rightLimit * viewDistance));

        Vector3 leftLimit = Quaternion.AngleAxis(-viewAngle, transform.up) * transform.forward;
        Gizmos.DrawLine(posLOS, posLOS + (leftLimit * viewDistance));

        Vector3 upLimit = Quaternion.AngleAxis(-viewAngle, transform.right) * transform.forward;
        Gizmos.DrawLine(posLOS, posLOS + (upLimit * viewDistance));

        Vector3 downLimit = Quaternion.AngleAxis(viewAngle, transform.right) * transform.forward;
        Gizmos.DrawLine(posLOS, posLOS + (downLimit * viewDistance));
    }
    
    protected override void OnDestroy()
    {
        bulletPool?.Clear();
    }
}
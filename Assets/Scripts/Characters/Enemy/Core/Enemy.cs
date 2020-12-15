using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CustomMSLibrary.Unity;
using UnityEditor;

[SelectionBase]
public abstract class Enemy : Character, IDamager
{
    [Header("Enemy Variables", order = 0)] public float speed;
    public EnemyView viewEnem;
    public LayerMask groundMask;
    protected Vector3 startPos;

    [Header("Shooting")]
    public float cdTimer;
    public Transform output;

    [Header("Line Of Sight")] 
    public bool needLOS;
    public float viewAngle;
    public float viewDistance;
    public Transform offsetLOS;
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

    private void OnEnable()
    {
        if (isDead)
        {
            gameObject.SetActive(false); //TODO: arreglar esta crotedad
        }
    
    }

    public static void TurnOn(Enemy e)
    {
        e.gameObject.SetActive(true);
        e.Reset();
    }
    
    public void ReturnBullet(PoisonBullet p) => bulletPool.DisableObject(p);


    public static void TurnOff(Enemy e) => e.gameObject.SetActive(false);

    public static void TurnOff(Enemy e, float time) => GameManager.Instance.StartCoroutine(e.WaitToOff(e, time));

    IEnumerator WaitToOff(Enemy e, float time)
    {
        yield return new WaitForSeconds(time);
        TurnOff(e);
    }

    public virtual void Reset()
    {
        Health = maxHealth;
        transform.position = startPos.ZeroZ();
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
        if (target == null || !needLOS) return false;
        _dirToTarget = target.transform.position - transform.position;
        _anglesToAngle = Vector3.Angle(transform.forward, _dirToTarget);
        _distanceToTarget = Vector3.Distance(offsetLOS.position, target.transform.position);
        if (_anglesToAngle <= viewAngle && _distanceToTarget <= viewDistance)
        {
            RaycastHit rch;
            bool obstacleBetween = false;
            if (Physics.Raycast(offsetLOS.position, _dirToTarget, out rch, _distanceToTarget))
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
                     Mathf.Sqrt(Mathf.Abs( 2 * (displacementY - height) / gravity.y));

        if(time == 0 || float.IsNaN(time))
            time = 0.01f;

        Vector3 velocityY = Vector3.up * Mathf.Sqrt(Mathf.Abs(-2 * gravity.y * height));
        Vector3 velocityXZ = displacementXZ / time;

        return velocityXZ + (velocityY * -Mathf.Sign(gravity.y));

    }

    private void OnDrawGizmosSelected()
    {
        if (!showGizmos || !needLOS) return;
        
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(offsetLOS.position, viewDistance);

        Gizmos.color = Color.cyan;
        Gizmos.DrawLine(offsetLOS.position, offsetLOS.position + (transform.forward * viewDistance));

        Vector3 rightLimit = Quaternion.AngleAxis(viewAngle, transform.up) * transform.forward;
        Gizmos.DrawLine(offsetLOS.position, offsetLOS.position + (rightLimit * viewDistance));

        Vector3 leftLimit = Quaternion.AngleAxis(-viewAngle, transform.up) * transform.forward;
        Gizmos.DrawLine(offsetLOS.position, offsetLOS.position + (leftLimit * viewDistance));

        Vector3 upLimit = Quaternion.AngleAxis(-viewAngle, transform.right) * transform.forward;
        Gizmos.DrawLine(offsetLOS.position, offsetLOS.position + (upLimit * viewDistance));

        Vector3 downLimit = Quaternion.AngleAxis(viewAngle, transform.right) * transform.forward;
        Gizmos.DrawLine(offsetLOS.position, offsetLOS.position + (downLimit * viewDistance));
    }
    
    protected override void OnDestroy()
    {
        bulletPool?.Clear();
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CustomMSLibrary.Unity;

public enum StatesEnemies
{
    Idle,
    Shoot,
    Walk,
    Attack,
    Null
}

public class Enemy : Character
{
    [Header("Enemy Variables")]
    public float speed;
    public EnemyView viewEnem;
    public LayerMask groundMask;

    [Header("Shooting")]
    public float cdTimer;
    public Transform output;

    [Header("Line Of Sight")]
    public float viewAngle;
    public float viewDistance;

    private Vector3 _dirToTarget;
    private float _anglesToAngle;
    private float _distanceToTarget;
    public Vector3 lastPosition;
    public LayerMask gameAreaMask;

    public bool showGizmos;

    [HideInInspector]
    public Player target;

    protected virtual void Awake()
    {
        target = FindObjectOfType<Player>();
        viewEnem = GetComponent<EnemyView>();
        rb = this.GetComponent<Rigidbody>();
    }

    public bool LineOfSight()
    {
        if (target == null) return false;
        _dirToTarget = target.transform.position - transform.position;
        _anglesToAngle = Vector3.Angle(transform.forward, _dirToTarget);
        _distanceToTarget = Vector3.Distance(transform.position, target.transform.position);
        if (_anglesToAngle <= viewAngle && _distanceToTarget <= viewDistance)
        {
            RaycastHit rch;
            bool obstacleBetween = false;
            if (Physics.Raycast(transform.position, _dirToTarget, out rch, _distanceToTarget))
                if (rch.collider.gameObject.layer == 1 << 10)
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

    public override void Die(IDamager source)    {    }

    public Vector3 ParabolicShot(Transform target, float height, Vector3 gravity)
    {
        float displacementY = target.position.y - output.position.y;
        Vector3 displacementXZ = new Vector3(target.position.x - output.position.x, 0, target.transform.position.z - output.position.z);

        float time = Mathf.Sqrt(Mathf.Abs(-2 * height / gravity.y)) + Mathf.Sqrt(Mathf.Abs(2 * (displacementY - height) / gravity.y));

        Vector3 velocityY = Vector3.up * Mathf.Sqrt(Mathf.Abs(2 * gravity.y * height));
        Vector3 velocityXZ = displacementXZ / time;

        return velocityXZ + velocityY * -Mathf.Sign(gravity.y);
    }

    public override void Move(Vector2 direction)
    {
        var tempVel = rb.velocity;
        Vector3 newVel = new Vector3(direction.x * speed, tempVel.y);      

        rb.velocity = newVel;
        
    }

    private void OnDrawGizmosSelected()
    {
        if (!showGizmos) return;
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, viewDistance);

        Gizmos.color = Color.cyan;
        Gizmos.DrawLine(transform.position, transform.position + (transform.forward * viewDistance));

        Vector3 rightLimit = Quaternion.AngleAxis(viewAngle, transform.up) * transform.forward;
        Gizmos.DrawLine(transform.position, transform.position + (rightLimit * viewDistance));

        Vector3 leftLimit = Quaternion.AngleAxis(-viewAngle, transform.up) * transform.forward;
        Gizmos.DrawLine(transform.position, transform.position + (leftLimit * viewDistance));
    }
}

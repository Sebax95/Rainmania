using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [Header("Enemy Variables")]
    //temporal state machine, cuando se herede de Enemy crearemos las maquinas de estados ahi, no acá
    public FSM<Enemy> fsm;
    public float speed;

    [Header("Line Of Sight")]
    public float viewAngle;
    public float viewDistance;

    private Vector3 _dirToTarget;
    private float _anglesToAngle;
    private float _distanceToTarget;

    [HideInInspector]
    public Rigidbody rb;
    public Player target;

    private void Awake()
    {
        target = FindObjectOfType<Player>();
        rb = GetComponent<Rigidbody>();
        fsm = new FSM<Enemy>(this);
        fsm.AddState("Idle", new IdleState(this, fsm));
        fsm.AddState("SimplePatrol", new SimplePatrolState(this, fsm));
    }

    private void Start()
    {
        fsm.SetState("SimplePatrol");
    }
    private void Update()
    {
        fsm.Update();
    }
    private void FixedUpdate()
    {
        fsm.FixedUpdate();
    }

    public bool LineOfSight()
    {
        _dirToTarget = target.transform.position - transform.position;
        _anglesToAngle = Vector3.Angle(transform.forward, _dirToTarget);
        _distanceToTarget = Vector3.Distance(transform.position, target.transform.position);
        if (_anglesToAngle <= viewAngle && _distanceToTarget <= viewDistance)
        {
            RaycastHit rch;
            bool obstacleBetween = false;
            if (Physics.Raycast(transform.position, _dirToTarget, out rch, _distanceToTarget))
                if (rch.collider.gameObject.layer == Layers.Wall)
                    obstacleBetween = true;
            if (!obstacleBetween)
                return true;
            else
                return false;
        }
        else
            return false;
    }
    //temp function
    public Vector3 DirFromAngle(float angleInDegrees, bool angleIsGlobal)
    {
        if (!angleIsGlobal)
            angleInDegrees += transform.eulerAngles.x;
        return new Vector3(Mathf.Cos(angleInDegrees * Mathf.Deg2Rad), Mathf.Sin(angleInDegrees * Mathf.Deg2Rad), 0);
    }
}

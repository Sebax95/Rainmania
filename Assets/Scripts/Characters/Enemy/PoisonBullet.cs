using System;
using System.Collections;
using System.Collections.Generic;
using System.Resources;
using UnityEngine;

public class PoisonBullet : MonoBehaviour, IDamager
{
    public Rigidbody rb;
    [SerializeField]
    private float speed = 5f;
    [SerializeField]
    private int damage;
    public Team AssignTeam;
    public GameObject SourceObject => gameObject;
    public Vector3 gravity;
    public bool useGravity;
    private Transform auraChild;
    public bool isChildRotating; 
    public Team GetTeam => AssignTeam;
    private Enemy sourcePool;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        auraChild = transform.GetChild(0);
        rb.useGravity = false;
        gravity = Physics.gravity;
        isChildRotating = false;
        gameObject.SetActive(false);
    }

    public void Reset()
    {
        isChildRotating = false;
        rb.useGravity = false;
        gravity = Physics.gravity;
        transform.rotation = Quaternion.identity;
        rb.velocity = Vector3.zero;
        StartCoroutine(WaitForBullet(5));
    }

    public static void Enable(PoisonBullet p) 
    {
        p.gameObject.SetActive(true);
        p.Reset();
    }
    public static void Disable(PoisonBullet p) => p.gameObject.SetActive(false);
    
    IEnumerator WaitForBullet(float timer)
    {
        yield return  new  WaitForSeconds(timer);
        sourcePool.ReturnBullet(this);
    }

    public void SetSource(Enemy source) => sourcePool = source;
    
    public void SetValues(Vector3 pos, Vector3 forw, bool canRotate)
    {
        isChildRotating = canRotate;
        transform.position = pos;
        transform.forward = forw;
    }
    
    private void FixedUpdate()
    {
        if(useGravity)
            rb.AddForce(gravity.y * Vector3.up);
        else
            transform.position += transform.forward * speed * Time.deltaTime;
    }

    private void LateUpdate()
    {
        if(isChildRotating)
            auraChild.forward = rb.velocity.normalized;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 8)
        {
            other.GetComponent<Character>().Damage(damage, this);
           // DetachParticle();
           sourcePool.ReturnBullet(this);
           
        }
        if(other.gameObject)
        {
            //DetachParticle();
            sourcePool.ReturnBullet(this);
        }
    }
}

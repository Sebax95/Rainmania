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
    public Team GetTeam => AssignTeam;
    private MushroomEnemy sourcePool;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        rb.useGravity = false;
        gravity = Physics.gravity;
        gameObject.SetActive(false);
    }

    public void Reset()
    {
        rb.useGravity = false;
        gravity = Physics.gravity;
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

    public void SetSource(MushroomEnemy source) => sourcePool = source;
    
    public void SetValues(Vector3 pos, Vector3 forw, Quaternion rot)
    {
        transform.position = pos;
        transform.forward = forw;
        transform.rotation = rot;
    }
    
    private void FixedUpdate()
    {
        if(useGravity)
            rb.AddForce(gravity.y * Vector3.up);
        else
            transform.position += transform.forward * speed * Time.deltaTime;
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

    void DetachParticle()
    {
        var childPart = transform.GetChild(0);
        childPart.transform.parent = null;
        childPart.transform.position = transform.position;
        Destroy(childPart.gameObject, 1f);
    }
}

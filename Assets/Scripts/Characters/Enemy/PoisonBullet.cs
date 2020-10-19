using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoisonBullet : MonoBehaviour, IDamager
{
    public Rigidbody rb;
    public float force;
    public int damage;
    public Team AssignTeam;
    public GameObject SourceObject => gameObject;
    public Vector3 gravity;
    public bool useGravity;
    public Team GetTeam => AssignTeam;
    public GameObject particlesGRound;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        rb.useGravity = false;
        gravity = Physics.gravity;
        Disable(5f);
    }

    public void Enable() => gameObject.SetActive(true);
    
    public void Disable() => gameObject.SetActive(false);

    public void Disable(float timer) => StartCoroutine(WaitForBullet(timer));

    IEnumerator WaitForBullet(float timer)
    {
        yield return  new  WaitForSeconds(timer);
        Disable();
    }

    public void SetValues(Vector3 pos, Quaternion rot)
    {
        transform.right = pos;
        transform.rotation = rot;
    }
    
    private void FixedUpdate()
    {
        if(useGravity)
            rb.AddForce(gravity.y * Vector3.up);
        else
            transform.position += transform.forward *5 * Time.deltaTime;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 8)
        {
            other.GetComponent<Character>().Damage(damage, this);
            var child = transform.GetChild(0);
            child.transform.parent = other.transform;
            child.transform.localScale = new Vector3(0.3f, 0.3f, 0.3f);
            child.transform.localRotation = Quaternion.Euler(0, -90, 0);
            Destroy(child.gameObject, 5f);
            Disable();
        }
        if(other.gameObject)
        {
            if (other.gameObject.layer == 9)
            {
                var part = Instantiate(particlesGRound, transform.position + Vector3.up *0.3f, Quaternion.identity, null);
                Destroy(part.gameObject, 8);
            }
            Disable();
        }
    }
}

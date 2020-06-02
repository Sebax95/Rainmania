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

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        rb.useGravity = false;
        gravity = Physics.gravity;
        Destroy(this.gameObject, 5);
    }

    private void FixedUpdate()
    {
        if(useGravity)
            rb.AddForce(gravity.y * Vector3.up);
        else
        {
            transform.position += transform.forward *5 * Time.deltaTime;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 8)
        {
            other.GetComponent<Character>().Damage(damage, this);
            var child = transform.GetChild(0);
            child.transform.parent = other.transform;
            child.transform.localRotation = Quaternion.Euler(0, -90, 0);
            Destroy(child.gameObject, 5f);
            Destroy(gameObject);
        }
        if(other.gameObject)
        {
            Destroy(gameObject);
        }
    }
}

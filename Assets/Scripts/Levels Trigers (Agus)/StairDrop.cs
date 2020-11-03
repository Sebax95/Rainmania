using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StairDrop : MonoBehaviour, IDamageable
{
    
    BoxCollider _box;
    public bool trigger;
    public float speed;

    public GameObject SourceObject => throw new NotImplementedException();

    public Team GetTeam => throw new NotImplementedException();

    public event Action<IDamager> OnDeath;

    void Start()
    {
        _box = GetComponent<BoxCollider>();
        trigger = false;
    }

    
    void Update()
    {
        if (trigger)
            transform.position = transform.position + new Vector3(0, -1, 0) * speed;

        Debug.Log(trigger);
    }

   
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Roof"))
        {
            trigger = false;
            _box.enabled = false;
        }
    }

    public bool Damage(int amount, IDamager source)
    {
        trigger = true;
        return this;
    }

    public void Die(IDamager source)
    {
        throw new NotImplementedException();
    }
}

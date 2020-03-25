using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour
{

    public float speed;
    bool stop;

    private void Awake()
    {
    }
    void Update()
    {
        if(!stop)
        transform.position += transform.forward * speed * Time.deltaTime;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.collider)
        {
            stop = true;
        }
    }
}

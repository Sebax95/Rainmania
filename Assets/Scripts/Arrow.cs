using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour
{

    public float speed;
    bool stop;
    Rigidbody rigid;

    private void Awake()
    {
        rigid = GetComponent<Rigidbody>();
    }
    void Update()
    {
        if(!stop)
        transform.position += transform.forward * speed * Time.deltaTime;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.collider.gameObject)
        {
            stop = true;
            if(collision.collider.gameObject.layer == 9 && this.gameObject.layer != 9 && this.gameObject.layer != 1)
            {
                this.gameObject.layer = 9;
                Destroy(GetComponent<Rigidbody>());
            }
            else if (this.gameObject.layer != 9 && this.gameObject.layer != 1)
            {
                this.gameObject.layer = 1;
                rigid.constraints = RigidbodyConstraints.None;
                rigid.constraints = RigidbodyConstraints.FreezePositionZ;
                rigid.constraints = RigidbodyConstraints.FreezeRotationZ;

            }
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour
{

    public float speed;
    bool stop;
    Rigidbody rigid;
    float timer;
    bool seconTime = false;

    private void Awake()
    {
        rigid = GetComponent<Rigidbody>();
    }
    void Update()
    {
        if(!stop)
        transform.position += transform.forward * speed * Time.deltaTime;
        else
            timer += 1 * Time.deltaTime;
        if(timer > 3)
            Bow.Instance.ReturnArrow(this);
    }
    public void Reset()
    {
        gameObject.layer = 10;
        stop = false;
        timer = 0;
        if (seconTime)
        {
        rigid = this.gameObject.AddComponent<Rigidbody>();
        rigid.constraints = RigidbodyConstraints.FreezeAll;

        }
    }
    public static void TurnOn(Arrow a)
    {
        a.Reset();
        a.gameObject.SetActive(true);
    }

    public static void TurnOff(Arrow a)
    {
        
        a.gameObject.SetActive(false);
    }
    private void OnCollisionEnter(Collision collision)
    {
        if(collision.collider.gameObject)
        {
            seconTime = true;
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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float speed;
    public float forceJump;
    public float fallMultiplier = 2.5f;
    public float lowJumpMultiplier = 2f;

    private Rigidbody rb;
    private Animator anim;
    public Vector3 movement;
    bool canMove;

    private void Start()
    {
        canMove = true;
        rb = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();
    }

    private void FixedUpdate()
    {

        if (rb.velocity.y < 0)
            rb.velocity += Vector3.up * Physics.gravity.y * (fallMultiplier - 1) * Time.deltaTime;
        else if(rb.velocity.y > 0 && !Input.GetButton("Jump"))
            rb.velocity += Vector3.up * Physics.gravity.y * (lowJumpMultiplier - 1) * Time.deltaTime;
        if(Input.GetButtonDown("Jump"))
            Jump();
        //if(canMove)
            Movement();
    }
    void Jump()
    {
        if (Physics.Raycast(transform.position, Vector3.down, 1.1f, 1 << 9))
        {
            /*canMove = false;
            rb.velocity = new Vector3(rb.velocity.x, 0, 0);*/
            rb.velocity = Vector3.up * forceJump;
        }
    }
    void Movement()
    {
        anim.SetFloat("SpeedX", movement.x);
        movement = new Vector3(Input.GetAxis("Horizontal"), 0, 0);
        rb.velocity = (new Vector3(movement.x * speed, rb.velocity.y, 0));
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.gameObject.layer == 9)
            canMove = true;
    }
}

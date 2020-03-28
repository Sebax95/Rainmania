using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
    
    //Attack
    public GameObject attack1;
    public GameObject attack2;
    public GameObject attack3;
    public GameObject arrow;

    float aDisable = 0.2f;
    float aEnable = 0.4f;
    bool isAttacking;
    bool isAttackingUp;
    bool isAttackingDiag;

    public Material weaponColor;
    bool changeWeapon;

    private void Start()
    {
       // weaponChange.SetColor("_EmissionColor",Color.red );
        attack1.SetActive(false);
        attack2.SetActive(false);
        attack3.SetActive(false);

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

        if (Input.GetKeyDown(KeyCode.K))
            changeWeapon = !changeWeapon;

        if(changeWeapon)
            weaponColor.SetColor("_EmissionColor", Color.blue);
        else
            weaponColor.SetColor("_EmissionColor", Color.red);

            WhipInput();
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

    void WhipInput()
    {
        if (Input.GetKey(KeyCode.W))
        {
            if (Input.GetAxis("Horizontal") != 0)
            {
                if (Input.GetKeyDown(KeyCode.J))
                {
                    isAttackingUp = false;
                    isAttacking = false;
                    isAttackingDiag = true;
                }
            }
            else if (Input.GetKeyDown(KeyCode.J))
            {
                isAttackingUp = true;
                isAttacking = false;
                isAttackingDiag = false;
            }
        }
        else if (Input.GetKeyDown(KeyCode.J))
        {
            isAttackingUp = false;
            isAttacking = true;
            isAttackingDiag = false;

        }
        if (changeWeapon)
        {
            if (isAttacking)
                StartCoroutine(AttackTime1());
            if (isAttackingUp)
                StartCoroutine(AttackTime2());
            if (isAttackingDiag)
                StartCoroutine(AttackTime3());
        }
        else
        {
            if (isAttacking)
            {
                BowNormal();
                isAttacking = false;
            }
            if (isAttackingUp)
            {
                BowUp();
                isAttackingUp = false;
            }
            if (isAttackingDiag)
            {
                BowDiag();
                isAttackingDiag = false;
            }
        }
     

       /* if(timer > 0.7f)
        {
            isAttacking = false;
            isAttackingUp = false;
            isAttackingDiag = false;

            timer = 0;
        }*/
    }

   /* void WhipNormal()
    {
        timer += 1 * Time.deltaTime;
        if (timer >= 0.4f && timer < 0.6f)
            attack1.SetActive(true);
        else if (timer > 0.6f)
        {
            attack1.SetActive(false);
        }
    }

    void WhipUp()
    {
        timer += 1 * Time.deltaTime;
        if (timer >= 0.4f && timer < 0.6f)
            attack2.SetActive(true);
        else if (timer > 0.6f)
        {
            attack2.SetActive(false);
        }
    }
    void WhipDiag()
    {
        timer += 1 * Time.deltaTime;
        if (timer >= 0.4f && timer < 0.6f)
            attack3.SetActive(true);
        else if (timer > 0.6f)
        {
            attack3.SetActive(false);
        }
    }*/

    void BowNormal()
    {
        var _arrow = Instantiate(arrow);
        _arrow.transform.position = transform.position + new Vector3(1, 1, 0);
        _arrow.transform.forward = transform.forward;
    }

    void BowUp()
    {
        var _arrow = Instantiate(arrow);
        _arrow.transform.position = transform.position + new Vector3(0, 2.25f, 0);
        _arrow.transform.forward = transform.up ;
    }
    void BowDiag()
    {
        var _arrow = Instantiate(arrow);
        _arrow.transform.position = transform.position + new Vector3(1.25f, 2, 0);
        _arrow.transform.forward = transform.forward + transform.up;
    }


    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.gameObject.layer == 9)
            canMove = true;
    }

    IEnumerator AttackTime1()
    {
        isAttacking = false;
        yield return new WaitForSeconds(aEnable);
        attack1.SetActive(true);
        yield return new WaitForSeconds(aDisable);
        attack1.SetActive(false);
    }
    IEnumerator AttackTime2()
    {
        isAttackingUp = false;
        yield return new WaitForSeconds(aEnable);
        attack2.SetActive(true);
        yield return new WaitForSeconds(aDisable);
        attack2.SetActive(false);
    }
    IEnumerator AttackTime3()
    {
        isAttackingDiag = false;
        yield return new WaitForSeconds(aEnable);
        attack3.SetActive(true);
        yield return new WaitForSeconds(aDisable);
        attack3.SetActive(false);
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Saltador : HongoCaminante
{
    public override IEnumerator Attack()
    {
        viewEnem.ActivateBool(0, false);
        viewEnem.ActivateTriggers(0);
        rb.AddForce(Vector3.up * jumpForce + transform.forward * 2, ForceMode.Impulse);
        bool inGround = false;
        RaycastHit hit;
        yield return new WaitForSeconds(0.1f);
        do
        {
            Debug.DrawRay(transform.position, -transform.up * 0.1f, Color.green);
            if (Physics.Raycast(transform.position, -transform.up * 0.2f, out hit, groundMask))
            {
                viewEnem.ActivateBool(0, true);
                inGround = true;
            }
            yield return new WaitForSeconds(0.05f);
        } while (!inGround);
        StartCoroutine(CdJump());
    }
}

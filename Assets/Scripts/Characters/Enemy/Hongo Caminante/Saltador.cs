using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Saltador : HongoCaminante
{
    public override IEnumerator Attack()
    {
        StartCoroutine(CdJump());
        viewEnem.ActivateBool(0, false);
        viewEnem.ActivateTriggers(0);
        rb.AddForce(Vector3.up * jumpForce + transform.forward * 2, ForceMode.Impulse);
        bool inGround = false;
        RaycastHit hit;
        yield return new WaitForSeconds(0.1f);
        do
        {
            if (Physics.Raycast(transform.position, -transform.up * 0.2f, out hit, groundMask))
            {
                viewEnem.ActivateBool(0, true);
                inGround = true;
            }
            yield return new WaitForSeconds(0.1f);
        } while (!inGround);
    }
}

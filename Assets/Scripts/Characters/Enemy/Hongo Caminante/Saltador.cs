using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Saltador : HongoCaminante
{
    public override IEnumerator Attack()
    {
        if(canJump) yield break;
        viewEnem.ActivateBool(0, false);
        viewEnem.ActivateTriggers(0);
        viewEnem.PlaySound(EnemyView.AudioEnemys.Attack);
        rb.AddForce(Vector3.up * jumpForce + transform.forward * 2, ForceMode.Impulse);
        bool inGround = false;
        RaycastHit hit;
        yield return new WaitForSeconds(0.1f);
        while (true)
        {
            Debug.DrawRay(transform.position, -transform.up * 0.1f, Color.green);
            if (Physics.Raycast(transform.position, -transform.up * 0.2f, out hit, groundMask) && GroundChecker())
            {
                viewEnem.ActivateBool(0, true);
                break;
            }

            yield return new WaitForSeconds(0.05f);
        }
        StartCoroutine(CdJump());
    }
}

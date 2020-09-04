using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Embestidor : HongoCaminante
{
    public override IEnumerator Attack()
    {
        //StartCoroutine(CdJump());
        var tempSpeed = speed;
        viewEnem.ActivateBool(2, true);
        speed = speed * 2;
        
        bool stopCor = false;
        while (!stopCor)
        {
            if (GroundChecker().collider.gameObject == true || FrontChecker().collider.gameObject == false)
            {
                Move(transform.forward);
                yield return new WaitForSeconds(0.1f);
            }
            else
                stopCor = true;

                Debug.Log("a");
        }
        
        /*do
        {
            Debug.Log("a");
            Move(transform.forward);
            yield return new WaitForSeconds(0.1f);
        } while (GroundChecker().collider || !FrontChecker().collider);*/ 
        Debug.Log("sadasdasd");

        speed = tempSpeed;
        viewEnem.ActivateBool(2, false);
        canJump = true;
        //ChangeState(StatesEnemies.Walk);
    }
}

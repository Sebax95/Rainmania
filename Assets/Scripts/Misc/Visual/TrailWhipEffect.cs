using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrailWhipEffect : StateMachineBehaviour
{
    public GameObject trailEffect;
    public Vector3 offset;
    public Vector3 rotationLeft;
    public Vector3 rotationRight;

    private PlayerAnim pA;

    private Transform play;
     //OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (!pA)
            pA = animator.GetComponent<PlayerAnim>();
        pA.StartCoroutine(WhipEffect(animator));
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    //override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    
    //}

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    //override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
        
    //}

    // OnStateMove is called right after Animator.OnAnimatorMove()
    //override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that processes and affects root motion
    //}

    // OnStateIK is called right after Animator.OnAnimatorIK()
    //override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that sets up animation IK (inverse kinematics)
    //}

    IEnumerator WhipEffect(Animator anim)
    {
        yield return new WaitForSeconds(0.2f);
        var posWhip = pA.whip.transform;
        var trail = Instantiate(trailEffect, posWhip.position + offset, Quaternion.identity); //TODO: implementar Pool en esto
        if(!play)
            play = pA.GetComponent<Transform>();
        trail.transform.rotation = (play.rotation.w < 0) ? Quaternion.Euler(rotationLeft): Quaternion.Euler(rotationRight);
        
        Destroy(trail, 1f);
    }
}
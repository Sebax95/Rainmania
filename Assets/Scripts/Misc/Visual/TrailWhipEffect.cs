using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class TrailWhipEffect : StateMachineBehaviour
{
    public GameObject trailEffect;
    private Vector3 offset;
    private Vector3 rotationLeft;
    private Vector3 rotationRight;
    private Tuple<float,float> speed;
    private PlayerAnim pA;
    private Transform posWhip;

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
        speed = Tuple.Create(anim.GetFloat("SpeedX"), anim.GetFloat("SpeedY"));

        if (speed.Item1.Equals(0) && speed.Item2.Equals(1))
            SpawnTrail("Arriba"); 
        else if(speed.Item1.Equals(1) && speed.Item2.Equals(1))
            SpawnTrail("Diagonal");
        else
            SpawnTrail("Normal");
        
    }

    void SpawnTrail(string dir)
    {
        switch (dir)
        {
            case "Arriba":
            {
                rotationLeft = new Vector3(0,180,-30);
                rotationRight = new Vector3(0,0,-40);
                offset = new Vector3(0, 0.5f, 0);
            }
            break;
            case "Diagonal":
            {
                rotationLeft = new Vector3(0,180,-65);
                rotationRight = new Vector3(0,0,-65);
                offset = new Vector3(0, 0.5f, 0);
            }
            break;
            case "Normal":
            {
                rotationLeft = new Vector3(0,180,-140);
                rotationRight = new Vector3(0,0,-140);
                offset = new Vector3(0, 0.5f, 0);
            }
            break;
        }
        if(!posWhip) posWhip = pA.whip.transform;
        var trail = Instantiate(trailEffect, Vector3.zero, Quaternion.identity); //TODO: implementar Pool en esto
        if(!play) play = pA.GetComponent<Transform>();
        trail.transform.position = posWhip.position + offset;
        Debug.Log(trail.transform.position);
        trail.transform.rotation = (play.rotation.w < 0) ? Quaternion.Euler(rotationLeft): Quaternion.Euler(rotationRight);
         
        Destroy(trail, 1f);
    }
}
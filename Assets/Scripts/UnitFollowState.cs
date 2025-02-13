using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class UnitFollowState : StateMachineBehaviour
{
    AttackController attackController;
    NavMeshAgent agent;
    public float attackingDistance = 1f;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        attackController = animator.transform.GetComponent<AttackController>();
        agent = animator.transform.GetComponent<NavMeshAgent>();
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // should unit transition to Idle State?
        if (attackController.targetToAttack == null)
        {
            animator.SetBool("isFollowing", false);
        }
        else
        {
            //if there is no other direct command to move
            if (animator.transform.GetComponent<UnitMovement>().isCommandedToMove == false)
            {
                agent.SetDestination(attackController.targetToAttack.position);

                // animator.transform.LookAt(attackController.targetToAttack); 
                // the code cannot make sure that the unit is moving in a good direction

                // Get the direction to the target
                Vector3 directionToTarget = attackController.targetToAttack.position - animator.transform.position;
                // Project the forward direction onto the gravity plane to prevent tilting
                Vector3 projectedForward = Vector3.ProjectOnPlane(directionToTarget, Vector3.up).normalized;
                // Create a rotation that looks at the target but keeps the 'up' aligned with gravity
                Quaternion lookRotation = Quaternion.LookRotation(projectedForward, Vector3.up);
                // Apply the rotation
                animator.transform.rotation = lookRotation;
            }
        }



        // should unit transition to Attack State?
        // float distanceFromTarget = Vector3.Distance(attackController.targetToAttack.position, animator.transform.position);
        // if (distanceFromTarget < attackingDistance)
        // {
        //     animator.SetBool("isAttacking", true); // Move to Attacking state
        // }
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    // public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    // {
    //     agent.SetDestination(animator.transform.position);
    // }
}

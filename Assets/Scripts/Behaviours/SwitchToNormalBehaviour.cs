using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchToNormalBehaviour : StateMachineBehaviour
{
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    //override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    
    //}

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    //override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    
    //}

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        Debug.Log("Here!");

        PlayerEquipment equipment = animator.gameObject.GetComponent<PlayerEquipment>();

        equipment.ChangeWeaponGraphics(equipment.PlayerWeapon);
        equipment.SetTwoHanded(equipment.PlayerWeapon.isTwoHanded);
        equipment.ChangeWeaponGraphics(equipment.PlayerWeapon);

        animator.gameObject.GetComponent<AttackController>().CanAttack = true;
        animator.gameObject.GetComponent<BlockController>().CanBlock = true;
    }

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
}

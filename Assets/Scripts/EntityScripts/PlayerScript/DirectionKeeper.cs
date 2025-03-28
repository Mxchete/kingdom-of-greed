using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DirectionKeeper : StateMachineBehaviour
{
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        float lastX = animator.GetFloat("LastInputX");
        float lastY = animator.GetFloat("LastInputY");
        animator.SetFloat("InputX", lastX);
        animator.SetFloat("InputY", lastY);

    }
}


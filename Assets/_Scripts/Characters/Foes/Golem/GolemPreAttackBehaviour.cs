using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GolemPreAttackBehaviour : StateMachineBehaviour
{
    private GolemCTRL _model;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (_model == null) _model = animator.GetComponent<GolemCTRL>();
        _model.DoCrying();
    }


    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        _model.FaceTarget();
    }


    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {

    }


}

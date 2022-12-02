using UnityEngine;
public class GolemAttackBehaviour : StateMachineBehaviour
{
    private GolemCTRL _model;   

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if(_model == null) _model = animator.GetComponent<GolemCTRL>();
        _model.DoCrying(2);
        
    }

    
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        
    }

    
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.SetBool("isAttacking", false);
    }
  
}

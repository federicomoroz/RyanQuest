using UnityEngine;

public class ShooterAttackBehaviour : StateMachineBehaviour
{
    private FoeShooter   _model;    

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if(_model == null) _model = animator.GetComponent<FoeShooter>();      

        _model.Agent.SetDestination(_model.transform.position);
    }


    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        
        _model.FaceTarget();

        if (_model.Agent.remainingDistance > _model.AttackRange)        
           animator.SetBool("isAttacking", false);       

        
    }

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        _model.Agent.SetDestination(_model.transform.position);
    }
}

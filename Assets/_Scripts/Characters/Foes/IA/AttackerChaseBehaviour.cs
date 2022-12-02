using UnityEngine;

public class AttackerChaseBehaviour : StateMachineBehaviour
{
    private FoeAttacker  _model;
    private float        _timer;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if(_model == null) _model = animator.GetComponent<FoeAttacker>();        

        animator.SetBool("isAttacking", false);
        _model.SetAgentSpeed(_model.ChaseSpeed);
        _timer = 0;

    }


    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        _model.Agent.SetDestination(_model.Target.position);

        if(_model.Agent.remainingDistance < _model.AttackRange)
        {
            _timer += Time.deltaTime;

            if(_timer > _model.AttackDelay)            
                animator.SetBool("isAttacking", true);            
        }
        else if(_model.Agent.remainingDistance > _model.ChaseRadius)        
            animator.SetBool("isChasing", false);       

    }

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        _model.Agent.SetDestination(_model.transform.position);
    }
}

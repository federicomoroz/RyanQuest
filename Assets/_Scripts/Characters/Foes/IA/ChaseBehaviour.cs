using UnityEngine;

public class ChaseBehaviour : StateMachineBehaviour
{
    private FoeChaser _model;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if(_model == null) _model = animator.GetComponent<FoeChaser>();

        _model.SetAgentSpeed(_model.ChaseSpeed);
    }


    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        _model.Agent.SetDestination(_model.Target.position);
       
        if (_model.Agent.remainingDistance > _model.ChaseRadius)
            animator.SetBool("isChasing", false);

    }

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        _model.Agent.SetDestination(_model.transform.position);
    }
}

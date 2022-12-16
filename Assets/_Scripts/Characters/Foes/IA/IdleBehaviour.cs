using UnityEngine;

public class IdleBehaviour : StateMachineBehaviour
{
    private FoeChaser _model;
    private float     _timer;   

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if(_model == null) _model = animator.GetComponent<FoeChaser>();
        
        _model.isVulnerable       = true;
        _timer                    = 0;
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        _timer += Time.deltaTime;
        if (_timer > _model.IdleTime)
            animator.SetBool("isPatrolling", true);

        float distance = Vector3.Distance(_model.transform.position, _model.Target.position);

        if (distance < _model.ChaseRadius && _model.TargetOnSight(_model.transform,_model.ChaseRadius))
            animator.SetBool("isChasing", true);
    }
 
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        
    }



}

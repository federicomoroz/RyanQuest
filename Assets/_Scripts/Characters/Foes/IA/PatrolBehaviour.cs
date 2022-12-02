using UnityEngine;

public class PatrolBehaviour : StateMachineBehaviour
{
    private FoeChaser _model;
    private float     _timer;   

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if(_model == null)_model = animator.GetComponent<FoeChaser>();        

        _model.SetAgentSpeed(_model.PatrolSpeed);        
        _timer = 0;
    }


    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        _model.Patrol();

        _timer += Time.deltaTime;

        if (_timer > _model.IdleTime)
            animator.SetBool("isPatrolling", false);

        float distance = Vector3.Distance(_model.transform.position, _model.Target.position);

        if(distance < _model.ChaseRadius)        
            animator.SetBool("isChasing", true);        
    }

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        _model.Agent.SetDestination(_model.transform.position);
    }   
    
}

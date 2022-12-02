using UnityEngine;

public class BombChaseBehaviour : StateMachineBehaviour
{
    private BombdudeCTRL _model;
    private float        _timer;
    

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if(_model == null)
           _model = animator.GetComponent<BombdudeCTRL>();        

        _model.SetAgentSpeed(_model.ChaseSpeed);
        _timer = 0;

        _model.SparkVfx.PlayVfx();

    }

    
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        _model.Agent.SetDestination(_model.Target.position);
        _timer += Time.deltaTime;
        if(_timer >= _model.ExplodeTime)        
            animator.SetBool("isExploding", true);
        
    }
        
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        _model.Agent.SetDestination(_model.transform.position);
    }    

}

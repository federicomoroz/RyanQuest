using UnityEngine;

public class DamagedBehaviour : StateMachineBehaviour
{
    private Foe _model;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if(_model == null)         _model = animator.GetComponent<Foe>();        
        if(_model.isVulnerable)    _model.isVulnerable = false;
        if (_model.CurrentHp <= 0) _model.OnDeath();

        _model.Agent.SetDestination(_model.transform.position);

    }


    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
  
    }

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        _model.Agent.SetDestination(_model.transform.position);

        if (_model.IsAlive)        
            _model.isVulnerable = true;              
    }


}

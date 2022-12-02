using UnityEngine;


public class DeadBehaviour : StateMachineBehaviour
{
    private Foe _model;    
    
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if(_model == null) _model = animator.GetComponent<Foe>();

        _model.Agent.SetDestination(_model.transform.position);
        _model.IsVulnerable = false;
    }

    
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        
    }

    
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {         
        _model.InstantiateExplosion();
        
        if(_model.Irs != null)        
            _model.Irs.HandleSpawning();        
        
        _model.DeActivate();
    }    

}

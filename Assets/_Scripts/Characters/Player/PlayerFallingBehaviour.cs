using UnityEngine;

public class PlayerFallingBehaviour : StateMachineBehaviour
{
    private Player _model;
    
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (_model == null)
            _model = animator.GetComponent<Player>();
    }

    
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        _model.Locomotion.Rb.velocity += Vector3.up * Physics.gravity.y * 3f * Time.deltaTime;
        
    }

    
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {

    }


}

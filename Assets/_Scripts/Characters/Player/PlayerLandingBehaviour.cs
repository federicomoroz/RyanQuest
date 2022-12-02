using UnityEngine;

public class PlayerLandingBehaviour : StateMachineBehaviour
{
    
    private Player _model;
    private float timeToRecover;
    private float _speedMoveFull = 450f;
    private float _speedLandMultiplier = 0.2f;
 
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if(_model == null)        
            _model = animator.GetComponent<Player>();
              

        Debug.Log("Player entered Landing State");
        _model.Locomotion.SetNewSpeed(_model.Locomotion.CurrentSpeed*_speedLandMultiplier);

        timeToRecover = 0f;
        FXManager.Instance.CameraShake(_model.transform.position, 0.3f);
        FXManager.Instance.PlaySound(SfxName.Landing, _model.transform.position);

        
    }


    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
 

        timeToRecover += Time.deltaTime;
        if(timeToRecover < stateInfo.length)
        {
            _model.Locomotion.SetNewSpeed(Mathf.Lerp(_model.Locomotion.CurrentSpeed, _speedMoveFull, timeToRecover/stateInfo.length));
        }
        


    }
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
       
    }


}

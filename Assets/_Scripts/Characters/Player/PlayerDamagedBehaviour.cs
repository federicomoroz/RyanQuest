using UnityEngine;

public class PlayerDamagedBehaviour : StateMachineBehaviour
{

    private Player _model;
    private SfxName[] _hurtSfxList = new SfxName[3] {SfxName.PlayerHurt1, SfxName.PlayerHurt2, SfxName.PlayerHurt3 };

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (_model == null)
            _model = animator.GetComponent<Player>();

        Debug.Log("Player entered Damaged State");
        _model.isVulnerable = false;        
        FXManager.Instance.PlaySound(_hurtSfxList[GetSfxIndex(_hurtSfxList.Length)]);
    }


    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {

    }


    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (_model.CurrentHp <= 0)
            _model.OnDeath();
        else
        {
            _model.isVulnerable = true;
            _model.Locomotion.SetMoveSpeed(450);
        }
    }

    private int GetSfxIndex(int arrayLength)
    {        
        return Random.Range(0, arrayLength);
    }


}

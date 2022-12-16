using UnityEngine;

public class TkChargeState : BaseState
{
    private FSM_TK _fsm;

    public TkChargeState(FSM_TK fsm)
    {
        _fsm = fsm;
    }
    public override void OnEnterState()
    {        
        _fsm.model.throwForceCurrent = _fsm.model.ThrowForceMin;
        FXManager.Instance.PlaySound(SfxName.PlayerTkCharge);

        if (!_fsm.model.VfxAura.activeInHierarchy)
            _fsm.model.VfxAura.SetActive(true);
    }
    public override void OnUpdateState()
    {
        if(_fsm.model.objectPicked != null)
        {
            if (Input.GetButtonUp("Fire2"))
                Drop();

            if (!_fsm.model.CheckDistance(_fsm.model.objectPicked, _fsm.model.PickupMaxDistance*0.25f))
                Drop();      


            ChargePower();

            if (Input.GetButtonUp("Fire1"))
                Throw();
        }
        else
        {
            _fsm.SwitchState(TkState.IDLE);
        }

    }

    public override void OnExitState()
    {

        EventManager.Trigger(EventName.PlayerCanShootObj, false);
    }
    #region Methods
    private void ChargePower()
    {
        _fsm.model.throwForceCurrent = Mathf.Lerp(_fsm.model.throwForceCurrent, _fsm.model.ThrowForceMax, _fsm.model.ThrowForceMultiplier);
        
    }

    private void Throw()
    {
        if (_fsm.model.objectPicked == null)
            return; 

        Vector3 target;
        RaycastHit hit;
        if (Physics.Raycast(_fsm.model.Cam.position, _fsm.model.Cam.forward, out hit, Mathf.Infinity, _fsm.model.PickupLayerMask))
            target = hit.point;
        else
            target = _fsm.model.Cam.position + _fsm.model.Cam.forward * 50f;

        _fsm.model.objectPicked.Throw(target, _fsm.model.throwForceCurrent);
        _fsm.model.Animator.SetTrigger("Tk_Release");
        FXManager.Instance.PlaySound(SfxName.PlayerTkRelease1);
        FXManager.Instance.CameraShake(_fsm.model.objectPicked.transform.position, 0.5f);
        _fsm.SwitchState(TkState.IDLE);

    }

    #endregion
    private void Drop()
    {
        _fsm.model.DropObject();
        _fsm.SwitchState(TkState.IDLE);
    }
}

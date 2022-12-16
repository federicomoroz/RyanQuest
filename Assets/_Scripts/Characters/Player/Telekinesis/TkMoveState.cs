using UnityEngine;

public class TkMoveState : BaseState
{ 
    private FSM_TK _fsm; 

    public TkMoveState(FSM_TK fsm)
    {
        _fsm = fsm;
    }
    public override void OnEnterState()
    {
        Debug.Log("Entering Moving State");

        _fsm.model.Animator.SetTrigger("Tk_Load");       
            _fsm.model.VfxWeapon.PlayVfx();
    }
    public override void OnUpdateState()
    {
        CheckObjectForPull();

        if (!_fsm.model.CheckDistance(_fsm.model.objectPicked, _fsm.model.PickupMaxDistance))        
            Drop();

        if (Input.GetButtonUp("Fire2"))
            Drop();
    }

    public override void OnExitState()
    {
        Debug.Log("Leaving Moving State");
        EventManager.Trigger(EventName.PlayerCanPullObj, false);
    }

    private void CheckObjectForPull()
    {
        if (_fsm.model.IsVisible(_fsm.model.Cam.GetComponent<Camera>(), _fsm.model.objectPicked.transform)
              && !_fsm.model.IsObstructed(_fsm.model.objectPicked.transform.position, _fsm.model.ThrowPoint.position))
        {
            if (_fsm.model.objectPicked.TryGetComponent(out IPushable pickable) && pickable.IsThrowable())
            {
                EventManager.Trigger(EventName.PlayerCanPullObj, true);

                if (Input.GetButtonDown("Fire1"))
                    Pull(pickable);
            }
        }
        else
            EventManager.Trigger(EventName.PlayerCanPullObj, false);
    }

    private void Pull(IPushable pickable)
    {
        pickable.Pull(_fsm.model.ThrowPoint);
        _fsm.SwitchState(TkState.PULL);
    }

    private void Drop()
    {
        _fsm.model.DropObject();
        _fsm.SwitchState(TkState.IDLE);
    }

}

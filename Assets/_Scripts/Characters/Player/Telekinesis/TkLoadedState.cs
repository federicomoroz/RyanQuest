using UnityEngine;

public class TkLoadedState : BaseState
{
    private FSM_TK _fsm;

    public TkLoadedState(FSM_TK fsm)
    {
        _fsm = fsm;
    }
    public override void OnEnterState()
    {
        Debug.Log("Entering Loaded State");
        EventManager.Trigger(EventName.PlayerCanShootObj, true);
        if (_fsm.model.ObjectPicked == null)
            _fsm.SwitchState(TkState.IDLE);

    }


    public override void OnUpdateState()
    {
        if (Input.GetButtonUp("Fire2"))
            Drop();


        if (!_fsm.model.CheckDistance(_fsm.model.ObjectPicked, _fsm.model.PickupMaxDistance * 0.25f)) 
            Drop();

        CheckObjectForThrow();
    }
    public override void OnExitState()
    {
        Debug.Log("Leaving Loaded State");
  
    }

    private void CheckObjectForThrow()
    {

        if (Input.GetButtonDown("Fire1"))
            _fsm.SwitchState(TkState.CHARGE);


        if (Input.GetButton("Fire1"))
            _fsm.SwitchState(TkState.CHARGE);
    }

    private void Drop()
    {
        _fsm.model.DropObject();
        _fsm.SwitchState(TkState.IDLE);
    }
}

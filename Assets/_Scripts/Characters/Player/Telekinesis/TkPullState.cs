using UnityEngine;

public class TkPullState : BaseState
{
    private FSM_TK _fsm;
    private float  _timer = 0;
    public TkPullState(FSM_TK fsm)
    {
        _fsm = fsm;
    }
    public override void OnEnterState()
    {
        Debug.Log("Entering Pull State");
        _timer = 0;

    }
    public override void OnUpdateState()
    {

        if (Input.GetButtonUp("Fire2"))
            Drop();

        _timer += Time.deltaTime;

        if (_timer > 0.15f)
        {
            if (_fsm.model.ObjectPicked != null)
                _fsm.SwitchState(TkState.LOADED);
            else
                _fsm.SwitchState(TkState.IDLE);
        }                 
    }

    public override void OnExitState()
    {
        Debug.Log("Leaving Pull State");
    }

    private void Drop()
    {
        _fsm.model.DropObject();
        _fsm.SwitchState(TkState.IDLE);
    }

}

using System.Collections.Generic;

public enum TkState
{
    IDLE,
    MOVE,
    PULL,
    LOADED,
    CHARGE,
}

public class FSM_TK
{
    private Dictionary<TkState, BaseState> _states = new Dictionary<TkState, BaseState>();

    private BaseState _currentState;

    public TelekinesisCTRL model;

    public FSM_TK(TelekinesisCTRL modelCTRL)
    {
        model = modelCTRL;        

        _states.Add(TkState.IDLE,     new TkIdleState(this));
        _states.Add(TkState.MOVE,     new TkMoveState(this));
        _states.Add(TkState.PULL,     new TkPullState(this));
        _states.Add(TkState.LOADED,   new TkLoadedState(this));
        _states.Add(TkState.CHARGE,   new TkChargeState(this));

        SwitchState(TkState.IDLE);
    }    
    public void SwitchState(TkState newState)
    {
        if (_states.ContainsKey(newState))
        {
            if (_currentState != _states[newState])
            {
                if(_currentState != null)
                    _currentState.OnExitState();

                _currentState = _states[newState];
                _currentState.OnEnterState();
            }
        }
    }

    public void VirtualUpdate()
    {
        _currentState.OnUpdateState();
    }


}

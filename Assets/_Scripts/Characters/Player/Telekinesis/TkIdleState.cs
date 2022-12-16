using UnityEngine;

public class TkIdleState : BaseState
{
    private FSM_TK _fsm;

    public TkIdleState(FSM_TK fsm)
    {
        _fsm = fsm;
    }
    public override void OnEnterState()
    {       

        Debug.Log("Entering telekinesis State: IDLE");

        EventManager.Trigger(EventName.PlayerCanPickObj, false);
        EventManager.Trigger(EventName.PlayerCanPullObj, false);
        EventManager.Trigger(EventName.PlayerCanShootObj, false);

        _fsm.model.throwForceCurrent = 0;

        _fsm.model.VfxWeapon.StopVfx();
        _fsm.model.Animator.SetTrigger("Tk_Off");     
        

        if (_fsm.model.VfxAura.activeInHierarchy)
            _fsm.model.VfxAura.SetActive(false);

        _fsm.model.objectPicked = null;
    }
    public override void OnUpdateState()
    {
        CheckObjectForPick();
    }

    public override void OnExitState()
    {
        EventManager.Trigger(EventName.PlayerCanPickObj, false);
        _fsm.model.Animator.ResetTrigger("Tk_Release");
        _fsm.model.Animator.ResetTrigger("Tk_Off");
        _fsm.model.Animator.SetTrigger("Tk_On");
        Debug.Log("Leaving telekinesis State: IDLE");
    }


    public void CheckObjectForPick()
    {
        RaycastHit hit;
        if (Physics.Raycast(_fsm.model.Cam.position, _fsm.model.Cam.forward, out hit, _fsm.model.PickupMaxDistance, _fsm.model.PickupLayerMask))
        {
            _fsm.model.GrabPoint.position = hit.point;

            if (hit.transform.TryGetComponent(out IPickable pickable))
            {
                if (_fsm.model.previous != pickable)
                {
                    if (_fsm.model.previous != null)
                        RayOut();

                    _fsm.model.previous = pickable;
                    _fsm.model.previous.OutlineOn();
                    EventManager.Trigger(EventName.PlayerCanPickObj, true);
                }

                if (Input.GetButtonDown("Fire2"))
                    PickObject(pickable);
            }
            else
            {
                if (_fsm.model.previous != null)
                    RayOut();
            }
        }
        else
        {
            if (_fsm.model.previous != null)
                RayOut();
        }


    }

    private void RayOut()
    {
        _fsm.model.previous.OutlineOff();
        _fsm.model.previous = null;
        EventManager.Trigger(EventName.PlayerCanPickObj, false);
    }
    private void PickObject(IPickable pickable)
    {
        _fsm.model.objectPicked = pickable.GetObject();
        pickable.OutlineOff();
        pickable.Grab(_fsm.model.GrabPoint);
        _fsm.SwitchState(TkState.MOVE);

    }
}

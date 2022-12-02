using UnityEngine;
using System;

public abstract class Switch : MonoBehaviour, IActivatable
{   
    protected bool    isPressed = false;              
    public    Action  e_switchUpdate;

    public virtual void Toogle(bool state)
    {
        isPressed = state;
        FXManager.Instance.PlaySound(SfxName.SwitchToggle);
    }

    public bool CheckState()
    {
        return isPressed;
    }
}

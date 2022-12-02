using UnityEngine;

public interface IPickable
{
    public void OutlineOn();
    public void OutlineOff();
    public void Grab(Transform grabPoint);
    public void Drop();    
    public PickableObject GetObject();

    public enum PickableState { IDLE, MOVING, PULLING, LOADED, THROWN };
    void SetState(PickableState newState);

}

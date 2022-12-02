using UnityEngine;

public interface IPushable
{
    public bool IsThrowable();
    public void Pull(Transform throwPoint);
    public void Throw(Vector3 target, float force);
    
    


}

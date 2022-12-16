using UnityEngine;

public interface IBlastable
{  
    void HandleBlast(ExplosionValues explosion);    
}

[System.Serializable]
public struct ExplosionValues
{   
    public int       Damage;
    public float     Force;
    public float     ForceUp;
    public Vector3   OriginPosition;
    public float     Radius;
    public ForceMode Mode;

    public ExplosionValues(int damage, float force, float forceVertical, Vector3 originPosition, float radius, ForceMode mode = ForceMode.Impulse )
    {
        Damage         = damage;
        Force          = force;
        ForceUp        = forceVertical;
        OriginPosition = originPosition;
        Radius         = radius;
        Mode           = mode;
    }

}


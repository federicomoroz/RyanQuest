using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class StaticObject : MonoBehaviour, IBlastable
{
    private Rigidbody _rb;
    
    private void Awake()
    {
        if(_rb == null)
        {
            if (TryGetComponent(out Rigidbody rb))
                _rb = rb;
        }
    }
    public void HandleBlast(ExplosionValues explosion)
    {
        _rb.AddExplosionForce(explosion.Force, explosion.OriginPosition, explosion.Radius, explosion.ForceUp, explosion.Mode);
    }
}

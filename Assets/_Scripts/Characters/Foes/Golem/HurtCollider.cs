using UnityEngine;

public class HurtCollider : MonoBehaviour, IFoeDamageable
{
    private Foe _model;
    private void Awake()
    {
        _model = GetComponentInParent<Foe>();
    }

    public void Knockback(Vector3 direction, float force)
    {
        throw new System.NotImplementedException();
    }

    public void OnDeath()
    {
        throw new System.NotImplementedException();
    }

    public void TakeDamage(int value)
    {
        _model.TakeDamage(value);
    }

    

    public Rigidbody GetRigidbody()
    {
        throw new System.NotImplementedException();
    }
}

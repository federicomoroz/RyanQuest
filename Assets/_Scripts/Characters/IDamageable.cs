using UnityEngine;

public interface IDamageable
{
    abstract void TakeDamage(int value, float knockbackForce);
    abstract void OnDeath();
    abstract void Knockback(Vector3 direction, float force);
  
}

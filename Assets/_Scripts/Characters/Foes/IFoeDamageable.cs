using UnityEngine;

public interface IFoeDamageable
{
    public void TakeDamage(int dmg);
    public Rigidbody GetRigidbody();
}

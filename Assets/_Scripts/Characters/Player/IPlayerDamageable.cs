using UnityEngine;

public interface IPlayerDamageable
{
    public void TakeDamage(int dmg);
    public bool IsVulnerable();

    public void Knockback(Vector3 otherPosition, float force);
    public Rigidbody GetRigidbody();

}

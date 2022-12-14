using UnityEngine;

public class DamagingCollider : MonoBehaviour
{
    [SerializeField] protected int   _dmgPower;
    [SerializeField] protected bool  _doesKnockback = false;
    [SerializeField] protected float _knockbackForce;

    protected virtual void OnCollisionEnter(Collision other)
    {
        if(other.gameObject.TryGetComponent(out IPlayerDamageable player) && player.IsVulnerable())
        {
            print("Damaging Collider collided with player");
            player.TakeDamage(_dmgPower);
            if (_doesKnockback)
                player.Knockback(other.contacts[0].point, _knockbackForce);
            FXManager.Instance.PlayVfx(VfxName.PhysicalHitImpactCommon, other.contacts[0].point, this.transform.rotation);
        }
    }

    protected virtual void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.TryGetComponent(out IPlayerDamageable player) && player.IsVulnerable())
        {
            print("Damaging Collider collided with player");
            player.TakeDamage(_dmgPower);
            if (_doesKnockback)
                player.Knockback(this.transform.position, _knockbackForce);
            FXManager.Instance.PlayVfx(VfxName.PhysicalHitImpactCommon, this.transform.position, this.transform.rotation);
        }
    }


    public void SetPower(int power)
    {
        print("Set Power to " + power);
        _dmgPower = power;
        print(_dmgPower);
    }
}

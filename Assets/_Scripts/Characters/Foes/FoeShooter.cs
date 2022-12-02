using UnityEngine;

public class FoeShooter : FoeAttacker, IFoeDamageable
{

    [Header("SHOOTING VALUES")]
    [SerializeField] private ProjectileName _projectile;
    [SerializeField] private Transform      _shootPoint;

    public void Shoot()
    {

        GameObject projectile = Instantiate(FXManager.Instance.GetProjectile(_projectile), _shootPoint.position, Quaternion.identity, null);
        Rigidbody rb          = projectile.GetComponent<Rigidbody>();

        if(rb != null)
        {
            rb.AddForce(transform.forward * 50f, ForceMode.Impulse);
            rb.AddForce(transform.up * 12f, ForceMode.Impulse);
        }
    }
    


}

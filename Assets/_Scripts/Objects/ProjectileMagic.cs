using UnityEngine;
 
public class ProjectileMagic : MonoBehaviour
{
    [Header("Projectile Components")]
    [SerializeField] private GameObject   _impactParticle;
    [SerializeField] private GameObject   _projectileParticle;
    [SerializeField] private GameObject   _muzzleParticle;
    [SerializeField] private GameObject[] _trailParticles;

    [Header("Parameters")]
    [SerializeField] private float   _lifetimeMuzzle     = 1.5f;
    [SerializeField] private float   _lifetimeImpact     = 3f;
    [SerializeField] private float   _lifetimeProjectile = 3f;
    [SerializeField] private float   _lifetimeTrail      = 2f;
                     private bool    _hasCollided        = false;
                     private Vector3 _impactNormal;                 //Used to rotate impactparticle. 
 
    void Start()
    {
        _projectileParticle = Instantiate(_projectileParticle, transform.position, transform.rotation);
        _projectileParticle.transform.parent = transform;
		if (_muzzleParticle)
        {
        _muzzleParticle = Instantiate(_muzzleParticle, transform.position, transform.rotation);
        Destroy(_muzzleParticle, _lifetimeMuzzle);
		}
    }
 
    void OnCollisionEnter(Collision other)
    {
        if (!_hasCollided)
        {
            _hasCollided = true;
       
            _impactParticle = Instantiate(_impactParticle, transform.position, Quaternion.FromToRotation(Vector3.up, _impactNormal));

 
            if(other.gameObject.TryGetComponent(out IPlayerDamageable player))
            {
                player.Knockback(other.contacts[0].point, 300f);
                player.TakeDamage(5);
            }
 

            foreach (GameObject trail in _trailParticles)
            {
                GameObject curTrail = transform.Find(_projectileParticle.name + "/" + trail.name).gameObject;
                curTrail.transform.parent = null;
                Destroy(curTrail, _lifetimeProjectile);
            }
            Destroy(_projectileParticle, _lifetimeProjectile);
            Destroy(_impactParticle, _lifetimeImpact);
            Destroy(gameObject);

			
			ParticleSystem[] trails = GetComponentsInChildren<ParticleSystem>();

            for (int i = 1; i < trails.Length; i++)
            {
                ParticleSystem trail = trails[i];
                if (!trail.gameObject.name.Contains("Trail"))
                    continue;

                trail.transform.SetParent(null);
                Destroy(trail.gameObject, _lifetimeTrail);
            }
        }
    }
}
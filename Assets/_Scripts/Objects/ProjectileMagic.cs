using UnityEngine;
 
public class ProjectileMagic : MonoBehaviour
{
    [SerializeField] private GameObject   _impactParticle;
    [SerializeField] private GameObject   _projectileParticle;
    [SerializeField] private GameObject   _muzzleParticle;
    [SerializeField] private GameObject[] _trailParticles;   
                     private Vector3      _impactNormal; //Used to rotate impactparticle.
 
    private bool hasCollided = false;
 
    void Start()
    {
        _projectileParticle = Instantiate(_projectileParticle, transform.position, transform.rotation) as GameObject;
        _projectileParticle.transform.parent = transform;
		if (_muzzleParticle){
        _muzzleParticle = Instantiate(_muzzleParticle, transform.position, transform.rotation) as GameObject;
        Destroy(_muzzleParticle, 1.5f); // Lifetime of muzzle effect.
		}
    }
 
    void OnCollisionEnter(Collision other)
    {
        if (!hasCollided)
        {
            hasCollided = true;
       
            _impactParticle = Instantiate(_impactParticle, transform.position, Quaternion.FromToRotation(Vector3.up, _impactNormal)) as GameObject;

 
            if(other.gameObject.TryGetComponent(out IPlayerDamageable player) && player != null)
            {
                player.Knockback(other.contacts[0].point, 300f);
                player.TakeDamage(5);
            }
 

            foreach (GameObject trail in _trailParticles)
            {
                GameObject curTrail = transform.Find(_projectileParticle.name + "/" + trail.name).gameObject;
                curTrail.transform.parent = null;
                Destroy(curTrail, 3f);
            }
            Destroy(_projectileParticle, 3f);
            Destroy(_impactParticle, 3f);
            Destroy(gameObject);

			
			ParticleSystem[] trails = GetComponentsInChildren<ParticleSystem>();

            for (int i = 1; i < trails.Length; i++)
            {
                ParticleSystem trail = trails[i];
                if (!trail.gameObject.name.Contains("Trail"))
                    continue;

                trail.transform.SetParent(null);
                Destroy(trail.gameObject, 2);
            }
        }
    }
}
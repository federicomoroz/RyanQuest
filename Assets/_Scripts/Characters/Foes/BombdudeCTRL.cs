using UnityEngine;

public class BombdudeCTRL : FoeChaser, IFoeDamageable
{       
    [Header("EXPLOSION VALUES")]
    [SerializeField] private float  _explodeTime;
    [SerializeField] private float  _explosionRadius;
    [SerializeField] private float  _explosionForce   = 500f;
    [SerializeField] private float  _explosionForceUp = 250f;
    [SerializeField] private int    _explosionDamage  = 5;

    [Header("VFX")]
    [SerializeField] private VFX _sparkVfx;
    [HideInInspector] public VFX SparkVfx { get { return _sparkVfx; } }
    [HideInInspector] public float ExplodeTime { get { return _explodeTime; } }

    protected override void OnDisable()
    {        
        base.OnDisable();
    }

    public void Explosion()
    {

        FXManager.Instance.PlayVfx(VfxName.BombDudeExplosion, transform.position, Quaternion.identity);
        FXManager.Instance.CameraShake(this.transform.position, 2f);

        ExplosionValues newExplosion = new ExplosionValues(_explosionDamage, _explosionForce, _explosionForceUp, this.transform.position, _explosionRadius, ForceMode.Impulse);       
        

        Collider[] colliders = Physics.OverlapSphere(transform.position, _explosionRadius);                       
        
        foreach(Collider nearby in colliders)
        {            
            if(nearby.TryGetComponent(out IBlastable blastable))
            {                
                blastable.HandleBlast(newExplosion);
            }            
        }
        
    }

    public override void TakeDamage(int dmg)
    {
        base.TakeDamage(dmg);
        _animator.SetBool("isExploding", true);
    }


}
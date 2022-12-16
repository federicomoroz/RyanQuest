using UnityEngine;

public class AreaDamageCollider : DamagingCollider
{
    [SerializeField] private float    _colliderLifeTime = 1f;
                     private Collider _collider;

    private void Awake()
    {
        if(_collider == null)
        {
            if (this.TryGetComponent(out Collider collider))
                _collider = collider;
        }

    }

    private void Update()
    {
        if(_colliderLifeTime > 0)        
            _colliderLifeTime -= Time.deltaTime;        
        else        
            _collider.enabled = false;

        
        
    }
}

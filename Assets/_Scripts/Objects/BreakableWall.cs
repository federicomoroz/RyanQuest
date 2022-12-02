using UnityEngine;

[RequireComponent(typeof(Collider))]
public class BreakableWall : MonoBehaviour, IBlastable
{
    private Renderer _renderer;
    private Collider _collider;
    [SerializeField] private VfxName _destructionVfx;
    [SerializeField] private SfxName _destructionAlertSfx;

    private void Awake()
    {
        if(_renderer == null)
        {
            if (this.TryGetComponent(out Renderer renderer))
                _renderer = renderer;
        }

        if(_collider == null)
        {
            if (this.TryGetComponent(out Collider collider))
                _collider = collider;
            
        }
            
    }

    public void HandleBlast(ExplosionValues explosion)
    {
        _renderer.enabled = false;
        _collider.enabled = false;
        FXManager.Instance.PlayVfx(_destructionVfx, this.transform.position, this.transform.rotation);
        FXManager.Instance.PlaySound(_destructionAlertSfx);
        FXManager.Instance.CameraShake(this.transform.position, 10f);
    }
}

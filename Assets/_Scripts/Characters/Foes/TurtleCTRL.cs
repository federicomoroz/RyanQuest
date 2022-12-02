using UnityEngine;
public class TurtleCTRL : FoeChaser, IFoeDamageable
{
    private DamagingCollider _damagingCollider;
    protected override void Awake()
    {
        base.Awake();
        _damagingCollider = GetComponent<DamagingCollider>();
        _damagingCollider.SetPower(GetAttackValue);
    }

    public override void OnDeath()
    {
        base.OnDeath();
       
    }

}

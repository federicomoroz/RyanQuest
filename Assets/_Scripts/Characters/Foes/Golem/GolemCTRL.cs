using UnityEngine;

public class GolemCTRL : FoeAttacker, IFoeDamageable
{
    
    [Header("Attack Values")]    

                     public DamagingCollider _attackCollider;
    [SerializeField] private SfxName[]       _sfxCries;
    [SerializeField] private Transform       _earthSlamPoint;
    [SerializeField] private float           _phaseTwoHpThreshold = 0.5f;

    protected override void OnEnable()
    {
        base.OnEnable();
        _attackCollider.SetPower(_attackValue);        
    }

    public void HitColliderOn()
    {
        _attackCollider.gameObject.SetActive(true);
        FXManager.Instance.CameraShake(this.transform.position, 13f);
        FXManager.Instance.PlaySound(SfxName.FoeGolemPunchTrail, _attackCollider.transform.position);
    }
    public void HitColliderOff()
    {
        _attackCollider.gameObject.SetActive(false);
    }    

    public void DoCrying(int track = 0)
    {
        FXManager.Instance.PlaySound(_sfxCries[track], transform.position);
    }

    public override void TakeDamage(int dmg)
    {
        base.TakeDamage(dmg);
        if(_currentHp/_maxHp < _phaseTwoHpThreshold)
        {
            _animator.SetBool("SlamPhase", true);
            print("PHASE 1");
        }
    }

    public void EarthSlamAttack()
    {
        print("EarthSlam attack");
        GameObject slam               = FXManager.Instance.GetProjectile(ProjectileName.EarthSlam);
        AreaDamageCollider damagingco = slam.gameObject.GetComponent<AreaDamageCollider>();
        Instantiate(slam, _earthSlamPoint.position, Quaternion.Euler(-90,0,0), null);
        print("attack value = " + _attackValue);
        damagingco.SetPower(_attackValue);
        FXManager.Instance.CameraShake(_earthSlamPoint.position, 10f);
        FXManager.Instance.PlaySound(SfxName.ImpactGround, _earthSlamPoint.position);

    }


}

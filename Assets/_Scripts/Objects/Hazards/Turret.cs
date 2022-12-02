using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret : MonoBehaviour
{
    
    [Header("Components")]
    [SerializeField] private LineRenderer   _lr;
    [SerializeField] private ParticleSystem _laserImpactVfx;
    [SerializeField] private ParticleSystem _laserStartVfx;
    [SerializeField] private Transform      _shootPoint;
    [SerializeField] private LayerMask      _hitLayerMask;
    [SerializeField] private LayerMask      _playerLayerMask;

                     private Vector3        _laserLengthDefault = new Vector3(0f,0f,200f);
    [SerializeField] private bool           _isActive           = true;
    [SerializeField] private Transform[]    _leftSensors;
    [SerializeField] private Transform[]    _rightSensors;
    [SerializeField] private GameObject     _vfxExplosion;

    

    private void Awake()
    {
        _lr.useWorldSpace = true;
        
        
    }
    private void Update()
    {
        if (_isActive)
            LaserOn();
        else
            LaserOff();
    }

    private void LaserOn()
    {
        if (_lr.enabled != true)
        {
            _lr.enabled = true;

        }
       

        Vector3 laserHit = GetHitPosition();
        
        //Draw Ray
        _lr.SetPosition(0, _shootPoint.position);
        _lr.SetPosition(1, laserHit);

        //Start Vfx
        _laserStartVfx.transform.position = _shootPoint.position;
        _laserStartVfx.transform.rotation = _shootPoint.rotation;

        _laserStartVfx.Play();
        
        //Impact Vfx
        _laserImpactVfx.transform.position = laserHit;
        Vector3 dir = _shootPoint.position - laserHit;
        _laserImpactVfx.transform.rotation = Quaternion.LookRotation(dir);
        
        _laserImpactVfx.Play();
        KnockEntities(_leftSensors, Vector3.Distance(_shootPoint.position, laserHit), -transform.right);
        KnockEntities(_rightSensors, Vector3.Distance(_shootPoint.position, laserHit), transform.right);


    }

    private void LaserOff()
    {
        _lr.enabled = false;
        _laserImpactVfx.Stop();
        _laserStartVfx.Stop();
    }

    private Vector3 GetHitPosition()
    {
        RaycastHit hit;
        if(Physics.Raycast(_shootPoint.position, _shootPoint.up, out hit, 200f, _hitLayerMask))
           return hit.point;
        
        else        
           return _laserLengthDefault;        
    }

    private void KnockEntities(Transform[] sensors, float rayLength, Vector3 direction)
    {
        for (int i = 0; i < sensors.Length; i++)
        {
            RaycastHit hit;
            if(Physics.Raycast(sensors[i].position, transform.forward, out hit, rayLength, _playerLayerMask))
            {
                if(hit.transform.TryGetComponent(out IPlayerDamageable player))
                {
                    if (player.Vulnerable())
                    {
                        print("player reached");                        
                        player.TakeDamage(3);
                        Rigidbody rb = player.GetRigidbody();
                        rb.Knockback(hit.point, 100f);                        
                        Instantiate(_vfxExplosion, hit.point, Quaternion.identity, null);
                    }

                    
                }
            }

        }
    }




}

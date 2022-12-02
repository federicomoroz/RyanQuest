using UnityEngine;
using Cinemachine;

[RequireComponent(typeof(CinemachineImpulseSource))]
public class ImpulseCTRL : MonoBehaviour
{
    private CinemachineImpulseSource _cis;    

    private void Awake()
    {
        if (_cis == null)
        {
            if (this.TryGetComponent(out CinemachineImpulseSource source))
                _cis = source;
        }
    }

    public void ShakeHandler(Vector3 position, float force)
    {
        MoveSource(position);
        CameraShake(force);
    }

    private void MoveSource(Vector3 position)
    {
        this.transform.position = position;
    }

    private void CameraShake(float force)
    {
        _cis.GenerateImpulse(force);        
    }


    
}


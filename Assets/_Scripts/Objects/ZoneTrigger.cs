using UnityEngine;
using System;

public class ZoneTrigger : MonoBehaviour
{  
    public Action<bool> e_PlayerInZone;

    private void OnTriggerEnter(Collider other)
    {       
        if(other.TryGetComponent(out IWarpable warpable))
        {
            e_PlayerInZone?.Invoke(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent(out IWarpable warpable))
        {
            e_PlayerInZone?.Invoke(false);
        }
    }

}

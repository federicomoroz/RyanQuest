using UnityEngine;
using System;

public class TriggerCollider : MonoBehaviour
{  
    public Action<bool> e_PlayerInZone;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            print("Player in zone");
            e_PlayerInZone?.Invoke(true); 
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            print("Player off zone");           
            e_PlayerInZone?.Invoke(false);
        }
    }

}

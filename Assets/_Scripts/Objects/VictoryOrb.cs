using UnityEngine;

public class VictoryOrb : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if(other.TryGetComponent(out IWarpable player))
        {
            print("Player got the orb");
            EventManager.Trigger(EventName.VictoryOrb);
        }
    }
}

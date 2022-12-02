using UnityEngine;

public class MovingPlatform : MovingStructure
{


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
            other.transform.parent = this.transform;


    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
            other.transform.parent = null;

    }
}

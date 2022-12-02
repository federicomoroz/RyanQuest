using UnityEngine;


public class Checkpoint : MonoBehaviour
{
  [SerializeField] private Transform _wrap;

    private void Awake()
    {
        if (_wrap == null)        
            _wrap = this.GetComponentInChildren<IWrap>().GetTransform();
        
    }    

    private void OnTriggerEnter(Collider other)
    {
        ProcessCollision(other);
    }

    private void ProcessCollision(Collider other)
    {
        if (other.TryGetComponent<IWarpable>(out IWarpable obj) && obj != null)
            EventManager.Trigger(EventName.PlayerCheckpoint, _wrap);
    }


   

}

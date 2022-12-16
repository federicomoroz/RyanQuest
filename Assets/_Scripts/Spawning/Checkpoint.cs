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
        if (other.TryGetComponent(out IWarpable warpable))
            EventManager.Trigger(EventName.PlayerCheckpoint, _wrap);
    }


   

}

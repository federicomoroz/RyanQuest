using UnityEngine;

public class PowerUp : MonoBehaviour
{
    [SerializeField] private int _hpCureValue;
    [SerializeField] private GameObject _destroyVfx;

    private void OnTriggerEnter(Collider other)
    {
        if(other.TryGetComponent(out ICurable curable))
        {
            curable.Cure(_hpCureValue);
            print("collided with curable");
            Instantiate(_destroyVfx, this.transform.position, this.transform.rotation, null);
            Destroy(this.gameObject);
        }

        
    }

}

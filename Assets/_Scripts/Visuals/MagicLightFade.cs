using UnityEngine;

namespace MagicArsenal
{
    public class MagicLightFade : MonoBehaviour
    {        
        [SerializeField] private float _lifeTime         = 0.2f;
        [SerializeField] private bool  _destroyAfterTime = true; 
                         private Light _light;
                         private float _initIntensity;

        private void Awake()
        {
            if(_light == null)
            {
                if (this.TryGetComponent(out Light light))
                    _light = light;
                else
                    Debug.Log("No light object found on " + gameObject.name);
            }

            _initIntensity = _light.intensity;

        }

        void Update()
        {     
           _light.intensity -= _initIntensity * (Time.deltaTime / _lifeTime);

           if (_destroyAfterTime && _light.intensity <= 0)
               Destroy(gameObject);
        }
    }
}
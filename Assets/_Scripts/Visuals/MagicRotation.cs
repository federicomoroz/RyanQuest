using UnityEngine;
 
namespace MagicArsenal
{
    public class MagicRotation : MonoBehaviour
    {
        public enum spaceEnum { Local, World };
 
        [Header("Rotate axises by degrees per second")]
        [SerializeField] private Vector3 _rotateVector = Vector3.zero; 
        [SerializeField] private spaceEnum _rotateSpace; 
   
        void Update()
        {
            if (_rotateSpace == spaceEnum.Local)
                transform.Rotate(_rotateVector * Time.deltaTime);
            else
                transform.Rotate(_rotateVector * Time.deltaTime, Space.World);
        }
    }
}
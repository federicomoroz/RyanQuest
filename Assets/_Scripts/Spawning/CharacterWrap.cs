using UnityEngine;

public class CharacterWrap : MonoBehaviour, IWrap
{
    private Transform _tr;    

    private void Awake()
    {
        _tr = GetComponent<Transform>();        
    }

    public Transform GetTransform()
    {
        return _tr;
    }

}

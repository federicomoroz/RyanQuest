using UnityEngine;

public class ItemRandomSpawner : MonoBehaviour
{
    [SerializeField] private GameObject[] _itemsList;

    [Range(0,1)]
    [SerializeField] private float _chanceOfSpawning;   


public void HandleSpawning()
    {
        if (WillSpawn(_chanceOfSpawning))        
            Instantiate(ChooseObject(), transform.position + Vector3.up, Quaternion.identity, null);
        
    }

    private GameObject ChooseObject()
    {
        int index = Random.Range(0, _itemsList.Length);
        GameObject go = _itemsList[index];
        return go;
    }

    private bool WillSpawn(float chance)
    {
        print("Checking if will spawn");
        float number = Random.Range(0f, 1f);
        if (number < chance)        
            return true;
        
        else        
            return false;
        
    }
}

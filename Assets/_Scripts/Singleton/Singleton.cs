using UnityEngine;

public abstract class Singleton<T> : MonoBehaviour where T : Singleton<T>
{
    public static T Instance { get; protected set; }
    protected virtual void Awake()
    {
        if(Instance != null)
        {
            Destroy(this);
            return;
        }

        Instance = this as T;
        DontDestroyOnLoad(this);
    }
}

using UnityEngine;

public class PlayerSpawner : MonoBehaviour
{
                     public static PlayerSpawner  instance; 
    [SerializeField] public        Player         currentPlayer;
    [SerializeField] private       PlayerData     _storedData = null;  

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        Player player = FindObjectOfType<Player>();
        UpdateWarp(player.transform);
        currentPlayer = player;

    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
            EventManager.Trigger(EventName.PlayerRespawn);
    }

    private void OnEnable()
    {    
        EventManager.Subscribe(EventName.PlayerCheckpoint, UpdateWarp);
        EventManager.Subscribe(EventName.PlayerRespawn, WarpPlayer);       

    }

    private void OnDisable()
    {
        EventManager.Unsubscribe(EventName.PlayerCheckpoint, UpdateWarp);
        EventManager.Unsubscribe(EventName.PlayerRespawn, WarpPlayer);
    }
    private void UpdateWarp(params object[] parameters)
    {
        Transform t = (Transform)parameters[0];
        _storedData = new PlayerData(t.position, t.rotation);
        print($"Updated Wrap: Last position {_storedData.lastPosition} Last rotation {_storedData.lastRotation}");
    }

    private void WarpPlayer(params object[] parameters)
    {
        currentPlayer.WarpTo(_storedData.lastPosition, _storedData.lastRotation);        
    } 



    
}

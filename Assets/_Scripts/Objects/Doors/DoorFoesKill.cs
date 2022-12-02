using UnityEngine;

public class DoorFoesKill : Door
{
    [SerializeField] private TriggerCollider _triggerCollider;
    [SerializeField] private Foe[]           _foesToKill;
                     private int             _foesAlive;


    private void OnEnable()
    {
        CheckFoes();
        SubscribeFoes();       
        _triggerCollider.e_PlayerInZone += PlayerInZone;

    }


    private void OnDisable()
    {
        UnsubscribeFoes();    
        _triggerCollider.e_PlayerInZone -= PlayerInZone;
    }

    private void SubscribeFoes()
    {
        for (int i = 0; i < _foesToKill.Length; i++)
        {
            _foesToKill[i].e_FoeDeath += CheckFoes;       
        }
    }

    private void UnsubscribeFoes()
    {
        for (int i = 0; i < _foesToKill.Length; i++)
        {
            _foesToKill[i].e_FoeDeath -= CheckFoes;            
        }
    }

    private void CheckFoes()
    {
        if (_foesToKill.Length > 0 && !_isOpen)
        {
            _foesAlive = _foesToKill.Length;
            for (int i = 0; i < _foesToKill.Length; i++)
            {
                if (_foesToKill[i].gameObject.activeInHierarchy == false)
                    _foesAlive--;
            }
            if (_foesAlive <= 0)
            {
                Open();                            
                PlayerInZone(false);
            }
        }
        else
        {
            Close();
        }
    }    

    private void PlayerInZone(bool state)
    {
        if (state)
        {
            if(_foesAlive != 0)
                EventManager.Trigger(EventName.PlayerInKillZone, state);
        }    
        else
            EventManager.Trigger(EventName.PlayerInKillZone, false);
        
    }

}


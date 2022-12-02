using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager1 : LevelManager<LevelManager1>
{
    [SerializeField] private MusicName _boss1Music;
    [SerializeField] private MusicName _boss2Music;

    private void OnEnable()
    {
        EventManager.Subscribe(EventName.Boss1Death, OnBoss1Death);
        EventManager.Subscribe(EventName.VictoryOrb, OnVictoryOrbGot);
        EventManager.Subscribe(EventName.PlayerDeath, OnPlayerDeath);
        
    }

    private void OnDisable()
    {
        EventManager.Unsubscribe(EventName.Boss1Death, OnBoss1Death);
        EventManager.Unsubscribe(EventName.VictoryOrb, OnVictoryOrbGot);
        EventManager.Unsubscribe(EventName.PlayerDeath, OnPlayerDeath);
    }

    private void OnVictoryOrbGot(params object[] parameters)
    {
        GameManager.Instance.GameWin();
    }

    private void OnEnterBossZone(params object[] parameters) 
    {
        switch ((int)parameters[0])
        {
            case 1:
                FXManager.Instance.PlayMusic(_boss1Music, true);
                break;
            case 2:
                FXManager.Instance.PlayMusic(_boss2Music, true);
                break;
            default:
                Debug.Log($"Boss track {(int)parameters[0]} not found. Playing default BG track");
                FXManager.Instance.PlayMusic(bgMusic, true);
                break;
        }        
    }

    private void OnLeavingBossZone(params object[] parameters)
    {
        FXManager.Instance.PlayMusic(bgMusic, true);
    }

    private void OnBoss1Death(params object[] parameters)
    {
        
    }

    private void OnBoss2Death(params object[] parameters)
    {

    }

    private void OnPlayerDeath(params object[] parameters)
    {
        if ((bool)parameters[0] == true)
        {
            GameManager.Instance.Pause();            
        }
        else
        {
            FXManager.Instance.PlayMusic(bgMusic, true);
        }

        
        
        
    }
}

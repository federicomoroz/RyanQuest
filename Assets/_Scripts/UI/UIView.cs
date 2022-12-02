using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class UIView : MonoBehaviour
{
    [Header("TELEKINESIS UI")]
    [SerializeField] private GameObject _pickObjectContainer;
    [SerializeField] private GameObject _pullObjectContainer;
    [SerializeField] private GameObject _throwObjectContainer;

    [Header("SCREENS")]
    [SerializeField] private GameObject _pauseMenu;
    [SerializeField] private GameObject _deathMenu;

    [Header("STAGE ELEMENTS UI")]
    [SerializeField] private GameObject _killDoorContainer;

    [Header("FLASH SCREEN FX")]
    [SerializeField] private FlashImage _redScreenContainer;    

    private void OnEnable()
    {     
        EventManager.Subscribe(EventName.PlayerCanPickObj, SetPickObjContainer);
        EventManager.Subscribe(EventName.PlayerCanPullObj, SetPullObjContainer);
        EventManager.Subscribe(EventName.PlayerCanShootObj, SetThrowObjContainer);
        EventManager.Subscribe(EventName.PlayerInKillZone, SetKillDoorContainer);
        EventManager.Subscribe(EventName.PlayerGotHurt, PlayRedScreenFlash);
        EventManager.Subscribe(EventName.Pause, SetPauseMenu);
        EventManager.Subscribe(EventName.PlayerDeath, SetDeathMenu);
    }

    private void OnDisable()
    {
        EventManager.Unsubscribe(EventName.PlayerCanPickObj, SetPickObjContainer);
        EventManager.Unsubscribe(EventName.PlayerCanPullObj, SetPullObjContainer);
        EventManager.Unsubscribe(EventName.PlayerCanShootObj, SetThrowObjContainer);
        EventManager.Unsubscribe(EventName.PlayerInKillZone, SetKillDoorContainer);
        EventManager.Unsubscribe(EventName.PlayerGotHurt, PlayRedScreenFlash);
        EventManager.Unsubscribe(EventName.Pause, SetPauseMenu);
        EventManager.Unsubscribe(EventName.PlayerDeath, SetDeathMenu);
    }

    private void SetPickObjContainer(params object[] parameters)
    {
        _pickObjectContainer.SetActive((bool)parameters[0]);
    }

    private void SetPullObjContainer(params object[] parameters)
    {
        _pullObjectContainer.SetActive((bool)parameters[0]);
    }

    private void SetThrowObjContainer(params object[] parameters)
    {
        _throwObjectContainer.SetActive((bool)parameters[0]);
    }

    private void SetKillDoorContainer(params object[] parameters)
    {
        _killDoorContainer.SetActive((bool)parameters[0]);
    }

    private void PlayRedScreenFlash(params object[] parameters)
    {
        _redScreenContainer.DoFlash(Color.red);
    }

    private void SetPauseMenu(params object[] parameters)
    {
        _pauseMenu.SetActive((bool)parameters[0]);
    }

    private void SetDeathMenu(params object[] parameters)
    {
        _deathMenu.SetActive((bool)parameters[0]);
    }

    public void UnPause()
    {
        GameManager.Instance.Pause();
    }

    public void QuitGame()
    {
        UnPause();
        GameManager.Instance.BackToMainMenu();
    }

    public void Respawn()
    {
        UnPause();
        GameManager.Instance.RespawnOnScene();
    }

}

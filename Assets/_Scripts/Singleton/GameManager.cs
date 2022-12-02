using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : Singleton<GameManager> 
{                     
    private bool _isPaused = false;
    [HideInInspector] public bool IsPaused { get { return _isPaused; } }

   
    void Start() => Initialize();    

    public void Pause()
    {
        if (!_isPaused)        
            _isPaused = true;            
        
        else        
            _isPaused = false;

        EventManager.Trigger(EventName.Pause, _isPaused);
        TimeFreezing(_isPaused);
        ToggleMouse(_isPaused);
    }

    private void TimeFreezing(bool state)
    {
        if (state == true)
            Time.timeScale = 0;
        else
            Time.timeScale = 1f; 
    }


    private void Initialize()
    {
        Application.targetFrameRate = 60;
    }

    public void RespawnOnScene()
    {
        
        EventManager.Trigger(EventName.PlayerRespawn);
        EventManager.Trigger(EventName.PlayerDeath, false);
        
    }
    
    public void ToggleMouse(bool state)
    {
        if(state == false)
        {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }
        else
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.Confined;
        }
    }

    public void QuitGame()
    {
        print("Quit");
        Application.Quit();
    }

    public void BackToMainMenu()
    {
        SceneLoader.Instance.LoadSceneWithFade("TitleScreen");
    }


    public void GameWin()
    {
        SceneLoader.Instance.LoadSceneWithFade("WinScreen");
    }

    public void StartGame()
    {
        SceneLoader.Instance.LoadSceneWithFade("IntroScreen");
    }


}


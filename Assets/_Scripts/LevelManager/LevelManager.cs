using UnityEngine;

public abstract class LevelManager<T> : MonoBehaviour where T : LevelManager<T>
{
                     public    static    T Instance    { get; private set; }
    [SerializeField] protected MusicName   bgMusic;
    [SerializeField] private   bool        _needsMouse;
    [SerializeField] private   bool        _needsPause;
    [SerializeField] private   bool        _needsBgMusic;

    protected virtual void Awake()
    {
        Instance = this as T;
    }

    protected virtual void Start()
    {
        if(_needsBgMusic) 
            FXManager.Instance.PlayMusic(bgMusic, true);

        GameManager.Instance.ToggleMouse(_needsMouse);        
    }

    protected virtual void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && _needsPause)        
            GameManager.Instance.Pause();
        
    }


    public virtual void StartGame()
    {
        FXManager.Instance.PlaySound(SfxName.UiStartGame);
        GameManager.Instance.StartGame();

    }

    public virtual void OnQuitGame()
    {
        FXManager.Instance.PlaySound(SfxName.UiQuitGame);
        GameManager.Instance.QuitGame();
    }


}

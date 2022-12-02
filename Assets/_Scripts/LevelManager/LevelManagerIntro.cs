using System.Collections;
using UnityEngine;
using UnityEngine.Video;
using UnityEngine.UI;

public class LevelManagerIntro : LevelManager<LevelManagerIntro>
{
    [SerializeField] private VideoPlayer _vp;
    [SerializeField] private RawImage    _bg;
    [SerializeField] private VideoName   _clip;    
    private AudioSource _as;
    

    protected override void Awake()
    {
        base.Awake();  
    }
    protected override void Start()
    {
        base.Start();
        FXManager.Instance.StopMusic();
        StartCoroutine(PlayVideo());
        
    }

    private IEnumerator PlayVideo()
    {
        _vp.clip = FXManager.Instance.GetVideoClip(_clip);
        
        _vp.Prepare();
        while (!_vp.isPrepared)        
            yield return null;
        

        _bg.texture = _vp.texture;
        _vp.Play();

        while (_vp.isPlaying)        
            yield return null;
        

        SceneLoader.Instance.LoadSceneWithFade("Sandbox");      
    

    }


}

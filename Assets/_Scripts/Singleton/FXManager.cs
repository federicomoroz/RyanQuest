using System.Collections;
using UnityEngine;
using UnityEngine.Video;

public class FXManager : Singleton<FXManager>
{
    private AudioSource _asMusic;
    private ImpulseCTRL _impulseController;

    [SerializeField] private float _musicFadeTime = 1f;

    protected override void Awake()
    {
        base.Awake();

        if (_asMusic == null)
        {
            if (this.TryGetComponent(out AudioSource source))
                _asMusic = source;
        }
        if (_impulseController == null)
        {
            if (this.transform.GetChild(0).TryGetComponent(out ImpulseCTRL impulse))
                _impulseController = impulse;
        }

    }

    public void PlayMusic(MusicName music, bool isLoop)
    {
        GameAssets.SfxMusic track = GetAudioMusic(music);

        if (_asMusic.isPlaying)
        {
            if (_asMusic.clip == track.audioClip)                          
                return;            
            else
            {
                StartCoroutine(SwitchMusicCO(track.audioClip, isLoop));
                return;
            }
        }
        StartMusic(track.audioClip, isLoop);
    }

    public void StopMusic()
    {
        if (_asMusic.isPlaying)
            _asMusic.Stop();
    }

    private IEnumerator SwitchMusicCO(AudioClip newTrack, bool isLoop)
    {

        float currentVolume = _asMusic.volume;
        //Reset Elapsed timer
        float elapsed = 0f;

        //Fade Out currentSong
        while(elapsed < _musicFadeTime)
        {
            elapsed += Time.deltaTime;            
            _asMusic.volume = Mathf.Lerp(_asMusic.volume, 0, elapsed/_musicFadeTime);      
            yield return null;
        }

        //Set new song
        StartMusic(newTrack, isLoop);

        //Reset timer
        elapsed = 0f;
        //Fade In newSong

        while(elapsed < _musicFadeTime)
        {
            elapsed += Time.deltaTime;
            
            _asMusic.volume = Mathf.Lerp(_asMusic.volume, currentVolume, elapsed/_musicFadeTime);
            yield return null;
        } 

    }

    private void StartMusic(AudioClip newTrack, bool isLoop)
    {
        _asMusic.clip = newTrack;
        print("Playing " + newTrack);
        _asMusic.Play();
        _asMusic.loop = isLoop;
    }

    public void PlaySound(SfxName sound, Vector3 position)
    {
        GameObject sfxGameObj             = new GameObject("Sfx");
        sfxGameObj.transform.position     = position;
        AudioSource audioSource           = sfxGameObj.AddComponent<AudioSource>();
        GameAssets.SfxClip sfxClip        = GetSfxClip(sound);
        sfxGameObj.name = "Sfx " + sfxClip.name;
        audioSource.outputAudioMixerGroup = sfxClip.mixerGroup;
        audioSource.clip = sfxClip.audioClip;
        audioSource.Play();

        StartCoroutine(DestroyObjCo(sfxGameObj, sfxClip.audioClip.length));
    }
    
    public void PlaySound(SfxName sound)
    {
        GameObject sfxGameObj = new GameObject("Sfx");
        AudioSource audioSource = sfxGameObj.AddComponent<AudioSource>();
        GameAssets.SfxClip sfxClip = GetSfxClip(sound);
        sfxGameObj.name = "Sfx " + sfxClip.name;
        audioSource.outputAudioMixerGroup = sfxClip.mixerGroup;
        audioSource.clip = sfxClip.audioClip;
        audioSource.Play();

        StartCoroutine(DestroyObjCo(sfxGameObj, sfxClip.audioClip.length));
    }

    public void PlayVfx(VfxName vfx, Vector3 position, Quaternion rotation)
    {
        Instantiate(GetVfx(vfx), position, rotation, null);

    }

    public GameObject GetProjectile(ProjectileName name)
    {
        foreach(GameAssets.Projectile projectile in GameAssets.Instance.ProjectilesArray)
        {
            if (projectile.name == name)
                return projectile.projectileGo;
        }
        Debug.LogError("Projectile" + name + " not found");
        return null;
    }
    public VideoClip GetVideoClip(VideoName name)
    {
        foreach (GameAssets.Video video in GameAssets.Instance.VideoClipArray)
        {
            if (video.name == name)
                return video.videoClip;
        }
        Debug.LogError("Video" + name + " not found");
        return null;
    }



    public void CameraShake(Vector3 position, float force)
    {
        _impulseController.ShakeHandler(position, force);
    }

    private IEnumerator DestroyObjCo(GameObject go, float time)
    {
        yield return new WaitForSeconds(time);
        Destroy(go);
    }


    private GameAssets.SfxClip GetSfxClip(SfxName sound)
    {
        foreach(GameAssets.SfxClip sfxClip in GameAssets.Instance.SfxClipArray)
        {
            if (sfxClip.name == sound)
                return sfxClip;
        }
        Debug.LogError("Sound" + sound + "not found");
        return null;
    }

    private GameAssets.SfxMusic GetAudioMusic(MusicName music)
    {
        foreach (GameAssets.SfxMusic sfxMusic in GameAssets.Instance.MusicArray)
        {
            if (sfxMusic.name == music)
                return sfxMusic;
        }
        Debug.LogError("Music" + music + " not found");
        return null;
    }



    private GameObject GetVfx(VfxName vfx)
    {
        foreach(GameAssets.Vfx vfxGo in GameAssets.Instance.VfxArray)
        {
            if (vfxGo.name == vfx)
                return vfxGo.vfxGo;
        }
        Debug.LogError("Vfx" + vfx + " not found");
        return null;
    }


}



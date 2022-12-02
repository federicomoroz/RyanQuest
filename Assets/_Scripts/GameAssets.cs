using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Video;

public class GameAssets : MonoBehaviour
{
    private static GameAssets _instance;

    public static GameAssets Instance
    {
        get
        {
            if (_instance == null)
                _instance = (Instantiate(Resources.Load("GameAssets")) as GameObject).GetComponent<GameAssets>();

            return _instance;
        }
    }

    public SfxClip[] SfxClipArray;
    public Vfx[] VfxArray;
    public Projectile[] ProjectilesArray;
    public SfxMusic[] MusicArray;
    public Video[] VideoClipArray;
    

    [System.Serializable]
    public class Projectile
    {
        public ProjectileName name;
        public GameObject projectileGo;
    }
    [System.Serializable]
    public class SfxClip
    {
        public SfxName name;
        public AudioClip audioClip;
        public AudioMixerGroup mixerGroup;
    }

    [System.Serializable]
    public class Vfx
    {
        public VfxName name;
        public GameObject vfxGo;
    }
    [System.Serializable]
    public class SfxMusic
    {
        public MusicName name;
        public AudioClip audioClip;
        public AudioMixerGroup mixerGroup;
    }

    [System.Serializable]

    public class Video
    {
        public VideoName name;
        public VideoClip videoClip;
    }

}
    public enum VfxName
    {
        PhysicalHitImpactCommon,
        PickableImpact,
        FoeDeathExplosion,
        BombDudeExplosion,
        WallExplosion,
        GolemEarthSlam,
    }
    public enum SfxName
    {
        PlayerStep,
        PlayerJump,
        PlayerTkCharge,
        PlayerTkRelease1,
        PlayerTkRelease2,
        PlayerHurt1,
        PlayerHurt2,
        PlayerHurt3,
        PlayerMock1,
        PlayerMock2,
        PlayerMock3,
        PlayerDeath,
        PlayerStart1,
        PlayerCheckpoint,
        FoePlantDeath,
        FoeExplosion,
        FoeBombdudeCry1,
        FoeBombdudeCry2,
        FoeGolemCry1,
        FoeGolemCry2,
        FoeGolemCry3,
        FoeGolemPunchTrail,
        FoeTurtleDeath,
        TkObjTrail,
        SwitchToggle,
        UiStartGame,
        UiHighlightButton,
        UiSelectButton,
        UiBack,
        UiQuitGame,
        JumpSlash,
        Landing,
        ImpactGround,
    }

    public enum MusicName
    {   
        BreakIn,
        TitleScreen,
        Intro,
        Victory,
        GameOver,
        BossFight1,
        BossFight2,
    
    }

    public enum VideoName
    {
        Intro,


    }

    public enum ProjectileName
    {   
        Poison,
        EarthSlam,

    }



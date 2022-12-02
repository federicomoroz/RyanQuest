using UnityEngine;
[RequireComponent(typeof(ParticleSystem))]
public class VFX : MonoBehaviour
{
    private ParticleSystem _ps;
    private AudioSource _as;

    private void Awake()
    {
        if(_ps == null) 
            _ps = GetComponent<ParticleSystem>();        

        if(_as == null)
        {
            if(TryGetComponent(out AudioSource audioSource))
            {
                _as = audioSource;
            }
        }


    }
        

    public void PlayVfx()
    {

        if (!_ps.isPlaying) _ps.Play();
        else return;
        
        if(_as != null)
        {
            if(!_as.isPlaying) 
                _as.Play();
        }
    }

    public void StopVfx()
    {
        if(_ps != null)
        {
            if (_ps.isPlaying) _ps.Stop();
            else return;
        
            if(_as != null)
            {
                if (_as.isPlaying) _as.Stop();
            }
        }
    }

}

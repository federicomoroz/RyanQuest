using UnityEngine;

public class FoeView
{
    private GameObject _vfxExplosion;
    private GameObject _vfxHit;

    public FoeView SetVfxExplosion(GameObject obj)
    {
        _vfxExplosion = obj;
        return this;    
    }

    public FoeView SetVfxHit(GameObject obj)
    {
        _vfxHit = obj;
        return this;
    }


}

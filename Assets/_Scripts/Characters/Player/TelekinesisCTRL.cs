using UnityEngine;

public class TelekinesisCTRL
{
    //Components
    private Player         _player;
    private FSM_TK         _fsm;
    private Transform      _cam;                    
    private Animator       _animator;    
    private Transform      _grabPoint;
    private Transform      _throwPoint;

    //Values
    private LayerMask      _pickupLayerMask;
    private float          _pickupMaxDistance;

    //Throw Force parameters
    [HideInInspector] public  float throwForceCurrent;
                      private float _throwForceMultiplier = 0.05f;
                      private float _throwForceMin        = 900f;
                      private float _throwForceMax        = 1300f;    

    //Vfx
                      private VFX        _vfxWeapon;
                      private GameObject _vfxAura;

    //Picked obj ref
    
    
    [HideInInspector] public PickableObject objectPicked;
    [HideInInspector] public IPickable      previous;
    
    

    [HideInInspector] public Transform      Cam                  { get { return _cam;                  } }
    [HideInInspector] public Animator       Animator             { get { return _animator;             } }
    [HideInInspector] public Transform      GrabPoint            { get { return _grabPoint;            } }
    [HideInInspector] public float          PickupMaxDistance    { get { return _pickupMaxDistance;    } }
    [HideInInspector] public Transform      ThrowPoint           { get { return _throwPoint;           } }
    [HideInInspector] public float          ThrowForceMin        { get { return _throwForceMin;        } }
    [HideInInspector] public float          ThrowForceMax        { get { return _throwForceMax;        } }
    [HideInInspector] public float          ThrowForceMultiplier { get { return _throwForceMultiplier; } }
    [HideInInspector] public VFX            VfxWeapon            { get { return _vfxWeapon;            } }
    [HideInInspector] public GameObject     VfxAura              { get { return _vfxAura;              } }
    [HideInInspector] public LayerMask      PickupLayerMask      { get { return _pickupLayerMask;      } }
    

    public TelekinesisCTRL SetPlayer(Player player)
    {
        _player = player;
        return this;
    }
    public TelekinesisCTRL SetCamera(Transform cam)
    {
        _cam = cam;
        _grabPoint = _cam.Find("GrabPoint");
        return this;
    }

    public TelekinesisCTRL SetAnimator(Animator animator)
    {
        _animator = animator;
        return this;
    }

    public TelekinesisCTRL SetThrowPoint(Transform t)
    {
        _throwPoint = t;
        return this;
    }

    public TelekinesisCTRL SetPickupLayerMask(LayerMask layerMask)
    {
        _pickupLayerMask = layerMask;
        return this;
    }

    public TelekinesisCTRL SetPickupMaxDistance(float distance)
    {
        _pickupMaxDistance = distance;
        return this;
    }

    public TelekinesisCTRL SetThrowForceMultiplier(float value)
    {
        _throwForceMultiplier = value;
        return this;
    }

    public TelekinesisCTRL SetThrowForceMin(float value)
    {
        _throwForceMin = value;
        return this;
    }

    public TelekinesisCTRL SetThrowForceMax(float value)
    {
        _throwForceMax = value;
        return this;
    }

    public TelekinesisCTRL SetVfxWeapon(VFX vfx)
    {
        _vfxWeapon = vfx;
        return this;
    }

    public TelekinesisCTRL SetVfxAura(GameObject go)
    {
        _vfxAura = go;
        return this;
    }


    public void VirtualOnEnable()
    {
        _fsm = new FSM_TK(this);
        _fsm.SwitchState(TkState.IDLE);
        _grabPoint = _cam.Find("GrabPoint");    
        EventManager.Subscribe(EventName.PlayerGotHurt, DropObject);
        
    }

    public void VirtualOnDisable()
    {
        EventManager.Unsubscribe(EventName.PlayerGotHurt, DropObject);
    }

    public void VirtualUpdate()
    {
        _fsm.VirtualUpdate();
    }

    public void DropObject(params object[] parameters)
    {
        if(objectPicked != null)
           objectPicked.Drop();
        _animator.SetTrigger("Tk_Off");
        _fsm.SwitchState(TkState.IDLE); 
  
    }

    public bool CheckDistance(PickableObject pickable, float distance)
    {
        if (pickable == null)
            return false;

        bool detect = true;

        if (Vector3.Distance(pickable.transform.position, _throwPoint.position) > distance)
            detect = false;

        return detect;
    }


    #region Objects Visible & Reachable methods
    public bool IsVisible(Camera cam, Transform target)
    {
        var planes = GeometryUtility.CalculateFrustumPlanes(cam);
        foreach (var plane in planes)
        {
            if (plane.GetDistanceToPoint(target.position) < 0)
                return false;
        }
        return true;
    }

    public bool IsObstructed(Vector3 a, Vector3 b)
    {
        if (Physics.Raycast(a, (b - a).normalized, out _, (b - a).magnitude, _pickupLayerMask))
            return true;

        return false;
    }
    #endregion
}
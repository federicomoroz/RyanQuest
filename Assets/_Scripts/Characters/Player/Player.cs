using System;
using UnityEngine;
using Cinemachine;

public class Player : Entity, IWarpable, ICurable, IPlayerDamageable, IBlastable
{
    #region Parameters
    [Header("COMPONENTS")]
    #region Parameters
    [SerializeField] private Transform[]         _groundCheckers;
    [SerializeField] private LayerMask           _layerMaskWalk;
    [SerializeField] private GameObject          _virtualCam;
    #endregion

    [Header("CAMERA")]
    #region Parameters
                     private Transform           _cam;
    [SerializeField] private Transform           _camFollowTarget;
    [SerializeField] private Transform           _camLookTarget;
    #endregion

    [Header("MOVEMENT VALUES")]
    #region Parameters
    [SerializeField] private float               _speedMove           = 2.0f;
    [SerializeField] private float               _speedRot            = 0.8f;
    #endregion

    [Header("JUMP VALUES")]
    #region Parameters
    [SerializeField] private float               _heightJump          = 1.0f;
    [SerializeField] private float               _coyoteJumpTimer     = 0.15f;
    #endregion

    [Header("ANIMATOR VALUES")]
    #region Parameters
    [SerializeField] private float               _animationSmoothTime = 0.1f;
    #endregion

    [Header("TELEKINESIS VALUES")]
    #region Parameters

    [SerializeField] private Transform           _throwPoint;
    [SerializeField] private LayerMask           _pickupLayerMask;
    [SerializeField] private float               _pickupMaxDistance;
    #endregion

    [Header("TK: THROW FORCE VALUES")]
    #region Parameters
    [SerializeField] private float               _throwForceMultiplier = 0.05f;
    [SerializeField] private float               _throwForceMin        = 900f;
    [SerializeField] private float               _throwForceMax        = 1300f;
    #endregion*/

    [Header("VFX")]
    #region Parameters
    [SerializeField] private VFX        _vfxWeapon;
    [SerializeField] private GameObject _vfxAura;
    [SerializeField] private VFX        _vfxCure;
    [SerializeField] private VFX        _vfxCheckpoint;
    #endregion

    #endregion

    [Header("CLASSES")]
    private PlayerLocomotion  _locomotion;
    private PlayerCollisions  _collisions;
    private PlayerInputs      _inputs;
    private TelekinesisCTRL   _telekinesis;

    public Action e_playerGotHurt;
    public Action e_playerOnRampEdge;
    public Action e_playerOffRampEdge;
    public PlayerCollisions CollisionManager { get { return _collisions; } }
    public PlayerLocomotion Locomotion       { get { return _locomotion; } }  

    #region Methods

    protected override void Awake()
    {
        base.Awake();
            
        SetupCamera();        

        //Build classes
        _inputs     = new PlayerInputs();
        _collisions = new PlayerCollisions().SetPlayer(this)
                                            .SetAnimator(_animator)
                                            .SetRigidbody(_rb);
        _locomotion = new PlayerLocomotion().SetPlayer(this)
                                            .SetTransform(this.transform)
                                            .SetRigidbody(_rb)
                                            .SetCamera(_cam)
                                            .SetAnimator(_animator)
                                            .SetMoveSpeed(_speedMove)
                                            .SetRotateSpeed(_speedRot)
                                            .SetJumpHeight(_heightJump)
                                            .SetCoyoteJumpTimer(_coyoteJumpTimer)
                                            .SetGroundCheckers(_groundCheckers)
                                            .SetWalkableLayerMask(_layerMaskWalk)
                                            .SetAnimationSmoothTime(_animationSmoothTime)
                                            .SetInputs(_inputs);

        _telekinesis = new TelekinesisCTRL().SetAnimator(_animator)
                                            .SetPlayer(this)
                                            .SetCamera(_cam)
                                            .SetPickupLayerMask(_pickupLayerMask)
                                            .SetPickupMaxDistance(_pickupMaxDistance)
                                            .SetThrowPoint(_throwPoint)
                                            .SetThrowForceMin(_throwForceMin)
                                            .SetThrowForceMax(_throwForceMax)
                                            .SetThrowForceMultiplier(_throwForceMultiplier)
                                            .SetVfxAura(_vfxAura)
                                            .SetVfxWeapon(_vfxWeapon);


    }


    private void Update()
    {
        if (!GameManager.Instance.IsPaused)
        {
            _locomotion.VirtualUpdate();
            _inputs.VirtualUpdate();
            _telekinesis.VirtualUpdate();     
        }

        if (Input.GetKeyDown(KeyCode.Y))
        {
            EventManager.Trigger(EventName.PlayerDeath, true);
        }
    }

    private void FixedUpdate()
    {
        if (!GameManager.Instance.IsPaused)
        {
            _inputs.VirtualFixedUpdate();
        }
        
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        EventManager.Trigger(EventName.PlayerSpawned, this.transform);
        EventManager.Subscribe(EventName.PlayerCheckpoint, OnPlayerCheckpoint);

        _locomotion.VirtualOnEnable();
        _telekinesis.VirtualOnEnable();
       
    }

    private void OnDisable()
    {
        _locomotion.VirtualOnDisable();
        EventManager.Unsubscribe(EventName.PlayerCheckpoint, OnPlayerCheckpoint);
    }
    private void OnTriggerEnter(Collider other)
    {

        _collisions.VirtualOnTriggerEnter(other);
        
    }

    private void OnTriggerStay(Collider other)
    {
        _collisions.VirtualOnTriggerStay(other);                
    }

    private void OnTriggerExit(Collider other)
    {
        _collisions.VirtualOnTriggerExit(other);  
    }


    public override void TakeDamage(int dmg)
    {
        if (IsVulnerable)
        {
            _collisions.HandleDamage(dmg);            
        }

    }


    public override void Cure(int hp)
    {
        AddHp(hp);
        _vfxCure.PlayVfx();
        EventManager.Trigger(EventName.PlayerHpUpdate, _currentHp / _maxHp);
    }

    public bool Vulnerable()
    {
        return IsVulnerable;
    }

    private void OnPlayerCheckpoint(params object[] parameters)
    {
        
            _vfxCheckpoint.PlayVfx();
    }

    public override void Knockback(Vector3 otherPosition, float force)
    {
        _rb.Knockback(otherPosition, force);
    }


    public override void OnDeath()
    {
        EventManager.Trigger(EventName.PlayerDeath, true);
        
    }

    public void WarpTo(Vector3 position, Quaternion rotation)
    {
        AddHp(_maxHp);
        _rb.isKinematic = true;
        _rb.Warp(position, rotation);
        _rb.isKinematic = false;
        IsVulnerable    = true;
        EventManager.Trigger(EventName.PlayerHpUpdate, _currentHp / _maxHp);
    }
    private void SetupCamera()
    {
        _cam = GameObject.Find("MainCamera").transform;

        GameObject vCamGo = Instantiate(_virtualCam, null);
        var vCam          = vCamGo.GetComponent<CinemachineVirtualCamera>();
        vCam.LookAt       = _camLookTarget;
        vCam.Follow       = _camFollowTarget;
    }

    public void HandleBlast(ExplosionValues explosion)
    {
        TakeDamage(explosion.Damage);
        _rb.AddExplosionForce(explosion.Force, explosion.OriginPosition, explosion.Radius, explosion.ForceUp, explosion.Mode);
    }

    #endregion
}

public interface IPlayerDamageable 
{
    public void TakeDamage(int dmg);
    public bool Vulnerable();

    public void Knockback(Vector3 otherPosition, float force);
    public Rigidbody GetRigidbody();

    
}

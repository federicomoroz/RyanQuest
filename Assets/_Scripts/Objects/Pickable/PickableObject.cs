using System.Collections;
using UnityEngine;

public class PickableObject : MonoBehaviour, IPickable, IPushable, IBlastable
{

    public IPickable.PickableState currentState;

    [Header("Components")]
                     protected Rigidbody      _rb;                             
                     protected Transform      _followPoint;                    
                     private   Outline        _outline;
                     protected Collider       _collider;
    [SerializeField] protected PhysicMaterial _noFrictionMaterial;
                     protected AudioSource _audioSource;

    [Header("VFX")]
    [SerializeField] protected ParticleSystem _vfxGlow;
    [SerializeField] protected ParticleSystem _vfxLoaded;
    [SerializeField] protected GameObject     _vfxThrowTrail;
    [SerializeField] protected GameObject     _vfxExplosion;

    [Header("SFX")]
    [SerializeField] protected AudioClip      _sfxThrowTrail;
    [SerializeField] protected AudioClip      _sfxPull;
    [SerializeField] protected AudioClip      _sfxMoveLoop;
    [SerializeField] protected AudioClip      _sfxLoadedLoop;

    [Header("Parameters")]
    [SerializeField] public    bool           isThrowable;
                     private   bool           _canBeOutlined           = true;

    [SerializeField] protected LayerMask      _obstaclesLayerMask;
    [SerializeField] protected float          _trailVfxSwitchThreshold = 5f;
    [SerializeField] protected float          _magnesisForce           = 10f;
    [SerializeField] protected float          _physicsDrag             = 3f;
    [SerializeField] protected float          _lerpSpeedPulled;            
                     protected const float    _loadedSpeed             = 500f;  

    protected virtual void Awake()
    {
        if(_rb == null)
        {
            if (TryGetComponent(out Rigidbody rb))
                _rb = rb;
        }

        if(_collider == null)
        {
            if (TryGetComponent(out Collider collider))
                _collider = collider;
        }

        if(_outline == null)
        {
            if (TryGetComponent(out Outline outline))
                _outline = outline;
        }

        if(_audioSource == null)
        {
            if (TryGetComponent(out AudioSource audioSource))
                _audioSource = audioSource;          
                
        }

        if (_vfxLoaded == null)
            _vfxLoaded = _vfxGlow;

        SetState(IPickable.PickableState.IDLE);
    }

    protected void Update()
    {
        OnUpdateState(currentState);

    }


    protected void FixedUpdate()
    {
        OnFixedUpdateState(currentState);

    }


    #region FSM
    public void SetState(IPickable.PickableState newState)
    {
        if (newState != currentState)
        {
            OnExitState(currentState);
            currentState = newState;
            OnEnterState(newState);
        }        
    }

    private void OnEnterState(IPickable.PickableState newState)
    {
        switch (newState)
        {
            case IPickable.PickableState.IDLE:
                this.gameObject.layer      = 7;     
                SetRigidbodyState(ref _rb, RigidBodyState.IDLE);
                OutlineOff();
                if(_vfxGlow.isPlaying)         _vfxGlow.Stop(true);
                if (_vfxLoaded.isPlaying)      _vfxLoaded.Stop(true);
                if (_vfxThrowTrail.activeSelf) _vfxThrowTrail.SetActive(false);
                break;
            case IPickable.PickableState.MOVING:
                this.gameObject.layer           = 10;
                SetRigidbodyState(ref _rb, RigidBodyState.MAGNESIS);
                PlayAudioLoop(_sfxMoveLoop);
                _vfxGlow.Play(true);
                break;
            case IPickable.PickableState.PULLING:
                if (_vfxGlow.isPlaying)        _vfxGlow.Stop(true);
                SetRigidbodyState(ref _rb, RigidBodyState.ATTACK);                
                break;
            case IPickable.PickableState.LOADED:
                PlayAudioLoop(_sfxLoadedLoop);
                _vfxGlow.Play(true);
                _vfxLoaded.Play(true);
                break;
            case IPickable.PickableState.THROWN:              
                break;
        }
    }

    private void OnUpdateState(IPickable.PickableState state)
    {
        switch (state)
        {
            case IPickable.PickableState.IDLE:
                break;
            case IPickable.PickableState.MOVING:
                break;
            case IPickable.PickableState.PULLING:
                CheckTrailVfx();
                break;
            case IPickable.PickableState.LOADED:
                CheckTrailVfx();
                break;
            case IPickable.PickableState.THROWN:
                CheckTrailVfx();
                break;
        }
    }

    private void OnFixedUpdateState(IPickable.PickableState state)
    {
        switch (state)
        {
            case IPickable.PickableState.IDLE:
                break;
            case IPickable.PickableState.MOVING:
                ConstantRotation(ref _rb);
                MagnesisMovement(ref _rb, _followPoint, _magnesisForce);
                break;
            case IPickable.PickableState.PULLING:
                PullMovement(ref _rb, _followPoint, _lerpSpeedPulled);
                break;
            case IPickable.PickableState.LOADED:
                LoadedMovement(ref _rb, _followPoint, _loadedSpeed);
                break;
            case IPickable.PickableState.THROWN:
                break;
        }
    }

    private void OnExitState(IPickable.PickableState state)
    {
        switch (state)
        {
            case IPickable.PickableState.IDLE:
                break;
            case IPickable.PickableState.MOVING:
                StopAudioLoop();
                break;
            case IPickable.PickableState.PULLING:                
                break;
            case IPickable.PickableState.LOADED:
                StopAudioLoop();
                break;
            case IPickable.PickableState.THROWN:
                break;
        }
    }

    private void StopAudioLoop()
    {
        _audioSource.clip = null;
        _audioSource.loop = false;
        _audioSource.Stop();
    }


    #endregion

    #region PickableMethods
    public PickableObject GetObject()
    {
        return this;
    }

    #region OutlineMethods

    public void OutlineOn()
    {
        if (_canBeOutlined)
            _outline.enabled = true;
    }

    public void OutlineOff()
    {
        _outline.enabled = false;
    }
    private IEnumerator OutlineTimerCo()
    {
        _canBeOutlined = false;
        yield return new WaitForSeconds(0.6f);
        _canBeOutlined = true;
    }

    #endregion

    public void Grab(Transform grabPoint)
    {
        _followPoint = grabPoint;
        SetState(IPickable.PickableState.MOVING);      
    }

    public void Drop()
    {
        StartCoroutine(OutlineTimerCo());       
        SetState(IPickable.PickableState.IDLE);
    }
    protected void ConstantRotation(ref Rigidbody rb)
    {        
        rb.angularVelocity = new Vector3(1,1,1); 
        
    }

    #endregion

    #region ThrowableMethods

    public bool IsThrowable()
    {
        return isThrowable;
    }

    public void Pull(Transform throwPoint)
    {        
        StartCoroutine(PullCo(throwPoint));
    }

    private IEnumerator PullCo(Transform throwPoint)
    {
        SetState(IPickable.PickableState.PULLING);
        _audioSource.PlayOneShot(_sfxPull);
        _followPoint = throwPoint;        
        yield return new WaitForSeconds(0.1f);
        if (currentState == IPickable.PickableState.PULLING)
            SetState(IPickable.PickableState.LOADED);
        else
            StopCoroutine(PullCo(throwPoint));
    }

    public void Throw(Vector3 target, float force)
    {
       
        Vector3 dir = (target - transform.position).normalized;
        Debug.Log(this.transform.name + " thrown at " + force + " force.");
        _rb.AddForce(dir * force, ForceMode.Impulse);
        _rb.AddRelativeTorque(Vector3.forward * 500f, ForceMode.Impulse);
        SetState(IPickable.PickableState.THROWN);
        
        _audioSource.PlayOneShot(_sfxThrowTrail);
        
    }

    private void OnCollisionEnter(Collision other)
    {

        if (currentState == IPickable.PickableState.THROWN)
        {
            FXManager.Instance.PlayVfx(VfxName.PickableImpact, other.GetContact(0).point, Quaternion.identity);
            FXManager.Instance.CameraShake(this.transform.position, 0.7f);

            if (other.gameObject.TryGetComponent(out IFoeDamageable foe) && foe != null)
            {
                print("Collided with foe");
                float magnitude = _rb.velocity.magnitude;
                FXManager.Instance.PlayVfx(VfxName.PhysicalHitImpactCommon, other.GetContact(0).point, Quaternion.identity);
                Rigidbody rb = foe.GetRigidbody();
                rb.Knockback(other.GetContact(0).point, magnitude * 2f);
                foe.TakeDamage(1);
            }


            VelocityDecrease(_rb, 0.55f);            
        }
            
    }






    private void VelocityDecrease(Rigidbody rb, float decreaseMultiplier)
    {
        Vector3 velocity  = rb.velocity;
        velocity         *= decreaseMultiplier;
        rb.velocity       = velocity;
        StartCoroutine(OutlineTimerCo());
        SetState(IPickable.PickableState.IDLE);

    }
    private void CheckTrailVfx()
    {        
        _vfxThrowTrail.SetActive(_rb.velocity.magnitude > _trailVfxSwitchThreshold);
    }

    #endregion

    private void PlayAudioLoop(AudioClip sound)
    {
        _audioSource.clip = sound;
        _audioSource.loop = true;
        _audioSource.Play();
    }

    #region MovementMethods

    private void MagnesisMovement(ref Rigidbody rb,Transform target, float force)
    {
        
        Vector3 deltaPosition = target.position - rb.position;
        Vector3 dir           = deltaPosition.normalized;
        float   forceOverMass = force/_rb.mass;
        float   dynamicForce  = forceOverMass * Mathf.Sqrt(deltaPosition.magnitude);

        rb.AddForce(dir * dynamicForce * Time.fixedDeltaTime, ForceMode.VelocityChange);             
    }

    private void PullMovement(ref Rigidbody rb, Transform target, float lerp)
    {
        Vector3 deltaPosition = LerpDir(rb.position, target.position, lerp);
        rb.MovePosition(deltaPosition);

    }

    private void LoadedMovement(ref Rigidbody rb, Transform target, float speed)
    {        
        Vector3 deltaPosition = target.position - rb.position;
        Vector3 dir           = deltaPosition.normalized;
        float   dynamicSpeed  = speed * deltaPosition.magnitude;

        rb.velocity           = dir * dynamicSpeed * Time.fixedDeltaTime;      
    }
    
    private Vector3 LerpDir(Vector3 currentPosition, Vector3 targetPosition, float lerp)
    {
        return Vector3.Lerp(currentPosition, targetPosition, lerp * Time.fixedDeltaTime);
    }
    #endregion

    #region Rigidbody State methods
    private void SetRigidbodyState(ref Rigidbody rb, RigidBodyState newState)
    {
        switch (newState)
        {
            case RigidBodyState.IDLE:
                SetRigidBodyValues(ref rb, true, CollisionDetectionMode.Discrete, RigidbodyInterpolation.None, 0f);
                break;
            case RigidBodyState.MAGNESIS:
                SetRigidBodyValues(ref rb, false, CollisionDetectionMode.Continuous, RigidbodyInterpolation.Interpolate, _physicsDrag, _noFrictionMaterial);
                break;
            case RigidBodyState.ATTACK:
                SetRigidBodyValues(ref rb, false, CollisionDetectionMode.Continuous, RigidbodyInterpolation.Interpolate, 0f);
                break;
        }
    }

    private enum RigidBodyState
    {
        IDLE,
        MAGNESIS,
        ATTACK,
    }

    private void SetRigidBodyValues(ref Rigidbody rb, bool useGravity, CollisionDetectionMode collisionDetectMode, RigidbodyInterpolation interpolation, float drag, PhysicMaterial physicMat = null)
    {
        rb.useGravity             = useGravity;
        rb.collisionDetectionMode = collisionDetectMode;
        rb.interpolation          = interpolation;
        rb.drag                   = drag;

        _collider.material = physicMat != null ? physicMat : null;
            
       
       
       
    }

    public void HandleBlast(ExplosionValues explosion)
    {
        _rb.AddExplosionForce(explosion.Force, explosion.OriginPosition, explosion.Radius, explosion.ForceUp, explosion.Mode);
    }

    #endregion
}




using UnityEngine;

public class PlayerLocomotion
{
    [Header("Components")]
    private Player              _player;
    private PlayerInputs        _inputs;
    private Rigidbody            _rb;
    private Animator            _animator;
    private Transform           _transform;
    private Transform           _cam;    
    private Transform[]         _groundCheckers;
    private LayerMask           _layerMaskWalk;

    [Header("Strafe Handling")]
    private float               _speedMove              =  450f;
    private float               _speedRot               =  0.8f;
    private Vector3             _moveVector;
    private Vector2             _leftStickInputValue;


    [Header("Slope Movement Handling")]
    private float               _maxSlopeAngle          = 35;
    private RaycastHit          _slopeHit;

    [Header("Jump Handling")]
    private float               _heightJump             = 1.0f;
    private float               _coyoteJumpTime         = 0.33f;
    private float?              _lastGroundedTime;
    private float?              _jumpButtonPressedTime;
    private float               _jumpTimerElapsed       = 0f;
    private bool                _canJump                = true;

    [Header("Animator hashs")]
    private int                 _moveXanimator;
    private int                 _moveZanimator;
    private int                 _jumpAnimator;

    [Header("Animator Values")]
    private float               _animationSmoothTime  = 0.1f;
    private Vector2             _currentAnimationBlend;
    private Vector2             _animationVelocity    = new Vector2(0.75f,0.75f);

    public float CurrentSpeed     { get { return _speedMove;         } }
    public Rigidbody Rb           { get { return _rb;                } }
    
    #region BUILDER  

    
    public PlayerLocomotion SetPlayer(Player player)
    {
        _player = player;
        return this;
    }
    public PlayerLocomotion SetInputs(PlayerInputs inputs)
    {
        _inputs = inputs;
        return this;
    }

    public PlayerLocomotion SetTransform(Transform t)
    {
        _transform = t;
        return this;
    }

    public PlayerLocomotion SetCamera(Transform cam)
    {
        _cam = cam;
        return this;
    }

    public PlayerLocomotion SetRigidbody(Rigidbody rb)
    {
        _rb = rb;
        return this;
    }

    public PlayerLocomotion SetAnimator(Animator animator)
    {
        _animator = animator;
        return this;
    }

    public PlayerLocomotion SetMoveSpeed(float speed)
    {
        _speedMove = speed;
        return this;
    }

    public PlayerLocomotion SetRotateSpeed(float speed)
    {
        _speedRot = speed;
        return this;
    }

    public PlayerLocomotion SetJumpHeight(float height)
    {
        _heightJump = height;
        return this;
    }

    public PlayerLocomotion SetMaxSlopeAngle(float angle)
    {
        _maxSlopeAngle = angle;
        return this;
    }

    public PlayerLocomotion SetCoyoteJumpTimer(float time)
    {
        _coyoteJumpTime = time;
        return this;
    }

    public PlayerLocomotion SetAnimationSmoothTime(float value)
    {
        _animationSmoothTime = value;
        return this;
    }

    public PlayerLocomotion SetGroundCheckers(Transform[] checkers)
    {
        _groundCheckers = checkers;
        return this;
    }

    public PlayerLocomotion SetWalkableLayerMask(LayerMask mask)
    {
        _layerMaskWalk = mask;
        return this;
    }

    #endregion
    public void VirtualOnEnable()
    {        
        //Set Animator parameters Id's
        _moveXanimator = Animator.StringToHash("MoveX");
        _moveZanimator = Animator.StringToHash("MoveZ");
        _jumpAnimator  = Animator.StringToHash("Jump");

        // Subscribe to Inputs
        _inputs.e_LeftStickValue += LeftStickInputHandler;
        _inputs.e_OnJumpPressed  += Jump;

        _player.e_playerOnRampEdge += JumpHeightIncrease;
        _player.e_playerOffRampEdge += JumpHeightDecrease;
        
    }

    public void VirtualOnDisable()
    {
        _inputs.e_OnJumpPressed  -= Jump;
        _inputs.e_LeftStickValue -= LeftStickInputHandler;

        _player.e_playerOnRampEdge -= JumpHeightIncrease;
        _player.e_playerOffRampEdge -= JumpHeightDecrease;
    }   

    public void VirtualUpdate()
    {
        // Check grounding //

        GroundedHandler();

        _animator.SetFloat("VelocityY", _rb.velocity.y);

        if (!_canJump)
        {
            _jumpTimerElapsed += Time.deltaTime;
            if (_jumpTimerElapsed > 0.35f)
            {
                _canJump = true;
                _jumpTimerElapsed = 0;
            }
        }
    }

    public void SetNewSpeed(float value)
    {
        _speedMove = value;
    }
    private void GroundedHandler()
    {
        if (CheckGround() || OnSlope())
        {
            _lastGroundedTime = Time.time;        
            _animator.SetBool("IsGrounded", true);
        }        
        else 
        {            
            _animator.SetBool("IsGrounded", false);     
        }
    }

    private void LeftStickInputHandler(Vector2 input)
    {
        _leftStickInputValue = input;
        if (_player.isVulnerable)        
            Movement(input);
        
    }

    private void Movement(Vector2 input)
    {
           //Smooth the inputs blend values
           _currentAnimationBlend  = Vector2.SmoothDamp(_currentAnimationBlend, input, ref _animationVelocity, _animationSmoothTime);

           //Create movement direction
           Vector3 deltaPosition   = new Vector3(_currentAnimationBlend.x, 0, _currentAnimationBlend.y);          


        //Check if walking on a slope
        if (OnSlope())
            {
                deltaPosition = GetSlopeMoveDirection(deltaPosition);
                _rb.useGravity = false;
            }
            else
                _rb.useGravity = true;

            //Movement according cam X and Z   
            deltaPosition = deltaPosition.x * _cam.right.normalized + deltaPosition.z * _cam.forward.normalized;     

            //Do movement
           float currentSpeedMove = _speedMove;
        

        //Set Falling speed 
        if (!CheckGround() && _rb.velocity.y < -8f)
            currentSpeedMove = _speedMove * 0.33f;
        else
            currentSpeedMove = _speedMove;


            _moveVector  = deltaPosition * currentSpeedMove * Time.deltaTime;

            //Move rigidbody
            _rb.velocity = new Vector3(_moveVector.x, _rb.velocity.y, _moveVector.z);            
            

            //Change Animation
            _animator.SetFloat(_moveXanimator, _currentAnimationBlend.x);
            _animator.SetFloat(_moveZanimator, _currentAnimationBlend.y);

            //Check if it's moving
            if (_rb.velocity.magnitude < 0.4 && _player.isVulnerable)
                _animator.SetBool("IsMoving", false);
            else            
                _animator.SetBool("IsMoving", true);            
        
       if(_player.isVulnerable) 
            RotateToCam();
        
    }   

    private void RotateToCam()
    {
        //getCam Y angle
        float targetAngle         = _cam.eulerAngles.y;
        //set Y rotation to cam Y angle.
        Quaternion deltaRotation  = Quaternion.Euler(0, targetAngle, 0);
        //rotate player towards target rotation
        _transform.rotation        = Quaternion.Lerp(_transform.rotation, deltaRotation, _speedRot * Time.deltaTime);           
    }

    public void Jump()
    {
        if (_canJump)
        {
            if (Time.time - _lastGroundedTime <= _coyoteJumpTime)               // Check last grounded time and compare to coyote timer
            {
                _jumpButtonPressedTime = Time.time;                             //Set time of jump button input
                if(Time.time - _jumpButtonPressedTime <= _coyoteJumpTime)       //Check time of jump button and compare to coyote jump time (it should always be true)
                {
                    _canJump = false;                                           // Set flag to false
                    _rb.VelocityFreezeY();                                      // Freeze Y velocity

                    Vector3 jumpDirection = new Vector3(_leftStickInputValue.x, 1f, _leftStickInputValue.y); // Get jump direction acording move inputs

                    SetNewSpeed(450f);
           
                    _rb.AddForce(jumpDirection * _heightJump, ForceMode.Impulse);  // Add vertical force
                    _animator.SetTrigger(_jumpAnimator);                        // Set animation
                    _jumpButtonPressedTime = null;                              // Null the timers
                    _lastGroundedTime      = null;
                    FXManager.Instance.PlaySound(SfxName.PlayerJump);           // Play SFX
                    FXManager.Instance.PlaySound(SfxName.JumpSlash, _groundCheckers[0].position);
                    FXManager.Instance.CameraShake(_groundCheckers[0].position, 0.1f);
                }
            }      
        }
    
    }

    public void ApplyGravityCompensation()
    {
        _rb.AddForce(Vector3.down * 1000f, ForceMode.Force);
    }
    private void JumpHeightIncrease()
    {
        Debug.Log("Jump height increased");
        _heightJump = 115f;
    }

    private void JumpHeightDecrease()
    {
        Debug.Log("Jump height decreased");
        _heightJump = 80f;
    }


    public bool CheckGround(float distance = 0.30f)
    {
        int counter = 0;
        for (int i = 0; i < _groundCheckers.Length; i++)
        {
            RaycastHit hit;
            if(Physics.Raycast(_groundCheckers[i].position, Vector3.down, out hit, distance, _layerMaskWalk))
               counter++;                          
            
        }    

        return counter>=1;
    }
    
    public bool OnSlope()
    {
        if(Physics.Raycast(_groundCheckers[0].position, Vector3.down, out _slopeHit, 0.25f, _layerMaskWalk))
        {
            float angle = Vector3.Angle(Vector3.up, _slopeHit.normal);
            return angle < _maxSlopeAngle && angle != 0;
        }

        return false;
    }

    private Vector3 GetSlopeMoveDirection(Vector3 direction)
    {
        return Vector3.ProjectOnPlane(direction, _slopeHit.normal);
    }
}
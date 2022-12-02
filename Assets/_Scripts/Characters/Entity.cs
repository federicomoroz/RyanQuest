using UnityEngine;

public abstract class Entity : MonoBehaviour
{       
                     protected Rigidbody _rb;
                     protected Animator  _animator;

    [Header("HEALTH VALUES")]
    [SerializeField] protected float  _maxHp;
                     protected float  _currentHp    = 0;
                     protected bool   _isAlive      = true;   
                     protected bool   _isVulnerable = true; 

   [HideInInspector] public float  MaxHp        { get { return _maxHp;        } }
   [HideInInspector] public float  CurrentHp    { get { return _currentHp;    } }
   [HideInInspector] public bool IsAlive        { get { return _isAlive;      } set { _isAlive      = value; } }                     
   [HideInInspector] public bool IsVulnerable   { get { return _isVulnerable; } set { _isVulnerable = value; } }

    public abstract void TakeDamage(int dmg); 
    public abstract void Cure(int hp);
    public abstract void OnDeath();
    protected virtual void Awake()
    {
        _rb       = GetComponent<Rigidbody>();
        _animator = GetComponentInChildren<Animator>();
    }
    protected virtual void OnEnable()
    {
        Initialize();
    }

    protected virtual void Initialize()
    {
        AddHp(_maxHp);
    }

    public Rigidbody GetRigidbody()
    {
        return _rb;
    }

    public virtual void Knockback(Vector3 direction, float force) { }
    protected void AddHp(float value)
    {
        _currentHp += value;

        if (_currentHp > _maxHp)
            _currentHp = _maxHp;
    }

    public void SubstractHp(float value)
    {
        _currentHp -= value;        
        _currentHp = _currentHp <= 0 ? 0 : _currentHp;        
    }

    
    
}

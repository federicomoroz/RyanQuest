using UnityEngine;
using UnityEngine.AI;
using System;

public abstract class Foe : Entity, IBlastable
{  
                     protected Transform         _target;
                     protected NavMeshAgent      _agent;
    [SerializeField] protected GameObject        _hpBar;
                     protected ItemRandomSpawner _irs;

    [Header("COMBAT STATS")]
    [SerializeField] protected int _defenseValue, _attackValue;

    [Header("SFX")] 
    [SerializeField] protected SfxName _deathSfx;

    #region Getters
    [HideInInspector] public Transform         Target          { get { return _target;       } }
    [HideInInspector] public NavMeshAgent      Agent           { get { return _agent;        } } 
    [HideInInspector] public GameObject        HpBar           { get { return _hpBar;        } }
    [HideInInspector] public ItemRandomSpawner Irs             { get { return _irs;          } }
    [HideInInspector] public int               GetDefenseValue { get { return _defenseValue; } }
    [HideInInspector] public int               GetAttackValue  { get { return _attackValue;  } }
    #endregion


    public Action<float> e_OnHealthUpdate = delegate { };
    public Action e_FoeDeath;


    protected override void Awake()
    {
        base.Awake();
        _agent = GetComponent<NavMeshAgent>();
        _irs = GetComponent<ItemRandomSpawner>();
        
    }

    protected virtual void Start()
    {
        _target = PlayerSpawner.instance.currentPlayer.transform;
    }

    protected override void OnEnable()
    {
        base.OnEnable();  
        EventManager.Subscribe(EventName.PlayerSpawned, GetPlayer);
        EventManager.Subscribe(EventName.PlayerDeath, OnPlayersDeath);

        e_OnHealthUpdate?.Invoke(_currentHp / _maxHp);


    }

    protected virtual void OnDisable()
    {
        e_FoeDeath?.Invoke();     
        EventManager.Unsubscribe(EventName.PlayerSpawned, GetPlayer);
        EventManager.Unsubscribe(EventName.PlayerDeath, OnPlayersDeath);
    }  

    protected void GetPlayer(params object[] parameters)
    {
        _target = (Transform) parameters[0];  
    }
    public override void TakeDamage(int dmg)
    {
        if(_isAlive && isVulnerable)
        {
            _animator.SetTrigger("Damaged");
            print($"{gameObject.name} received {dmg} damage");
            SubstractHp(dmg);        
            e_OnHealthUpdate?.Invoke(_currentHp / _maxHp);
        }
    }

    public override void Cure(int hp)
    {
        AddHp(hp);
        e_OnHealthUpdate?.Invoke(_currentHp / _maxHp);
        print($"{gameObject.name} has been cure for {hp}hp.");
    }

    public override void OnDeath()
    {
        _animator.SetBool("isAlive", false);
        FXManager.Instance.PlaySound(_deathSfx, this.transform.position);
        print($"{this} has died");

    }

    public void SetAgentSpeed(float speed)
    {
        _agent.speed = speed;
    }

    public void DeActivate()
    {
        gameObject.SetActive(false);
    }

    public void InstantiateExplosion()
    {
        FXManager.Instance.PlayVfx(VfxName.FoeDeathExplosion, transform.position + Vector3.up, Quaternion.identity);
        FXManager.Instance.CameraShake(this.transform.position, 1.2f);
    }

    protected void OnPlayersDeath(params object[] parameters)
    {
        if((bool)parameters[0] == true)
        {
           //print("Enemy: Player has died. I go back to IDLE state");
        } 
    }



    public bool TargetOnSight(Transform t, float range)
    {
        RaycastHit hit;
        Vector3 headPosition = _agent.transform.position + Vector3.up * 0.5f;
        if (Physics.Raycast(headPosition, Vector3.forward, out hit, range))
        {
            if (hit.Equals(_target.gameObject))          
                return true;            
        }
        return false;
    }

    public void HandleBlast(ExplosionValues explosion)
    {
        TakeDamage(explosion.Damage/2);
        _rb.AddExplosionForce(explosion.Force*2, explosion.OriginPosition, explosion.Radius, explosion.ForceUp*2, explosion.Mode);
    }
}
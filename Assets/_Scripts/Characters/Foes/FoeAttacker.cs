using UnityEngine;

public class FoeAttacker : FoeChaser
{
    [SerializeField] protected float _attackRange = 8f;
    [SerializeField] protected float _attackDelay = 2f;

    [HideInInspector] public float AttackRange { get { return _attackRange; } }
    [HideInInspector] public float AttackDelay { get { return _attackDelay; } }

    public virtual void Attack() { }
}

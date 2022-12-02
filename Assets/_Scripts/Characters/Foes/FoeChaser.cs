using UnityEngine;
using UnityEngine.AI;

public class FoeChaser : Foe
{
    [Header("PATROL VALUES")]
    [SerializeField] protected float _idleTime     = 5f;
    [SerializeField] protected float _patrolSpeed  = 1.5f;
    [SerializeField] protected float _chaseSpeed   = 2.25f;
    
    [Header("CHASE VALUES")]
    [SerializeField] protected float _chaseRadius  = 8f;
    [SerializeField] protected float _speedRotate  = 5f;

    [HideInInspector] public float PatrolSpeed { get { return _patrolSpeed; } }
    [HideInInspector] public float ChaseSpeed  { get { return _chaseSpeed;  } }
    [HideInInspector] public float ChaseRadius { get { return _chaseRadius; } }
    [HideInInspector] public float IdleTime    { get { return _idleTime;    } }
    [HideInInspector] public float PatrolTime  { get { return _patrolSpeed; } }

     
    public bool IsInRange(Vector3 target, Vector3 origin, float distance)
    {
        if (Vector3.Distance(target, origin) >= distance)
            return false;
        else
            return true;
    }

    public virtual void Patrol()
    {
        if (_agent.remainingDistance <= _agent.stoppingDistance)
        {
            Vector3 point;
            if (RandomPoint(transform.position, _chaseRadius, out point))
            {
                Debug.DrawRay(point, Vector3.up, Color.blue, 1.5f);
                _agent.SetDestination(point);
            }
        }
    }

    public bool RandomPoint(Vector3 center, float range, out Vector3 result)
    {
        Vector3 randomPoint = center + Random.insideUnitSphere * range;
        NavMeshHit hit;
        if(NavMesh.SamplePosition(randomPoint, out hit, 1f, NavMesh.AllAreas))
        {
            result = hit.position;
            return true;
        }

        result = Vector3.zero;
        return false;

    }

    public virtual void ChaseTarget()
    {        
         _agent.SetDestination(_target.position);    
    }


    public void FaceTarget()
    {
        Vector3    direction    = (_target.position - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
        transform.rotation      = Quaternion.Slerp(transform.rotation, lookRotation, _speedRotate * Time.deltaTime);
    }

    

    protected void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, _chaseRadius);       
    }



}

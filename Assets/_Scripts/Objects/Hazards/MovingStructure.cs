using System.Collections;
using UnityEngine;

public class MovingStructure : MonoBehaviour
{
    [SerializeField] protected float       _moveSpeed;
    [SerializeField] protected float       _stopDelay = 0.33f;
    [SerializeField] protected int         _startWaypointIndex;
    [SerializeField] protected Transform[] _waypoints;

    private bool stop = false;
    private int currentPoint;


    void Start()
    {
        this.transform.position = _waypoints[_startWaypointIndex].position;
    }


    void Update()
    {
        if (!stop)
        {
            MoveTo();
            if (Vector3.Distance(transform.position, _waypoints[currentPoint].position) <= 0.02f)            
                StartCoroutine(StopAtWaypoint());
            

        }

    }


    private void MoveTo()
    {
        transform.position = Vector3.MoveTowards(this.transform.position, _waypoints[currentPoint].position, _moveSpeed * Time.deltaTime);
    }

    private IEnumerator StopAtWaypoint()
    {
        stop = true;
        yield return new WaitForSeconds(_stopDelay);
        stop = false;
        SwitchWaypoint();
    }

    private void SwitchWaypoint()
    {
        currentPoint++;
        if (currentPoint == _waypoints.Length)        
            currentPoint = 0;
        
    }


}

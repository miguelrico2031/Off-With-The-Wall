using UnityEngine;
using UnityEngine.Events;
using UnityEngine.AI;

public class PedestrianAI : MonoBehaviour, IPoolObject
{
    #region Attributes

    public bool Active { get => gameObject.activeSelf; set => gameObject.SetActive(value); }
    public AIWaypoint Target { get; private set; }
    public readonly UnityEvent<PedestrianAI> OnTargetReached = new();
    private bool _pathEnded = true;
    private NavMeshAgent _agent;
    
    #endregion
    
    #region Unity Callbacks
    private void Awake()
    {
        _agent = GetComponent<NavMeshAgent>();
        _agent.updateRotation = false;
        _agent.updateUpAxis = false;
        _agent.avoidancePriority = 99;
    }

    private void Update()
    {
        // if (Target is not null && !_agent.pathPending && 
        //     _agent.hasPath && _agent.remainingDistance <= _agent.stoppingDistance &&
        //     _agent.velocity.sqrMagnitude <= .01f)
        if(!_pathEnded && Vector2.Distance(transform.position, Target.transform.position) <= .05f)
        {
            _pathEnded = true;
            OnTargetReached.Invoke(this);
        }
    }

    #endregion

    public void Spawn(AIWaypoint spawnPoint, float agentSpeed)
    {
        transform.position = spawnPoint.transform.position;
        _agent.speed = agentSpeed;
    }

    public void SetTarget(AIWaypoint target)
    {
        Target = target;
        _agent.SetDestination(Target.transform.position);
        _pathEnded = false;
    }

    public void Despawn()
    {
        
    }


    public void Clean()
    {
        Target = null;
        _pathEnded = true;
    }

    public IPoolObject Clone(Transform parent = null, bool active = false)
    {
        var instance = parent is null ? Instantiate(this) : Instantiate(this, parent);
        instance.gameObject.SetActive(active);
        return instance;
    }
}
